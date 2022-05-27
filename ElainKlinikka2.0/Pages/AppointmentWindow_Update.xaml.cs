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
    /// Interaction logic for AppointmentWindow_Update.xaml
    /// </summary>
    public partial class AppointmentWindow_Update : Window
    {
        database db;
        MainWindow window;
        List<string> bookedAppointments;
        bool doubleBooked = false;

        public AppointmentWindow_Update(MainWindow window, string id, bool isView)
        {
            InitializeComponent();
            db = new database();
            this.window = window;
            this.Topmost = true;
            loadComboBoxes();

            bookedAppointments = new List<string>();
            LoadBookedAppointments();
            LoadAppointment(id);

            SetupBoxes(isView);
        }

        void SetupBoxes(bool isView)
        {
            cb_day.IsEditable = !isView;
            cb_month.IsEditable = !isView;
            cb_year.IsEditable = !isView;
            cb_employee.IsEditable = !isView;
            cb_reason.IsEditable = !isView;
        }


        void LoadAppointment(string id)
        {

                Appointment app = db.GetAppointment(id);
                tb_appointmentID.Text = app.appointmentID; 
            
            double myNum = 0;
            String testVar = "Not A Number";

            if (Double.TryParse(app.Reason, out myNum))
            {
                // it is a number
                cb_reason.SelectedIndex = (Int32.Parse(app.Reason) - 1);
            }
            else
            { 
                Console.WriteLine("Not");
                cb_reason.Items.Clear();
                cb_reason.Items.Add("CANCELLED");
                cb_reason.SelectedIndex = 0;
            }
            
            
            
                tb_pet.Text = app.petID;

                cb_employee.SelectedIndex = (Int32.Parse(app.employeeID) - 1);

                cb_year.Text = app.Year;

                if (app.Month == "01") { cb_month.Text = "01|January"; }
                else if (app.Month == "02") { cb_month.Text = "02|February"; }
                else if (app.Month == "03") { cb_month.Text = "03|March"; }
                else if (app.Month == "04") { cb_month.Text = "04|April"; }
                else if (app.Month == "05") { cb_month.Text = "05|May"; }
                else if (app.Month == "06") { cb_month.Text = "06|June"; }
                else if (app.Month == "07") { cb_month.Text = "07|July"; }
                else if (app.Month == "08") { cb_month.Text = "08|August"; }
                else if (app.Month == "09") { cb_month.Text = "09|September"; }
                else if (app.Month == "10") { cb_month.Text = "10|October"; }
                else if (app.Month == "11") { cb_month.Text = "11|November"; }
                else if (app.Month == "12") { cb_month.Text = "12|December"; }

                cb_day.Text = app.Day;

        }

        void loadComboBoxes()
        {
            cb_year.Items.Add(2022);
            cb_year.Items.Add(2023);

            cb_month.Items.Add("01|January");
            cb_month.Items.Add("02|February");
            cb_month.Items.Add("03|March");
            cb_month.Items.Add("04|April");
            cb_month.Items.Add("05|May");
            cb_month.Items.Add("06|June");
            cb_month.Items.Add("07|July");
            cb_month.Items.Add("08|August");
            cb_month.Items.Add("09|September");
            cb_month.Items.Add("10|October");
            cb_month.Items.Add("11|November");
            cb_month.Items.Add("12|December");

            List<Employee> owners = db.GetEmployees();
            foreach (Employee o in owners)
            {
                cb_employee.Items.Add(o.employeeID + "|" + o.Forename + " " + o.Surname);
            }

            List<PriceList> pricing = db.GetPrices();
            foreach (PriceList o in pricing)
            {
                cb_reason.Items.Add(o.itemID + "|" + o.Procedure + "|" + o.Price);
            }
        }

        void LoadBookedAppointments()
        {
            List<Appointment> appointments = db.GetAppointments();

            foreach (Appointment p in appointments)
            {
                bookedAppointments.Add(p.employeeID + "|" + p.Day + "_" + p.Month + "_" + p.Year);
            }

        }

        private void cb_month_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cb_day.Items.Clear();

            if (cb_month.SelectedIndex == 0 || cb_month.SelectedIndex == 2 || cb_month.SelectedIndex == 4 || cb_month.SelectedIndex == 6 || cb_month.SelectedIndex == 7 || cb_month.SelectedIndex == 9 || cb_month.SelectedIndex == 11)
                for (int i = 1; i <= 31; i++)
                    if (i < 10)
                        cb_day.Items.Add("0" + i);
                    else
                        cb_day.Items.Add(i);

            else if (cb_month.SelectedIndex == 3 || cb_month.SelectedIndex == 5 || cb_month.SelectedIndex == 8 || cb_month.SelectedIndex == 10)
                for (int i = 1; i <= 30; i++)
                    if (i < 10)
                        cb_day.Items.Add("0" + i);
                    else
                        cb_day.Items.Add(i);
            else
                for (int i = 1; i <= 28; i++)
                    if (i < 10)
                        cb_day.Items.Add("0" + i);
                    else
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
                Appointment app = new Appointment
                {
                    appointmentID = tb_appointmentID.Text,
                    Reason = cb_reason.SelectedItem.ToString().Split('|')[0],
                    petID = tb_pet.Text,
                    employeeID = cb_employee.SelectedItem.ToString().Split('|')[0],
                    Day = cb_day.SelectedItem.ToString(),
                    Month = cb_month.SelectedItem.ToString().Split('|')[0],
                    Year = cb_year.SelectedItem.ToString()
                };

                db.UpdateTheAppointment(app);
                window.CloseAppointmentWindow();
                this.Close();
            }
            else
            {
                MessageBox.Show("That Employee is booked. Please Selected a new time");
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            if (window != null)
            {
                window.CloseAppointmentWindow();
            }
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
