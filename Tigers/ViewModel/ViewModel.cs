using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tigers.ViewModel
{
    class ViewModel : INotifyPropertyChanged
    {
        public Model.Model Model;


        public ViewModel()
        {
            this.Model = new Model.Model();

        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void ChangedProperty(string propertyName)
        {
            if (PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
