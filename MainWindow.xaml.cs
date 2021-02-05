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

namespace Korpa2019
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Dictionary<int, int> korpa = new Dictionary<int, int>();
        private List<Proizvod> listaProizvoda = ProizvodDal.VratiProizvode();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PrikaziKupce()
        {
            ComboBox1.ItemsSource = KupacDal.VratiKupce();
        }

        private void PrikaziProizvode()
        {
            ListBox1.ItemsSource = listaProizvoda;
        }
        private void DozvoliKupovinu(bool dozvola)
        {
            GroupBox1.IsEnabled = dozvola;
            ButtonNova.IsEnabled = !dozvola;
        }

        private void ButtonNova_Click(object sender, RoutedEventArgs e)
        {
            DozvoliKupovinu(true);
            Resetuj();
        }

        private void ListBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox1.SelectedIndex > -1)
            {
                Proizvod p = ListBox1.SelectedItem as Proizvod;
                TextBlockCena.Text = p.Cena.ToString();
                TextBoxKolicina.Text = "1";
                TextBoxKolicina.Focus();
            }
        }

        private void DodajUkorpu(StavkeKorpe st)
        {
            int id = st.ProizvodId;
            if (korpa.ContainsKey(id))
            {
                korpa[id] += st.Kolicina;
            }
            else
            {
                korpa.Add(id, st.Kolicina);
            }
        }

            private decimal VrednostKorpe()
        {
            decimal suma = 0;
            foreach (var stavka in korpa)
            {
                Proizvod p1 = listaProizvoda.SingleOrDefault(p => p.ProizvodId == stavka.Key);
                suma += stavka.Value * p1.Cena;
            }
            return suma;
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int id = (int)b.CommandParameter;
            korpa.Remove(id);
            StampajKorpu();
        }

        private void ButtonKupi_Click(object sender, RoutedEventArgs e)
        {
            if (ComboBox1.SelectedIndex > -1)
            {
                Kupac k = ComboBox1.SelectedItem as Kupac;
                if (korpa.Count > 0)
                {
                    int rezultat = KorpaDal.SacuvajKorpu(korpa, k.KupacId);
                    if (rezultat == 0)
                    {
                        MessageBox.Show("Kupovina izvrsena");
                        DozvoliKupovinu(false);
                    }
                    else
                    {
                        MessageBox.Show("Greska pri cuvanju podataka");
                    }
                }
                else
                {
                    MessageBox.Show("Vasa korpa je prazna");
                }
            }
            else
            {
                MessageBox.Show("Odaberite kupca");
            }
        }

        private void ButtonOdustani_Click(object sender, RoutedEventArgs e)
        {
            Resetuj();
            DozvoliKupovinu(false);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            int id = (int)b.CommandParameter;
            Proizvod p = listaProizvoda.SingleOrDefault(p1 => p1.ProizvodId == id);
            Window1 w1 = new Window1();
            w1.Title = "Promenite kolicinu";
            w1.TextBlock1.Text = "Proizvod: " + p.Naziv;
            w1.TextBox1.Text = korpa[id].ToString();
            if (w1.ShowDialog() == true)
            {
                korpa[id] = int.Parse(w1.TextBox1.Text);
                StampajKorpu();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrikaziKupce();
            PrikaziProizvode();
            DozvoliKupovinu(false);
        }
        public void Resetuj()
        {
            ComboBox1.SelectedIndex = -1;
            ListBox1.SelectedIndex = -1;
            DataGrid1.Items.Clear();
            TextBoxKolicina.Text = "1";
            TextBlockCena.Text = "";
            TextBlockVrijednost.Text = "";
            korpa.Clear();

        }
        private void StampajKorpu()
        {
            DataGrid1.Items.Clear();
            foreach (var st in korpa)
            {
                Proizvod p1 = listaProizvoda.SingleOrDefault(p => p.ProizvodId == st.Key);
                StavkaView pv = new StavkaView
                {
                    ProizvodId = st.Key,
                    Naziv = p1.Naziv,
                    Cena = p1.Cena,
                    Kolicina = st.Value
                };
                DataGrid1.Items.Add(pv);
            }
            DataGrid1.Focus();
            int indeks = DataGrid1.Items.Count - 1;
            DataGrid1.ScrollIntoView(DataGrid1.Items[indeks]);
            DataGrid1.SelectedIndex = indeks;
            TextBlockVrijednost.Text = VrednostKorpe().ToString();
        }

        private void ButtonDodaj_Click(object sender, RoutedEventArgs e)
        {
            if (ListBox1.SelectedIndex > -1)
            {
                if (int.TryParse(TextBoxKolicina.Text, out int kolicina))
                {
                    StavkeKorpe st = new StavkeKorpe();
                    Proizvod p1 = ListBox1.SelectedItem as Proizvod;
                    st.ProizvodId = p1.ProizvodId;
                    st.Kolicina = kolicina;
                    DodajUkorpu(st);
                    StampajKorpu();


                }
                else
                {
                    MessageBox.Show("Unesite broj");
                }
            }
            else
            {
                MessageBox.Show("Odeberi proizvod");
            }

        }
    }
}
