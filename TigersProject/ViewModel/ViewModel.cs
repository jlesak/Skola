using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace TigersProject.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model.Model Model;

        private DateTime date;
        public DateTime Date
        {
            get
            {
                return this.date;
            }
            set { this.date = value; }
        }
       
        public ViewModel()
        {
            this.Model = new Model.Model();
            date = DateTime.Today;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
