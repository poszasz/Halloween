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
using WpfApp4.Models;
using WpfApp4.Networking;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window
    {
        public StartWindow()
        {
            InitializeComponent();
            belepes.Click += (_, __) =>
            {
                string n = nev.Text.Trim().Replace('/', '-').Replace(' ', '-');
                Jatek jatek =API.StartGame(n);
                Game G = new Game(jatek);
                G.Show();
                this.Close();
            };
        } 
    }
}
