using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigersProject.Model
{
    class Database
    {
        public Entities Db;
        public List<instruktor> Instructors; 

        public Database()
        {
            Db = new Entities();
            Instructors = Db.instruktor.ToList();
        }
       
        public bool AddInstructor(instruktor instructor)
        {
            var exists = Db.instruktor.AsQueryable().Where(i => (i.JMENO == instructor.JMENO) && (i.PRIJMENI == instructor.PRIJMENI));
            if(!exists.Any())
            {
                Db.instruktor.Add(instructor);
                Db.SaveChanges();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Hledá vhodné instruktory pro novou lekci.
        /// Vyplivne jen ty, kteří mají čas v danou dobu.
        /// Filtruje podle jazyka, druhu
        /// </summary>
        /// <param name="startTime">datum a cas zacatku lekce</param>
        /// <param name="duration">délka výuky</param>
        /// <param name="language">jazyk</param>
        /// <param name="druh">SKI/SNB</param>
        /// <param name="name">jmeno instruktora</param>
        /// <param name="surname">prijmeni instruktora</param>
        public void SearchInstructors(DateTime startTime, int duration, jazyk language, druh druh ,string name, string surname)
        {
          
            var instructors = Db.instruktor.AsQueryable();
            //pridat dateTime
            if(duration > 1) instructors = instructors.Where(i => i.dispozice.ZACATEK == startTime.AddHours(1));//kdyz je lekce delsi nez hodinu, tak vybere instruktory, kteri maji volno i tu dalsi hodinu
            if (language != null) instructors = instructors.Where(i => i.jazyk == language);
            if(druh != null) instructors = instructors.Where(i => i.druh == druh);
            if (!string.IsNullOrEmpty(name)) instructors = instructors.Where(i => i.JMENO.Contains(name));
            if (!string.IsNullOrEmpty(surname)) instructors = instructors.Where(i => i.PRIJMENI.Contains(surname));

            if(instructors.Any()) this.Instructors = instructors.ToList();
            else this.Instructors = null;


        }

        public bool AddDisposition(dispozice disposition)
        {
            var exists = Db.dispozice.AsQueryable().Where(d => (d.ZACATEK == disposition.ZACATEK) && (d.instruktor == disposition.instruktor));
            if(!exists.Any())
            {
                Db.dispozice.Add(disposition);
                Db.SaveChanges();
                return true;
            }
            else return false;
        }

        public void DeleteDisposition(dispozice disposition)
        {
            Db.dispozice.Remove(disposition);
            Db.SaveChanges();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="shortLanguage">3 místná zkratka pro jazyk (ANJ, NEJ..)</param>
        /// <returns></returns>
        public bool AddLanguage(string shortLanguage)
        {
            if(!Db.jazyk.AsQueryable().Any(j => j.JAZYK1 == shortLanguage))
            {
                jazyk lang = new jazyk {JAZYK1 = shortLanguage};
                Db.jazyk.Add(lang);
                return true;
            }
            else return false;
        }

        //Do DB zapíšu jen jeden řádek (i pro lekci trvající 3h) a při přidávání do DataGridu to rozkopíruju => upravovat se bude vždycky ten jeden záznam v DB
        public bool AddLesson(lekce lesson)
        {
            var free = Db.dispozice.AsQueryable().Where(d => (d.ZACATEK == lesson.ZACATEK) && (d.instruktor == lesson.instruktor));
            if(free.Any())
            {
                Db.lekce.Add(lesson);
                DeleteDisposition(free.First());
                //Db.SaveChanges();
                return true;
            }
            else return false;
        }

    }
}
