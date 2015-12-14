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
        public List<instruktor> Instruktori; 

        public Database()
        {
           
        }
       
        public bool AddInstructor(instruktor instructor)
        {
            var exists = db.instruktor.AsQueryable().Where(i => (i.JMENO == instructor.JMENO) && (i.PRIJMENI == instructor.PRIJMENI));
            if(!exists.Any())
            {
                this.db.instruktor.Add(instructor);
                db.SaveChanges();
                return true;
            }
            else return false;
        }

        public bool AddDisposition(dispozice disposition)
        {
            var exists = db.dispozice.AsQueryable().Where(d => (d.ZACATEK == disposition.ZACATEK) && (d.instruktor == disposition.instruktor));
            if(!exists.Any())
            {
                db.dispozice.Add(disposition);
                db.SaveChanges();
                return true;
            }
            else return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortLanguage">3 místná zkratka pro jazyk (ANJ, NEJ..)</param>
        /// <returns></returns>
        public bool AddLanguage(string shortLanguage)
        {
            if(!db.jazyk.AsQueryable().Any(j => j.jazyk1 == shortLanguage))
            {
                jazyk lang = new jazyk {jazyk1 = shortLanguage};
                db.jazyk.Add(lang);
                return true;
            }
            else return false;
        }

        //Do DB zapíšu jen jeden řádek (i pro lekci trvající 3h) a při přidávání do DataGridu to rozkopíruju => upravovat se bude vždycky ten jeden záznam v DB
        public bool AddLesson(lekce lesson)
        {
            var free = db.dispozice.AsQueryable().Where(d => (d.ZACATEK == lesson.ZACATEK) && (d.instruktor == lesson.instruktor));
            if(free.Any())
            {
                db.lekce.Add(lesson);
                db.dispozice.Remove(db.dispozice.FirstOrDefault(d => (d.instruktor == lesson.instruktor) && (d.ZACATEK == lesson.ZACATEK)));
                //db.dispozice.Remove(free.FirstOrDefault()); //tohle by taky mělo fungovat jako horní řádek ↑
                db.SaveChanges();
                return true;
            }
            else return false;
        }

    }
}
