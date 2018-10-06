using PersonasDetalleTel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PersonasDetalleTel.UI.Registro
{
    public partial class rPersonas : Form
    {
        public List<TelefonosDetalle> Detalle { get; set; }

        public rPersonas()
        {
            InitializeComponent();
            this.Detalle = new List<TelefonosDetalle>();
        }

        private void CargarGrid()
        {
            TelefonoDataGridView.DataSource = null;
            TelefonoDataGridView.DataSource = this.Detalle;
        }

        private void Limpiar()
        {
            errorProvider1.Clear();

            IdNumericUpDown.Value = 0;
            NombreTextBox.Text = string.Empty;
            CedulaMaskedTextBox.Text = string.Empty;
            DireccionTextBox.Text = string.Empty;
            FechaNacimientoDateTimePicker.Value = DateTime.Now;

            this.Detalle = new List<TelefonosDetalle>();
            CargarGrid();
        }

        private Personas LlenaClase()
        {
            Personas persona = new Personas();
            persona.PersonaId = Convert.ToInt32(IdNumericUpDown.Value);
            persona.Nombre = NombreTextBox.Text;
            persona.Cedula = CedulaMaskedTextBox.Text;
            persona.Direccion = DireccionTextBox.Text;
            persona.FechaNacimiento = FechaNacimientoDateTimePicker.Value;

            persona.Telefonos = this.Detalle;

            return persona;
        }

        private void LlenaCampos(Personas persona)
        {
            IdNumericUpDown.Value = persona.PersonaId;
            NombreTextBox.Text = persona.Nombre;
            CedulaMaskedTextBox.Text = persona.Cedula;
            DireccionTextBox.Text = persona.Direccion;
            FechaNacimientoDateTimePicker.Value = persona.FechaNacimiento;

            this.Detalle = persona.Telefonos;
            CargarGrid();
        }

        private bool Validar()
        {
            bool validar = false;
            errorProvider1.Clear();

            if (String.IsNullOrWhiteSpace(NombreTextBox.Text))
            {
                errorProvider1.SetError(NombreTextBox, "El nombre esta vacio");
                validar = true;
            }
            if (String.IsNullOrWhiteSpace(CedulaMaskedTextBox.Text.Replace("-", " ")))
            {
                errorProvider1.SetError(CedulaMaskedTextBox, "La cedula esta vacia");
                validar = true;
            }
            if (String.IsNullOrWhiteSpace(DireccionTextBox.Text))
            {
                errorProvider1.SetError(DireccionTextBox, "La direccion esta vacia");
                validar = true;
            }

            if (this.Detalle.Count == 0)
            {
                errorProvider1.SetError(TelefonoDataGridView, "Debe agregar algun telefono");
                TelefonoMaskedTextBox.Focus();
                validar = true;
            }

            return validar;
        }

        private bool ValidarDetalle()
        {
            bool validarDetalle = false;
            errorProvider1.Clear();

            if (String.IsNullOrWhiteSpace(TelefonoMaskedTextBox.Text))
            {
                errorProvider1.SetError(DireccionTextBox, "El Telefono esta vacia");
                validarDetalle = true;
            }

            return validarDetalle;
        }

        private void MasTelefonosButton_Click(object sender, EventArgs e)
        {
            
            if (TelefonoDataGridView.DataSource != null)
                this.Detalle = (List<TelefonosDetalle>)TelefonoDataGridView.DataSource;

            if (ValidarDetalle())
            {
                MessageBox.Show("Favor revisar todos los campos", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this.Detalle.Add(
                new TelefonosDetalle(
                    id: 0,
                    personaId: (int)IdNumericUpDown.Value,
                    tipoTelefono: TipoComboBox.Text,
                    telefono: TelefonoMaskedTextBox.Text
                    )
                   );
            CargarGrid();
            TelefonoMaskedTextBox.Focus();
            TelefonoMaskedTextBox.Clear();
        }

        private void RemoverButton_Click(object sender, EventArgs e)
        {
            if(TelefonoDataGridView.Rows.Count > 0 && TelefonoDataGridView.CurrentRow != null)
            {
                Detalle.RemoveAt(TelefonoDataGridView.CurrentRow.Index);

                CargarGrid();
            }
        }

        private void NuevoButton_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private bool ExisteEnLaBaseDeDatos()
        {
            Personas persona = BLL.PersonasBLL.Buscar((int)IdNumericUpDown.Value);
            return (persona != null);
        }

        private void GuardarButton_Click(object sender, EventArgs e)
        {
            Personas persona;
            bool paso = false;
            if (Validar())
            {
                MessageBox.Show("Favor revisar todos los campos", "Validacion", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            persona = LlenaClase();

            if (IdNumericUpDown.Value == 0)
                paso = BLL.PersonasBLL.Guardar(persona);
            else
            {
                if (!ExisteEnLaBaseDeDatos())
                {
                    MessageBox.Show("El Vendedor no existe", "Fallo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                paso = BLL.PersonasBLL.Guardar(persona);
            }
            Limpiar();

            if (paso)
                MessageBox.Show("Guardado", "Con Exito!!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("No se pudo Guardar", "Error!!!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void EliminarButton_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(IdNumericUpDown.Value);
            if (BLL.PersonasBLL.Eliminar(id))
            {
                MessageBox.Show("Eliminado", "Exito!!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("No se pudo eliminar", "Fallido", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BuscarButton_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(IdNumericUpDown.Value);
            Personas persona = BLL.PersonasBLL.Buscar(id);
            if (persona != null)
            {
                NombreTextBox.Text = persona.Nombre;
                CedulaMaskedTextBox.Text = persona.Cedula;
                DireccionTextBox.Text = persona.Direccion;
                FechaNacimientoDateTimePicker.Value = persona.FechaNacimiento;
                TelefonoDataGridView.DataSource = persona.Telefonos;
            }
        }

        private void LlenaCombo()
        {

        }

      
        private void MasTiposButton_Click(object sender, EventArgs e)
        {
            rTipoTelefonos tipo = new rTipoTelefonos();
            if (tipo.ShowDialog() == DialogResult.OK)
                LlenaCombo();
        }
    }
}
