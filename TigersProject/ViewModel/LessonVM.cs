using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace TigersProject.ViewModel
{
    internal class LessonVM : ViewModel
    {
        private DateTime dateTime;
        private float duration;
        private string surname, name,phone, place, comment;
        private bool paid;
        private int people;
        private druh druh;
        private jazyk language;
        
        public DateTime DateTime
        {
            get { return this.dateTime; }
            set
            {
                this.dateTime = value;
                ChangedProperty("DateTime");
            }
        }

        //vytvořit vlastnost pro DRUH z comboboxu
        public List<druh> Druhy => DatabaseModel.Druhy;

        public druh Druh
        {
            get { return this.druh; }
            set
            {
                this.druh = value;
                ChangedProperty("Druh");
            }
        }
        //vytvořit vlastnost pro JAZYK z comboboxu
        public List<jazyk> Languages => DatabaseModel.Languages;
        public jazyk Language
        {
            get { return this.language; }
            set
            {
                this.language = value;
                ChangedProperty("Druh");
            }
        }
        public float Duration
        {
            get {return this.duration;}
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

        public int People
        {
            get { return this.people; }
            set
            {
                this.people = value;
                ChangedProperty("People");
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

        public bool Paid
        {
            get { return this.paid; }
            set
            {
                this.paid = value;
                ChangedProperty("Paid");
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

        public LessonVM()
        {
            this.paid = false;
            this.dateTime = DateTime.Today;
            this.people = 1;
        }
  
    }
}
