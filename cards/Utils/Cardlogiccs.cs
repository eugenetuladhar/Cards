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
        public static GameType CurrentGame { get; set; }
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

                if (CurrentGame == GameType.FLASH)
                {
                    Console.WriteLine($" {player.GetName} {hashave} {player.strength} :");
                }
                else
                {
                    Console.WriteLine($" {player.GetName} {hashave} :");
                }
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
        public static List<int> GetCardIntegerValue(List<Card> cardInHand)
        {
            List<int> listvalue = new List<int>();
            foreach (var singlecard in cardInHand)
            {
                var value = singlecard.GetCardValue();
                listvalue.Add(CardConversion.ConversionValuetoInteger(value));
            }
            return listvalue;
        }
    }
}
