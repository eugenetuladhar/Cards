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
            playerlist = Cardlogiccs.GetPlayers();

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
                    if (CheckXnumCards(playerlist[i],4))// checking quads
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Quad);
                        break;
                    }
                    else if (CheckXnumCards(playerlist[i],3)) // checking trial
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Trial);
                    }
                    else if (CheckColorRunCards(playerlist[i]))
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.ColorSequence);
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

        private static bool CheckColorRunCards(Player.Player p)
        {
            Dictionary<CardType, List<CardValue>> colorMap = new Dictionary<CardType, List<CardValue>>();
            foreach (var card in p.CardInHand)
            {
                if (colorMap.ContainsKey(card.GetCardType()))
                {
                    colorMap[card.GetCardType()].Add(card.GetCardValue());
                }
                else
                {
                    _ = colorMap[card.GetCardType()];
                }
            }
            foreach (var item in colorMap)
            {
                var currentcolor = item.Key;
                List<int> cardlistint= new List<int>();
                if (item.Value.Count() > 2)
                {
                    foreach (var singlecardvalue in item.Value)
                    {
                        cardlistint.Add(CardConversion.ConversionValuetoInteger(singlecardvalue));
                    }
                    cardlistint.Sort();

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
                        p.MovetoFinalKittyHand(c1);
                        p.MovetoFinalKittyHand(c2);
                        p.MovetoFinalKittyHand(c3);
                        return true;
                    }
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
                        p.MovetoFinalKittyHand(c1);
                        p.MovetoFinalKittyHand(c2);
                        p.MovetoFinalKittyHand(c3);
                        return true;
                    }
                    for (int i = cardlistint.Count()-1; i > 1; i--)
                    {
                        if (cardlistint[i - 1] == cardlistint[i]-1 &&
                            cardlistint[i+1] == cardlistint[i] + 1)
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
                            p.MovetoFinalKittyHand(c1);
                            p.MovetoFinalKittyHand(c2);
                            p.MovetoFinalKittyHand(c3);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static bool CheckXnumCards(Player.Player p,int num)
        {
            Dictionary<CardValue, int> countMap = new Dictionary<CardValue, int>();
            foreach (var card in p.CardInHand)
            {
                if (countMap.ContainsKey(card.GetCardValue()))
                {
                    countMap[card.GetCardValue()]++;
                    if (countMap[card.GetCardValue()] >= num)
                    {
                        foreach (var movingcard in p.CardInHand)
                        {
                            if(movingcard.GetCardValue()==card.GetCardValue())
                            {
                                p.Remove(movingcard);
                                p.FinalKittyHand.Add(movingcard);
                            }
                        }
                        return true;
                    }
                }
                else
                {
                    countMap[card.GetCardValue()] = 1;
                }

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
