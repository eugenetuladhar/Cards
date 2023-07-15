using cards.Cards_files;
using cards.Player;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
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
            playerlist = Cardlogiccs.GetPlayers();

            //shuffle
            carddeck.Shuffle();

            //Deal 
            KittyDeal(playerlist, carddeck);

            // start game
            StartPlaying(playerlist);

            //Cardlogiccs.ShowAllPlayersCard(playerlist, true);
        }

        private static void StartPlaying(List<Player.Player> playerlist)
        {
            //human player

            // comp player
            for (int i = 0; i < playerlist.Count(); i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    playerlist[i].cardsinInteger = Cardlogiccs.GetCardIntegerValue(playerlist[i].CardInHand);
                    playerlist[i].cardsinInteger.Sort();
                    if (CheckXnumCards(playerlist[i], 4))// checking quads
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Quad);
                        break;
                    }
                    else if (CheckXnumCards(playerlist[i], 3)) // checking trial
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Trial);
                    }
                    else if (CheckColorRunCards(playerlist[i]))
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.ColorSequence);
                    }
                    else if (CheckRunCards(playerlist[i]))
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Sequence);
                    }
                    else if (CheckColorCards(playerlist[i]))
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Color);
                    }
                    else if (CheckXnumCards(playerlist[i], 2))
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Double);
                    }
                    else
                    {
                        HandleCommonCards(playerlist[i]);
                        playerlist[i].kittyStrength.Add(CardStrength.Common);
                    }
                    Console.WriteLine($"Checking Hand {j} of all players completed.");
                }
            }
        }

        private static void HandleCommonCards(Player.Player p)
        {
            for (int j = 0; j < p.cardsinInteger.Count(); j++)//Ace to 14 value
            {
                if (p.cardsinInteger[j] == 1)
                {
                    p.cardsinInteger[j] = 14;
                }
            }
            p.cardsinInteger.Sort();
            Card? c1 = p.CardInHand.Find(c => c.GetCardValue() == CardConversion.ConversionIntegertoValue(p.cardsinInteger[p.cardsinInteger.Count() - 1]));
            Card? c2 = p.CardInHand.Find(c => c.GetCardValue() == CardConversion.ConversionIntegertoValue(p.cardsinInteger[p.cardsinInteger.Count() - 2]));
            Card? c3 = p.CardInHand.Find(c => c.GetCardValue() == CardConversion.ConversionIntegertoValue(p.cardsinInteger[p.cardsinInteger.Count() - 3]));
            //remove from cardinhand and move to final hand
            if (c1 != null && c2 != null && c3 != null)
            {
                p.MovetoFinalKittyHand(c1);
                p.MovetoFinalKittyHand(c2);
                p.MovetoFinalKittyHand(c3);
            }
            else
            {
                Console.WriteLine("error in common");
            }
        }

        private static bool CheckColorCards(Player.Player p)
        {
            Dictionary<CardType, List<CardValue>> colorMap = new Dictionary<CardType, List<CardValue>>();
            for (int j = 0; j < p.cardsinInteger.Count(); j++)//Ace to 14 value
            {
                if (p.cardsinInteger[j] == 1)
                {
                    p.cardsinInteger[j] = 14;
                }
            }
            p.cardsinInteger.Sort();
            // check same card types
            for (int i = p.cardsinInteger.Count() - 1; i <= 0; i--)
            {
                // converting int to card
                CardValue cv = CardConversion.ConversionIntegertoValue(p.cardsinInteger[i]);
                List<Card>? foundcardlist = p.CardInHand.FindAll(c => c.GetCardValue() == cv);
                foreach (var card in foundcardlist)
                {
                    if (colorMap.ContainsKey(card.GetCardType()))
                    {
                        colorMap[card.GetCardType()].Add(card.GetCardValue());

                        if (colorMap[card.GetCardType()].Count() >= 3 &&
                            colorMap.TryGetValue(card.GetCardType(), out var values))
                        {
                            foreach (var item in values)
                            {
                                Card? c1 = p.CardInHand.Find(c => c.GetCardValue() == item && c.GetCardType() == card.GetCardType());
                                if (c1 != null) 
                                { 
                                    p.MovetoFinalKittyHand(c1); 
                                }
                            }
                            return true;
                        }
                    }
                    else
                    {
                        colorMap[card.GetCardType()] = new List<CardValue> { };

                    }
                }
            }
            return false;
        }

        private static bool CheckRunCards(Player.Player p)
        {
            //check KQA
            if (p.cardsinInteger.Contains(1) && p.cardsinInteger.Contains(12)
                && p.cardsinInteger.Contains(13))
            {
                Card? c1 = p.CardInHand.Find(c => c.GetCardValue() == CardValue.Ace);
                Card? c2 = p.CardInHand.Find(c => c.GetCardValue() == CardValue.Queen);
                Card? c3 = p.CardInHand.Find(c => c.GetCardValue() == CardValue.King);
                //remove from cardinhand and move to final hand
                if (c1 != null && c2 != null && c3 != null)
                {
                    p.MovetoFinalKittyHand(c1);
                    p.MovetoFinalKittyHand(c2);
                    p.MovetoFinalKittyHand(c3);
                }
                else
                {
                    Console.WriteLine("error in run");
                }
                return true;
            }
            // check A23
            if (p.cardsinInteger.Contains(1) && p.cardsinInteger.Contains(2)
                && p.cardsinInteger.Contains(3))
            {
                Card? c1 = p.CardInHand.Find(c => c.GetCardValue() == CardValue.Ace);
                Card? c2 = p.CardInHand.Find(c => c.GetCardValue() == CardValue.Two);
                Card? c3 = p.CardInHand.Find(c => c.GetCardValue() == CardValue.Three);
                //remove from cardinhand and move to final hand
                if (c1 != null && c2 != null && c3 != null)
                {
                    p.MovetoFinalKittyHand(c1);
                    p.MovetoFinalKittyHand(c2);
                    p.MovetoFinalKittyHand(c3);
                }
                else
                {
                    Console.WriteLine("error in run");
                }
                return true;
            }
            //check other runs
            for (int i = p.cardsinInteger.Count() - 2; i > 0; i--)
            {
                if (p.cardsinInteger[i - 1] == p.cardsinInteger[i] - 1 &&
                            p.cardsinInteger[i + 1] == p.cardsinInteger[i] + 1)
                {
                    CardValue cv1 = CardConversion.ConversionIntegertoValue(i - 1);
                    CardValue cv2 = CardConversion.ConversionIntegertoValue(i);
                    CardValue cv3 = CardConversion.ConversionIntegertoValue(i + 1);
                    // get card
                    Card? c1 = p.CardInHand.Find(c => c.GetCardValue() == cv1);
                    Card? c2 = p.CardInHand.Find(c => c.GetCardValue() == cv2);
                    Card? c3 = p.CardInHand.Find(c => c.GetCardValue() == cv3);
                    //remove from cardinhand and move to final hand
                    if (c1 != null && c2 != null && c3 != null)
                    {
                        p.MovetoFinalKittyHand(c1);
                        p.MovetoFinalKittyHand(c2);
                        p.MovetoFinalKittyHand(c3);
                    }
                    else
                    {
                        Console.WriteLine("error in run");
                    }
                    return true;
                }
            }
            return false;
        }

        private static bool CheckColorRunCards(Player.Player p)
        {
            Dictionary<CardType, List<CardValue>> colorMap = new Dictionary<CardType, List<CardValue>>();
            // fill dictionary as per each card type
            foreach (var card in p.CardInHand)
            {
                if (colorMap.ContainsKey(card.GetCardType()))
                {
                    colorMap[card.GetCardType()].Add(card.GetCardValue());
                }
                else
                {
                    colorMap[card.GetCardType()]=new List<CardValue>{ };
                }
            }
            // find run in each card type
            foreach (var item in colorMap)
            {
                var currentcolor = item.Key;
                List<int> cardlistint = new List<int>();
                if (item.Value.Count() > 2)
                {
                    // convert to integer
                    foreach (var singlecardvalue in item.Value)
                    {
                        cardlistint.Add(CardConversion.ConversionValuetoInteger(singlecardvalue));
                    }
                    cardlistint.Sort();
                    // check KQA
                    if (cardlistint.Contains(1) && cardlistint.Contains(12) &&
                        cardlistint.Contains(13))
                    {
                        // get card
                        Card? c1 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                        c.GetCardValue() == CardValue.Ace);
                        Card? c2 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                        c.GetCardValue() == CardValue.Queen);
                        Card? c3 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                        c.GetCardValue() == CardValue.King);
                        //remove from cardinhand and move to final hand
                        if (c1 != null && c2 != null && c3 != null)
                        {
                            p.MovetoFinalKittyHand(c1);
                            p.MovetoFinalKittyHand(c2);
                            p.MovetoFinalKittyHand(c3);
                        }
                        else
                        {
                            Console.WriteLine("error in colorrun");
                        }
                        return true;
                    }
                    //Check A23
                    if (cardlistint.Contains(1) && cardlistint.Contains(2) &&
                        cardlistint.Contains(3))
                    {
                        // get card
                        Card? c1 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                        c.GetCardValue() == CardValue.Ace);
                        Card? c2 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                        c.GetCardValue() == CardValue.Two);
                        Card? c3 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                        c.GetCardValue() == CardValue.Three);
                        //remove from cardinhand and move to final hand
                        if (c1 != null && c2 != null && c3 != null)
                        {
                            p.MovetoFinalKittyHand(c1);
                            p.MovetoFinalKittyHand(c2);
                            p.MovetoFinalKittyHand(c3);
                        }
                        else
                        {
                            Console.WriteLine("error in colorrun");
                        }
                        return true;
                    }
                    // check for another runs
                    for (int i = cardlistint.Count() - 2; i > 0; i--)
                    {
                        if (cardlistint[i - 1] == cardlistint[i] - 1 &&
                            cardlistint[i + 1] == cardlistint[i] + 1)
                        {
                            CardValue cv1 = CardConversion.ConversionIntegertoValue(i - 1);
                            CardValue cv2 = CardConversion.ConversionIntegertoValue(i);
                            CardValue cv3 = CardConversion.ConversionIntegertoValue(i + 1);
                            // get card
                            Card? c1 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                            c.GetCardValue() == cv1);
                            Card? c2 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                            c.GetCardValue() == cv2);
                            Card? c3 = p.CardInHand.Find(c => c.GetCardType() == currentcolor &&
                            c.GetCardValue() == cv3);
                            //remove from cardinhand and move to final hand
                            if (c1 != null && c2 != null && c3 != null)
                            {
                                p.MovetoFinalKittyHand(c1);
                                p.MovetoFinalKittyHand(c2);
                                p.MovetoFinalKittyHand(c3);
                            }
                            else
                            {
                                Console.WriteLine("error in colorrun");
                            }
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool CheckXnumCards(Player.Player p, int num)
        {
            Dictionary<CardValue, int> countMap = new Dictionary<CardValue, int>();
            for (int j = 0; j < p.cardsinInteger.Count(); j++)//Ace to 14 value
            {
                if (p.cardsinInteger[j] == 1)
                {
                    p.cardsinInteger[j] = 14;
                }
            }
            p.cardsinInteger.Sort();

            // check same card types
            for (int i = p.cardsinInteger.Count() - 1; i <= 0; i--)
            {
                // converting int to card
                CardValue cv = CardConversion.ConversionIntegertoValue(p.cardsinInteger[i]);
                List<Card>? foundcardlist = p.CardInHand.FindAll(c => c.GetCardValue() == cv);

                if (foundcardlist.Count() == num)
                {
                    foreach (var singlecard in foundcardlist)
                    {
                        p.MovetoFinalKittyHand(singlecard);
                    }
                    return true;
                }
                //alternate way 

                //foreach (var card in foundcardlist)
                //{
                //    if (countMap.ContainsKey(card.GetCardValue()))
                //    {
                //        countMap[card.GetCardValue()]++;
                //        if (countMap[card.GetCardValue()] >= num)
                //        {
                //            foreach (var movingcard in p.CardInHand)
                //            {
                //                if (movingcard.GetCardValue() == card.GetCardValue())
                //                {
                //                    p.Remove(movingcard);
                //                    p.FinalKittyHand.Add(movingcard);
                //                }
                //            }
                //            return true;
                //        }
                //    }
                //    else
                //    {
                //        countMap[card.GetCardValue()] = 1;
                //    }
                //}
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
