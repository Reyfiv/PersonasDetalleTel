using PersonasDetalleTel.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonasDetalleTel.DAL
{
    public class contexto : DbContext
    {
        public DbSet<Personas> Personas { get; set; }

        public contexto() : base("ConStr")
        {   }
    }
}
