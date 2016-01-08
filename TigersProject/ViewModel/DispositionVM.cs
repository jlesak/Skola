using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.DataGrid.Converters;

namespace TigersProject.ViewModel
{
    class DispositionVM : ViewModel
    {
        private DateTime date;
        public DateTime Date
        {
            get { return this.date; }
            set
            {
                this.date = value;
                ChangedProperty("Date");
            }
        }
        public instruktor Instructor
        {get; set; }

        public Command AddCmd { get; set; }


        public DispositionVM()
        {
            AddCmd = new Command(AddDisposition, CExecute);
            this.date = DateTime.Today;


        }

        private bool CExecute()
        {
            if(this.Instructor != null) return true;
            else return false;
        }
        public void AddDisposition()
        {
            dispozice disposition = new dispozice();
            disposition.KLUB = 0;
            disposition.ZACATEK = Date;
            disposition.instruktor = Instructor;
            if(DatabaseModel.AddDisposition(disposition)) MessageBox.Show("Dispozice přidána");
            else MessageBox.Show("Dispozice, nebo lekce již existuje.");
        }
    }
}
