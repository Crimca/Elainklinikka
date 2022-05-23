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
        List<string> bookedAppointments;
        bool doubleBooked = false;

        public AppointmentWindow_New(MainWindow window)
        {
            InitializeComponent();
            db = new database();
            this.window = window;
            this.Topmost = true;
            tb_appointmentID.Text = db.GetNextAppointmentID().ToString();
            loadComboBoxes();

            bookedAppointments = new List<string>();
            LoadBookedAppointments();
        }

        void loadComboBoxes()
        {
            cb_year.Items.Add(2022);
            cb_year.Items.Add(2023);

            cb_month.Items.Add("1|January");           
            cb_month.Items.Add("2|February");          
            cb_month.Items.Add("3|March");             
            cb_month.Items.Add("4|April");           
            cb_month.Items.Add("5|May");               
            cb_month.Items.Add("6|June");            
            cb_month.Items.Add("7|July");              
            cb_month.Items.Add("8|August");            
            cb_month.Items.Add("9|September");       
            cb_month.Items.Add("10|October");           
            cb_month.Items.Add("11|November");        
            cb_month.Items.Add("12|December");



            List<Employee> owners = db.GetEmployees();
            foreach (Employee o in owners)
            {
                cb_employee.Items.Add(o.employeeID + "|" + o.Forename + " " + o.Surname);
            }

            List<Pet> pets = db.LoadPetTable();
            foreach (Pet o in pets)
            {
                cb_pet.Items.Add(o.petID + "|" + o.petName);
            }
        }

        void LoadBookedAppointments()
        {
            List<Appointment> appointments = db.GetAppointments();

            foreach (Appointment p in appointments)
            {
                bookedAppointments.Add(p.employeeID + "|" + p.Day + "_" + p.Month +"_"+ p.Year);
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
            doubleBooked = false;
            foreach (string s in bookedAppointments)
            {
                if (s.Split('|')[0].Contains(cb_employee.SelectedItem.ToString().Split('|')[0]))
                {
                    Console.WriteLine("Matched employee " + s.Split('|')[0] + " = " + cb_employee.SelectedItem.ToString().Split('|')[0]);
                    Console.WriteLine("Day " + s.Split('|')[1].Split('_')[0] + " = " + cb_day.SelectedItem.ToString());

                    if (s.Split('|')[1].Split('_')[0].Contains(cb_day.SelectedItem.ToString()))
                    {
                        Console.WriteLine("Matched day");
                        if (s.Split('|')[1].Split('_')[1].Contains(cb_month.SelectedItem.ToString().Split('|')[0]))
                        {
                            Console.WriteLine("Matched month");
                            if (s.Split('|')[1].Split('_')[2].Contains(cb_year.SelectedItem.ToString()))
                            {
                                Console.WriteLine("Matched year");
                                doubleBooked = true;
                                break;
                            }

                        }
                    }
                }
            }

            if (!doubleBooked)
            {
                //Owner owner = new Owner
                //{
                //    //   ownerID = Int32.Parse(tb_ownerID.Text.ToString()),
                //    //   Forename = tb_forename.Text,
                //    //   Surname = tb_surname.Text,
                //    //   Streetname = tb_streetname.Text,
                //    //   Postalcode = tb_postcode.Text,
                //    //   City = tb_city.Text,
                //    //   Phonenum = tb_phonenum.Text
                //};
                //
                //db.UpdateOwner(owner);
                MessageBox.Show("Not Double booked?");
            }
            else
            {
                MessageBox.Show("That Employee is booked. Please Selected a new time");
            }
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
