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
                // Demotarkistus, kovakoodatut arvot
                // Nämä haettaisiin tietokannasta tmv
                /*if (username.CompareTo("Hellu") == 0 &&
                    pw.CompareTo("qwerty") == 0)
                {
                    MessageBox.Show("Kirjautunut käyttäjä: " + username);

                    // Tallennetaan kirjautuneen käyttäjän username muuttujaan, tilatietona
                    kirjautunutUsername = username;

                    // Päivitetään käyttöliittymä, koska käyttäjä on kirjautunut
                    PäivitäKäyttöliittymä();
                }
                else
                {
                    MessageBox.Show("Väärä tunnus tai salasana");
                }*/
            }

        }

        private void PäivitäKäyttöliittymä()
        {
            // Tarkistetaan, onko käyttäjä kirjautuneena (onko kirjautunutUsername muuttujassa joku arvo)
            bool kirjautunut = String.IsNullOrEmpty(kirjautunutUsername) == false;

            if (kirjautunut == true)
            {
                // Jos on, näytä "Kirjaudu ulos" painike ja disabloi "Kirjaudu" painike
                kirjauduUlosButton.Visibility = Visibility.Visible;
                avaaKirjButton.IsEnabled = false;
                //Tabien näkyvyys
                AsiakasHakuTab.Visibility = Visibility.Visible;
                AsiakasLisääTab.Visibility = Visibility.Visible;
                HakuTab.Visibility = Visibility.Visible;
                AsiakasLisääTab.IsEnabled = true;
                HakuTab.IsEnabled = true;
                AsiakasHakuTab.IsEnabled = true;
            }
            else
            {
                // Jos ei ole kirjautuneena, piilota "Kirjaudu ulos" painike
                kirjauduUlosButton.Visibility = Visibility.Hidden;
                // Enabloi "Kirjaudu" painike
                avaaKirjButton.IsEnabled = true;
                //Tabien näkyvyys
                AsiakasHakuTab.Visibility = Visibility.Hidden;
                AsiakasLisääTab.Visibility = Visibility.Hidden;
                HakuTab.Visibility = Visibility.Hidden;
                AsiakasLisääTab.IsEnabled = false;
                HakuTab.IsEnabled = false;
                AsiakasHakuTab.IsEnabled = false;

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

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string hakuTeksti = textBox.Text;
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
            string hakutermi = textBox.Text.Trim();

            List<Eläin> osumat = valittuKlinikka.Eläimet.FindAll(
                x => x.EläimenNimi.Contains(hakutermi) == true ||
                     x.EläimenKuvaus.Contains(hakutermi) == true
                );

            // päivittää teoslistan hakijalle
            dataGrid.ItemsSource = osumat;
        }

        private void tyhjääButton_Click(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
            //TODO tyhjää myös muut hakuvalinnat

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

   }
}
