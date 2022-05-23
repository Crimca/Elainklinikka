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
    /// Interaction logic for AppointmentWindow_New.xaml
    /// </summary>
    public partial class AppointmentWindow_New : Window
    {
        database db;
        MainWindow window;

        public AppointmentWindow_New(MainWindow window)
        {
            InitializeComponent();
            db = new database();
            this.window = window;
            this.Topmost = true;
            tb_appointmentID.Text = db.GetNextAppointmentID().ToString();
            loadComboBoxes();
        }

        void loadComboBoxes()
        {
            cb_year.Items.Add(2022);
            cb_year.Items.Add(2023);

            cb_month.Items.Add("January");           
            cb_month.Items.Add("February");          
            cb_month.Items.Add("March");             
            cb_month.Items.Add("April");           
            cb_month.Items.Add("May");               
            cb_month.Items.Add("June");            
            cb_month.Items.Add("July");              
            cb_month.Items.Add("August");            
            cb_month.Items.Add("September");       
            cb_month.Items.Add("October");           
            cb_month.Items.Add("November");        
            cb_month.Items.Add("December");



            List<Employee> owners = db.GetEmployees();
            foreach (Employee o in owners)
            {
                cb_employee.Items.Add(o.employeeID + " | " + o.Forename + " " + o.Surname);
            }

            List<Pet> pets = db.LoadPetTable();
            foreach (Pet o in pets)
            {
                cb_pet.Items.Add(o.petID + " | " + o.petName);
            }
        }

        private void cb_month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_day.Items.Clear();

            if (cb_month.SelectedIndex == 0 || cb_month.SelectedIndex == 2 || cb_month.SelectedIndex == 4 || cb_month.SelectedIndex == 6 || cb_month.SelectedIndex == 7 || cb_month.SelectedIndex == 9 || cb_month.SelectedIndex == 11)           
                for(int i = 1; i <= 31; i++)
                    cb_day.Items.Add(i);

            else if (cb_month.SelectedIndex == 3 || cb_month.SelectedIndex == 5 || cb_month.SelectedIndex == 8 || cb_month.SelectedIndex == 10)
                for (int i = 1; i <= 30; i++)
                    cb_day.Items.Add(i);
            else 
                for (int i = 1; i <= 28; i++)
                    cb_day.Items.Add(i);
        }



        private void SaveInformation(object sender, RoutedEventArgs e)
        {
            Owner owner = new Owner
            {
             //   ownerID = Int32.Parse(tb_ownerID.Text.ToString()),
             //   Forename = tb_forename.Text,
             //   Surname = tb_surname.Text,
             //   Streetname = tb_streetname.Text,
             //   Postalcode = tb_postcode.Text,
             //   City = tb_city.Text,
             //   Phonenum = tb_phonenum.Text
            };

            db.UpdateOwner(owner);
        }


        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            window.CloseAppointmentWindow();
            this.Close();
        }

        private void ClickToMoveWindow(object sender, MouseButtonEventArgs e)
        {
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
        }
    }
}
