
using DataStorageAndProcessing.Data;
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
//using DataStorageAndProcessing.Data.Migrations;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Diagnostics;
using DataStorageAndProcessing.Data.Migrations;

namespace DataStorageAndProcessing.UI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            selectyearBox.SelectedIndex = 0;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (var contex = new Context())
            {

                //contex.Institutions.ToList();
                //contex.Database.Initialize(true);
                //contex.SaveChanges();
                Configuration configuration = new Configuration();
                configuration.ContextType = typeof(Context);
                var migrator = new DbMigrator(configuration);

                //This will get the SQL script which will update the DB and write it to debug
                var scriptor = new MigratorScriptingDecorator(migrator);
                string script = scriptor.ScriptUpdate(sourceMigration: null, targetMigration: null).ToString();
                Debug.Write(script);

                ////This will run the migration update script and will run Seed() method
                migrator.Update();

            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                context.Database.Delete();
            }
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            int[] scores = new int[] { 97, 92, 81, 60 };

            using (var context = new Context())
            {
                int tecyear = 2012 + selectyearBox.SelectedIndex;
                var seq = from t1 in context.Raitings
                          where t1.Year == tecyear 
                          select t1;
                IEnumerable<Repository.YearRait> sequence = from t1 in seq
                                                     join t2 in context.InstiutionRaitings on t1.Id equals t2.RaitingID
                                                     join t3 in context.Institutions on t2.InstitutionID equals t3.Id
                                                     join t4 in context.Locations on t3.LocationID equals t4.Id
                                                     select new Repository.YearRait
                                                     {
                                                         WordRank = t2.WordRank,
                                                         Institution = t3.Name,
                                                         Location = t4.CountryName,
                                                         NationalRank = t2.NationalRank,
                                                         QualityofEducation = t2.QualityofEducation,
                                                         AlumniEmployment = t2.AlumniEmployment,
                                                         QualityofFaculty = t2.QualityofFaculty,
                                                         Publications = t2.Publications,
                                                         Influence = t2.Influence,
                                                         Citations = t2.Citations,
                                                         BroadImpact = t2.BroadImpact,
                                                         Patents = t2.Patents,
                                                         Score = t2.Score
                                                     };
                //dataGrid.ItemsSource = sequence;
                foreach (Repository.YearRait tec in sequence)
                {
                    dataGrid.Items.Add(tec);
                }
            }
        }
    }
}
