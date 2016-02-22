using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;
using System.Xml.XPath;
using Xceed.Wpf.DataGrid.Converters;

namespace TigersProject.Model
{
    class Database
    {
        public Entities Db;
        public DateTime Date;
        public List<instruktor> Instructors;
        public List<druh> Types;
        public List<jazyk> Languages;  
        public ObservableCollection<MonthRow> MonthTable;
        public ObservableCollection<DayRow> DayTable;
        public DataTable WagesTable;

        public string Text;
        public Database()
        {
            DayTable = new ObservableCollection<DayRow>();
            MonthTable = new ObservableCollection<MonthRow>();
            WagesTable = new DataTable();
            WagesTable.Columns.Add("Instruktor");
            WagesTable.Columns.Add("Sazba");
            WagesTable.Columns.Add("Počet hodin");
            WagesTable.Columns.Add("Výplata");
            Db = new Entities();
            Instructors = Db.instruktor.ToList();
            Types = Db.druh.ToList();
            Languages = Db.jazyk.ToList();
            this.Date = DateTime.Today;
           
            RefreshMonth();
            RefreshDay();
        }
        
        /// <summary>
        /// vytvoří a přidá řádky do tabulky měsíce (rozpis)
        /// </summary>
        public void RefreshMonth()
        {
            MonthTable.Clear();
            Instructors = Db.instruktor.ToList();
            foreach (var instructor in this.Instructors)
            {
                string name = instructor.PRIJMENI + " " + instructor.JMENO;
                MonthRow row = new MonthRow();
                row.Days[0] = name;
                row.Instructor = instructor;
                var dispositions = Db.dispozice.AsQueryable().Where(d => d.instruktor_id == instructor.ID);
                var lessons = Db.lekce.AsQueryable().Where(l => l.instruktor_id == instructor.ID);
                
                foreach (var disposition in dispositions)
                {
                    row.Days[disposition.ZACATEK.Day] = "1";
                }
                foreach (var lesson in lessons)
                {
                    row.Days[lesson.ZACATEK.Day] = "1";
                }
                this.MonthTable.Add(row);
            }
        }
        /// <summary>
        /// obnoví tabulku pro den
        /// </summary>
        /// <summary>
        /// Vytvoří a přidá do DataTable řádky.
        /// pokud není nic buňka je prázdná
        /// pokud je dispozice zapíše 1
        /// pokud je lekce zapíše 2
        /// pokud je klub zapíše 3
        /// ve view se trigerama nastaví barva buněk
        /// </summary>
        public void RefreshDay()
        {
            DayTable = new ObservableCollection<DayRow>();
            string name;
            Instructors = Db.instruktor.ToList();
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
                    DayRow row = new DayRow();
                    //DataRow row = this.DTableDay.NewRow();
                    name = instructor.PRIJMENI + " " + instructor.JMENO;

                    foreach (dispozice disposition in dispositions)
                    {
                        int hour = disposition.ZACATEK.TimeOfDay.Hours;
                        switch (hour)
                        {
                            case 9:
                                if (disposition.KLUB == 1) row.Nine = "3";
                                else row.Nine = "1";
                                break;
                            case 10:
                                if (disposition.KLUB == 1) row.Ten = "3";
                                row.Ten= "1";
                                break;
                            case 11:
                                if (disposition.KLUB == 1) row.Eleven = "3";
                                row.Eleven = "1";
                                break;
                            case 12:
                                if (disposition.KLUB == 1) row.Twelve = "3";
                                row.Twelve = "1";
                                break;
                            case 13:
                                if (disposition.KLUB == 1) row.One = "3";
                                row.One = "1";
                                break;
                            case 14:
                                if (disposition.KLUB == 1) row.Two = "3";
                                row.Two = "1";
                                break;
                            case 15:
                                if (disposition.KLUB == 1) row.Three = "3";
                                row.Three = "1";
                                break;
                        }
                    }

                    foreach (lekce lesson in lessons)
                    {
                        int hour = lesson.ZACATEK.TimeOfDay.Hours;
                        switch (hour)
                        {
                            case 9:
                                row.Nine = "2";
                                row.LessonNine = lesson;
                                break;
                            case 10:
                                row.Ten = "2";
                                row.LessonTen = lesson;
                                break;
                            case 11:
                                row.Eleven = "2";
                                row.LessonEleven = lesson;
                                break;
                            case 12:
                                row.Twelve = "2";
                                row.LessonTwelve = lesson;
                                break;
                            case 13:
                                row.One = "2";
                                row.LessonOne = lesson;
                                break;
                            case 14:
                                row.Two = "2";
                                row.LessonTwo = lesson;
                                break;
                            case 15:
                                row.Three = "2";
                                row.LessonThree = lesson;
                                break;
                        }
                    }
                    row.Instructor = instructor;
                    row.Name = name;
                    
                    this.DayTable.Add(row);
                }
            }
        }
        /// <summary>
        /// Přidává nového instruktora do databáze
        /// </summary>
        /// <param name="instructor">přidávaný instruktor</param>
        /// <returns></returns>
        public bool SaveInstructor(instruktor instructor)
        {
            if(instructor.ID == 0)
            {
                var exists = Db.instruktor.AsQueryable().Where(i => (i.JMENO == instructor.JMENO) && (i.PRIJMENI == instructor.PRIJMENI));
                if(!exists.Any())
                {
                    
                    List<druh> types = new List<druh>();
                    List<jazyk> languages = new List<jazyk>();
                    types = instructor.druh.ToList();
                    languages = instructor.jazyk.ToList();
                    instructor.druh.Clear();
                    instructor.jazyk.Clear();

                    Db.instruktor.Add(instructor);
                    Db.SaveChanges();

                    var editInstructor = Enumerable.FirstOrDefault(Db.instruktor.AsQueryable().Where(i => (i.PRIJMENI == instructor.PRIJMENI)&&(i.JMENO == instructor.JMENO)));
                    editInstructor.druh = types;
                    editInstructor.jazyk = languages;
                    Db.Entry(editInstructor).State = EntityState.Modified;
                    Db.SaveChanges();
                    return true;
                }
                return false;
            }
                Db.SaveChanges();
                return true;
        }

        public void DeleteInstructor(instruktor instructor)
        {
            var areLessons = Db.lekce.AsQueryable().Where(l => l.instruktor_id == instructor.ID);
            var areDispositions = Db.dispozice.AsQueryable().Where(d => d.instruktor_id == instructor.ID);
            /*if (Enumerable.Any(areLessons))
            {
                foreach (lekce lesson in areLessons) { Db.lekce.Remove(lesson); }
            }
            if (Enumerable.Any(areDispositions))
            {
                foreach (dispozice disposition in areDispositions) { Db.dispozice.Remove(disposition); }
            }*/

            Db.instruktor.Remove(instructor);
            Db.SaveChanges();
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
        public void SearchInstructors(DateTime startTime, int duration, jazyk language, druh druh)
        {
            var instructors = Db.instruktor.AsQueryable();
            //kdyz je lekce delsi nez hodinu, tak vybere instruktory, kteri maji volno i tu dalsi hodinu
            //hleda se pouze pro dalsi hodinu (max 2 hodiny za sebou)
            //v pripade že by bylo potřeba 3+h tak se to udela jeste jednou    (DateTime.Compare(d.ZACATEK, lesson.ZACATEK) == 0

            if(startTime.Hour != 0)
            {
                // pokud je cas napr. 8:30, zmeni se na 8:00 a hledaji se dispozice od 8:00
                if(startTime.Minute != 0) startTime = new DateTime(startTime.Year, startTime.Month, startTime.Day, startTime.Hour, 0, 0);
                if(duration == 1) { instructors = from i in instructors from d in i.dispozice where d.ZACATEK == startTime select d.instruktor; }

                else
                {
                    for (int c = 1; c < duration; c++)
                    {
                        startTime = startTime.AddHours(c);
                        instructors = instructors.Where(i => i.dispozice.Any(d => d.ZACATEK == startTime));
                    }
                }
            }
            else
            {
                this.Instructors.Clear();
                return;
            }
            

            if (language != null)instructors = instructors.Where(i => i.jazyk.Any(j => j.ID == language.ID));
            if (druh != null) instructors = instructors.Where(i => i.druh.Any(d => d.ID == druh.ID));
            
            this.Instructors = instructors.ToList();
        }

        public List<lekce> SearchLessons(string surname, string name, string phone, DateTime date)
        {
            
            var lessons = Db.lekce.AsQueryable();
            if(!String.IsNullOrEmpty(surname)) lessons = lessons.Where(l => l.PRIJMENIKLIENT.ToLower().Contains(surname.ToLower()));
            if (!String.IsNullOrEmpty(name)) lessons = lessons.Where(l => l.JMENOKLIENT.ToLower().Contains(name.ToLower()));
            if (!String.IsNullOrEmpty(phone)) lessons = lessons.Where(l => l.TELEFON.Contains(phone));
            if (date.Hour != 0) lessons = lessons.Where(l => DbFunctions.TruncateTime(l.ZACATEK) == DbFunctions.TruncateTime(date));
            return lessons.ToList();
        }
        /// <summary>
        /// Přidává jednu dispozici (jednu hodinu) do tabulky
        /// pokud je čas nastáven na 0h, tak přidá dispozice na celý den
        /// </summary>
        /// <param name="disposition">přidávaná dispozice</param>
        /// <returns></returns>
        public bool AddDisposition(dispozice disposition)
        {
                //pokud chce napsat dispozice na celej den
                //kdyz jsou napsany nejaky lekce, nebo dispozice, tak je to preskoci a doplni celej den dispozicema
            if (disposition.ZACATEK.Hour == 0)
            {
                disposition.ZACATEK = disposition.ZACATEK.AddHours(8);
                for (int i = 0; i < 7; i++)
                {
                    disposition.ZACATEK = disposition.ZACATEK.AddHours(1);
                    if ((!Db.dispozice.Where(d => (d.ZACATEK == disposition.ZACATEK) && (d.instruktor.ID == disposition.instruktor.ID)).Any()) && (!Db.lekce.Where(l => (l.ZACATEK == disposition.ZACATEK) && (l.instruktor.ID == disposition.instruktor.ID)).Any()))
                    {
                        Db.dispozice.Add(disposition);
                        Db.SaveChanges();
                    }
                }
                    
                    return true;
            }
            
            else
            {
                if((!Db.dispozice.Where(d => (d.ZACATEK == disposition.ZACATEK) && (d.instruktor.ID == disposition.instruktor.ID)).Any()) && (!Db.lekce.Where(l => (l.ZACATEK == disposition.ZACATEK) && (l.instruktor.ID == disposition.instruktor.ID)).Any()))
                {
                    Db.dispozice.Add(disposition);
                    Db.SaveChanges();
                    return true;
                }
                else return false;
            }
        }

        public bool DeleteDispositions(instruktor instructor, DateTime date)
        {
            var lessons = Db.lekce.AsQueryable().Where(l => (l.instruktor_id == instructor.ID) && (DbFunctions.TruncateTime(l.ZACATEK) == DbFunctions.TruncateTime(date)));
            if (lessons.Any()) return false;
            var dispositions = Db.dispozice.AsQueryable().Where(d => (d.instruktor_id == instructor.ID)&&(DbFunctions.TruncateTime(d.ZACATEK) == DbFunctions.TruncateTime(date)));
            foreach (dispozice disposition in dispositions) {
                Db.dispozice.Remove(disposition);
            }
            Db.SaveChanges();
            return true;
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
                Db.SaveChanges();
                this.Languages.Add(lang);
                return true;
            }
            else return false;
        }

        public bool DeleteLanguage(jazyk language)
        {
            if((language.instruktor.Any()) || (language.lekce.Any())) return false;
            Db.jazyk.Remove(Db.jazyk.Find(language.ID));
            Db.SaveChanges();
            this.Languages = Db.jazyk.ToList();
            return true;
        }

        //Do DB zapíšu jen jeden řádek (i pro lekci trvající 3h) a při přidávání do DataGridu to rozkopíruju => upravovat se bude vždycky ten jeden záznam v DB
        /// <summary>
        /// Přidává záznam do tabulky lekce v databázi
        /// </summary>
        /// <param name="lesson">lekce, kterou přidáváme</param>
        /// <returns></returns>
        public bool SaveLesson(lekce lesson)
        {
            if(!(lesson.instruktor.druh.Contains(lesson.druh)) || !(lesson.instruktor.jazyk.Contains(lesson.jazyk))) return false;
            if(lesson.ID == 0)
            {
                var free = Db.dispozice.Where(d => (DateTime.Compare(d.ZACATEK, lesson.ZACATEK) == 0) && (d.instruktor.ID == lesson.instruktor.ID));

                if (lesson.DELKA > 1)
                {
                    List<dispozice> dispositions = new List<dispozice>();
                    for (int i = 0; i < lesson.DELKA; i++)
                    {
                        if(!Db.dispozice.Where(d => (DateTime.Compare(d.ZACATEK, lesson.ZACATEK) == 0) && (d.instruktor.ID == lesson.instruktor.ID)).Any()) return false;
                        
                        dispositions.Add(Enumerable.First(Db.dispozice.Where(d => (DateTime.Compare(d.ZACATEK, lesson.ZACATEK) == 0) && (d.instruktor.ID == lesson.instruktor.ID))));
                        lesson.ZACATEK = lesson.ZACATEK.AddHours(1);
                    }
                    //pridavani lekci
                    foreach (dispozice disposition in dispositions)
                    {
                        Db.dispozice.Remove(disposition);
                        Db.SaveChanges();
                        Db.lekce.Add(lesson);
                        lesson.ZACATEK = lesson.ZACATEK.AddHours(1);
                    }
                }

                if(free.Any())
                {
                    Db.dispozice.Remove(Enumerable.First(free));
                    Db.lekce.Add(lesson);
                    Db.SaveChanges();
                    return true;
                }
                return false;
            }

            Db.SaveChanges();
            return true;
        }

        public void DeleteLesson(lekce lesson)
        {
            dispozice disposition = new dispozice();
            disposition.KLUB = 0;
            disposition.ZACATEK = lesson.ZACATEK;
            disposition.instruktor = lesson.instruktor;
            Db.lekce.Remove(lesson);
            Db.SaveChanges();
            AddDisposition(disposition);
            Db.SaveChanges();
        }

        public void RefreshWages()
        {
            WagesTable.Rows.Clear();
            foreach (instruktor instructor in Db.instruktor.AsQueryable())
            {
                var lessons = Db.lekce.AsQueryable().Where(l => (l.instruktor_id == instructor.ID) && (DbFunctions.DiffMonths(l.ZACATEK, Date) == 0));
                DataRow row = WagesTable.NewRow();
                row[0] = instructor.PRIJMENI + " " + instructor.JMENO;
                row[1] = instructor.SAZBA.ToString("F");
                row[2] = Enumerable.Count(lessons).ToString();
                row[3] = (Enumerable.Count(lessons)*instructor.SAZBA).ToString("F");

                WagesTable.Rows.Add(row);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
