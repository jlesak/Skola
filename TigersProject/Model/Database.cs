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

        /// <summary>
        /// vytvoří a přidá řádky do tabulky měsíce (rozpis)
        /// </summary>
        public void AddMonthRows()
        {
            var dispositions = Db.dispozice.AsQueryable();
            var lessons = Db.lekce.AsQueryable();
            foreach (var instructor in this.Instructors)
            {
                var row = DTableMonth.NewRow();
                string name = instructor.PRIJMENI + " " + instructor.JMENO;
                dispositions = dispositions.Where(d => d.instruktor_id == instructor.ID);
                lessons = lessons.Where(l => l.instruktor_id == instructor.ID);


                foreach (var disposition in dispositions)
                {
                    string rowName = disposition.ZACATEK.Day.ToString() + ". " + disposition.ZACATEK.Month.ToString() + ".";
                    row[rowName] = "1";
                }

                row["Instruktor"] = name;
            }
        }
        /// <summary>
        /// obnoví tabulku pro den
        /// </summary>
        public void RefreshDay()
        {
            DTableDay.Clear();
            AddDayColumns();
            AddDayRows();
        }

        /// <summary>
        /// přidá sloupce do tabulky dne 
        /// </summary>
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
        /// <summary>
        /// Vytvoří a přidá do DataTable řádky.
        /// pokud není nic buňka je prázdná
        /// pokud je dispozice zapíše 1
        /// pokud je lekce zapíše 2
        /// pokud je klub zapíše 3
        /// ve view se trigerama nastaví barva buněk
        /// </summary>
        
        public void AddDayRows()
        {
            string name;

            foreach (instruktor instructor in Instructors)
            {
                var dispositions = Db.dispozice.AsQueryable();
                var lessons = Db.lekce.AsQueryable();
                dispositions = dispositions.Where(d => d.instruktor_id == instructor.ID);
                //mělo by vybrat jen ty dispozice/lekce, kde se POUZE datum (ne čas) shoduje s vybraným datem ve view
                dispositions = dispositions.Where(d => DbFunctions.TruncateTime(d.ZACATEK) == DbFunctions.TruncateTime(this.Date));
                lessons = lessons.Where(l => l.instruktor_id == instructor.ID);
                lessons = lessons.Where(l => DbFunctions.TruncateTime(l.ZACATEK) == DbFunctions.TruncateTime(this.Date));
                //přidá pouze instruktory, kteří jsou daný den v práci
                if ((Enumerable.Any(dispositions)) || (Enumerable.Any(lessons)))
                {
                    var row = this.DTableDay.NewRow();
                    name = instructor.PRIJMENI + " " + instructor.JMENO;

                    foreach (dispozice disposition in dispositions)
                    {
                        int hour = disposition.ZACATEK.TimeOfDay.Hours;
                        switch (hour)
                        {
                            case 9:
                                if (disposition.KLUB == 1) row["9 - 10"] = "3";
                                else row["9 - 10"] = "1";
                                break;
                            case 10:
                                if (disposition.KLUB == 1) row["10 - 11"] = "3";
                                row["10 - 11"] = "1";
                                break;
                            case 11:
                                if (disposition.KLUB == 1) row["11 - 12"] = "3";
                                row["11 - 12"] = "1";
                                break;
                            case 12:
                                if (disposition.KLUB == 1) row["12 - 13"] = "3";
                                row["12 - 13"] = "1";
                                break;
                            case 13:
                                if (disposition.KLUB == 1) row["13 - 14"] = "3";
                                row["13 - 14"] = "1";
                                break;
                            case 14:
                                if (disposition.KLUB == 1) row["14 - 15"] = "3";
                                row["14 - 15"] = "1";
                                break;
                            case 15:
                                if (disposition.KLUB == 1) row["15 - 16"] = "3";
                                row["15 - 16"] = "1";
                                break;
                        }
                    }

                    foreach (lekce lesson in lessons)
                    {
                        int hour = lesson.ZACATEK.TimeOfDay.Hours;
                        switch (hour)
                        {
                            case 9:
                                row["9 - 10"] = "2";
                                break;
                            case 10:
                                row["10 - 11"] = "2";
                                break;
                            case 11:
                                row["11 - 12"] = "2";
                                break;
                            case 12:
                                row["12 - 13"] = "2";
                                break;
                            case 13:
                                row["13 - 14"] = "2";
                                break;
                            case 14:
                                row["14 - 15"] = "2";
                                break;
                            case 15:
                                row["15 - 16"] = "2";
                                break;
                        }
                    }
                    row["Instruktor"] = name;
                    this.DTableDay.Rows.Add(row);
                }
            }
        }
       
        /// <summary>
        /// Přidává nového instruktora do databáze
        /// </summary>
        /// <param name="instructor">přidávaný instruktor</param>
        /// <returns></returns>
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
        /// Vyfiltruje jen ty, kteří mají čas v danou dobu.
        /// Filtruje podle vstupních parametrů.
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
            //instructors = instructors.Where(i => i.disp == startTime);
            /*Vybrat dispozice kde se shoduje čas, pak projet vybrane dispozice a pomoci instructor_id vypsat instruktory...izy*/
            //kdyz je lekce delsi nez hodinu, tak vybere instruktory, kteri maji volno i tu dalsi hodinu
            if (duration > 1)
            {
                for (int t = 1; t <= duration; t++)
                    instructors = instructors.Where(i => i.dispozice1.ZACATEK.AddHours(t) == startTime.AddHours(t));
            }
            if (language != null) instructors = instructors.Where(i => i.jazyk == language);
            if(druh != null) instructors = instructors.Where(i => i.druh == druh);
            if (!string.IsNullOrEmpty(name)) instructors = instructors.Where(i => i.JMENO.Contains(name));
            if (!string.IsNullOrEmpty(surname)) instructors = instructors.Where(i => i.PRIJMENI.Contains(surname));

            if(instructors.Any()) this.Instructors = instructors.ToList();
            else this.Instructors = null;


        }
        /// <summary>
        /// Přidává jednu dispozici (jednu hodinu) do tabulky
        /// </summary>
        /// <param name="disposition">přidávaná dispozice</param>
        /// <returns></returns>
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
        // přepsat databázi aby to dělala sama
        public void DeleteDisposition(dispozice disposition)
        {
            Db.dispozice.Remove(disposition);
            Db.SaveChanges();

        }

        /// <summary>
        /// Přidává nový jazyk do tabulky jazyk v databázi
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
        /// <summary>
        /// Přidává záznam do tabulky lekce v databázi
        /// </summary>
        /// <param name="lesson">lekce, kterou přidáváme</param>
        /// <returns></returns>
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
