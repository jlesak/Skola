using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Security.Policy;
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
        
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
