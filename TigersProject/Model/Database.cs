using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace TigersProject.Model
{
    class Database
    {
        public Entities Db;
        public List<instruktor> Instructors;
        public DataTable DTableMonth;
        public DataTable DTableDay;

        public Database()
        {
            Db = new Entities();
            Instructors = Db.instruktor.ToList();
        }

        //přidá sloupce do rozpisu měsíce
        //provést při změně měsíce
        public void AddMonthColumns(int days, int month)
        {
            DataColumn column = new DataColumn();
            column.ColumnName = "Instruktor";
            this.DTableMonth.Columns.Add(column);
            for (int i = 1; i <= days; i++) {
                column = new DataColumn();
                column.ColumnName = i.ToString() + ". " + month.ToString() + ".";
                this.DTableMonth.Columns.Add(column);
            }
        }

        public void AddMonthRows()
        {
            var dispositions = Db.dispozice.AsQueryable();
            string name;
            foreach (var instructor in this.Instructors)
            {
                var row = DTableMonth.NewRow();
                name = instructor.JMENO + " " + instructor.PRIJMENI;
                dispositions = dispositions.Where(d => d.instruktor.ID == instructor.ID);
                foreach (var disposition in dispositions)
                {
                    //tohle musim domyslet
                    //dodělat podle rozhodnutí o barevnosti. viz papír 1
                }

                row["Instruktor"] = name;
            }
        }
       /* public  MonthRowForInstructor(instruktor instructor)
        {
            DataTable dTable = new DataTable();
            var row = dTable.NewRow();

            row["Instruktor"] = instructor.JMENO + " " + instructor.PRIJMENI;
            var dispositions = Db.dispozice.AsQueryable();
            dispositions = dispositions.Where(d => d.instruktor.ID == instructor.ID);
            foreach (var disposition in dispositions) {
                
            }
        }*/
        //vytvoří rádek pro instruktora na daný den --zmenit void na cosi
        public void DayRowForInstructor(instruktor instructor)
        {
            
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

            if(startTime.Minute != 0) startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, 0, 0); // pokud je cas napr. 8:30, zmeni se na 8:00 a hledaji se dispozice od 8:00
            instructors = instructors.Where(i => i.dispozice.ZACATEK == startTime);

            //kdyz je lekce delsi nez hodinu, tak vybere instruktory, kteri maji volno i tu dalsi hodinu
            if (duration > 1)
            {
                for (int t = 2; t <= duration; t++)
                    instructors = instructors.Where(i => i.dispozice.ZACATEK.AddHours(t - 1) == startTime.AddHours(t - 1));
            }//instructors = instructors.Where(i => i.dispozice.ZACATEK == startTime.AddHours(duration-1));
            if (language != null) instructors = instructors.Where(i => i.jazyk == language);
            if(druh != null) instructors = instructors.Where(i => i.druh == druh);
            if (!string.IsNullOrEmpty(name)) instructors = instructors.Where(i => i.JMENO.Contains(name));
            if (!string.IsNullOrEmpty(surname)) instructors = instructors.Where(i => i.PRIJMENI.Contains(surname));

            if(instructors.Any()) this.Instructors = instructors.ToList();
            else this.Instructors = null;
            

        }

        public bool AddDisposition(dispozice disposition)
        {
            var exists = Db.dispozice.AsQueryable().Where(d => (d.ZACATEK == disposition.ZACATEK) && (d.instruktor.ID == disposition.instruktor.ID));
            if(Enumerable.Count(exists) > 0)
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

            if(lesson.delka > 1)
            {
                for (int i = 2; i <= lesson.delka; i++)
                    free = Db.dispozice.AsQueryable().Where(d => (d.ZACATEK.AddHours(1) == lesson.ZACATEK.AddHours(1)) && (d.instruktor == lesson.instruktor));
            }
          
            if(free.Any())
            {
                Db.lekce.Add(lesson);
                DeleteDisposition(free.First());
                //DatabaseModel.SaveChanges();
                return true;
            }
            else return false;
        }

    }
}
