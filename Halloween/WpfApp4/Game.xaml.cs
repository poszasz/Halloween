using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using WpfApp4.Models;
using WpfApp4.Networking;

namespace WpfApp4
{
    
    public partial class Game : Window
    {
        Stopwatch stopwatch = new Stopwatch();
        double jatekIdo = 60;
        Jatek jatek = new Jatek();
        int osszpont = 0;
        bool jatekVege = false;
        bool cheatAktiv = false;
        DispatcherTimer cheatTimer = new DispatcherTimer();
        public Game(Jatek jatek)
        {
            InitializeComponent();
            this.jatek = jatek;

            rekordPont.Text = $"Rekord: {jatek.Pont} pont";
            nev.Text =jatek.Nev.ToString();
            stopwatch.Start();
            tarolo.Children.OfType<Image>().ToList().ForEach(x => x.MouseDown += X_MouseDown);
            CompositionTarget.Rendering += (_, __) => KepernyoFrissites();

            cheatTimer.Interval = TimeSpan.FromSeconds(3);
            cheatTimer.Tick += (s, e) => {
                cheatIndicator.Text = "";
                cheatTimer.Stop();
            };
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                cheatAktiv = !cheatAktiv;

                if (cheatAktiv)
                {
                    cheatIndicator.Text = "BAROSS CHEAT AKTÍV";
                    cheatTimer.Stop();
                    cheatTimer.Start();
                }
                else
                {
                    cheatIndicator.Text = "BAROSS CHEAT INAKTÍV";
                    cheatTimer.Stop();
                    cheatTimer.Start();
                }
            }
        }

        private void X_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Image img = sender as Image;
            char elsoKarakter = (img.Source as BitmapImage).UriSource.ToString().Split('/')[1][0];
            if (elsoKarakter >= '0' && elsoKarakter <= '9')
            {
                int szam = Convert.ToInt32(elsoKarakter.ToString());
                osszpont += szam;
                if (osszpont>jatek.Pont)
                {
                    rekordPont.Text= $"Rekord: {osszpont} pont";
                }
                jelenlegiPont.Text = $"jelenlegi: {osszpont} pont";
            }
        }
        

        private void KepernyoFrissites()
        {
            double eltelt = stopwatch.Elapsed.TotalSeconds;
            if (jatekIdo<eltelt && jatekVege == false)
            {
                jatekVege = true;
                Felhasznalo felh = new Felhasznalo()
                {
                    Azon = jatek.Azon,
                    Nev = jatek.Nev,
                    Pont = osszpont
                };
                API.SaveData(felh);
                Score score = new Score();
                score.Show();
                this.Close();
            }
            else if(!(jatekIdo<eltelt))
            {
                double hatralevo = jatekIdo - eltelt;
                ido.Text = $"{Math.Round(hatralevo)} mp";
                tokfejMegjeleniese(eltelt);
            }
            
        }

        private void tokfejMegjeleniese(double ido)
        {
            List<Image> tokfejek = tarolo.Children.OfType<Image>().ToList();
            foreach (var img in tokfejek)
            {
                img.Source = new BitmapImage(new Uri("Images/sima_tokfej.png", UriKind.Relative));
            }
            if (cheatAktiv)
            {
                foreach (var esemeny in jatek.Esemenyek.Where(x => x.NyitasIdo - 1.0 <= ido && ido < x.ZarasIdo))
                {
                   
                    if (ido >= esemeny.NyitasIdo - 1.0 && ido < esemeny.NyitasIdo)
                    {
                        Image img = tokfejek[esemeny.TokAzon];
                        img.Source = new BitmapImage(new Uri($"Images/cheat_tokfej.png", UriKind.Relative));
                    }
                }
            }
            foreach (var esemeny in jatek.Esemenyek.Where(x=> x.NyitasIdo<=ido && ido<x.ZarasIdo))
            {
                Image img = tokfejek[esemeny.TokAzon];
                img.Source = new BitmapImage(new Uri($"Images/{esemeny.Erteke}_nyitott_tokfej.png", UriKind.Relative));
            }
        }
    }
}
