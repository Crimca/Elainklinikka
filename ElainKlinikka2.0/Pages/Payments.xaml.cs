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
    /// Interaction logic for Payments.xaml
    /// </summary>
    public partial class Payments : Window
    {
        database db;
        MainWindow window;
        List<Payment> payments;
        Pet pet;
        Owner owner;
        List<Appointment> apppointment;

        string selectedIDFromGrid = "";
        string petID = "";
        string ownerID = "";

        public Payments(MainWindow window, string petID, string ownerID)
        {
            InitializeComponent();
            this.window = window;
            this.petID = petID;
            this.ownerID = ownerID;
            db = new database();
            CreatePaymentTable();
            LoadPaymentGrid();

        }

        void CreatePaymentTable()
        {
            DataGridTextColumn paymentID_col = new DataGridTextColumn();
            DataGridTextColumn ownerID_col = new DataGridTextColumn();
            DataGridTextColumn petID_col = new DataGridTextColumn();
            DataGridTextColumn appointmentID_col = new DataGridTextColumn();
            DataGridTextColumn paymentValue_col = new DataGridTextColumn();
            DataGridTextColumn paid_col = new DataGridTextColumn();

            paymentGrid.Columns.Add(paymentID_col);
            paymentGrid.Columns.Add(ownerID_col);
            paymentGrid.Columns.Add(petID_col);
            paymentGrid.Columns.Add(appointmentID_col);
            paymentGrid.Columns.Add(paymentValue_col);
            paymentGrid.Columns.Add(paid_col);

            paymentID_col.Binding = new Binding("paymentID");
            ownerID_col.Binding = new Binding("ownerID");
            petID_col.Binding = new Binding("petID");
            appointmentID_col.Binding = new Binding("appointmentID");
            paymentValue_col.Binding = new Binding("paymentValue");
            paid_col.Binding = new Binding("paid");

            paymentID_col.Header = "ID";
            ownerID_col.Header = "Omistaja";
            petID_col.Header = "Nimi";
            appointmentID_col.Header = "Varaus ID";
            paymentValue_col.Header = "Summa (€)";
            paid_col.Header = "Maksun tilanne";
        }


        void LoadPaymentGrid()
        {
            paymentGrid.Items.Clear();
            payments = db.GetPayments(petID);

            pet = db.LoadPet(petID);
            owner = db.GetOwner(ownerID);


            foreach (Payment p in payments)
            {
                paymentGrid.Items.Add(new Payment
                {
                    paymentID = p.paymentID,
                    ownerID = owner.Forename + " " + owner.Surname,
                    petID = pet.petName,
                    appointmentID = p.appointmentID,
                    paymentValue = p.paymentValue,
                    paid = p.paid
                });
            }

            cb_paymentOptions.Items.Add("Maksettu|Käteinen");
            cb_paymentOptions.Items.Add("Maksettu|Kortti");
            cb_paymentOptions.Items.Add("Ei maksettu");
            cb_paymentOptions.Items.Add("Laskutettu");
        }
        private void AppointmentGridSelection(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (paymentGrid.SelectedItem != null)
                {
                    if (paymentGrid.SelectedItem is Payment)
                    {
                        var row = (Payment)paymentGrid.SelectedItem;

                        if (row != null)
                        {
                            selectedIDFromGrid = row.appointmentID;                            
                            Console.WriteLine(selectedIDFromGrid);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            window.ClosePetInfo();
            this.Close();
        }

        private void ClickToMoveWindow(object sender, MouseButtonEventArgs e)
        {
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
        }

        private void SaveInformation(object sender, RoutedEventArgs e)
        {
            var window = new AppointmentWindow_Update(null, selectedIDFromGrid, true);
            window.Owner = this;
            window.Show();
        }

        private void SetPaymentStatus(object sender, RoutedEventArgs e)
        {
            db.UpdatePaymentStatus(selectedIDFromGrid, cb_paymentOptions.SelectedItem.ToString());
        }
    }    
}
