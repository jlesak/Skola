﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FirebirdSql.Data.FirebirdClient;

namespace TigersProject.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LessonClick(object sender, RoutedEventArgs e)
        {
            LessonWindow window = new LessonWindow();
            window.Show();
        }
        private void InstructorClick(object sender, RoutedEventArgs e)
        {
            InstructorsWindowxaml window = new InstructorsWindowxaml();
            window.DataContext = this.DataContext;
            window.Show();
        }
        
        private void JazykyClick(object sender, RoutedEventArgs e)
        {
            LanguagesWindow window = new LanguagesWindow();
            window.DataContext = this.DataContext;
            window.Show();
        }

        private void WagesClick(object sender, RoutedEventArgs e)
        {
            WagesWindow window = new WagesWindow();
            window.DataContext = this.DataContext;
            window.Show();
        }
    }
}
