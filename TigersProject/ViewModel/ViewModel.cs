using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Windows;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.TextFormatting;
using TigersProject.Model;
using System.Windows.Navigation;
using TigersProject.View;

namespace TigersProject.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model.Database DatabaseModel;
        private DataGridCellInfo cellInfo;

        private instruktor instructor;
        public instruktor Instructor
        {
            get { return this.instructor; }
            set
            {
                this.instructor = value;
                ChangedProperty("Instructor");
            }
        }
        public List<instruktor> Instructors => DatabaseModel.Instructors;
      
        //DataGrid pro rozpis
        public DataTable DTableMonth => this.DatabaseModel.DTableMonth;

        //DataGrid pro den
        public ObservableCollection<DayRow> DayTable => DatabaseModel.DayTable;
        public DateTime Date
        {
            get { return DatabaseModel.Date; }
            set
            {
                DatabaseModel.Date = value;
                ChangedProperty("Date");
                DatabaseModel.RefreshDay();
                ChangedProperty("DayTable");
            }
        }
        public DataGridCellInfo CellInfo
        {
            get { return cellInfo; }
            set
            {
                cellInfo = value;
                
                ChangedProperty("CellInfo");
                if(value.IsValid)
                {
                    string clientName = "";
                    bool isLesson = false;
                    switch (value.Column.DisplayIndex) {
                        case 1:
                        {
                            if(((DayRow) value.Item).Nine == "2")
                            {
                                clientName = ((DayRow)value.Item).LessonNine.PRIJMENIKLIENT;
                                isLesson = true;
                            }
                                break;
                        }
                        case 2:
                            {
                                if (((DayRow)value.Item).Ten == "2")
                                {
                                    clientName = ((DayRow)value.Item).LessonTen.PRIJMENIKLIENT;
                                    isLesson = true;
                                }
                                break;
                            }
                        case 3:
                            {
                                if (((DayRow)value.Item).Eleven == "2")
                                {
                                    clientName = ((DayRow)value.Item).LessonEleven.PRIJMENIKLIENT;
                                    isLesson = true;
                                }
                                break;
                            }
                        case 4:
                            {

                                if (((DayRow)value.Item).Twelve == "2")
                                {
                                    clientName = ((DayRow)value.Item).LessonTwelve.PRIJMENIKLIENT;
                                    isLesson = true;
                                }
                                break;
                            }
                        case 5:
                            {

                                if (((DayRow)value.Item).One == "2")
                                {
                                    clientName = ((DayRow)value.Item).LessonOne.PRIJMENIKLIENT;
                                    isLesson = true;
                                }
                                break;
                            }
                        case 6:
                            {

                                if (((DayRow)value.Item).Two == "2")
                                {
                                    clientName = ((DayRow)value.Item).LessonTwo.PRIJMENIKLIENT;
                                    isLesson = true;
                                }
                                break;
                            }
                        case 7:
                            {

                                if (((DayRow)value.Item).Three == "2")
                                {
                                    clientName = ((DayRow)value.Item).LessonThree.PRIJMENIKLIENT;
                                    isLesson = true;
                                }
                                break;
                            }
                    }
                    DateTime begin = Date.AddHours(8 + value.Column.DisplayIndex);
                    instruktor instructor = ((DayRow)value.Item).Instructor;
                    CellClick(instructor, begin, clientName, isLesson);
                }
            }
        }
        //Okno s přidáváním dispozic---------------------------------------------------------------
        private DateTime dispositionDate;
        public DateTime DispositionDate
        {
            get { return this.dispositionDate; }
            set
            {
                this.dispositionDate = value;
                ChangedProperty("DispositionDate");
            }
        }
        public instruktor DispositionInstructor { get; set; }
        public List<instruktor> DispositionInstructors => DatabaseModel.Db.instruktor.ToList();
        public Command AddDispositionCmd { get; set; }
        
        //Okno s přidáním lekce---------------------------------------------------------------------
        private DateTime beginTime;
        private int duration;
        private string surname, name, phone, place, comment;
        private bool paid;
        private int people;
        private druh type;
        private jazyk language;
        private lekce lesson;

        public Command AddLessonCmd { get; set; }
        public Command SearchLessonCmd { get; set; }
        public Command DeleteLessonCmd { get; set; }
        public List<druh> Types => DatabaseModel.Types;
        public List<jazyk> Languages => DatabaseModel.Languages;
        public List<lekce> Lessons { get; set; }
        public lekce Lesson
        {
            get { return this.lesson; }
            set
            {
                this.lesson = value;
                ChangedProperty("Lesson");
                this.Surname = value.PRIJMENIKLIENT;
                this.Name = value.JMENOKLIENT;
                this.Phone = value.TELEFON;
                this.People = value.OSOB;
                this.Place = value.MISTO;
                this.Paid = Convert.ToBoolean(value.PLACENO);
                this.Comment = value.POZNAMKA;
                this.Instructor = value.instruktor;
                this.Type = value.druh;
                this.Language = value.jazyk;
                this.BeginTime = value.ZACATEK;
            }
        }
        public DateTime BeginTime
        {
            get { return this.beginTime; }
            set
            {
                this.beginTime = value;
                ChangedProperty("BeginTime");
            }
        }
        public int Duration
        {
            get { return this.duration; }
            set
            {
                this.duration = value;
                ChangedProperty("Duration");
            }
        }

        public string Surname
        {
            get { return this.surname; }
            set
            {
                this.surname = value;
                ChangedProperty("Surname");
            }
        }
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                ChangedProperty("Name");
            }
        }
        public string Phone
        {
            get { return this.phone; }
            set
            {
                this.phone = value;
                ChangedProperty("Phone");
            }
        }
        public string Place
        {
            get { return this.place; }
            set
            {
                this.place = value;
                ChangedProperty("Place");
            }
        }
        public string Comment
        {
            get { return this.comment; }
            set
            {
                this.comment = value;
                ChangedProperty("Comment");
            }
        }
        public bool Paid
        {
            get { return this.paid; }
            set
            {
                this.paid = value;
                ChangedProperty("Paid");
            }
        }
        public int People
        {
            get { return this.people; }
            set
            {
                this.people = value;
                ChangedProperty("People");
            }
        }

        public druh Type
        {
            get { return this.type; }
            set
            {
                this.type = value;
                ChangedProperty("Type");
            }
        }

        public jazyk Language
        {
            get { return this.language; }
            set
            {
                this.language = value; 
                ChangedProperty("Language");
            }
        }
        //____________________________________________________________________________________________
        public ViewModel()
        {
            this.DatabaseModel = new Model.Database();
            AddDispositionCmd = new Command(AddDisposition, () =>
            {
                if(this.DispositionInstructor != null) return true;
                else return false;
            });
            AddLessonCmd = new Command(SaveLesson, CanSaveLesson);
            DeleteLessonCmd = new Command(DeleteLesson, () =>
            {
                if(this.lesson != null) return true;
                else return false;
            });
            SearchLessonCmd = new Command(SearchLesson, CanSearchLesson);

            this.Lessons = new List<lekce>();
            this.DispositionDate = this.DatabaseModel.Date;
            ResetAttributes();
        }
        /// <summary>
        /// Provede se při kliknutí na buňku v dataGridu.
        /// Otevře nové okno pro přidání Lekce s předvyplněnými informacemi
        /// </summary>
        /// <param name="instructor">Instruktor</param>
        /// <param name="begin">Začátek výuky</param>
        private void CellClick(instruktor instructor, DateTime begin, string surname, bool isLesson)
        {

            ResetAttributes();
            if(isLesson)
            {
                lekce lesson = this.DatabaseModel.SearchLessons(surname, this.name, this.phone, begin).FirstOrDefault();
                this.Lesson = lesson;

                DatabaseModel.SearchInstructors(begin, 1, null, null, null, null);
                this.Instructors.Add(instructor);
                /* this.surname = lesson.PRIJMENIKLIENT;
                this.name = lesson.JMENOKLIENT;
                this.phone = lesson.TELEFON;
                this.place = lesson.MISTO;
                this.people = lesson.OSOB;
                this.paid = Convert.ToBoolean(lesson.PLACENO);
                this.comment = lesson.POZNAMKA;
                this.type = lesson.druh;
                this.language = lesson.jazyk;*/
            }
            this.instructor = instructor;
            this.beginTime = begin;
            
            LessonWindow window = new LessonWindow();
            window.DataContext = this;
            window.Show();
        }

        //okno s přidáváním dispozic----------------------------------------------------------------------------
        /// <summary>
        /// Vytvoří objekt dispozice a předá ho modelu pro přidání dispozice do záznamu
        /// </summary>
        private void AddDisposition()
        {
            dispozice disposition = new dispozice();
            disposition.KLUB = 0;
            disposition.ZACATEK = DispositionDate;
            disposition.instruktor = DispositionInstructor;
            if (DatabaseModel.AddDisposition(disposition)) MessageBox.Show("Dispozice přidána");
            else MessageBox.Show("Dispozice, nebo lekce již existuje.");

            DatabaseModel.RefreshDay();
            ChangedProperty("DayTable");
            ResetAttributes();
        }
        //okno lekce -------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Vytvoří objekt lekce, přidá mu parametry a předá modelu pro přidání lekce do databáze
        /// </summary>
        private void SaveLesson()
        {
            this.lesson = new lekce();
            this.lesson.JMENOKLIENT = this.name;
            this.lesson.PRIJMENIKLIENT = this.surname;
            this.lesson.TELEFON = this.phone;
            this.lesson.MISTO = this.place;
            this.lesson.OSOB = this.people;
            this.lesson.PLACENO = Convert.ToInt16(this.paid);
            this.lesson.POZNAMKA = this.comment;
            this.lesson.ZACATEK = this.beginTime;
            this.lesson.instruktor = this.instructor;
            this.lesson.jazyk = this.language;
            this.lesson.druh = this.type;
            this.lesson.DELKA = (short)this.duration;

            if(this.DatabaseModel.AddLesson(lesson))
            {
                MessageBox.Show("Lekce uložena.");
                DatabaseModel.RefreshDay();
                ChangedProperty("DayTable");
                ResetAttributes();
            }
            else MessageBox.Show("Lekce nemohla být přidána");
        }

        private void SearchLesson()
        {
            this.Lessons = DatabaseModel.SearchLessons(this.surname, this.name, this.phone, this.beginTime);
            ChangedProperty("Lessons");
        }

        private void DeleteLesson()
        {
            MessageBoxResult result = MessageBox.Show("Opravdu chcete smazat lekci?", "Tigers", MessageBoxButton.YesNo);
            switch (result) {
                case MessageBoxResult.Yes:
                    DatabaseModel.DeleteLesson(this.lesson);
                    MessageBox.Show("Lekce byla smazána.");
                    break;
                case MessageBoxResult.No:
                    break;
            }
            DatabaseModel.RefreshDay();
            ChangedProperty("DayTable");
            ResetAttributes();
            this.lesson = null;
            this.Lessons = null;
            ChangedProperty("Lesson");
            ChangedProperty("Lessons");
        }
        private bool CanSearchLesson()
        {
            if ((!String.IsNullOrEmpty(this.surname))
               || (!String.IsNullOrEmpty(this.name))
               || (!String.IsNullOrEmpty(this.phone)))
                return true;
            else return false;
        }
        /// <summary>
        /// Pokud jsou vyplněny všechny potřebné parametry, může uživatel přidat lekci
        /// </summary>
        /// <returns></returns>
        private bool CanSaveLesson()
        {
            if((!String.IsNullOrEmpty(this.surname))
               && (!String.IsNullOrEmpty(this.phone))
               && (this.beginTime.Hour > 8)
               && (this.beginTime.Hour < 16)) return true;
            else return false;
        }
        
        /// <summary>
        /// Vyresetuje všechny atributy ve ViewModelu
        /// </summary>
        private void ResetAttributes()
        {
            this.BeginTime = this.Date;
            this.DispositionDate = this.Date;
            this.People = this.Duration = 1;
            this.Instructor = null;
            this.Type = Types.Find(d => d.ID == 0);
            this.Language = Languages.Find(d => d.ID == 0);
            this.Phone = this.Place = this.Name = this.Surname = null;
            this.Paid = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
