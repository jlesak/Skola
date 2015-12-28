using System;
using System.Collections.Generic;
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
        public instruktor Instructor
        {get; set; }

        public Command AddCmd { get; set; }

        public DispositionVM()
        {
            AddCmd = new Command(AddDisposition, CExecute);
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
            DatabaseModel.AddDisposition(disposition);
            MessageBox.Show("Dispozice přidána");

        }
    }
}
