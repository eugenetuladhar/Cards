using cards.Cards_files;
using cards.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Utils
{
    public class CardStrengthLogic
    {
        public static void GetCardStrengthThreeCards(Player.Player p)
        {
            p.cardsinInteger = Cardlogiccs.GetCardIntegerValue(p.CardInHand);
            if (CheckTrial(p.CardInHand))
            {
                p.strength = CardStrength.Trial;
            }
            else if (CheckColor(p.CardInHand) && CheckRun(p.CardInHand))
            {
                p.strength = CardStrength.ColorSequence;
                p.cardsinInteger.Sort();
            }
            else if (CheckRun(p.CardInHand))
            {
                p.strength = CardStrength.Sequence;
                p.cardsinInteger.Sort();
            }
            else if (CheckColor(p.CardInHand))
            {
                p.strength = CardStrength.Color;
                p.cardsinInteger.Sort();
            }
            else if (CheckJoot(p.CardInHand))
            {
                p.strength = CardStrength.Double;
                p.cardsinInteger.Sort();
            }
            else
            {
                p.strength = CardStrength.Common;
                p.cardsinInteger.Sort();
            }
        }
        private static bool CheckTrial(List<Card> cardInHand)
        {
            if (cardInHand[0].GetCardValue() == cardInHand[1].GetCardValue() &&
                cardInHand[0].GetCardValue() == cardInHand[2].GetCardValue())
            {

                return true;
            }
            return false;
        }
        private static bool CheckColor(List<Card> cardInHand)
        {
            if (cardInHand[0].GetCardType() == cardInHand[1].GetCardType() &&
                cardInHand[0].GetCardType() == cardInHand[2].GetCardType())
            {

                return true;
            }
            return false;
        }
        private static bool CheckJoot(List<Card> cardInHand)
        {
            if (CheckTrial(cardInHand))
            {
                return false;
            }
            if (cardInHand[0].GetCardValue() == cardInHand[1].GetCardValue() ||
                cardInHand[0].GetCardValue() == cardInHand[2].GetCardValue() ||
                cardInHand[1].GetCardValue() == cardInHand[2].GetCardValue())
            {

                return true;
            }
            return false;
        }
        private static bool CheckRun(List<Card> cardInHand)
        {
            if (CheckTrial(cardInHand) || CheckJoot(cardInHand))
            {
                return false;
            }
            List<int> cardvalue = Cardlogiccs.GetCardIntegerValue(cardInHand);
            cardvalue.Sort();
            if (cardvalue[0] == cardvalue[1] - 1 && cardvalue[2] == cardvalue[1] + 1)
            {
                return true;
            }
            if (cardvalue[0] == 1 && cardvalue[1] == 12 && cardvalue[2] == 13)
            {
                return true;
            }
            return false;
        }
        public static bool CheckColorRunCards(List<Card> cardlista)
        {
            if (cardlista.Count < 3)// for run should be greater than 2
            {
                return false;
            }
            Dictionary<CardType, List<CardValue>> colorMap = new();
            // check all cards same color or not
            for (int i = 0; i < cardlista.Count - 1; i++)
            {
                if (i == 0)
                {
                    colorMap.Add(cardlista[0].GetCardType(), new List<CardValue> { cardlista[0].GetCardValue() });
                    continue;
                }
                if (cardlista[i].GetCardType() == cardlista[i - 1].GetCardType())
                {
                    colorMap[cardlista[i].GetCardType()].Add(cardlista[i].GetCardValue());
                }
                else
                {
                    return false;
                }
            }

            // find run in  card
            List<int> cardlistint = new List<int>();
            // convert to integer
            foreach (var singlecard in cardlista)
            {
                cardlistint.Add(CardConversion.ConversionValuetoInteger(singlecard.GetCardValue()));
            }
            cardlistint.Sort();
           
            if (cardlistint.Count == 3)
            {
                
                // check KQA
                if (cardlistint.Contains(1) && cardlistint.Contains(12) &&
                    cardlistint.Contains(13))
                {

                    return true;
                }
                
            }
            else if (cardlistint.Count == 4)
            {
                if (cardlistint.Contains(1) && cardlistint.Contains(12) &&
                    cardlistint.Contains(13) &&
                    cardlistint.Contains(11))
                {

                    return true;
                }
            }
            else if (cardlistint.Count == 5)
            {
                if (cardlistint.Contains(1) && cardlistint.Contains(12) &&
                    cardlistint.Contains(13) &&
                    cardlistint.Contains(11) && cardlistint.Contains(10))
                {

                    return true;
                }
            }
            int runcards = 0;
            for (int i = 1; i < cardlistint.Count - 1; i++)
            {
                if (cardlistint[i] == cardlistint[i - 1] + 1)
                {
                    runcards++;
                }
                else
                {
                    return false;
                }
            }
            if (runcards == cardlistint.Count - 1)
            {
                return true;
            }
            return false;
        }
        public static bool CheckXnumCards(List<Card> cardlist)// returns true if list has double or trail or quads
        {
            for (int i = 1; i < cardlist.Count; i++)
            {
                if (cardlist[i] != cardlist[i-1])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool CheckXnumCards(Player.Player p, int num, CardGameType game)
        {
            for (int j = 0; j < p.cardsinInteger.Count; j++)//Ace to 14 value
            {
                if (p.cardsinInteger[j] == 1)
                {
                    p.cardsinInteger[j] = 14;
                }
            }
            p.cardsinInteger.Sort();

            // check same card types
            for (int i = p.cardsinInteger.Count - 1; i >= 0; i--)
            {
                // converting int to card
                CardValue cv = CardConversion.ConversionIntegertoValue(p.cardsinInteger[i]);
                List<Card>? foundcardlist = p.CardInHand.FindAll(c => c.GetCardValue() == cv);

                if (foundcardlist.Count == num)
                {
                    foreach (var singlecard in foundcardlist)
                    {
                        if (game == CardGameType.KITTY)
                        {
                            p.MovetoFinalKittyHand(singlecard);
                        }
                        else if (game == CardGameType.DHUMBAL)
                        {
                            p.MovetoThrowCardList(singlecard);
                        }
                    }
                    if (num == 2 && game == CardGameType.KITTY)
                    {
                        p.MovetoFinalKittyHand(p.CardInHand[0]);
                    }
                    return true;
                }
            }
            return false;
        }
        public static bool CheckColorRunCards(Player.Player p, int number, CardGameType game)
        {
            for (int j = 0; j < p.cardsinInteger.Count; j++)//Ace to 1 value
            {
                if (p.cardsinInteger[j] == 14)
                {
                    p.cardsinInteger[j] = 1;
                }
            }
            p.cardsinInteger.Sort();
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
                    colorMap.Add(card.GetCardType(), new List<CardValue> { card.GetCardValue() });
                }
            }
            // find run in each card type
            foreach (var item in colorMap)
            {
                var currentcolor = item.Key;
                List<int> cardlistint = new List<int>();
                if (item.Value.Count() > number - 1)
                {
                    if (number == 3)
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
                                MoveCard(p, c1, game);
                                MoveCard(p, c2, game);
                                MoveCard(p, c3, game);
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
                                MoveCard(p, c1, game);
                                MoveCard(p, c2, game);
                                MoveCard(p, c3, game);
                            }
                            else
                            {
                                Console.WriteLine("error in colorrun");
                            }
                            return true;
                        }
                    }
                    else if (number == 4)
                    {
                        // add logic
                    }
                    else if (number == 5)
                    {
                        // add logic
                    }

                    // check for other runs
                    for (int i = cardlistint.Count() - (number - 1); i > 0; i--)
                    {
                        if (number == 3)
                        {
                            if (cardlistint[i - 1] == cardlistint[i] - 1 &&
                                cardlistint[i + 1] == cardlistint[i] + 1)
                            {
                                CardValue cv1 = CardConversion.ConversionIntegertoValue(cardlistint[i - 1]);
                                CardValue cv2 = CardConversion.ConversionIntegertoValue(cardlistint[i]);
                                CardValue cv3 = CardConversion.ConversionIntegertoValue(cardlistint[i + 1]);
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
                                    MoveCard(p, c1, game);
                                    MoveCard(p, c2, game);
                                    MoveCard(p, c3, game);
                                }
                                else
                                {
                                    Console.WriteLine("error in colorrun");
                                }
                                return true;
                            }
                            else if (number == 4)
                            {
                                //add logic
                            }
                            else if (number == 5)
                            {
                                //add logic
                            }
                        }
                    }
                }
            }
            return false;
        }
        private static void MoveCard(Player.Player p, Card card, CardGameType gameName)
        {
            if (gameName == CardGameType.KITTY)
            {
                p.MovetoFinalKittyHand(card);
            }
            else if (gameName == CardGameType.DHUMBAL)
            {
                p.MovetoThrowCardList(card);
            }
        }
    }
}
