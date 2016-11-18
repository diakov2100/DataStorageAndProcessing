
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
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            using (var contex= new Context())
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
    }
}
