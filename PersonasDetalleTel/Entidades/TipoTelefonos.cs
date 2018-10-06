using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonasDetalleTel.Entidades
{
    public class TipoTelefonos
    {
        public int Id { get; set; }
        public string Tipo { get; set; }

        public TipoTelefonos()
        {
            Id = 0;
            Tipo = string.Empty;
        }
    }
}
