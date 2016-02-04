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
using System.Windows.Media.TextFormatting;
using TigersProject.Model;
using System.Windows.Navigation;

namespace TigersProject.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model.Database DatabaseModel;

        public List<instruktor> Instructors => DatabaseModel.Instructors;
      
        //DataGrid pro rozpis
        public DataTable DTableMonth => this.DatabaseModel.DTableMonth;

        //DataGrid pro den
        public DataTable DTableDay => this.DatabaseModel.DTableDay;
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
        private DayRow selectedRow;
        public DayRow SelectedRow
        {
            get { return this.selectedRow; }
            set
            {
                this.selectedRow = value;
                ChangedProperty("SelectedRow");
            }
        }
        public Command ClickCommand { get; set; }
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
        public Command AddDispositionCmd { get; set; }
        
        //____________________________________________________________________________________________
        public ViewModel()
        {
            this.DatabaseModel = new Model.Database();
            ClickCommand = new Command(() => MessageBox.Show("ahoooooj"));
            AddDispositionCmd = new Command(AddDisposition, () =>
            {
                if(this.DispositionInstructor != null) return true;
                else return false;
            });
            this.DispositionDate = this.DatabaseModel.Date;
        }

        private void CellStyle()
        {
            foreach (DataRow row in DTableDay.Rows) {
                
            }
        }
        //okno s přidáváním dispozic----------------------------------------------------------------------------
        public void AddDisposition()
        {
            dispozice disposition = new dispozice();
            disposition.KLUB = 0;
            disposition.ZACATEK = DispositionDate;
            disposition.instruktor = DispositionInstructor;
            if (DatabaseModel.AddDisposition(disposition)) MessageBox.Show("Dispozice přidána");
            else MessageBox.Show("Dispozice, nebo lekce již existuje.");
            DatabaseModel.RefreshDay();
            ChangedProperty("DayTable");
        }


        //okno lekce -------------------------------------------------------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
