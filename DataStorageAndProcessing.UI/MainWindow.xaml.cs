using DataStorageAndProcessing.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;
using System.Diagnostics;
using DataStorageAndProcessing.Data.Migrations;
using System.Data;
using System.Data.Entity;

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
        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            using (var contex = new Context())
            {
                contex.Database.Initialize(true);
                contex.SaveChanges();
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                context.Database.Delete();
            }
        }


        private void FullRank_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {

                if (2012 + selectyearBox.SelectedIndex < 2014)
                {
                    IEnumerable<Repository.YearRait> sequence = from t1 in context.Raitings
                                                                where t1.Year == 2012 + selectyearBox.SelectedIndex
                                                                join t2 in context.InstiutionRaitings on t1.Id equals t2.RaitingID
                                                                join t3 in context.Institutions on t2.InstitutionID equals t3.Id
                                                                join t4 in context.Locations on t3.LocationID equals t4.Id
                                                                orderby t2.WorldRank ascending
                                                                select new Repository.YearRait
                                                                {
                                                                    WordRank = t2.WorldRank,
                                                                    Institution = t3.Name,
                                                                    Location = t4.CountryName,
                                                                    NationalRank = t2.NationalRank,
                                                                    QualityofEducation = t2.QualityofEducation,
                                                                    AlumniEmployment = t2.AlumniEmployment,
                                                                    QualityofFaculty = t2.QualityofFaculty,
                                                                    Publications = t2.Publications,
                                                                    Influence = t2.Influence,
                                                                    Citations = t2.Citations,
                                                                    Patents = t2.Patents,
                                                                    Score = t2.Score
                                                                };
                    if (GroupByLocCheckBox.IsChecked == true)
                    {
                        IEnumerable<Repository.YearRaitGroup> sequence2 = from t1 in sequence
                                                                          group t1 by t1.Location into g
                                                                          orderby g.Key descending
                                                                          select new Repository.YearRaitGroup
                                                                          {

                                                                              Location = g.Key,
                                                                              Institutions = g.OrderByDescending(x => x.Score)
                                                                          };
                        DataGridUniv.ItemsSource = null;
                        foreach (var tec in sequence2)
                        {
                            if (DataGridUniv.ItemsSource != null)
                                DataGridUniv.ItemsSource = tec.Institutions.Union(DataGridUniv.ItemsSource as IEnumerable<Repository.YearRait>);
                            else DataGridUniv.ItemsSource = tec.Institutions;
                        }
                    }
                    else
                    {
                        DataGridUniv.ItemsSource = sequence.ToList();
                    }
                }
                else
                {
                    IEnumerable<Repository.NewYearRait> sequence = from t1 in context.Raitings
                                                                   where t1.Year == 2012 + selectyearBox.SelectedIndex
                                                                   join t2 in context.NewInstitutionsRaitings on t1.Id equals t2.RaitingID
                                                                   join t3 in context.Institutions on t2.InstitutionID equals t3.Id
                                                                   join t4 in context.Locations on t3.LocationID equals t4.Id
                                                                   orderby t2.WorldRank ascending
                                                                   select new Repository.NewYearRait
                                                                   {
                                                                       WordRank = t2.WorldRank,
                                                                       Institution = t3.Name,
                                                                       Location = t4.CountryName,
                                                                       NationalRank = t2.NationalRank,
                                                                       QualityofEducation = t2.QualityofEducation,
                                                                       AlumniEmployment = t2.AlumniEmployment,
                                                                       QualityofFaculty = t2.QualityofFaculty,
                                                                       Publications = t2.Publications,
                                                                       Influence = t2.Influence,
                                                                       Citations = t2.Citations,
                                                                       Patents = t2.Patents,
                                                                       Score = t2.Score,
                                                                       BroadImpact = t2.BroadImpact
                                                                   };
                    if (GroupByLocCheckBox.IsChecked == true)
                    {
                        sequence.ToList();
                        IEnumerable<Repository.NewYearRaitGroup> sequence2 = from t1 in sequence
                                                                             group t1 by t1.Location into g
                                                                             orderby g.Key descending
                                                                             select new Repository.NewYearRaitGroup
                                                                             {
                                                                                 Location = g.Key,
                                                                                 Institutions = g.OrderByDescending(x => x.Score)
                                                                             };
                        DataGridUniv.ItemsSource = null;
                        foreach (var tec in sequence2)
                        {
                            try
                            {
                                if (DataGridUniv.ItemsSource != null)
                                    DataGridUniv.ItemsSource = tec.Institutions.Union(DataGridUniv.ItemsSource as IEnumerable<Repository.NewYearRait>);
                                else DataGridUniv.ItemsSource = tec.Institutions;
                            }
                            catch { }
                        }

                    }
                    else
                    {
                        DataGridUniv.ItemsSource = sequence.ToList();
                        DataGridUniv.Columns[0].DisplayIndex = 11;
                    }
                }
            }
        }
        private void SelectUniversity_Click(object sender, RoutedEventArgs e)
        {
            SelectUniversity dialog = new UI.SelectUniversity();
            dialog.ShowDialog();
            if (dialog.DialogResult == true)
                University.Text = dialog.UnivList.SelectedItem.ToString();
        }

        private void ShowStat_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                IEnumerable<Repository.UniversityDynamic> sequence = from t1 in context.Institutions
                                                                     where t1.Name == University.Text
                                                                     join t2 in (context.InstiutionRaitings.Union(context.InstiutionRaitings)) on t1.Id equals t2.InstitutionID
                                                                     join t3 in context.Raitings on t2.RaitingID equals t3.Id
                                                                     select new Repository.UniversityDynamic
                                                                     {
                                                                         Institution = t1.Name,
                                                                         Year = t3.Year,
                                                                         WorldRank = t2.WorldRank,
                                                                         Score = t2.Score
                                                                     };
                DataGridUniv.ItemsSource = sequence.ToList();
            }
        }

        private void UnivScore_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new Context())
            {
                IEnumerable<Repository.UniversityScore> sequence = from t1 in context.Institutions
                                                                   join t2 in (context.InstiutionRaitings.Union(context.InstiutionRaitings)) on t1.Id equals t2.InstitutionID
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

                DataGridUniv.ItemsSource = sequence.ToList();
            }
        }



    }
}


