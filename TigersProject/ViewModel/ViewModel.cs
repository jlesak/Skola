﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;
using TigersProject.Model;
using System.Windows.Navigation;
using TigersProject.View;

namespace TigersProject.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model.Database DatabaseModel;
        private DataGridCellInfo cellDay;
        private DataGridCellInfo cellMonth;

        private instruktor instructor;
        public instruktor Instructor
        {
            get { return this.instructor; }
            set
            {
                this.instructor = value;
                if(value != null)
                {
                    if(value.druh.Any())this.type = value.druh.First();
                    if (value.jazyk.Any()) this.language = value.jazyk.First();
                    ChangedProperty("Type");
                    ChangedProperty("Language");
                }
                ChangedProperty("Instructor");
            }
        }
        public List<instruktor> Instructors {get { return DatabaseModel.Instructors; } }
        public List<instruktor> DbInstructors => DatabaseModel.Db.instruktor.ToList(); 

        //DataGrid pro rozpis
        public ObservableCollection<MonthRow> MonthTable => this.DatabaseModel.MonthTable;

        //DataGrid pro den
        public ObservableCollection<DayRow> DayTable => DatabaseModel.DayTable;
        public DateTime Date
        {
            get { return DatabaseModel.Date; }
            set
            {
                if(value.Month != DatabaseModel.Date.Month)
                {
                    DatabaseModel.RefreshMonth();
                    ChangedProperty("MonthTable");
                }
                DatabaseModel.Date = value;
                ChangedProperty("Date");
                DatabaseModel.RefreshDay();
                ChangedProperty("DayTable");
            }
        }
        public DataGridCellInfo CellDay
        {
            get { return cellDay; }
            set
            {
                cellDay = value;
                
                ChangedProperty("CellDay");
                if((value.IsValid)&&(value.Column.DisplayIndex > 0))
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

        public DataGridCellInfo CellMonth
        {
            get { return this.cellMonth; }
            set
            {
                this.cellMonth = value;
                string content = "";

                if(value.IsValid && value.Column.DisplayIndex != 0)
                {
                    this.DispositionDate = new DateTime(Date.Year, Date.Month, value.Column.DisplayIndex);
                    this.DispositionInstructor = ((MonthRow) value.Item).Instructor;
                    content = ((MonthRow)value.Item).Days[value.Column.DisplayIndex];
                    if (content == "1")
                    {
                        //smaze dispozice pro dany den, pokud tam nejsou lekce
                        if (!DatabaseModel.DeleteDispositions(this.DispositionInstructor, this.dispositionDate)) MessageBox.Show("Instruktor má v tento den nějaké lekce.");
                    }
                    else if (String.IsNullOrEmpty(content))
                    {
                        //prida dispozice pro cely den
                        AddDisposition();
                    }
                }
                DatabaseModel.RefreshMonth();
                ChangedProperty("CellMonth");
                ChangedProperty("MonthTable");

            }
        }

        //přidávání dispozic---------------------------------------------------------------
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
        public List<jazyk> Languages {get { return DatabaseModel.Languages; } }
       
        public List<lekce> Lessons { get; set; }
        public lekce Lesson
        {
            get { return this.lesson; }
            set
            {
                this.lesson = value;
                ChangedProperty("Lesson");
                if(value != null)
                {
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
                    this.Duration = value.DELKA;
                }
            }
        }
        public DateTime BeginTime
        {
            get { return this.beginTime; }
            set
            {
                this.beginTime = value;
                if (value.Hour != 0) SearchInstructors();
                ChangedProperty("BeginTime");
            }
        }
        public int Duration
        {
            get { return this.duration; }
            set
            {
                this.duration = value;
                SearchInstructors();
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
                if(value != null)SearchInstructors();
                ChangedProperty("Type");
            }
        }

        public jazyk Language
        {
            get { return this.language; }
            set
            {
                this.language = value;
                if (value != null) SearchInstructors();
                ChangedProperty("Language");
            }
        }
        //okno instruktoři
        private float money;
        private List<druh> selectedTypes;
        private List<jazyk> selectedLanguages;
        private instruktor editInstructor;
        public instruktor EditInstructor
        {
            get { return this.editInstructor; }
            set
            {
                if(value != null)
                {
                    this.editInstructor = value;
                    ChangedProperty("EditInstructor");
                    this.Name = value.JMENO;
                    this.Surname = value.PRIJMENI;
                    this.Money = value.SAZBA;
                    this.Phone = value.TELEFON;

                    this.SelectedLanguages = value.jazyk.ToList();
                    this.SelectedTypes = value.druh.ToList();
                }
                else ResetAttributes();
            }
        }
        public float Money
        {
            get { return this.money; }
            set
            {
                this.money = value;
                ChangedProperty("Money");
            }
        }
        public Command DeleteInstructorCmd { get; set; }
        public Command NewCmd { get; set; }
        public Command SaveInstructorCmd { get; set; }
        public List<druh> SelectedTypes
        {
            get { return this.selectedTypes; }
            set
            {
                this.selectedTypes = value;
                ChangedProperty("SelectedTypes");
            }
        }
        public List<jazyk> SelectedLanguages
        {
            get { return this.selectedLanguages; }
            set
            {
                this.selectedLanguages = value;
                ChangedProperty("SelectedLanguages");
            }
        }
        //okno JAZYKY-------------------------------------------------------------------
        private string shortLang;
        public string ShortLang
        {
            get { return this.shortLang; }
            set
            {
                this.shortLang = value;
                ChangedProperty("ShortLang");
            }
        }
        public Command AddLanguageCmd { get; set; }
        public Command DelLanguageCmd { get; set; }
        
        //okno VYPLATY--------------------------------------------------------------------------
        public DataTable WagesTable => DatabaseModel.WagesTable;
        public Command RefreshWagesCmd { get; set; }
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
            SaveInstructorCmd = new Command(SaveInstructor, CanSaveInstructor);
            DeleteInstructorCmd = new Command(DeleteInstructor, () =>
            {
                if(this.editInstructor.ID != 0) return true;
                else return false;
            });
            NewCmd = new Command(ResetAttributes);
            AddLanguageCmd = new Command(AddLanguage,CanAddLanguage);
            DelLanguageCmd = new Command(DeleteLanguage, () =>
            {
                if(this.language != null) return true;
                else return false;
            });
            RefreshWagesCmd = new Command(RefreshWages);


            this.Lessons = new List<lekce>();
            SelectedTypes = new List<druh>();
            SelectedLanguages = new List<jazyk>();
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

                DatabaseModel.SearchInstructors(begin, 1, null, null);
                this.Instructors.Add(instructor);
            }
            this.Instructor = instructor;
            this.beginTime = begin;
            SearchInstructors();
            
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
            if (!DatabaseModel.AddDisposition(disposition)) MessageBox.Show("Dispozice, nebo lekce již existuje.");

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
            if(this.beginTime.Minute != 0)
            {
                MessageBox.Show("Nastavte začátek lekce na celou hodinu.");
                return;
            }
            if(this.lesson == null)this.lesson = new lekce();
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

            if(this.DatabaseModel.SaveLesson(lesson))
            {
                MessageBox.Show("Lekce uložena.");
                DatabaseModel.RefreshDay();
                ChangedProperty("DayTable");
                ResetAttributes();
            }
            else MessageBox.Show("Lekce nebyla přidána, protože instruktor nemá vybraný druh, nebo jazyk.");
        }
        private void SearchInstructors()
        {
            DatabaseModel.SearchInstructors(this.beginTime, this.duration,this.language, this.type);
            if(this.Instructors.Count == 0)
            {
                this.Instructor = null;
                this.Language = null;
                this.Type = null;
            }
            ChangedProperty("Instructors");
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
               && (this.instructor != null)
               && (this.language != null)
               && (this.type != null)
               && (this.beginTime.Hour < 16)) return true;
            else return false;
        }

        //okno INSTRUKTOR---------------------------------------------------------------------------
        private void DeleteInstructor()
        {
            MessageBoxResult result = MessageBox.Show("Opravdu chcete smazat instruktora a všechny jeho záznamy?", "Tigers", MessageBoxButton.YesNo);
            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (DatabaseModel.DeleteInstructor(this.editInstructor)) MessageBox.Show("Instruktor byl smazán.");
                    else MessageBox.Show("Instruktor má nějaké lekce.");
                    break;
                case MessageBoxResult.No:
                    break;
            }
            ResetAttributes();
            ChangedProperty("Instructors");
            try {
                EditInstructor = Instructors.First();
            }
            catch(Exception e) {
            }
        }
        private void SaveInstructor()
        {
           if(this.editInstructor == null) this.editInstructor = new instruktor();
            editInstructor.JMENO = this.name;
            editInstructor.PRIJMENI = this.surname;
            editInstructor.SAZBA = this.money;
            editInstructor.TELEFON = this.phone;

            editInstructor.druh = this.SelectedTypes;
            editInstructor.jazyk = this.SelectedLanguages;

            if(DatabaseModel.SaveInstructor(this.editInstructor))
            {
                MessageBox.Show("Instruktor uložen.");
                ResetAttributes();
                ChangedProperty("DbInstructors");
            }
            else
            { MessageBox.Show("Instruktor nemohl být uložen."); }
        }
        private bool CanSaveInstructor()
        {
            if((!String.IsNullOrEmpty(this.name))
               && (!String.IsNullOrEmpty(this.surname))
               && (!String.IsNullOrEmpty(this.phone))
               && (this.money > 0)
               &&(this.selectedLanguages.Count > 0)
               &&(this.selectedTypes.Count > 0)) return true;
            else return false;
        }
        
        //okno JAZYKY---------------------------------------------------------------------------------
        private void AddLanguage()
        {
            DatabaseModel.AddLanguage(this.shortLang);
            ChangedProperty("Languages");
            ResetAttributes();
        }
        private void DeleteLanguage()
        {
            if(!DatabaseModel.DeleteLanguage(this.language)) MessageBox.Show("Některý z instruktorů mluví tímto jazykem, tudíž nemůže být smazán.");
            ChangedProperty("Languages");
            this.Language = null;
        }
        private bool CanAddLanguage()
        {
            if ((!String.IsNullOrEmpty(this.shortLang)) && (this.shortLang.Length >= 2) && (this.shortLang.Length <= 5)) return true;
            else return false;
        }
        //-okno VYPLATY------------------------------------------------------------------
        private void RefreshWages()
        {
            DatabaseModel.RefreshWages();
            ChangedProperty("WagesTable");
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
            this.Type = null;
            this.Language = null;
            this.Phone = this.Place = this.Name = this.Surname = null;
            this.Paid = false;
            this.Money = 0;
            this.EditInstructor = new instruktor();
            this.Money = 0;
            this.SelectedLanguages.Clear();
            this.SelectedTypes.Clear();
            this.Lessons = null;
            ChangedProperty("Lessons");
            this.Lesson = null;
        }
        public void Mouse(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show(CellDay.Column.DisplayIndex.ToString());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
