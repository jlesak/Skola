using System;
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
          /*  FbCommand cmd;
            string hlaska;
            FbConnection connection = new FbConnection("User=SYSDBA;Password=masterkey;Database=D:\\DATABASE.fdb;DataSource=localhost;Port=3050;Dialect=3;Charset=NONE;Role=;Connection lifetime=15;Pooling=true;MinPoolSize = 0; MaxPoolSize = 50; Packet Size = 8192; ServerType = 1;");
            connection.Open();
            hlaska = "pripojeno";


            cmd = new FbCommand("INSERT INTO TABULKA(ID, JMENO) VALUES(@id,@name)", connection);
            cmd.Parameters.AddWithValue("id", 3);
            cmd.Parameters.AddWithValue("name", "Jirka");
            cmd.ExecuteNonQuery();
            

            cmd = new FbCommand("SELECT * from TABULKA", connection);
            FbDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    hlaska = hlaska + reader.GetValue(i).ToString();
                }
            }
            vystup.Text = hlaska;*/
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            TigersProject.DataSet1 dataSet1 = ((TigersProject.DataSet1)(this.FindResource("dataSet1")));
            // Load data into the table TABULKA. You can modify this code as needed.
            TigersProject.DataSet1TableAdapters.TABULKATableAdapter dataSet1TABULKATableAdapter = new TigersProject.DataSet1TableAdapters.TABULKATableAdapter();
            dataSet1TABULKATableAdapter.Fill(dataSet1.TABULKA);
            System.Windows.Data.CollectionViewSource tABULKAViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("tABULKAViewSource")));
            tABULKAViewSource.View.MoveCurrentToFirst();
        }
    }
}
