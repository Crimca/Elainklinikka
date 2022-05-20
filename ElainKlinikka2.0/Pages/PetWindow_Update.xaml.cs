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
    /// Interaction logic for PetWindow.xaml
    /// </summary>
    public partial class PetWindow_Update : Window
    {
        database db;
        MainWindow window;

        public PetWindow_Update(string petID, MainWindow window)
        {
            InitializeComponent();
            db = new database();
            LoadPet(petID);
            this.window = window;
            this.Topmost = true;
        }

        void LoadPet(string petID)
        {
            Pet pet = db.LoadPet(petID);
            tb_petID.Text = pet.petID;
            tb_petOwnerID.Text = pet.ownerID;
            tb_petAnimalID.Text = pet.animalID.ToString();
            tb_petName.Text = pet.petName;
            tb_petWeight.Text = pet.weight.ToString();
            tb_petAge.Text = pet.age.ToString();
            tb_petVaccination.Text = pet.vaccinations;
            tb_petPrescriptions.Text = pet.prescriptions;
            tb_petDiagnosis.Text = pet.diagnoses;
            tb_petComment.Text = pet.comment;


            Console.WriteLine(pet.petName);
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
            Pet pet = new Pet
            {
                petID = tb_petID.Text,
                ownerID = tb_petOwnerID.Text,
                animalID = Int32.Parse(tb_petAnimalID.Text),
                petName = tb_petName.Text,
                weight = float.Parse(tb_petWeight.Text),
                age = Int32.Parse(tb_petAge.Text),
                vaccinations = tb_petVaccination.Text,
                prescriptions = tb_petPrescriptions.Text,
                diagnoses = tb_petDiagnosis.Text,
                comment = tb_petComment.Text
            };

            db.UpdatePet(pet);
        }
    }
}