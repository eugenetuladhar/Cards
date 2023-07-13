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
            Cardlogiccs.CurrentGame = GameType.KITTY;
            //New card instance
            CardCompleteDeck carddeck = new CardCompleteDeck();
            Console.WriteLine("*** New Card deck created ***");
            // pleyer logic
            List<Player.Player> playerlist = new List<Player.Player>();
            bool menu = true;
            int numofplayers = 0;
            while (menu)
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
                if (numofplayers > 1 && numofplayers < 11)
                {
                    menu = false;
                }
                else
                {
                    Console.WriteLine(" Try Again! ");
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
                    playerlist[i].cardsinInteger =Cardlogiccs.GetCardIntegerValue(playerlist[i].CardInHand);
                    playerlist[i].cardsinInteger.Sort();
                    if (CheckXnumCards(playerlist[i],4,CardStrength.Quad))// checking quads
                    {
                        break;
                    }
                    else if (CheckXnumCards(playerlist[i],3,CardStrength.Trial)) // checking trial
                    {

                    }
                    else if (CheckColorRunCards(playerlist[i].CardInHand))
                    {
                    }
                    //else if (CheckRun(playerlist[i].CardInHand))
                    //{

                    //}
                    //else if (CheckColor(playerlist[i].CardInHand))
                    //{

                    //}
                    //else if (CheckJoot(playerlist[i].CardInHand))
                    //{

                    //}
                    else
                    {

                    }
                }
            }
        }

        private static bool CheckColorRunCards(List<Card> cardInHand)
        {
            return true;
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
                        p.kittyStrength[cardstrength].Add( card.GetCardValue());
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
                    for (int i = 0; i < p.kittyStrength[cardstrength].Count; i++)
                    {
                        if (card.GetCardValue() == p.kittyStrength[cardstrength][i]) 
                        {
                            p.Remove(card);
                            p.FinalKittyHand.Add(card);
                        }

                    }
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
