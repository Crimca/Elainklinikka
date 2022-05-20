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
using ElainKlinikka2._0.Helpers;
using ElainKlinikka2._0.Pages;

namespace ElainKlinikka2._0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string kirjautunutUsername = "";
        private string selectedPetID = "";

        //Add login page param
        private KirjautumisSivu loginPage;

        database db;

        //add login page as a parameter
        public MainWindow(KirjautumisSivu loginPage, string name)
        {            
            //Set login page to the login page just hidden
            kirjautunutUsername = name;
            this.loginPage = loginPage;

            InitializeComponent();

            db = new database();
            loginNameLable.Content = name;
            //valittuKlinikka = new Klinikka("Palosaaren Klinikka", "Pikitehtaankatuu 19");

            //valittuKlinikka.LuoMockDataa(30);
            Loading();
            InitializeComponent();
            HideAll();
            CreatePetTable();
            //kerrotaan listview komponentille, että sen pitää näyttää
            // valitun kirjaston teokset listaa sen sisällä
            //ListSource kertoo itemeiden lähteen ja jokainen näytetään rivinä ListViewn sisällä
            //dataGrid.ItemsSource = valittuKlinikka.Eläimet;
        }


        #region PET WINDOW

        //Logout button
        void LoadPetTable()
        {
            PetDB.Items.Clear();
            List<Pet> pets = db.LoadPetTable();

            foreach (Pet pet in pets)
            {
                Console.WriteLine(pet.petName);
                PetDB.Items.Add(new Pet
                {

                    petID = pet.petID,
                    petName = pet.petName,
                    weight = pet.weight,
                    age = pet.age,
                    vaccinations = pet.vaccinations,
                    prescriptions = pet.prescriptions,
                    diagnoses = pet.diagnoses,
                    ownerID = pet.ownerID,
                    animalID = pet.animalID,
                    comment = pet.comment
                });
            }
        }

        void CreatePetTable()
        {
            DataGridTextColumn petID_col = new DataGridTextColumn();
            DataGridTextColumn petName_col = new DataGridTextColumn();

            DataGridTextColumn weight_col = new DataGridTextColumn();
            DataGridTextColumn age_col = new DataGridTextColumn();
            DataGridTextColumn vaccinations_col = new DataGridTextColumn();
            DataGridTextColumn prescriptions_col = new DataGridTextColumn();
            DataGridTextColumn diagnoses_col = new DataGridTextColumn();
            DataGridTextColumn ownerID_col = new DataGridTextColumn();
            DataGridTextColumn animalID_col = new DataGridTextColumn();
            DataGridTextColumn comment_col = new DataGridTextColumn();

            PetDB.Columns.Add(petID_col);
            PetDB.Columns.Add(petName_col);
            PetDB.Columns.Add(weight_col);
            PetDB.Columns.Add(age_col);
            PetDB.Columns.Add(vaccinations_col);
            PetDB.Columns.Add(prescriptions_col);
            PetDB.Columns.Add(diagnoses_col);
            PetDB.Columns.Add(ownerID_col);
            PetDB.Columns.Add(animalID_col);
            PetDB.Columns.Add(comment_col);

            petID_col.Binding = new Binding("petID");
            petName_col.Binding = new Binding("petName");
            weight_col.Binding = new Binding("weight");
            age_col.Binding = new Binding("age");
            vaccinations_col.Binding = new Binding("vaccinations");
            prescriptions_col.Binding = new Binding("prescriptions");
            diagnoses_col.Binding = new Binding("diagnoses");
            ownerID_col.Binding = new Binding("ownerID");
            animalID_col.Binding = new Binding("animalID");
            comment_col.Binding = new Binding("comment");

            petID_col.Header = "Lemmikin ID";
            petName_col.Header = "Nimi";
            weight_col.Header = "Paino";
            age_col.Header = "Ikä";
            vaccinations_col.Header = "Rokotukset";
            prescriptions_col.Header = "Reseptit";
            diagnoses_col.Header = "Diagnoosit";
            ownerID_col.Header = "Omistajan ID";
            animalID_col.Header = "Lemmikin tyyppi";
            comment_col.Header = "Kommentit";
        }

        private void PetDB_Selected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (PetDB.SelectedItem != null)
                {
                    if (PetDB.SelectedItem is Pet)
                    {
                        var row = (Pet)PetDB.SelectedItem;

                        if (row != null)
                        {
                            selectedPetID = row.petID;
                            Console.WriteLine(selectedPetID); 
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void OpenPetInformation(object sender, RoutedEventArgs e)
        {
            if (selectedPetID != "")
            {
                var window = new PetWindow_Update(selectedPetID, this);
                window.Owner = this;
                window.Show();
                this.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Valitse ensin lemmikki");
            }
        }

        private void OpenPetNew(object sender, RoutedEventArgs e)
        {
                var window = new PetWindow_New(this);
                window.Owner = this;
                window.Show();
                this.IsEnabled = false;
        }

        public void ClosePetInfo()
        {
            this.IsEnabled = true;
            this.Activate();
            LoadPetTable();
            Loading();
            PetDB.UnselectAll();
            selectedPetID = "";
        }


        #endregion PET WINDOW

        #region window buttons

        //Mouse down to move window
        private void ClickToMoveWindow(object sender, MouseButtonEventArgs e)
        {
            {
                if (e.ChangedButton == MouseButton.Left)
                    this.DragMove();
            }
        }

        private void CloseApplication(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        //Logout button
        private void LogOut(object sender, RoutedEventArgs e)
        {
            loginPage.Show();
            loginNameLable.Content = "";
            this.Close();
        }

        //Show Pet database
        private void Show_PetDB(object sender, RoutedEventArgs e)
        {
            HideAll();
            LoadPetTable();
            PetDBCanvas.Visibility = Visibility.Visible;
        }

        //Show Owner database
        private void Show_OwnerDB(object sender, RoutedEventArgs e)
        {
            HideAll();
          //  OmistajatCanvas.Visibility = Visibility.Visible;
        }

        //Show Booking database
        private void Show_BookingDB(object sender, RoutedEventArgs e)
        {
            HideAll();
         //   VarauksetCanvas.Visibility = Visibility.Visible;
        }

        //Show Items database
        private void Show_PriceDB(object sender, RoutedEventArgs e)
        {
            HideAll();
         //   HinnastoCanvas.Visibility = Visibility.Visible;
        }

        void HideAll()
        {
            PetDBCanvas.Visibility = Visibility.Hidden;
           // OmistajatCanvas.Visibility = Visibility.Hidden;
           // VarauksetCanvas.Visibility = Visibility.Hidden;
           // HinnastoCanvas.Visibility = Visibility.Hidden;
        }

        #endregion


        async Task Loading()
        {
            loadingCanva.Visibility = Visibility.Visible;

            loadingCanva.Opacity = 1;
            loadLabel.Opacity = 1;

            imgCatLoad.Opacity = 0;
            imgDogLoad.Opacity = 0;
            imgGuineaLoad.Opacity = 0;
            imgFrogLoad.Opacity = 0;


            loadLabel.Content = "L";
            double incVal = 0.10;


            for (double i = 0; i < 1;)
            {
                await Task.Delay(1);
                imgCatLoad.Opacity = (i);
                i += incVal;
            }

            loadLabel.Content = "LA";
            for (double i = 1; i > -.1;)
            {
                await Task.Delay(1);
                imgCatLoad.Opacity = (i);
                i -= incVal;
            }

            loadLabel.Content = "LAD";
            for (double i = 0; i < 1;)
            {
                await Task.Delay(1);
                imgDogLoad.Opacity = (i);
                i += incVal;
            }
            loadLabel.Content = "LADA";
            for (double i = 1; i > -.1;)
            {
                await Task.Delay(1);
                imgDogLoad.Opacity = (i);
                i -= incVal;
            }
            loadLabel.Content = "LADAT";

            for (double i = 0; i < 1;)
            {
                await Task.Delay(1);
                imgGuineaLoad.Opacity = (i);
                i += incVal;
            }

            loadLabel.Content = "LADATA";
            for (double i = 1; i > -.1;)
            {
                await Task.Delay(1);
                imgGuineaLoad.Opacity = (i);
                i -= incVal;
            }

            loadLabel.Content = "LADATAA";
            for (double i = 0; i < 1;)
            {
                await Task.Delay(1);
                imgFrogLoad.Opacity = (i);
                i += incVal;
            }
            loadLabel.Content = "LADATAAN";
            for (double i = 1; i > -.1;)
            {
                await Task.Delay(1);
                imgFrogLoad.Opacity = (i);
                loadingCanva.Opacity = i;
                loadLabel.Opacity = i;
                i -= incVal;
            }


            loadingCanva.Visibility = Visibility.Hidden;
        }


    }
}
