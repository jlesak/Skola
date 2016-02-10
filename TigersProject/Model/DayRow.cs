using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace TigersProject.Model
{
    public class DayRow
    {
        public instruktor Instructor { get; set; }
        public string Name { get; set; }
        public string Nine { get; set; }
        public lekce LessonNine { get; set; }
        public string Ten { get; set; }
        public lekce LessonTen { get; set; }
        public string Eleven { get; set; }
        public lekce LessonEleven { get; set; }
        public string Twelve { get; set; }
        public lekce LessonTwelve { get; set; }
        public string One { get; set; }
        public lekce LessonOne { get; set; }
        public string Two { get; set; }
        public lekce LessonTwo { get; set; }
        public string Three { get; set; }
        public lekce LessonThree { get; set; }
        public DayRow() {}
    }
}
