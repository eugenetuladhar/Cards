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
        private static Dictionary< int,Player.Player> AllRoundWinners = new Dictionary<int, Player.Player>();
        public static void RunKitty()
        {
            Cardlogiccs.CurrentGame = CardGameType.KITTY;
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

            //Testcodes
            //var b = playerlist[0].CardInHand;
            //b.Clear();
            //b.Add(new Card(CardType.Heart, CardValue.King));
            //b.Add(new Card(CardType.Heart, CardValue.Queen));
            //b.Add(new Card(CardType.Heart, CardValue.Jacks));
            //b.Add(new Card(CardType.Heart, CardValue.Seven));
            //b.Add(new Card(CardType.Heart, CardValue.Six));
            //b.Add(new Card(CardType.Heart, CardValue.Five));
            //b.Add(new Card(CardType.Diamond, CardValue.Ten));
            //b.Add(new Card(CardType.Club, CardValue.Jacks));
            //b.Add(new Card(CardType.Diamond, CardValue.Queen));
            //playerlist[0].DisplayeCardInHand();
            //b = Cardlogiccs.SwapPositionofCards(b, 0, 1);
            //playerlist[0].DisplayeCardInHand();
            //b = Cardlogiccs.SwapPositionofCards(b, 3, 6);
            //playerlist[0].DisplayeCardInHand();

            //var c = playerlist[1].CardInHand;
            //c.Clear();
            //c.Add(new Card(CardType.Spade, CardValue.Jacks));
            //c.Add(new Card(CardType.Spade, CardValue.Nine));
            //c.Add(new Card(CardType.Spade, CardValue.Ten));
            //c.Add(new Card(CardType.Spade, CardValue.Eight));
            //c.Add(new Card(CardType.Spade, CardValue.Seven));
            //c.Add(new Card(CardType.Spade, CardValue.Six));
            //c.Add(new Card(CardType.Diamond, CardValue.Ace));
            //c.Add(new Card(CardType.Club, CardValue.Queen));
            //c.Add(new Card(CardType.Club, CardValue.King));
            //var a = playerlist[2].CardInHand;
            //a.Clear();
            //a.Add(new Card(CardType.Heart, CardValue.Nine));
            //a.Add(new Card(CardType.Club, CardValue.Eight));
            //a.Add(new Card(CardType.Spade, CardValue.Seven));
            //a.Add(new Card(CardType.Diamond, CardValue.Two));
            //a.Add(new Card(CardType.Heart, CardValue.Jacks));
            //a.Add(new Card(CardType.Heart, CardValue.Two));
            //a.Add(new Card(CardType.Club, CardValue.Two));
            //a.Add(new Card(CardType.Spade, CardValue.Two));
            //a.Add(new Card(CardType.Club, CardValue.Nine));
            //Cardlogiccs.ShowAllPlayersCard(playerlist);


            //start game
            StartPlaying(playerlist);

            //compare and choose winner
            DetermineWinner(playerlist);

            Console.ReadLine();
            Console.Clear();
        }

        private static void DetermineWinner(List<Player.Player> playerlist)
        { 
            for (int round = 0; round < 3; round++)
            {
                int CurrentIndexWinner = 0;
                for (int i = 0; i < playerlist.Count(); i++)
                {
                    
                    //reuse flash logic code adjusting value
                    CardCompareLogics.ResetWinnerlist();
                    playerlist[i].strength = playerlist[i].kittyStrength[round];
                    playerlist[i].cardsinInteger.Clear();
                    playerlist[i].CardInHand.Clear();
                    for (int k = 0; k < 3; k++)
                    {
                        playerlist[i].CardInHand.Add(playerlist[i].FinalKittyHand[k+(round*3)]);
                        playerlist[i].cardsinInteger.Add(CardConversion.ConversionValuetoInteger(playerlist[i].CardInHand[k].GetCardValue()));
                    }
                    for (int j = 0; j < playerlist[i].cardsinInteger.Count(); j++)//Ace to 14 value
                    {
                        if (playerlist[i].cardsinInteger[j] == 1)
                        {
                            playerlist[i].cardsinInteger[j] = 14;
                        }
                    }
                    if (i == 0 || playerlist[i].kittyStrength.Contains(CardStrength.Quad))
                    {
                        continue;
                    }
                    CardCompareLogics.CompareTwoHands(playerlist[i], playerlist[CurrentIndexWinner]);
                    if (playerlist[i].result == CardResult.WIN)
                    {
                        CurrentIndexWinner = i;
                    }
                }
                //quad logic
                if (HaveQuadCards(playerlist))
                {
                    break;
                }
                //adding value in kittystrength every round
                var roundwinnerlist = CardCompareLogics.GetWinnerlist();
                Console.WriteLine($"*********************** Round {round + 1} ***********************");
                Cardlogiccs.ShowAllPlayersCard(playerlist);

                if (roundwinnerlist.Count()>1)
                {
                    //all draw
                    AssignAllPlayerkittyStrength(CardResult.DRAW, round, playerlist);
                    Console.WriteLine($"******  Round {round + 1} has ended as DRAW ******");
                    Console.WriteLine();
                }
                else
                {
                    //assign round winner
                    AssignAllPlayerkittyStrength(CardResult.LOSE, round, playerlist);
                    foreach (var roundwinner in roundwinnerlist)
                    {
                        roundwinner.Kittyresult[round] = CardResult.WIN;
                        Console.WriteLine($"****** Winner of Round {round+1} is {roundwinner.GetName} by {roundwinner.kittyStrength[round]} ******");
                        Console.WriteLine();
                        AllRoundWinners[round + 1] = roundwinner;
                    }
                }
                Console.ReadLine();
            }
            GetKittyResult(playerlist);
        }

        private static bool HaveQuadCards(List<Player.Player> playerlist)
        {
            bool returnvalue = false;
            int index = 1;
            foreach (var p in playerlist)
            {
                if (p.kittyStrength.Contains(CardStrength.Quad))
                {
                    AllRoundWinners[index] =p;
                    index=index+1;
                    returnvalue= true;
                }
            }
            return returnvalue;
        }

        private static void GetKittyResult(List<Player.Player> playerlist)
        {
            if (AllRoundWinners.Keys.Contains(1) && AllRoundWinners[1].kittyStrength.Contains(CardStrength.Quad))
            {
                string quadwinner=AllRoundWinners[1].GetName;
                int highestquadvalue = CardConversion.ConversionValuetoInteger(AllRoundWinners[1].FinalKittyHand[0].GetCardValue(),true);
                foreach (var item in AllRoundWinners)
                {
                    if (highestquadvalue < CardConversion.ConversionValuetoInteger(item.Value.FinalKittyHand[0].GetCardValue(),true))
                    {
                        highestquadvalue = CardConversion.ConversionValuetoInteger(item.Value.FinalKittyHand[0].GetCardValue(), true);
                        quadwinner = item.Value.GetName;
                    }
                }
                Console.WriteLine($" ******* The Winner of this game is {quadwinner} with Quads!! ******* ");
            }
            else if (AllRoundWinners.Keys.Contains(1) && AllRoundWinners.Keys.Contains(2) && AllRoundWinners.Keys.Contains(3) && AllRoundWinners[1] == AllRoundWinners[2] && AllRoundWinners[2] == AllRoundWinners[3])
            {
                Console.WriteLine($" ******* The Winner of this game is {AllRoundWinners[1].GetName} with SALAM!! ******* ");
            }
            else if (AllRoundWinners.Keys.Contains(1) && AllRoundWinners.Keys.Contains(2) && AllRoundWinners[1] == AllRoundWinners[2])
            {
                Console.WriteLine($" ******* The Winner of this game is {AllRoundWinners[1].GetName} ******* ");
            }
            else if (AllRoundWinners.Keys.Contains(3) && AllRoundWinners.Keys.Contains(2) && AllRoundWinners[3] == AllRoundWinners[2])
            {
                Console.WriteLine($" ******* The Winner of this game is {AllRoundWinners[3].GetName} ******* ");
            }
            else
            {
                Console.WriteLine($" *** The Game has ended in KITTY *** ");
            }
        }

        private static void AssignAllPlayerkittyStrength(CardResult result, int round, List<Player.Player> playerlist)
        {
            foreach (var p in playerlist)
            {
                p.Kittyresult.Add(result);
            }
        }

        private static void StartPlaying(List<Player.Player> playerlist)
        {
            //human player
            HumanPlayerLogic(playerlist[0]);

            // comp player
            for (int i = 1; i < playerlist.Count(); i++)
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
                    //Console.WriteLine($"Checking Hand {j} of all players completed.");
                }
            }
        }

        private static void HumanPlayerLogic(Player.Player HumanPlayer)
        {
            bool finalform = false;
            string value = "";
            while (!finalform)
            {
                Console.WriteLine("Your cards are as follow, Please rearrange it to final form : (Fill From and To to SWAP Position OR Press X to confirm Final form)");
                Console.WriteLine();
                HumanPlayer.DisplayeCardInHand();
                Console.WriteLine("Pos : 1          2           3           4           5           6           7           8           9");
                int fromvalue, tovalue;
                Console.Write(" Swap cards From : ");
                value = Console.ReadLine();
                if (value == "X" || value == "x")
                {
                    finalform = true;
                }
                else if (checkInputRange(value))
                {
                    fromvalue = int.Parse(value);
                    bool tovaluecheck = false;
                    while (!tovaluecheck)
                    {
                        Console.Write(" Swap cards To : ");
                        value = Console.ReadLine();
                        if (value == "X" || value == "x")
                        {
                            finalform = true;
                            tovaluecheck = true;
                        }
                        else if (checkInputRange(value))
                        {
                            tovalue = int.Parse(value);
                            HumanPlayer.CardInHand = Cardlogiccs.SwapPositionofCards(HumanPlayer.CardInHand, fromvalue - 1, tovalue - 1);
                            Console.WriteLine("Swap Completed");
                            tovaluecheck = true;
                        }
                        else
                        {
                            Console.WriteLine("Invalid To value!! Try Again!!");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input! Try Again!");
                }

                Console.ReadLine();
                Console.Clear();
            }
            Console.WriteLine("Cards Confirmed!");
            //after confirm fill kitty strength
            foreach (var item in HumanPlayer.CardInHand)
            {
                HumanPlayer.FinalKittyHand.Add(item);

            }
            for (int i = 0; i < 3; i++)
            {
                HumanPlayer.CardInHand.Clear();
                for (int j = 0; j < 3; j++)
                {
                    HumanPlayer.CardInHand.Add( HumanPlayer.FinalKittyHand[j + (i * 3)]);
                }
                CardStrengthLogic.InsertStrengthThreeCards(HumanPlayer);
                HumanPlayer.kittyStrength.Add(HumanPlayer.strength);
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
            for (int i = p.cardsinInteger.Count() - 1; i >= 0; i--)
            {
                // converting int to card
                CardValue cv = CardConversion.ConversionIntegertoValue(p.cardsinInteger[i]);
                List<Card>? foundcardlist = p.CardInHand.FindAll(c => c.GetCardValue() == cv);
                foreach (var card in foundcardlist)
                {
                    if (colorMap.ContainsKey(card.GetCardType()))
                    {
                        if (!colorMap[card.GetCardType()].Contains(card.GetCardValue()))// bad coding fix later, added for foundnewlist duplicate cards
                        {
                            colorMap[card.GetCardType()].Add(card.GetCardValue());
                        }

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
                                else
                                {
                                    Console.WriteLine("error in color");
                                }
                            }
                            return true;
                        }
                    }
                    else
                    {
                        colorMap.Add(card.GetCardType(), new List<CardValue> { card.GetCardValue() });
                    }
                }
            }
            return false;
        }

        private static bool CheckRunCards(Player.Player p)
        {
            for (int j = 0; j < p.cardsinInteger.Count(); j++)//Ace to 1 value
            {
                if (p.cardsinInteger[j] == 14)
                {
                    p.cardsinInteger[j] = 1;
                }
            }
            p.cardsinInteger.Sort();
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
                    CardValue cv1 = CardConversion.ConversionIntegertoValue(p.cardsinInteger[i - 1]);
                    CardValue cv2 = CardConversion.ConversionIntegertoValue(p.cardsinInteger[i]);
                    CardValue cv3 = CardConversion.ConversionIntegertoValue(p.cardsinInteger[i + 1]);
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
            for (int j = 0; j < p.cardsinInteger.Count(); j++)//Ace to 1 value
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
                    colorMap.Add(card.GetCardType(),new List<CardValue> { card.GetCardValue() });
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
            for (int i = p.cardsinInteger.Count() - 1; i >= 0; i--)
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
                    if(num==2)
                    {
                        p.MovetoFinalKittyHand(p.CardInHand[0]);
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
        private static bool checkInputRange(string value)
        {
            return value == "1" || value == "2" || value == "3" || value == "4" || value == "5" || value == "6" || value == "7" || value == "8" || value == "9";
        }
    }
}
