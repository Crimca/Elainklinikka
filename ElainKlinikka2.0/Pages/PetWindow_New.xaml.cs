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
    /// Interaction logic for PetWindow_New.xaml
    /// </summary>
    public partial class PetWindow_New : Window
    {
        database db;
        MainWindow window;

        public PetWindow_New(MainWindow window)
        {
            InitializeComponent();

            InitializeComponent();
            db = new database();
            this.window = window;
            this.Topmost = true;
            tb_petID.Text = db.GetNextPetID().ToString();

            loadComboBoxes();
                
        }

        void loadComboBoxes()
        {
            List<Animal> animals = db.GetAnimalTypes();
            foreach (Animal a in animals)
            {
                cb_AnimalID.Items.Add(a.animalID + " | " + a.species + " | " + a.breed);
            }

            List<Owner> owners = db.GetOwners();
            foreach (Owner o in owners)
            {
                cb_OwnerID.Items.Add(o.ownerID + " | " + o.Forename + " " + o.Surname);
            }
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
            window.ClosePetInfo();
            this.Close();
        }



        private void SavePetInfo(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(cb_OwnerID.SelectedItem.ToString().Split('|')[0]);
            Console.WriteLine(cb_AnimalID.SelectedItem.ToString().Split('|')[0]);
            Pet pet = new Pet();
            pet.petID = tb_petID.Text;
            pet.petName = tb_petName.Text;
            pet.weight = float.Parse(tb_petWeight.Text);
            pet.age = Int32.Parse(tb_petAge.Text);
            pet.vaccinations = "";
            pet.prescriptions = "";
            pet.Appointments = "";
            pet.diagnoses = "";
            pet.ownerID = cb_OwnerID.SelectedItem.ToString().Split('|')[0];
            pet.paymenthistory = "";
            pet.animalID = Int32.Parse(cb_AnimalID.SelectedItem.ToString().Split('|')[0]);
            pet.comment = "";

            db.InsertPet(pet);
        }
    }
}
