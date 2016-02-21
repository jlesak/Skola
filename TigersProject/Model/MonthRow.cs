using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigersProject.Model
{
    class MonthRow
    {
        public instruktor Instructor { get; set; }
        public string[] Days { get; set; }

        public MonthRow()
        {
            Days = new string[32];
        }
    }
}
