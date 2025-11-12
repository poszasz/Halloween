using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp4.Models;
using NetworkHelper;

namespace WpfApp4.Networking
{
    public class API
    {
        const string baseUrl = "http://localhost:3000/api";
       public static List<Felhasznalo> GetLeaderboard()
       {
            return Backend.GET(baseUrl + "/leaderboard").Send().As<List<Felhasznalo>>();
       }


        public static Jatek StartGame(string nev)
        {

            Response r = Backend.POST(baseUrl + "/start/" + nev).Send();
            return new Jatek
            {
                Azon = r.ValueOf("azon").As<int>(),
                Nev = r.ValueOf("nev").As<string>(),
                Pont = r.ValueOf("pont").As<int>(),
                Esemenyek = r.ValueOf("esemenyek").As<List<Esemeny>>()
            };
        }


        public static string SaveData( Felhasznalo felhasznalo)
        {
            return Backend.POST(baseUrl + "/score/" + felhasznalo.Azon).Body(felhasznalo).Send().As<string>();
        }
    }
}
