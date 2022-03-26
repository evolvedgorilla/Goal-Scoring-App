using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Xml;
using MessageBox = System.Windows.Forms.MessageBox;

namespace smLiiga
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        struct pelaajaTiedot
        {
            public string nimi;
            public string nro;
            public int maalienLkm;
        }


        pelaajaTiedot[] kotiPelaaja = new pelaajaTiedot[100];
        pelaajaTiedot[] vierasPelaaja = new pelaajaTiedot[100];

        int kotiMaalit = 0;
        int vierasMaalit = 0;

        string kotiJoukkue = "kotiJoukkue";
        string vierasJoukkue = "vierasJoukkue";
        public MainWindow()
        {

            InitializeComponent();

            kalenteri.DisplayDateStart = DateTime.Now;

            KirjaaOttelunTiedot();
            

            XmlReader lukija = XmlReader.Create("SMliiga.xml");
            string joukkue = "";
            lukija.MoveToContent();

            while (lukija.Read())
            {
                if (lukija.NodeType == XmlNodeType.Element &&
                    lukija.Name == "Joukkue")
                {
                    if (lukija.HasAttributes)
                    {
                        joukkue = lukija.GetAttribute("nimi");
                        lstKotijoukkue.Items.Add(joukkue);
                        lstVierasjoukkue.Items.Add(joukkue);
                    }
                }

            }
        }

        void tyhjennäkotiPelaajaArray()
        {
            for (int i = 0; i < 100; i++)
            {
                //koti
                kotiPelaaja[i].nimi = "";
                kotiPelaaja[i].nro = "";
                kotiPelaaja[i].maalienLkm = 0;


            }
        }

        void tyhjennävierasPelaajaArray()
        {
            for (int i = 0; i < 100; i++)
            {

                
                vierasPelaaja[i].nimi = "";
                vierasPelaaja[i].nro = "";
                vierasPelaaja[i].maalienLkm = 0;

            }
        }
        private void lstKotijoukkue_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

            if (tarkista())
            {
               
            }
            else
            {
               DialogResult vastaus = MessageBox.Show("Haluatko lopettaa ottelun?", "Varmista", (MessageBoxButtons)MessageBoxButton.OKCancel);

                if (vastaus == System.Windows.Forms.DialogResult.OK)
                {

                    lblVierasMaalit.Content = ("0");
                    lstVierasMaalit.Items.Clear();   // poistetaa maalintekijäit
                    vierasMaalit = 0;                // aloitetaan ottelu nollamaalein
                    vierasPelaaja[lstVieraspelaajat.SelectedIndex].maalienLkm = 0;  //tyhjennetään pelaajien maaliLkm         
                }
                if(vastaus == System.Windows.Forms.DialogResult.Cancel)
                {

                    return;
                }
                
            }
            

                                         //Tyhjää pelaajia sisältävän arraytaulun    
            tyhjennäkotiPelaajaArray();  //poistetaan pelaajat muistista
            lstKotipelaajat.Items.Clear(); //poistetaan pelaajat käyttöliittymästä
            lstKotiMaalit.Items.Clear();   // poistetaa maalintekijäit
            kotiMaalit = 0;                // aloitetaan ottelu nollamaalein

            string haettavaJoukkue = lstKotijoukkue.SelectedItem.ToString();
            kotiJoukkue = haettavaJoukkue;


            lblKotijoukkue.Content = haettavaJoukkue;
            lblKotiMaalit.Content = kotiMaalit;
        //    KirjaaOttelunTiedot();
            XmlReader lukija = XmlReader.Create("SMliiga.xml");
            string joukkue = "";

            lukija.MoveToContent();
            KirjaaOttelunTiedot();
            int ind = 0;

            while (lukija.Read())
            {
                if (lukija.NodeType == XmlNodeType.Element &&
                    lukija.Name == "Joukkue")
                {
                    if (lukija.HasAttributes)
                    {
                        joukkue = lukija.GetAttribute("nimi");
                        if (joukkue == haettavaJoukkue)
                        {
                            while (lukija.Read())
                            {
                                // lopetetaan tämä silmukka, kun joukkue vaihtuu (huom. EndElement)
                                // break-käsky lopettaa silmukan
                                if (lukija.NodeType == XmlNodeType.EndElement &&
                                    lukija.Name == "Joukkue")
                                {

                                    break;
                                }

                                if (lukija.NodeType == XmlNodeType.Element &&
                                 lukija.Name == "Nimi")
                                {
                                    lukija.Read();
                                    kotiPelaaja[ind].nimi = lukija.Value;
                                    lstKotipelaajat.Items.Add(lukija.Value);

                                }
                                if (lukija.NodeType == XmlNodeType.Element &&
                                      lukija.Name == "Pelaajanro")
                                {
                                    lukija.Read();
                                    kotiPelaaja[ind].nro = lukija.Value;
                                    ind++;

                                }
                            }
                        }
                    }
                }

            }
  
        }


        private void lstVierasjoukkue_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (tarkista())
            {

            }
            else
            {
                DialogResult vastaus = MessageBox.Show("Haluatko lopettaa ottelun?", "Varmista", (MessageBoxButtons)MessageBoxButton.OKCancel);

                if(vastaus == System.Windows.Forms.DialogResult.OK)
                {
                    lblKotiMaalit.Content = ("0");
                    lstKotiMaalit.Items.Clear();   // poistetaa maalintekijäit
                    kotiMaalit = 0;                // aloitetaan ottelu nollamaalein
                    kotiPelaaja[lstKotipelaajat.SelectedIndex].maalienLkm = 0;
                }
                if (vastaus == System.Windows.Forms.DialogResult.Cancel)
                {

                    return;
                }

            }

            tyhjennävierasPelaajaArray();  //poistetaan pelaajat muistista
            lstVieraspelaajat.Items.Clear(); //poistetaan pelaajat käyttöliittymästä
            lstVierasMaalit.Items.Clear();   // poistetaa maalintekijäit
            vierasMaalit = 0;                // aloitetaan ottelu nollamaalein

            string haettavaJoukkue = lstVierasjoukkue.SelectedItem.ToString();
            vierasJoukkue = haettavaJoukkue;

            
            lblVierasjoukkue.Content = haettavaJoukkue;
            lblVierasMaalit.Content = vierasMaalit;
     //       KirjaaOttelunTiedot();
            XmlReader lukija = XmlReader.Create("SMliiga.xml");
            string joukkue = "";

            lukija.MoveToContent();
            KirjaaOttelunTiedot();
            int ind = 0;
            

            while (lukija.Read())
            {
                if (lukija.NodeType == XmlNodeType.Element &&
                    lukija.Name == "Joukkue")
                {
                    if (lukija.HasAttributes)
                    {
                        joukkue = lukija.GetAttribute("nimi");
                        if (joukkue == haettavaJoukkue)
                        {
                            while (lukija.Read())
                            {
                                // lopetetaan tämä silmukka, kun joukkue vaihtuu (huom. EndElement)
                                // break-käsky lopettaa silmukan
                                if (lukija.NodeType == XmlNodeType.EndElement &&
                                    lukija.Name == "Joukkue")
                                {

                                    break;

                                }

                                if (lukija.NodeType == XmlNodeType.Element &&
                                lukija.Name == "Nimi")
                                {
                                    lukija.Read();
                                    vierasPelaaja[ind].nimi = lukija.Value;
                                    lstVieraspelaajat.Items.Add(lukija.Value);

                                    if (lstVieraspelaajat.Items.Count >= 0 && lblVierasjoukkue.Content == "")
                                    {
                                        lstVieraspelaajat.Items.Clear();
                                    }
                                }
                                if (lukija.NodeType == XmlNodeType.Element &&
                                      lukija.Name == "Pelaajanro")
                                {
                                    lukija.Read();
                                    vierasPelaaja[ind].nro = lukija.Value;
                                    ind++;
                                }

                            }
                        }
                    }
                }

            }

        }


        private void btnKirjaaKotiMaali_Click(object sender, RoutedEventArgs e)
        {
            if (lstKotipelaajat.SelectedIndex < 0)
            {
                MessageBox.Show("Valitse maalin tehnyt pelaaja");
                return;
            }

            //lisää maalin kellon aika
            DateTime tämäHetki = DateTime.Now;
            string kellonAika = tämäHetki.ToShortTimeString();

            int pelinro = lstKotipelaajat.SelectedIndex;
            int maalinTekijäInd = lstKotipelaajat.SelectedIndex;
            string pelaajanNro = kotiPelaaja[pelinro].nro;
            string pelaajanNimi = kotiPelaaja[maalinTekijäInd].nimi;



            if (vierasJoukkue == "vierasJoukkue")
            {
                MessageBox.Show("Valitse vierasjoukkue.");
            }
            else if (vierasJoukkue != "vierasJoukkue")
            {
                lstKotiMaalit.Items.Add(kellonAika + " " + pelaajanNro + " " + pelaajanNimi);
                kotiMaalit++;
                KirjaaOttelunTiedot();
                lblKotiMaalit.Content = kotiMaalit;

                kotiPelaaja[maalinTekijäInd].maalienLkm++;

                Onnittelu();
            }


        }


        private void btnKirjaaVierasMaali_Click(object sender, RoutedEventArgs e)
        {
            if (lstVieraspelaajat.SelectedIndex < 0)
            {
                MessageBox.Show("Valitse maalin tehnyt pelaaja");
                return;
            }

            //lisää maalin kellon aika
            DateTime tämäHetki = DateTime.Now;
            string kellonAika = tämäHetki.ToShortTimeString();
            int pelinro = lstVieraspelaajat.SelectedIndex;
            int maalinTekijäInd = lstVieraspelaajat.SelectedIndex;
            string pelaajanNro = vierasPelaaja[pelinro].nro;
            string pelaajanNimi = vierasPelaaja[maalinTekijäInd].nimi;

            if (kotiJoukkue == "kotiJoukkue")
            {
                MessageBox.Show("Valitse kotijoukkue.");
            }
            else if (kotiJoukkue != "kotiJoukkue")
            {
                lstVierasMaalit.Items.Add(kellonAika + " " + pelaajanNro + " " + pelaajanNimi);
                vierasMaalit++;
                KirjaaOttelunTiedot();

                lblVierasMaalit.Content = vierasMaalit;

                vierasPelaaja[maalinTekijäInd].maalienLkm++;
                vierasOnnittelu();
            }

        }

        void Onnittelu()
        {

            int tiedot = lstKotipelaajat.SelectedIndex;
            string pelaajanNimi = kotiPelaaja[tiedot].nimi;
            int maalinTekijäInd = lstKotipelaajat.SelectedIndex;
            int maali = kotiPelaaja[maalinTekijäInd].maalienLkm;

            if (maali == 3)
            {
                MessageBox.Show("Onnea " + pelaajanNimi + " hattutempusta.");
            }
            if (maali > 3)
            {
                MessageBox.Show("Onnea " + pelaajanNimi + " " + maali + ":stä maalista ja " + "hattutempusta.");
            }

            return;
        }
        void vierasOnnittelu()
        {
            int vierasTiedot = lstVieraspelaajat.SelectedIndex;
            string vierasPelaajanNimi = vierasPelaaja[vierasTiedot].nimi;
            int maalinTekijäInd = lstVieraspelaajat.SelectedIndex;
            int vierasMaali = vierasPelaaja[maalinTekijäInd].maalienLkm;

            if (vierasMaali == 3)
            {
                MessageBox.Show("Onnea " + vierasPelaajanNimi + " hattutempusta.");
            }
            if (vierasMaali > 3)
            {
                MessageBox.Show("Onnea " + vierasPelaajanNimi + " " + vierasMaali + ":stä maalista ja " + "hattutempusta.");

            }
            return;
        }

        void onkoVierasSamaKuinKoti()
        {
            if (kotiJoukkue == vierasJoukkue)
            {
                MessageBox.Show("Vierasjoukkue ei voi olla sama kuin kotijoukkue.");
                lblVierasjoukkue.Content = "";
                lblVierasMaalit.Content = "";
                lstVieraspelaajat.Items.Clear();
                vierasJoukkue = "vierasJoukkue";

            }

        }


        void KirjaaOttelunTiedot()
        {


            if (kalenteri.DisplayDate.Date == DateTime.Today.Date)
            {
                DateTime otteluPäivä = Convert.ToDateTime(kalenteri.SelectedDate);
                string otteluPvm = otteluPäivä.ToShortDateString();

                if (otteluPvm == "1.1.0001")
                {
                    otteluPvm = DateTime.Today.Date.ToShortDateString();
                    onkoVierasSamaKuinKoti();
                    lblPelinTiedot.Content = otteluPvm + " " + kotiJoukkue + " - " + vierasJoukkue;
                    lblTilanne.Content = kotiMaalit + " - " + vierasMaalit;


                }
                onkoVierasSamaKuinKoti();
                lblPelinTiedot.Content = otteluPvm + " " + kotiJoukkue + " - " + vierasJoukkue;
                lblTilanne.Content = kotiMaalit + " - " + vierasMaalit;

            }


            else if (kalenteri.SelectedDate != DateTime.Today)
            {
                DateTime dateTime = Convert.ToDateTime(kalenteri.SelectedDate);
                string otteluPvm = dateTime.ToShortDateString();

                onkoVierasSamaKuinKoti();
                lblPelinTiedot.Content = otteluPvm + " " + kotiJoukkue + " - " + vierasJoukkue;
                lblTilanne.Content = kotiMaalit + " - " + vierasMaalit;

            }

        }

        private void kalenteri_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            KirjaaOttelunTiedot();
        }

        bool tarkista()
        {
            
            if (kotiMaalit >= 1 || vierasMaalit >= 1)
            {
                return false;

            }
            else
            {
                return true;
            }
        }


    }


}
