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
using ElainKlinikka2._0.Helpers;

namespace ElainKlinikka2._0.Pages
{
    /// <summary>
    /// Interaction logic for OwnerWindow_New.xaml
    /// </summary>
    public partial class OwnerWindow_New : Window
    {      
        database db;
        MainWindow window;

        public OwnerWindow_New(MainWindow window)
        {
            InitializeComponent();

            InitializeComponent();
            db = new database();
            this.window = window;
            this.Topmost = true;
            tb_ownerID.Text = db.GetNextOwnerID().ToString();
        }


        private void ClickToMoveWindow(object sender, MouseButtonEventArgs e)
        {
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            window.CloseOwnerInfo();
            this.Close();
        }



        private void SaveInformation(object sender, RoutedEventArgs e)
        {
            Owner owner = new Owner
            {
                ownerID = Int32.Parse(tb_ownerID.Text),
                Forename = tb_forename.Text,
                Surname = tb_surname.Text,
                Streetname = tb_streetname.Text,
                Postalcode = tb_postcode.Text,
                City = tb_city.Text,
                Phonenum = tb_phonenum.Text
            };

            db.InsertOwner(owner);

            window.CloseOwnerInfo();
            this.Close();
        }
    }
}
