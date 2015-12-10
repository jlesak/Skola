using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigersProject.Model
{
    class Database
    {
        public Entities db;
        public List<jazyk> Druhy; 

        public Database()
        {
            db = new Entities();
            Druhy = new List<jazyk>();
            jazyk jazyk = new jazyk();
            jazyk.jazyk1 = "anj";
            db.jazyk.Add(jazyk);
            db.SaveChanges();
            db.druh.ToList();
        }
    }
}
