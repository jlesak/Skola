using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
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
        public DateTime Date
        {
            get { return DatabaseModel.Date; }
            set
            {
                DatabaseModel.Date = value;
                ChangedProperty("Date");
                DatabaseModel.RefreshDay();
            }
        }
       //------------------------------------------------------------------------------------------------------------------------------//
        public ViewModel()
        {
            this.DatabaseModel = new Model.Database();
            
        }
        //přidá sloupce do rozpisu měsíce
        //provést při změně měsíce
        /*private void AddColumnsMonth()
        {
            int days = DateTime.DaysInMonth(this.date.Year, this.date.Month);
            DataColumn column = new DataColumn();
            column.ColumnName = "Instruktor";
            this.dTableMonth.Columns.Add(column);

            for (int i = 1; i <= days; i++)
            {
                column = new DataColumn();
                column.ColumnName = i.ToString() + ". " + this.date.Month.ToString() + ".";
                this.dTableMonth.Columns.Add(column);
            }
        }*/
        //přidá řádky do datagridu rozpisu z dispozic
        //provést při změně dispozic (přidání, odebrání.. NE přidání lekce) a měsíce
       /* private void AddRowsMonth()
        {
            foreach (var instructor in this.Instructors)
            {
                var row = dTableMonth.NewRow();
                
                string name = instructor.JMENO + " " + instructor.PRIJMENI;
                row["Instruktor"] = name;


                dTableMonth.Rows.Add(row);
            }
        }*/
        
        //provést při změně dne, lekci, dispozic
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
