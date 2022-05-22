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
        private string selectedIDFromGrid = "";

        //Add login page param
        private KirjautumisSivu loginPage;

        List<Owner> owners;
        List<Pet> pets;
        List<Animal> animals;
        List<PriceList> prices;

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
            pets = new List<Pet>();
            InitializeComponent();
            HideAll();
            CreateOwnerTable();
            CreatePetTable();
            CreatePriceTable();

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
            pets = db.LoadPetTable();
            animals = db.GetAnimalTypes();
            Console.WriteLine(animals[0].animalID + " | " + animals[0].breed + " | " + animals[0].species);
            List<Animal> SortedList = animals.OrderBy(o => o.animalID).ToList();
            Console.WriteLine(animals[0].animalID + " | " + animals[0].breed + " | " + animals[0].species);
            foreach (Pet pet in pets)
            {
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
                    animalID = animals[Int32.Parse(pet.animalID) -1].species + " | " + animals[Int32.Parse(pet.animalID)-1].breed,
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
                            selectedIDFromGrid = row.petID;
                            Console.WriteLine(selectedIDFromGrid); 
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
            if (selectedIDFromGrid != "")
            {
                var window = new PetWindow_Update(selectedIDFromGrid, this);
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
            selectedIDFromGrid = "";
            tb_petSearcher.Text = "";
        }

        private void petResetBtn(object sender, RoutedEventArgs e)
        {
            ClosePetInfo();
        }

        //Search Pet table for value
        private void SearchPetDataGrid(object sender, RoutedEventArgs e)
        {
            if (tb_petSearcher.Text != null)
            {
                if (tb_petSearcher.Text == "")
                {
                    LoadPetTable();
                }
                else
                {
                    PetDB.Items.Clear();
                    List<Pet> filteredPets = new List<Pet>();

                    foreach (Pet p in pets)
                    {
                        if (p.petID.Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.petName.Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.weight.ToString().Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.age.ToString().Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.vaccinations.Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.prescriptions.Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.diagnoses.Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.ownerID.Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.animalID.ToString().Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }
                        else if (p.comment.Contains(tb_petSearcher.Text)) { filteredPets.Add(p); }

                        Console.WriteLine("in here");
                    }

                    if (filteredPets.Count > 0)
                    {
                        foreach (Pet p in filteredPets)
                        {
                            PetDB.Items.Add(p);
                        }
                    }
                }
            }
        }


        #endregion PET WINDOW


        #region OWNER WINDOW

        //Logout button
        void LoadOwnerTable()
        {
            ownerDB.Items.Clear();
            owners = db.GetOwners();

            foreach (Owner o in owners)
            {
                ownerDB.Items.Add(new Owner
                {
                    ownerID = o.ownerID,
                    Forename = o.Forename,
                    Surname = o.Surname,
                    Streetname = o.Streetname,
                    Postalcode = o.Postalcode,
                    City = o.City,
                    Phonenum = o.Phonenum
                });                
            }

        }

        void CreateOwnerTable()
        {
            DataGridTextColumn ownerID_col = new DataGridTextColumn();
            DataGridTextColumn ownerForename_col = new DataGridTextColumn();

            DataGridTextColumn ownerSurname_col = new DataGridTextColumn();
            DataGridTextColumn ownerStreetName_col = new DataGridTextColumn();
            DataGridTextColumn ownerPostCode_Col = new DataGridTextColumn();
            DataGridTextColumn ownerCity_Col = new DataGridTextColumn();
            DataGridTextColumn ownerPhonenum_Col = new DataGridTextColumn();

            ownerDB.Columns.Add(ownerID_col);
            ownerDB.Columns.Add(ownerForename_col);
            ownerDB.Columns.Add(ownerSurname_col);
            ownerDB.Columns.Add(ownerStreetName_col);
            ownerDB.Columns.Add(ownerPostCode_Col);
            ownerDB.Columns.Add(ownerCity_Col);
            ownerDB.Columns.Add(ownerPhonenum_Col);

            ownerID_col.Binding = new Binding("ownerID");
            ownerForename_col.Binding = new Binding("Forename");
            ownerSurname_col.Binding = new Binding("Surname");
            ownerStreetName_col.Binding = new Binding("Streetname");
            ownerPostCode_Col.Binding = new Binding("Postalcode");
            ownerCity_Col.Binding = new Binding("City");
            ownerPhonenum_Col.Binding = new Binding("Phonenum");

            ownerID_col.Header = "Owner ID";
            ownerForename_col.Header = "Forename";
            ownerSurname_col.Header = "Surname";
            ownerStreetName_col.Header = "Streetname";
            ownerPostCode_Col.Header = "Postalcode";
            ownerCity_Col.Header = "City";
            ownerPhonenum_Col.Header = "Phonenum";
        }

        private void OwnerDB_Selected(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (ownerDB.SelectedItem != null)
                {
                    if (ownerDB.SelectedItem is Owner)
                    {
                        var row = (Owner)ownerDB.SelectedItem;

                        if (row != null)
                        {
                            selectedIDFromGrid = row.ownerID.ToString();
                            Console.WriteLine(selectedIDFromGrid);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void OpenOwnerInformation(object sender, RoutedEventArgs e)
        {
            if (selectedIDFromGrid != "")
            {
                var window = new OwnerWindow_Update(selectedIDFromGrid, this);
                window.Owner = this;
                window.Show();
                this.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Valitse ensin Owner");
            }
        }

        private void OpenOwnerNew(object sender, RoutedEventArgs e)
        {
            var window = new OwnerWindow_New(this);
            window.Owner = this;
            window.Show();
            this.IsEnabled = false;
        }

        public void CloseOwnerInfo()
        {
            this.IsEnabled = true;
            this.Activate();
            LoadOwnerTable();
            Loading();
            ownerDB.UnselectAll();
            selectedIDFromGrid = "";
            tb_ownerpetSearcher.Text = "";
        }

        private void ownerResetBtn(object sender, RoutedEventArgs e)
        {
            CloseOwnerInfo();
        }

        //Search Pet table for value
        private void SearchOwnerDataGrid(object sender, RoutedEventArgs e)
        {
            if (tb_ownerpetSearcher.Text != null)
            {
                if (tb_ownerpetSearcher.Text == "")
                {
                    LoadPetTable();
                }
                else
                {
                    ownerDB.Items.Clear();
                    List<Owner> filteredOwners = new List<Owner>();

                    foreach (Owner p in owners)
                    {
                        if (p.ownerID.ToString().Contains(tb_ownerpetSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.Forename.Contains(tb_ownerpetSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.Surname.Contains(tb_ownerpetSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.Streetname.ToString().Contains(tb_ownerpetSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.Postalcode.Contains(tb_ownerpetSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.City.Contains(tb_ownerpetSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.Phonenum.Contains(tb_ownerpetSearcher.Text)) { filteredOwners.Add(p); }
                        Console.WriteLine("in here");
                    }

                    if (filteredOwners.Count > 0)
                    {
                        foreach (Owner p in filteredOwners)
                        {
                            ownerDB.Items.Add(p);
                        }
                    }
                }
            }
        }

        #endregion OWNER WINDOW

        #region PRICE WINDOW

        //Logout button
        void LoadPriceTable()
        {
            priceDB.Items.Clear();
            prices = db.GetPrices();

            foreach (PriceList p in prices)
            {
                priceDB.Items.Add(new PriceList
                {
                    itemID = p.itemID,
                    Price = p.Price,
                    Procedure = p.Procedure,
                    MedProc = p.MedProc == "1" ? "Procedure" : "Medicine"
            });
            }

        }

        void CreatePriceTable()
        {
            DataGridTextColumn itemID_col = new DataGridTextColumn();
            DataGridTextColumn itemPrice_col = new DataGridTextColumn();
            DataGridTextColumn itemProcedure_col = new DataGridTextColumn();
            DataGridTextColumn itemMedProc_col = new DataGridTextColumn();

            priceDB.Columns.Add(itemID_col);
            priceDB.Columns.Add(itemPrice_col);
            priceDB.Columns.Add(itemProcedure_col);
            priceDB.Columns.Add(itemMedProc_col);

            itemID_col.Binding = new Binding("itemID");
            itemPrice_col.Binding = new Binding("Price");
            itemProcedure_col.Binding = new Binding("Procedure");
            itemMedProc_col.Binding = new Binding("MedProc");

            itemID_col.Header = "Item ID";
            itemPrice_col.Header = "Price";
            itemProcedure_col.Header = "Item";
            itemMedProc_col.Header = "Medical or Procedure";
        }

        public void ClosePriceWindow()
        {
            LoadPriceTable();
            Loading();
            priceDB.UnselectAll();
            tb_priceSearcher.Text = "";
        }

        private void PriceResetBtn(object sender, RoutedEventArgs e)
        {
            ClosePriceWindow();
        }

        //Search Pet table for value
        private void SearchPriceDataGrid(object sender, RoutedEventArgs e)
        {
            if (tb_priceSearcher.Text != null)
            {
                if (tb_priceSearcher.Text == "")
                {
                    LoadPriceTable();
                }
                else
                {
                    priceDB.Items.Clear();
                    List<PriceList> filteredOwners = new List<PriceList>();

                    foreach (PriceList p in prices)
                    {
                        if (p.itemID.ToString().Contains(tb_priceSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.Price.Contains(tb_priceSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.Procedure.Contains(tb_priceSearcher.Text)) { filteredOwners.Add(p); }
                        else if (p.MedProc.ToString().Contains(tb_priceSearcher.Text)) { filteredOwners.Add(p); }
                    }

                    if (filteredOwners.Count > 0)
                    {
                        foreach (PriceList p in filteredOwners)
                        {
                            priceDB.Items.Add(p);
                        }
                    }
                }
            }
        }

        #endregion OWNER WINDOW

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
            LoadOwnerTable();
            ownerDBCanvas.Visibility = Visibility.Visible;
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
            LoadPriceTable();
            priceDBCanvas.Visibility = Visibility.Visible;
        }

        void HideAll()
        {
            PetDBCanvas.Visibility = Visibility.Hidden;
            ownerDBCanvas.Visibility = Visibility.Hidden;
            // VarauksetCanvas.Visibility = Visibility.Hidden;
            priceDBCanvas.Visibility = Visibility.Hidden;
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
