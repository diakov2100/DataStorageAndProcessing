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
using System.Windows.Shapes;

namespace DataStorageAndProcessing.UI
{
    /// <summary>
    /// Логика взаимодействия для SelectUniversity.xaml
    /// </summary>
    public partial class SelectUniversity : Window
    {
        public SelectUniversity()
        {
            InitializeComponent();
            using (var context = new Context())
            {
                UnivList.ItemsSource = context.Institutions.OrderBy(x=>x.Name).Select(x => x.Name).ToList();
            }
        }

        private void UnivList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
