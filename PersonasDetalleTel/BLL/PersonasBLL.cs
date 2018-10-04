using PersonasDetalleTel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonasDetalleTel.DAL;
using System.Data.Entity;
using System.Linq.Expressions;

namespace PersonasDetalleTel.BLL
{
    public class PersonasBLL
    {
        public static bool Guardar(Personas persona)
        {
            bool paso = false;
            contexto contexto = new contexto();
            try
            {
                if (contexto.Personas.Add(persona) != null)
                {
                    contexto.SaveChanges();
                    paso = true;
                }
                contexto.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            return paso;
        }

        public static  bool Modificar(Personas persona)
        {
            bool paso = false;
            contexto db = new contexto();

            try
            {
                var Anterior = db.Personas.Find(persona.PersonaId);
                foreach (var item in Anterior.Telefonos)
                {
                    if (!persona.Telefonos.Exists(d => d.Id == item.Id))
                        db.Entry(item).State = EntityState.Deleted;
                }
                db.Entry(persona).State = EntityState.Modified;
                paso = (db.SaveChanges() > 0);
            }
            catch(Exception)
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }

            return paso;
        }

        public static bool Eliminar(int id)
        {
            bool paso = false;
            contexto contexto = new contexto();
            try
            {
                Personas persona = contexto.Personas.Find(id);

                contexto.Personas.Remove(persona);

                if (contexto.SaveChanges() > 0)
                {
                    paso = true;
                }
                contexto.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            return paso;
        }

        public static Personas Buscar(int id)
        {
            contexto db = new contexto();
            Personas persona = new Personas();
            try
            {
                persona = db.Personas.Find(id);
                persona.Telefonos.Count();
            }
            catch
            {
                throw;
            }
            finally
            {
                db.Dispose();
            }
            return persona;
        }

        public static List<Personas> GetList(Expression<Func<Personas, bool>> expression)
        {
            List<Personas> persona = new List<Personas>();
            contexto contexto = new contexto();
            try
            {
                persona = contexto.Personas.Where(expression).ToList();
                contexto.Dispose();
            }
            catch (Exception)
            {
                throw;
            }
            return persona;
        }

    }
}
