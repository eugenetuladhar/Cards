using cards.Cards_files;
using cards.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Utils
{
    public static class Cardlogiccs
    {
        public static void Deal(List<Player.Player> list, CardCompleteDeck c, int numberofcardstodeal,bool reveal = true)
        {
            Console.WriteLine("*** Dealing cards ***");
            for (int i = 0; i < numberofcardstodeal; i++)
            {
                foreach (var name in list)
                {
                    c.Draw(name,reveal);
                }
            }
        }

        public static void ShowAllPlayersCard(List<Player.Player> list)
        {
            foreach (var player in list)
            {
                string hashave = "has";
                if (player.Checkplayer_you())
                {
                    hashave = "have";
                }
                Console.WriteLine($" {player.GetName} {hashave} :");
                player.GetCardtoPrint();
            }

        }
        public static List<Player.Player> GetPlayers(int numofplayers)
        {
            List<Player.Player> playerlist = new List<Player.Player>();
            for (int i = 0; i < numofplayers; i++)
            {
                PlayerNames playername = (PlayerNames)i;
                playerlist.Add(new Player.Player(playername.ToString()));
            }
            return playerlist;
        }
    }
}
