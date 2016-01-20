using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using Xceed.Wpf.DataGrid.Converters;

namespace TigersProject.Model
{
    class Database
    {
        public Entities Db;
        public DateTime Date;
        public List<instruktor> Instructors;
        public List<druh> Druhy;
        public List<jazyk> Languages;  
        public DataTable DTableMonth;
        public DataTable DTableDay;
        public DataGrid d;

        public Database()
        {
           
            
            Db = new Entities();
            Instructors = Db.instruktor.ToList();
            Druhy = Db.druh.ToList();
            Languages = Db.jazyk.ToList();
            this.DTableDay = new DataTable();
            this.DTableMonth = new DataTable();
            this.Date = DateTime.Today;
            RefreshDay();
          

            
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

        public void RefreshDay()
        {
            DTableDay.Clear();
            AddDayColumns();
            AddDayRows(this.Date);
        }
        //???predelat jen na 1 instruktora?? - pro lepsi aktualizaci tabulky
        public void AddMonthRows()
        {
            var dispositions = Db.dispozice.AsQueryable();
            string name;
            foreach (var instructor in this.Instructors)
            {
                var row = DTableMonth.NewRow();
                name = instructor.PRIJMENI + " " + instructor.JMENO;
                dispositions = dispositions.Where(d => d.instruktor.ID == instructor.ID);

                //vypsani dispozic do 'row', ??přidat barvu buňky??...
                foreach (var disposition in dispositions)
                {
                    //tohle musim domyslet
                    //dodělat podle rozhodnutí o barevnosti. viz papír 1
                }

                row["Instruktor"] = name;
            }
        }

        //přidávají se i "statický" sloupce hodin, protože předávám celej datagrid z modelu do VM/V
        //?????Dá se nějak z view předat datatable do modelu, tady to upravit a předat zpět do view?????
        public void AddDayColumns()
        {DataColumn column = new DataColumn();
            column.ColumnName = "Instruktor";
            this.DTableDay.Columns.Add(column);
            for (int i = 9; i <= 15; i++)
            {
                int y = i + 1;
                column = new DataColumn();
                column.ColumnName = i.ToString() + " - " + y.ToString();
                
                DTableDay.Columns.Add(column);
            }
        }
        public void AddDayRows(DateTime date)
        {
            string name;

            foreach (instruktor instructor in Instructors)
            {
                var dispositions = Db.dispozice.AsQueryable();
                var row = this.DTableDay.NewRow();
                name = instructor.PRIJMENI + " " + instructor.JMENO;
                dispositions = dispositions.Where(d => d.instruktor.ID == instructor.ID);
                dispositions = dispositions.Where(d => DbFunctions.TruncateTime(d.ZACATEK) == DbFunctions.TruncateTime(date));
                foreach (dispozice disposition in dispositions)
                {
                    int hour = disposition.ZACATEK.TimeOfDay.Hours;
                    //pokud je dispozice, tak do bunky zapisu "1"
                    //???co tam psát??? jak měnit barvu??
                    switch (hour) {
                        case 9:
                            row["9 - 10"] = "1";
                            break;
                        case 10:
                            row["10 - 11"] = "1";
                            break;
                        case 11:
                            row["11 - 12"] = "1";
                            break;
                        case 12:
                            row["12 - 13"] = "1";
                            break;
                        case 13:
                            row["13 - 14"] = "1";
                            break;
                        case 14:
                            row["14 - 15"] = "1";
                            break;
                        case 15:
                            row["15 - 16"] = "1";
                            break;
                    }
                }
                row["Instruktor"] = name;
                this.DTableDay.Rows.Add(row);
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
                for (int t = 1; t <= duration; t++)
                    instructors = instructors.Where(i => i.dispozice.ZACATEK.AddHours(t) == startTime.AddHours(t));
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
           
            var exists = Db.dispozice.Where(d => (d.ZACATEK == disposition.ZACATEK) && (d.instruktor.ID == disposition.instruktor.ID));
            
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

            if(lesson.DELKA > 1)
            {
                for (int i = 2; i <= lesson.DELKA; i++)
                    free = Db.dispozice.AsQueryable().Where(d => (d.ZACATEK.AddHours(1) == lesson.ZACATEK.AddHours(1)) && (d.instruktor == lesson.instruktor));
            }
          
            if(free.Any())
            {
                Db.lekce.Add(lesson);
                DeleteDisposition(free.First());
                Db.SaveChanges();
                return true;
            }
            else return false;
        }

    }
}
