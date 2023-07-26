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
            for (int i = 0; i < numberofcardstodeal; i++)
            {
                foreach (var name in list)
                {
                    c.Draw(name,reveal);
                }
            }
        }
        public static int ReadNumberOfPlayers(int MAXNUM)
        {
            
            int numofplayers = 0;
            while (true)
            {
                Console.Write($" Enter number of players (Min = 2 , Max = {MAXNUM}) : ");
                try
                {
                    numofplayers = int.Parse(Console.ReadLine());

                }
                catch
                {
                    Console.WriteLine("You have entered Invalid Number ");
                }
                if (numofplayers > 1 && numofplayers < 11)
                {
                    return numofplayers;
                }
                else
                {
                    Console.WriteLine(" Try Again! ");
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

                Console.WriteLine($"{player.GetName} {hashave} {player.strength} :");
                
                player.DisplayeCardInHand();
                Console.WriteLine();
            }

        }
        public static List<Player.Player> GetPlayers(int MAX_NUM_PLAYERS,bool needreadnumberofplayers=true)
        {
            int numofplayers;
            if (needreadnumberofplayers)
            {
                numofplayers = ReadNumberOfPlayers(MAX_NUM_PLAYERS);
            }
            else
            {
                numofplayers = MAX_NUM_PLAYERS;
            }
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
        public static List<Card> SwapPositionofCards(List<Card> cardlist,int from,int to) 
        {
            Card temp = cardlist[from];
            cardlist[from] = cardlist[to];
            cardlist[to] = temp;
            return cardlist;
        }
        public static void PrintCard(Card card)
        {
            if (card.GetCardType() == CardType.Diamond || card.GetCardType() == CardType.Heart)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write(" " + CardConversion.GetPrintedForm(card) + " ");
            }
            else
            {
                Console.Write(" " + CardConversion.GetPrintedForm(card) + " ");
            }
            Console.ResetColor();
        }
    }
}
