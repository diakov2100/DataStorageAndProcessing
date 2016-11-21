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
            Repository.InitilizeDB();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Repository.DeleteDB();
        }


        private void FullRank_Click(object sender, RoutedEventArgs e)
        {
            var Result = Repository.GetFullRank(2012 + selectyearBox.SelectedIndex, GroupByLocCheckBox.IsChecked.Value);
            if (Result is List<Repository.YearRait>)
            {
                DataGridUniv.ItemsSource = Result as List<Repository.YearRait>;
            }
            else
            {
                DataGridUniv.ItemsSource = Result as List<Repository.NewYearRait>;
                DataGridUniv.Columns[0].DisplayIndex = 11;
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
            if (University.Text== "Choose university")
            {
                SelectUniversity dialog = new UI.SelectUniversity();
                dialog.ShowDialog();
                if (dialog.DialogResult == true)
                    University.Text = dialog.UnivList.SelectedItem.ToString();
            }         
            DataGridUniv.ItemsSource = Repository.GetDynamic(University.Text);
        }

        private void UnivScore_Click(object sender, RoutedEventArgs e)
        {
            DataGridUniv.ItemsSource = Repository.GetScore();
        }



    }
}


