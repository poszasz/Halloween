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
using WpfApp4.Networking;

namespace WpfApp4
{
    /// <summary>
    /// Interaction logic for Score.xaml
    /// </summary>
    public partial class Score : Window
    {
        public Score()
        {
            InitializeComponent();
            tarolo.Children.Clear();
            foreach (var item in API.GetLeaderboard())
            {
                ScoreRowUC suc = new ScoreRowUC();
                suc.nev.Text = item.Nev;
                suc.pont.Text = $"{item.Pont} pont";
                tarolo.Children.Add(suc);
            }
        }

       
    }
}
