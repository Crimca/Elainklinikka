﻿using System;
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

        private void avaaKirjButtonClick(object sender, RoutedEventArgs e)
        {
            // Luodaan instanssi meidän omasta dialogista
            KirjautumisSivu d = new KirjautumisSivu();

            // ShowDialog funktio näyttää sen dialogina
            // Paluuarvo false = käyttäjä ei suorittanut dialogia loppuun
            // Paluuarvo true = käyttäjä suoritti dialogin loppuun
            // (eli pääsi klikkaamaan Kirjaudu-painiketta)
            bool tulos = (bool)d.ShowDialog();
            if (tulos == false)
            {
                // Käyttäjä klikkasi "Peruuta" tai sulki ikkunan
                MessageBox.Show("Ei kirjautunut.");
            }
            else
            {
                // Käyttäjä klikkasi "Kirjaudu" painiketta
                // KirjautumisSivu-luokassa sen painikkeen tapahtumakäsittelijässä asetetaan DialogResult = true
                // Siksi päädymme tähän haaraan

                // Luetaan dialogi-ikkunasta käyttäjän syöttämä username ja salasana
                string username = d.EnteredUsername;
                //string pw = d.EnteredPassword;
                kirjautunutUsername = d.EnteredUsername;

                PäivitäKäyttöliittymä();
            }

        }

        #region KÄYTTÖLIITTYMÄ
        private void PäivitäKäyttöliittymä()
        {
            // Tarkistetaan, onko käyttäjä kirjautuneena (onko kirjautunutUsername muuttujassa joku arvo)
            bool kirjautunut = String.IsNullOrEmpty(kirjautunutUsername) == false;

            if (kirjautunut == true)
            {
                List<User> u = DatabaseHandler.Instance.FindUsersWithEmail(kirjautunutUsername);

                // Jos on, näytä "Kirjaudu ulos" painike ja disabloi "Kirjaudu" painike
                kirjauduUlosButton.Visibility = Visibility.Visible;
                avaaKirjButton.IsEnabled = false;
                //Tabien näkyvyys

                if (u[0].UserRooli == "1")
                {
                    AsiakasHakuTab.Visibility = Visibility.Visible;
                    AsiakasHakuTab.IsEnabled = true;
                    AsiakasLisääTab.Visibility = Visibility.Visible;
                    AsiakasLisääTab.IsEnabled = true;
                    HakuTab.Visibility = Visibility.Visible;
                    HakuTab.IsEnabled = true;
                    EläinLisääTab.Visibility = Visibility.Visible;
                    EläinLisääTab.IsEnabled = true;
                }

                if (u[0].UserRooli == "2")
                {
                    AsiakasHakuTab.Visibility = Visibility.Visible;
                    AsiakasHakuTab.IsEnabled = true;
                    AsiakasLisääTab.Visibility = Visibility.Visible;
                    AsiakasLisääTab.IsEnabled = true;
                    HakuTab.Visibility = Visibility.Collapsed;
                    HakuTab.IsEnabled = false;
                }
            }
            else
            {
                // Jos ei ole kirjautuneena, piilota "Kirjaudu ulos" painike
                kirjauduUlosButton.Visibility = Visibility.Hidden;
                // Enabloi "Kirjaudu" painike
                avaaKirjButton.IsEnabled = true;
                //Tabien näkyvyys
                AsiakasHakuTab.Visibility = Visibility.Hidden;
                AsiakasHakuTab.IsEnabled = false;
                AsiakasLisääTab.Visibility = Visibility.Collapsed;
                AsiakasLisääTab.IsEnabled = true;
                HakuTab.Visibility = Visibility.Hidden;
                HakuTab.IsEnabled = false;
                EläinLisääTab.Visibility = Visibility.Collapsed;
                EläinLisääTab.IsEnabled = true;
            }
        }

        private void kirjauduUlosButton_Click(object sender, RoutedEventArgs e)
        {
            // Kun käyttäjä klikkaa "Kirjaudu ulos", tyhjää käyttämämme tilatieto
            kirjautunutUsername = "";
            MessageBox.Show("Kirjauduit ulos.");
            // Päivitä käyttöliittymä, koska käyttäjä kirjautui ulos
            PäivitäKäyttöliittymä();
        }

        #endregion

        private Klinikka valittuKlinikka;
        public MainWindow()
        {
            valittuKlinikka = new Klinikka("Palosaaren Klinikka", "Pikitehtaankatuu 19");

            valittuKlinikka.LuoMockDataa(30);

            InitializeComponent();

            //kerrotaan listview komponentille, että sen pitää näyttää
            // valitun kirjaston teokset listaa sen sisällä
            //ListSource kertoo itemeiden lähteen ja jokainen näytetään rivinä ListViewn sisällä
            dataGrid.ItemsSource = valittuKlinikka.Eläimet;
        }

        #region HAKUTAB
        private void nimiBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string hakuTeksti = nimiBox.Text;
            bool onTyhjä = string.IsNullOrWhiteSpace(hakuTeksti);

            if (onTyhjä == false)
            {
                haeButton.IsEnabled = true;
                tyhjääButton.IsEnabled = true;
            }
            else
            {
                haeButton.IsEnabled = false;
                tyhjääButton.IsEnabled = false;
            }
        }

        private void haeButton_Click(object sender, RoutedEventArgs e)
        {
            string hakutermi = nimiBox.Text.Trim();

            List<Eläin> osumat = valittuKlinikka.Eläimet.FindAll(
                x => x.EläimenNimi.Contains(hakutermi) == true ||
                     x.EläimenKuvaus.Contains(hakutermi) == true
                );

            // päivittää teoslistan hakijalle
            dataGrid.ItemsSource = osumat;
        }

        private void tyhjääButton_Click(object sender, RoutedEventArgs e)
        {
            nimiBox.Text = "";
            //TODO tyhjää myös muut hakuvalinnat
            tyyppiBox.SelectedIndex = -1;
            lajiBox.SelectedIndex = -1;
            //Palautetaan alkuperäinen lista
            dataGrid.ItemsSource = valittuKlinikka.Eläimet;
        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                Eläin klikattuEläin = (Eläin)dataGrid.SelectedItem;

                TabItem välilehtiJoOlemassa = null;
                for (int i = 0; i < tabControl.Items.Count; i++)
                {
                    TabItem t = (TabItem)tabControl.Items.GetItemAt(i);
                    string tHeader = (string)t.Header;
                    if (tHeader.Contains(klikattuEläin.EläimenNimi) == true)
                    {
                        välilehtiJoOlemassa = t;
                        break;
                    }
                }

                if (välilehtiJoOlemassa != null)
                {
                    tabControl.SelectedItem = välilehtiJoOlemassa;
                }
                else
                {
                    CustomTabItem t = new CustomTabItem();
                    // Luodaan olio tab item sisällöstä
                    CustomTabContent tcont = new CustomTabContent();

                    // Laitetaan tab itemin otsikkoon teksti
                    t.Header = klikattuEläin.EläimenNimi;

                    // Tämä on olio siitä testidatasta, jota halutaan näyttää (esim. Teos/Asiakas)
                    Eläin tl = new Eläin(1, "Testi", "testi2");
                    // Asetetaan testiolioon joku näytettävä data


                    //tl.Sivumäärä = klikattuTeos.Sivumäärä;
                    //tl.Kesto = klikattuTeos.Kesto;
                    tl.EläimenKuvaus = klikattuEläin.EläimenKuvaus;

                    // Sijoitetaan TabItem sisällön DataContext-muuttujaan yllä luotu olio
                    // DataContext:iin sijoitettu olio on se, johon DataBinding-linkitykset tehdään
                    //tcont.DataContext = tl;

                    // Asetetaan TabItem:n sisällöksi meidän CustomTabContent olio

                    tcont.DataContext = tl;
                    t.Content = tcont;

                    // Lisätään lopuksi meidän custom tab item näkymään
                    tabControl.Items.Add(t);
                    tabControl.SelectedItem = t;

                }

            }
        }
        #endregion

        #region ASIAKASTAB
        private void tyhjääButtonAsiakas_Click(object sender, RoutedEventArgs e)
        {
            textbox_AsiakasOsoite.Text = "";
            textbox_AID.Text = "";
            textbox_AsiakasEmail.Text = "";
            textbox_AsiakasNimi.Text = "";
            textbox_AsiakasPaikkakunta.Text = "";
            textbox_AsiakasOsoite.Text = "";
            textbox_AsiakasPuhnro.Text = "";
            textbox_AsiakasSukunimi.Text = "";
        }

        private void BTN_AsiakasSV_Click(object sender, RoutedEventArgs e)
        {
            DatabaseHandler.Instance.AddAsiakas(textbox_AID.Text, textbox_AsiakasNimi.Text, textbox_AsiakasSukunimi.Text);         
        }
        #endregion

        #region ELÄINTAB
        private void tyhjääButtonPotilas_Click(object sender, RoutedEventArgs e)
        {
            textBox_EläinIkä.Text = "";
            textBox_EläinNimi.Text = "";
            textBox_EläinPaino.Text = "";
            textBox_Laji.Text = "";
            textBox_OmistajaHetu.Text = "";
            textBox_Rotu.Text = "";
        }

        private void BTN_EläinSV_Click(object sender, RoutedEventArgs e)
        {
            int x = int.Parse(textBox_EläinIkä.Text);

            DatabaseHandler.Instance.AddEläin(textBox_EläinNimi.Text, textBox_Laji.Text, textBox_Rotu.Text, x);
        }
        #endregion
    }
}
