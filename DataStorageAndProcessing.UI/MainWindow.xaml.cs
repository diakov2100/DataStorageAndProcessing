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
            using (var context = new Context())
            {
                UnivList.ItemsSource = context.Institutions.Select(x => x.Name).ToList();
            }
            UnivList.SelectedIndex = 0;
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
            using (var context = new Context())
            {
                IEnumerable<Repository.YearRait> sequence = from t1 in context.Raitings
                                                            where t1.Year == 2012 + selectyearBox.SelectedIndex
                                                            join t2 in context.InstiutionRaitings on t1.Id equals t2.RaitingID
                                                            join t3 in context.Institutions on t2.InstitutionID equals t3.Id
                                                            join t4 in context.Locations on t3.LocationID equals t4.Id
                                                            orderby t2.WordRank
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
                Data.ItemsSource = sequence.ToList();
            }
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                var sequence1 = from t1 in context.Raitings
                                where t1.Year == 2012 + selectyearBox.SelectedIndex
                                join t2 in context.InstiutionRaitings on t1.Id equals t2.RaitingID
                                join t3 in context.Institutions on t2.InstitutionID equals t3.Id
                                join t4 in context.Locations on t3.LocationID equals t4.Id
                                orderby t2.WordRank
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
                IEnumerable<Repository.YearRaitGroup> sequence2 = from t1 in sequence1
                                                                  group t1 by t1.Location into g
                                                                  select new Repository.YearRaitGroup
                                                                  {
                                                                      Location = g.Key,
                                                                      Institutions = g.OrderByDescending(x => x.Score)
                                                                  };


                foreach (var tec in sequence2)
                {
                    if (Data.ItemsSource != null)
                        Data.ItemsSource = tec.Institutions.Union(Data.ItemsSource as IEnumerable<Repository.YearRait>);
                    else Data.ItemsSource = tec.Institutions;
                }
            };
        }
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                IEnumerable<Repository.UniversityDynamic> sequence = from t1 in context.Institutions
                                                                     where t1.Name == UnivList.SelectedItem.ToString()
                                                                     join t2 in context.InstiutionRaitings on t1.Id equals t2.InstitutionID
                                                                     join t3 in context.Raitings on t2.RaitingID equals t3.Id
                                                                     select new Repository.UniversityDynamic
                                                                     {
                                                                         Institution = t1.Name,
                                                                         Year = t3.Year,
                                                                         WorldRank = t2.WordRank,
                                                                         Score = t2.Score
                                                                     };
                Data.ItemsSource = sequence.ToList();
            }
        }
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                IEnumerable<Repository.UniversityScore> sequence = from t1 in context.Institutions
                                                                   join t2 in context.InstiutionRaitings on t1.Id equals t2.InstitutionID
                                                                   join t3 in context.Raitings on t2.RaitingID equals t3.Id
                                                                   group t2 by t2.Institution into g
                                                                   orderby g.Average(x => x.Score) descending
                                                                   select new Repository.UniversityScore
                                                                   {
                                                                       Institution = g.Key.Name,
                                                                       MinScore = g.Min(x => x.Score),
                                                                       MaxScore = g.Max(x => x.Score),
                                                                       AverScore = Math.Round(g.Average(x => x.Score), 2)
                                                                   };

                Data.ItemsSource = sequence.ToList();
            }
        }


    }
}

