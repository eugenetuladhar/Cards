using cards.Cards_files;
using cards.Interface;
using cards.Player;
using cards.Utils;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace cards.Game
{
    public class KittyGame : IGame
    {
        public CardGameType GameName { get; set; }
        public int MAX_NUM_PLAYERS { get; set; }
        public int NUM_CARDS_TO_DEAL { get; set; }
        private Dictionary<int, Player.Player> AllRoundWinners = new Dictionary<int, Player.Player>();
        private bool IsKitty = false;
        private int numberofKitty;

        public KittyGame()
        {
            GameName = CardGameType.KITTY;
            MAX_NUM_PLAYERS = 5;
            NUM_CARDS_TO_DEAL = 9;
            numberofKitty= 0;
        }
        public void Run()
        {
            //New card instance
            CardCompleteDeck carddeck = new CardCompleteDeck();
            Console.WriteLine();
            // pleyer logic
            List<Player.Player> playerlist = new List<Player.Player>();
            playerlist = Cardlogiccs.GetPlayers(MAX_NUM_PLAYERS);

            //shuffle
            carddeck.Shuffle();

            //Deal 
            Deal(playerlist, carddeck);

            //Testcodes
            //var b = playerlist[0].CardInHand;
            //b.Clear();
            //b.Add(new Card(CardType.Heart, CardValue.King));
            //b.Add(new Card(CardType.Heart, CardValue.Queen));
            //b.Add(new Card(CardType.Heart, CardValue.Jacks));
            //b.Add(new Card(CardType.Heart, CardValue.Seven));
            //b.Add(new Card(CardType.Heart, CardValue.Six));
            //b.Add(new Card(CardType.Heart, CardValue.Five));
            //b.Add(new Card(CardType.Spade, CardValue.Six));
            //b.Add(new Card(CardType.Diamond, CardValue.Two));
            //b.Add(new Card(CardType.Diamond, CardValue.King));
            //playerlist[0].DisplayeCardInHand();
            //b = Cardlogiccs.SwapPositionofCards(b, 0, 1);
            //playerlist[0].DisplayeCardInHand();
            //b = Cardlogiccs.SwapPositionofCards(b, 3, 6);
            //playerlist[0].DisplayeCardInHand();

            //var c = playerlist[1].CardInHand;
            //c.Clear();
            //c.Add(new Card(CardType.Diamond, CardValue.Three));
            //c.Add(new Card(CardType.Spade, CardValue.Four));
            //c.Add(new Card(CardType.Diamond, CardValue.Five));
            //c.Add(new Card(CardType.Spade, CardValue.Seven));
            //c.Add(new Card(CardType.Spade, CardValue.Eight));
            //c.Add(new Card(CardType.Club, CardValue.Nine));
            //c.Add(new Card(CardType.Heart, CardValue.Nine));
            //c.Add(new Card(CardType.Heart, CardValue.Eight));
            //c.Add(new Card(CardType.Heart, CardValue.Two));
            //var a = playerlist[2].CardInHand;
            //a.Clear();
            //a.Add(new Card(CardType.Heart, CardValue.Nine));
            //a.Add(new Card(CardType.Club, CardValue.Eight));
            //a.Add(new Card(CardType.Club, CardValue.Ten));
            //a.Add(new Card(CardType.Spade, CardValue.Nine));
            //a.Add(new Card(CardType.Spade, CardValue.Eight));
            //a.Add(new Card(CardType.Spade, CardValue.Two));
            //a.Add(new Card(CardType.Club, CardValue.Two));
            //a.Add(new Card(CardType.Heart, CardValue.Five));
            //a.Add(new Card(CardType.Club, CardValue.Four));
            //Cardlogiccs.ShowAllPlayersCard(playerlist);

            //start game
            StartPlaying(playerlist);

            //compare and choose winner
            DetermineWinner(playerlist);
            Console.ReadLine();

            if (IsKitty)
            {
                AfterKittyorplayagainLogic(playerlist, carddeck);
                Console.ReadLine();
            }
            else
            {
                Askplayagain(playerlist, carddeck);
            }

            Console.Clear();
        }

        private void Askplayagain(List<Player.Player> playerlist, CardCompleteDeck carddeck)
        {
            numberofKitty = 0;
            Console.Clear();
            Console.WriteLine("Do you want to play again?(y/n)");
            string playagain = Console.ReadLine();
            if (playagain == "y" || playagain == "Y")
            {
                AfterKittyorplayagainLogic(playerlist, carddeck);
            }
        }

        private void AfterKittyorplayagainLogic(List<Player.Player> playerlist, CardCompleteDeck carddeck)
        {
            do
            {
                Console.Clear();
                if (IsKitty)
                {
                    numberofKitty++;
                    Console.WriteLine($"Previous game was ended in Kitty, Current Number of Kittys : {numberofKitty}");
                }
                carddeck.Reset();
                foreach (var p in playerlist)
                {
                    p.Reset();

                }
                //shuffle
                carddeck.Shuffle();

                //Deal 
                Deal(playerlist, carddeck);
                //start game
                StartPlaying(playerlist);

                //compare and choose winner
                DetermineWinner(playerlist);
                Console.ReadLine();
            } while (IsKitty);
            Console.WriteLine($"Total Number of Kittys : {numberofKitty}");
            Console.ReadLine();
            Askplayagain(playerlist, carddeck);
        }

        private void DetermineWinner(List<Player.Player> playerlist)
        {
            for (int round = 0; round < 3; round++)
            {
                int CurrentIndexWinner = 0;
                for (int i = 0; i < playerlist.Count(); i++)
                {

                    //reuse flash logic code adjusting value
                    bool doescurrentwinnerhasdraw = false;
                    if (playerlist[CurrentIndexWinner].result == CardResult.DRAW && i!=1)
                    {
                        doescurrentwinnerhasdraw = true;
                    }
                    CardCompareLogics.ResetWinnerlist();
                    playerlist[i].strength = playerlist[i].kittyStrength[round];
                    playerlist[i].cardsinInteger.Clear();
                    playerlist[i].CardInHand.Clear();
                    for (int k = 0; k < 3; k++)
                    {
                        playerlist[i].CardInHand.Add(playerlist[i].FinalKittyHand[k + round * 3]);
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
                    }else if (doescurrentwinnerhasdraw == true)
                    {
                        playerlist[CurrentIndexWinner].result= CardResult.DRAW;
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

                if (roundwinnerlist.Count() > 1)
                {
                    //all draw
                    AssignAllPlayerkittyStrength(CardResult.DRAW, round, playerlist);
                    Console.WriteLine($"******  Round {round + 1} has ended as DRAW ******");
                    Console.WriteLine();
                }
                else if (roundwinnerlist.Count==1 && roundwinnerlist[0].result==CardResult.DRAW)
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
                        Console.WriteLine($"****** Winner of Round {round + 1} is {roundwinner.GetName} by {roundwinner.kittyStrength[round]} ******");
                        Console.WriteLine();
                        AllRoundWinners[round + 1] = roundwinner;
                    }
                }
                Console.ReadLine();
            }
            GetKittyResult(playerlist);
        }

        private bool HaveQuadCards(List<Player.Player> playerlist)
        {
            bool returnvalue = false;
            int index = 1;
            foreach (var p in playerlist)
            {
                if (p.kittyStrength.Contains(CardStrength.Quad))
                {
                    AllRoundWinners[index] = p;
                    index = index + 1;
                    returnvalue = true;
                }
            }
            return returnvalue;
        }

        private void GetKittyResult(List<Player.Player> playerlist)
        {
            if (AllRoundWinners.Keys.Contains(1) && AllRoundWinners[1].kittyStrength.Contains(CardStrength.Quad))
            {
                string quadwinner = AllRoundWinners[1].GetName;
                int highestquadvalue = CardConversion.ConversionValuetoInteger(AllRoundWinners[1].FinalKittyHand[0].GetCardValue(), true);
                foreach (var item in AllRoundWinners)
                {
                    if (highestquadvalue < CardConversion.ConversionValuetoInteger(item.Value.FinalKittyHand[0].GetCardValue(), true))
                    {
                        highestquadvalue = CardConversion.ConversionValuetoInteger(item.Value.FinalKittyHand[0].GetCardValue(), true);
                        quadwinner = item.Value.GetName;
                    }
                }
                Cardlogiccs.ShowAllPlayersCard(playerlist);
                Console.WriteLine($" ******* The Winner of this game is {quadwinner} with Quads!! ******* ");
                IsKitty = false;
            }
            else if (AllRoundWinners.Keys.Contains(1) && AllRoundWinners.Keys.Contains(2) && AllRoundWinners.Keys.Contains(3) && AllRoundWinners[1] == AllRoundWinners[2] && AllRoundWinners[2] == AllRoundWinners[3])
            {
                Console.WriteLine($" ******* The Winner of this game is {AllRoundWinners[1].GetName} with SALAM!! ******* ");
                IsKitty = false;
            }
            else if (AllRoundWinners.Keys.Contains(1) && AllRoundWinners.Keys.Contains(2) && AllRoundWinners[1] == AllRoundWinners[2])
            {
                Console.WriteLine($" ******* The Winner of this game is {AllRoundWinners[1].GetName} ******* ");
                IsKitty = false;
            }
            else if (AllRoundWinners.Keys.Contains(3) && AllRoundWinners.Keys.Contains(2) && AllRoundWinners[3] == AllRoundWinners[2])
            {
                Console.WriteLine($" ******* The Winner of this game is {AllRoundWinners[3].GetName} ******* ");
                IsKitty = false;
            }
            else
            {
                Console.WriteLine($" *** The Game has ended in KITTY *** ");
                IsKitty = true;
            }
        }

        private void AssignAllPlayerkittyStrength(CardResult result, int round, List<Player.Player> playerlist)
        {
            foreach (var p in playerlist)
            {
                p.Kittyresult.Add(result);
            }
        }

        private void StartPlaying(List<Player.Player> playerlist)
        {
            Console.Clear();
            //human player
            HumanPlayerLogic(playerlist[0]);

            // comp player
            for (int i = 1; i < playerlist.Count(); i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    playerlist[i].cardsinInteger = Cardlogiccs.GetCardIntegerValue(playerlist[i].CardInHand);
                    playerlist[i].cardsinInteger.Sort();
                    if (CardStrengthLogic.CheckXnumCards(playerlist[i], 4,GameName))// checking quads
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Quad);
                        break;
                    }
                    else if (CardStrengthLogic.CheckXnumCards(playerlist[i], 3,GameName)) // checking trial
                    {
                        playerlist[i].kittyStrength.Add(CardStrength.Trial);
                    }
                    else if (CardStrengthLogic.CheckColorRunCards(playerlist[i],3,GameName))
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
                    else if (CardStrengthLogic.CheckXnumCards(playerlist[i], 2,GameName))
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
                AdjustStrengthinOrder(playerlist[i]);
            }
        }

        private void HumanPlayerLogic(Player.Player HumanPlayer)
        {
            // Sort the list based on Property1
            HumanPlayer.CardInHand.Sort((a, b) => a.GetCardType().CompareTo(b.GetCardType()));
            bool finalform = false;
            string value = "";

            ConsoleKeyInfo keyInfo;
            int gapSize = 0;
            bool move = false;
            string symbol = "\u2191";
            while (!finalform)
            {
                Console.WriteLine("Your cards are as follow, Please rearrange it to final form : (Use <- and -> OR Press 'X' to confirm Final form)");
                Console.WriteLine();
                Console.WriteLine("            1              2              3");
                Console.WriteLine("        ---------      ---------      ---------");
                Console.Write("     ");
                HumanPlayer.DisplayeCardInHand();

                string gap = new string(' ', gapSize);
                string formattedText = gap + gap + gap + gap + gap + symbol;
                Console.WriteLine("      {0}", formattedText);
                keyInfo = Console.ReadKey(true); // Read key without displaying it

                if (keyInfo.Key == ConsoleKey.LeftArrow && gapSize > 0)
                {
                    if (move == true)
                    { HumanPlayer.CardInHand = Cardlogiccs.SwapPositionofCards(HumanPlayer.CardInHand, gapSize, gapSize - 1); }
                    gapSize--;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && gapSize < 8)
                {
                    if (move == true)
                    { HumanPlayer.CardInHand = Cardlogiccs.SwapPositionofCards(HumanPlayer.CardInHand, gapSize, gapSize + 1); }
                    gapSize++;
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (move == false)
                    {
                        move = true;
                        symbol = "\u2194";
                    }
                    else if (move == true)
                    {
                        move = false;
                        symbol = "↑";
                    }
                }
                else if (keyInfo.Key == ConsoleKey.X)
                {
                    finalform = true;
                }
                Console.Clear();
            }
            Console.WriteLine("Cards Confirmed!");
            // quad logic
            if (HumanPlayer.CardInHand[0].GetCardValue() == HumanPlayer.CardInHand[1].GetCardValue() &&
                HumanPlayer.CardInHand[1].GetCardValue() == HumanPlayer.CardInHand[2].GetCardValue() &&
                HumanPlayer.CardInHand[1].GetCardValue() == HumanPlayer.CardInHand[3].GetCardValue())
            {
                for (int i = 0; i < 5; i++)
                {
                    HumanPlayer.FinalKittyHand.Add(HumanPlayer.CardInHand[i]);

                }
                HumanPlayer.kittyStrength.Add(CardStrength.Quad);
            }
            else
            {
                AdjustStrengthinOrder(HumanPlayer);
            }
            HumanPlayer.cardsinInteger = Cardlogiccs.GetCardIntegerValue(HumanPlayer.CardInHand);
            HumanPlayer.cardsinInteger.Sort();
        }
        private void AdjustStrengthinOrder(Player.Player player)
        {
            //after confirm fill kitty strength
            foreach (var item in player.CardInHand)
            {
                player.FinalKittyHand.Add(item);

            }
            for (int i = 0; i < 3; i++)
            {
                player.CardInHand.Clear();
                for (int j = 0; j < 3; j++)
                {
                    player.CardInHand.Add(player.FinalKittyHand[j + i * 3]);
                }
                CardStrengthLogic.GetCardStrengthThreeCards(player);
                player.kittyStrength.Add(player.strength);
            }
            //check kitty strength and put it in order if its not
            if (CardStrengthUtils.GetPriorty(player.kittyStrength[1]) <
                CardStrengthUtils.GetPriorty(player.kittyStrength[2]))
            {
                CardStrength temp = player.kittyStrength[1];
                player.kittyStrength[1] = player.kittyStrength[2];
                player.kittyStrength[2] = temp;

                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 4 - 1, 7 - 1);
                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 5 - 1, 8 - 1);
                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 6 - 1, 9 - 1);
            }
            if (CardStrengthUtils.GetPriorty(player.kittyStrength[0]) <
                CardStrengthUtils.GetPriorty(player.kittyStrength[2]))
            {
                CardStrength temp = player.kittyStrength[0];
                player.kittyStrength[0] = player.kittyStrength[2];
                player.kittyStrength[2] = temp;

                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 1 - 1, 7 - 1);
                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 2 - 1, 8 - 1);
                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 3 - 1, 9 - 1);
            }
            if (CardStrengthUtils.GetPriorty(player.kittyStrength[0]) <
                CardStrengthUtils.GetPriorty(player.kittyStrength[1]))
            {
                CardStrength temp = player.kittyStrength[0];
                player.kittyStrength[0] = player.kittyStrength[1];
                player.kittyStrength[1] = temp;

                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 1 - 1, 4 - 1);
                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 2 - 1, 5 - 1);
                Cardlogiccs.SwapPositionofCards(player.FinalKittyHand, 3 - 1, 6 - 1);
            }
        }
        private void HandleCommonCards(Player.Player p)
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

        private bool CheckColorCards(Player.Player p)
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

        private bool CheckRunCards(Player.Player p)
        {
            for (int j = 0; j < p.cardsinInteger.Count; j++)//Ace to 1 value
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

        private void Deal(List<Player.Player> list, CardCompleteDeck c)
        {
            Cardlogiccs.Deal(list, c, NUM_CARDS_TO_DEAL, false);
        }
    }
}
