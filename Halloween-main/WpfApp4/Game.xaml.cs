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
using WpfApp4.Models;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        Stopwatch stopwatch = new Stopwatch();
        double jatekIdo = 60;
        Jatek jatek = new Jatek();
        public Game(Jatek jatek)
        {
            InitializeComponent();
            this.jatek = jatek;
            rekordPont.Text = $"Rekord: {jatek.Pont} pont";
            nev.Text =jatek.Nev.ToString();
            stopwatch.Start();


            CompositionTarget.Rendering += (_, __) => KepernyoFrissites();
        }

        private void KepernyoFrissites()
        {
            double eltelt = stopwatch.Elapsed.TotalSeconds;
            if (jatekIdo<eltelt)
            {

            }
            else
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
            foreach (var esemeny in jatek.Esemenyek.Where(x=> x.NyitasIdo<=ido && ido<x.ZarasIdo))
            {
                Image img = tokfejek[esemeny.TokAzon];
                img.Source = new BitmapImage(new Uri("Images/0_nyitott_tokfej.png", UriKind.Relative));
            }
        }
    }
}
