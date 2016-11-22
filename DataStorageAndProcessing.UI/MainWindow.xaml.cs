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
        bool ProcessRunning = false;
        public MainWindow()
        {
            InitializeComponent();
            selectyearBox.SelectedIndex = 0;
            PRBar.Value = 0;
            PRBar.Maximum = 3200;
            PRBar.Visibility = Visibility.Collapsed;
            Updatetext.Visibility = Visibility.Collapsed;
            FillDatabase.Update += (prog => Dispatcher.Invoke(() =>
             {
                 ProcessRunning = true;
                 PRBar.Value = (prog);
                 if (prog == 3200)
                 {
                     Dispatcher.Invoke(() => PRBar.Visibility = Visibility.Collapsed);
                     Dispatcher.Invoke(() => Updatetext.Visibility = Visibility.Collapsed);
                     ProcessRunning = false;
                 }
             }));
        }
        private void Initialize_Click(object sender, RoutedEventArgs e)
        {
            if (!ProcessRunning)
            {
                PRBar.Visibility = Visibility.Visible;
                Updatetext.Visibility = Visibility.Visible;
                Task t = new Task(Repository.InitilizeDB);
                t.Start();
            }

        }
        private void ProgressUp(int prog)
        {
            Dispatcher.Invoke(() =>
            {
                ProcessRunning = true;
                PRBar.Value = (prog);
                if (prog == 3200)
                {
                    Dispatcher.Invoke(() => PRBar.Visibility = Visibility.Collapsed);
                    Dispatcher.Invoke(() => Updatetext.Visibility = Visibility.Collapsed);
                    ProcessRunning = false;
                }
            });
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!ProcessRunning)
            {
                Repository.DeleteDB();
            }
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
            if (University.Text == "Choose university")
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


