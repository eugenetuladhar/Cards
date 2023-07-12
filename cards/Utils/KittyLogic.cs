using cards.Cards_files;
using cards.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Utils
{
    public class KittyLogic
    {
        public static void RunKitty()
        {
            //New card instance
            CardCompleteDeck carddeck = new CardCompleteDeck();
            Console.WriteLine("*** New Card deck created ***");
            // pleyer logic
            List<Player.Player> playerlist = new List<Player.Player>();
            bool t = true;
            int numofplayers = 0;
            while (t)
            {
                Console.WriteLine(" Enter number of players :");
                try
                {
                    numofplayers = int.Parse(Console.ReadLine());

                }
                catch
                {
                    Console.WriteLine("You have entered Invalid Number ");
                }
                switch (numofplayers)
                {
                    case 2:
                        t = false;
                        break;
                    case 3:
                        t = false;
                        break;
                    case 4:
                        t = false;
                        break;
                    case 5:
                        t = false;
                        break;
                    default:
                        Console.WriteLine(" Invalid number or too high ");
                        Console.WriteLine(" Again! ");
                        break;
                }
            }

            playerlist = Cardlogiccs.GetPlayers(numofplayers);

            //shuffle
            carddeck.Shuffle();

            //Deal 
            KittyDeal(playerlist, carddeck);
            Console.ReadLine();

            // start game
            StartPlaying(playerlist);
        }

        private static void StartPlaying(List<Player.Player> playerlist)
        {
            //human player

            // comp player
            for (int i = 0; i < playerlist.Count(); i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    if (CheckXnumCards(playerlist[i],4,CardStrength.Quad))// checking quads
                    {
                        
                    }
                    else if (CheckXnumCards(playerlist[i],3,CardStrength.Trial))
                    {

                    }
                    else if (CheckColor(playerlist[i].CardInHand) && CheckRun(playerlist[i].CardInHand))
                    {
                        p.strength = CardStrength.ColorSequence;
                        p.cardsinInteger = GetCardValue(playerlist[i].CardInHand);
                        p.cardsinInteger.Sort();
                    }
                    else if (CheckRun(playerlist[i].CardInHand))
                    {
                        p.strength = CardStrength.Sequence;
                        p.cardsinInteger = GetCardValue(playerlist[i].CardInHand);
                        p.cardsinInteger.Sort();

                    }
                    else if (CheckColor(playerlist[i].CardInHand))
                    {
                        p.strength = CardStrength.Color;
                        p.cardsinInteger = GetCardValue(p.CardInHand);
                        p.cardsinInteger.Sort();

                    }
                    else if (CheckJoot(playerlist[i].CardInHand))
                    {
                        p.strength = CardStrength.Double;
                        p.cardsinInteger = GetCardValue(p.CardInHand);
                        p.cardsinInteger.Sort();

                    }
                    else
                    {
                        p.strength = CardStrength.Common;
                        p.cardsinInteger = GetCardValue(p.CardInHand);
                        p.cardsinInteger.Sort();

                    }
                }
            }
        }

        private static bool CheckXnumCards(Player.Player p,int num,CardStrength cardstrength)
        {
            Dictionary<CardValue, int> countMap = new Dictionary<CardValue, int>();
            foreach (var card in p.CardInHand)
            {
                if (countMap.ContainsKey(card.GetCardValue()))
                {
                    countMap[card.GetCardValue()]++;
                    if (countMap[card.GetCardValue()] >= num)
                    {
                        p.kittyStrength[cardstrength] = card.GetCardValue();
                    }
                }
                else
                {
                    countMap[card.GetCardValue()] = 1;
                }

            }
            if ( p.kittyStrength.ContainsKey(cardstrength))
            {
                // removing card in hand
                foreach (var card in p.CardInHand)
                {
                    if (card.GetCardValue() == p.kittyStrength[cardstrength]) { }
                }
                return true;
            }
            return false;
        }

        public static void KittyDeal(List<Player.Player> list, CardCompleteDeck c)
        {
            int numberofcardstodeal = 9;
            Cardlogiccs.Deal(list, c, numberofcardstodeal, false);
        }
    }
}
