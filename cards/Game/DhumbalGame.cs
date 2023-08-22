using cards.Cards_files;
using cards.Interface;
using cards.Player;
using cards.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace cards.Game
{
    public class DhumbalGame : IGame
    {
        public CardGameType GameName { get; set; }
        public int MAX_NUM_PLAYERS { get; set; }
        public int NUM_CARDS_TO_DEAL { get; set; }
        private List<List<Card>> CardsOnGround { get; set; }
        CardCompleteDeck carddeck;
        private bool gamecomplete = false;
        private bool PICK = true;
        private bool THROW = false;
        private Player.Player Finisher { get; set; }
        private string playerturn { get; set; }
        List<Player.Player> playerlist;
        List<string> Dhumballog = new List<string>();
        public DhumbalGame()
        {
            carddeck = new CardCompleteDeck();
            CardsOnGround = new List<List<Card>>();
            GameName = CardGameType.DHUMBAL;
            MAX_NUM_PLAYERS = 4;
            NUM_CARDS_TO_DEAL = 5;
            playerturn = "None";
        }
        public void Run()
        {
            //New card instance
            Console.WriteLine();
            // Player logic
            playerlist = Cardlogiccs.GetPlayers(MAX_NUM_PLAYERS, false);

            //shuffle
            carddeck.Shuffle();

            //Deal
            Deal();

            //Start
            StartPlaying();

            //Display winner
            DetermineWinner();

            //Asking Play again
            Askplayagain(playerlist, carddeck);
        }
        private void Askplayagain(List<Player.Player> playerlist, CardCompleteDeck carddeck)
        {
            Console.Clear();
            Console.WriteLine("Do you want to play again?(y/n)");
            string playagain = Console.ReadLine();
            if (playagain == "y" || playagain == "Y")
            {
                carddeck.Reset();
                CardsOnGround = new List<List<Card>>();
                Finisher.Reset();
                Dhumballog.Clear();
                gamecomplete = false;
                foreach (var player in playerlist)
                {
                    player.Reset();
                }
                Run();
            }
        }
        private void DetermineWinner()
        {
            playerturn = "Game Completed";
            Cardlogiccs.ShowAllPlayersCard(playerlist, false);
            Console.ReadLine();
            //Display winners
            Console.WriteLine("**  Winner   **");
            Console.WriteLine();
            foreach (var player in playerlist)
            {
                if (player == Finisher)
                {
                    continue;
                }
                else if (Cardlogiccs.GetTotalIntegerValue(Finisher.CardInHand) >=
                    Cardlogiccs.GetTotalIntegerValue(player.CardInHand))
                {
                    Finisher = player;
                }
            }
            Console.WriteLine("  " + Finisher.GetName);
            Console.ReadLine();
        }

        private void Deal()
        {
            Cardlogiccs.Deal(playerlist, carddeck, NUM_CARDS_TO_DEAL, false);
        }
        private void HumanPlayerLogic(Player.Player HumanPlayer)
        {
            playerturn = playerlist[0].GetName;
            // Sort the list based on Property1
            HumanPlayer.CardInHand.Sort((a, b) => a.GetCardType().CompareTo(b.GetCardType()));

            ConsoleKeyInfo keyInfo;// for arrow movement user interface
            int gapSize = 0;
            bool move = false;
            string symbol = "\u2191";

            bool menu = true;
            while (menu)
            {
                DhumbalLogAdd("** Your turn **");
                RefreshScreen();

                List<Card> currentcardsonground = CardsOnGround[CardsOnGround.Count - 1];
                string gap = new string(' ', gapSize);
                string formattedText = gap + gap + gap + gap + gap + symbol;
                Console.WriteLine("            {0}", formattedText);

                if (HumanPlayer.ThrowCardList.Count > 0) // display throw card list
                {
                    Console.Write("Throwcard List: ");
                    foreach (var card in HumanPlayer.ThrowCardList)
                    {
                        Cardlogiccs.PrintCard(card);
                    }
                }
                keyInfo = Console.ReadKey(true); // Read key without displaying it

                if (keyInfo.Key == ConsoleKey.LeftArrow && gapSize > 0)
                {
                    //if (move == true)
                    //{ HumanPlayer.CardInHand = Cardlogiccs.SwapPositionofCards(HumanPlayer.CardInHand, gapSize, gapSize - 1); }
                    gapSize--;
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow && gapSize < HumanPlayer.CardInHand.Count - 1)
                {
                    //if (move == true)
                    //{ HumanPlayer.CardInHand = Cardlogiccs.SwapPositionofCards(HumanPlayer.CardInHand, gapSize, gapSize + 1); }
                    gapSize++;
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (HumanPlayer.CardInHand.Count > 0)
                    {
                        HumanPlayer.ThrowCardList.Add(HumanPlayer.CardInHand[gapSize]);
                        HumanPlayer.ThrowCards();
                        gapSize = 0;
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("No more cards in hand");
                        Console.ReadLine();
                    }
                    //if (move == false)
                    //{
                    //    move = true;
                    //    symbol = "\u2194";
                    //}
                    //else if (move == true)
                    //{
                    //    move = false;
                    //    symbol = "↑";
                    //}
                }
                else if (keyInfo.Key == ConsoleKey.X)
                {
                    if (Cardlogiccs.GetTotalIntegerValue(HumanPlayer.CardInHand) < 6)
                    {
                        //determinewinner
                        Finisher = HumanPlayer;
                        gamecomplete = true;
                        menu = false;
                    }
                    else
                    {
                        Console.WriteLine("Not eligible to submit. Should be less than 6");
                        Thread.Sleep(1000);
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    foreach (var item in HumanPlayer.ThrowCardList)// returning throwlist cards to hand
                    {
                        HumanPlayer.CardInHand.Add(item);
                    }
                    HumanPlayer.ThrowCardList.Clear();
                }
                else if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (HumanPlayer.ThrowCardList.Count > 0 && (CardStrengthLogic.CheckXnumCards(HumanPlayer.ThrowCardList) ||
                        HumanPlayer.ThrowCardList.Count == 1 || CardStrengthLogic.CheckColorRunCards(HumanPlayer.ThrowCardList)))
                    {
                        //refresh screen logic
                        DhumbalLogAddThrowlist(HumanPlayer);
                        RefreshScreen();

                        //extralogic
                        Cardlogiccs.CardInGroundThrowLogic(HumanPlayer.ThrowCardList, CardsOnGround);

                        HumanPlayer.ThrowCardList.Clear();

                        // pick from ground or deck
                        Console.Write(" Pick from: DECK ");
                        foreach (var singlecard in currentcardsonground)
                        {
                            Console.Write("or");
                            Cardlogiccs.PrintCard(singlecard);
                        }
                        Console.Write("?\n");

                        gapSize = 0;
                        gap = new string(' ', gapSize);
                        string formattedText1 = gap + gap + gap + gap + gap + symbol;
                        Console.WriteLine("             {0}", formattedText1);
                        bool pickcomplete = false;
                        while (!pickcomplete)
                        {
                            keyInfo = Console.ReadKey(true);

                            if (keyInfo.Key == ConsoleKey.LeftArrow && gapSize > 0)
                            {
                                gapSize--;
                                //refresh screen logic
                                RefreshScreen();

                                Console.Write("Pick from: DECK ");
                                foreach (var singlecard in currentcardsonground)
                                {
                                    Console.Write("or");
                                    Cardlogiccs.PrintCard(singlecard);
                                }
                                Console.Write("?\n");

                                gap = new string(' ', gapSize);
                                formattedText1 = gap + gap + gap + gap + gap + gap + gap + symbol;
                                Console.WriteLine("             {0}", formattedText1);
                            }
                            else if (keyInfo.Key == ConsoleKey.RightArrow && gapSize < currentcardsonground.Count())
                            {
                                gapSize++;
                                //refresh screen logic
                                RefreshScreen();
                                Console.Write(" Pick from: DECK ");
                                foreach (var singlecard in currentcardsonground)
                                {
                                    Console.Write("or");
                                    Cardlogiccs.PrintCard(singlecard);
                                }
                                Console.Write("?\n");

                                gap = new string(' ', gapSize);
                                formattedText1 = gap + gap + gap + gap + gap + gap + gap + symbol;
                                Console.WriteLine("             {0}", formattedText1);

                            }
                            else if (keyInfo.Key == ConsoleKey.Enter)
                            {
                                if (gapSize == 0)
                                {
                                    DhumbalLogAdd("** You Picked from Deck **");
                                    pickcomplete = true;
                                    carddeck.Draw(HumanPlayer);
                                }
                                else if (gapSize == 1)
                                {
                                    DhumbalLogAdd("** You Picked from Ground **");
                                    HumanPlayer.PickSingleCard(currentcardsonground[0]);
                                    Cardlogiccs.CardInGroundPickLogic(currentcardsonground[0], CardsOnGround);
                                    pickcomplete = true;
                                }
                                else if (gapSize == 2)
                                {
                                    DhumbalLogAdd("** You Picked from Ground **");
                                    HumanPlayer.PickSingleCard(currentcardsonground[1]);
                                    Cardlogiccs.CardInGroundPickLogic(currentcardsonground[1], CardsOnGround);
                                    pickcomplete = true;
                                }
                                else if (gapSize == 3)
                                {
                                    DhumbalLogAdd("** You Picked from Ground **");
                                    HumanPlayer.PickSingleCard(currentcardsonground[2]);
                                    Cardlogiccs.CardInGroundPickLogic(currentcardsonground[2], CardsOnGround);
                                    pickcomplete = true;
                                }
                                else if (gapSize == 4)
                                {
                                    DhumbalLogAdd("** You Picked from Ground **");
                                    HumanPlayer.PickSingleCard(currentcardsonground[3]);
                                    Cardlogiccs.CardInGroundPickLogic(currentcardsonground[3], CardsOnGround);
                                    pickcomplete = true;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input");
                            }
                        }
                        gapSize = 0;
                        menu = false;
                    } else
                    {
                        if(HumanPlayer.ThrowCardList.Count==0)
                        {
                            Console.WriteLine(" Press DownArrow to move cards in throwlist, Try again!!");
                        }
                        else
                        {
                            Console.WriteLine(" Invalid Throw!, Try again!!");
                        }
                        Thread.Sleep(1500);
                        foreach (var item in HumanPlayer.ThrowCardList)// returning throwlist cards to hand
                        {
                            HumanPlayer.CardInHand.Add(item);
                        }
                        HumanPlayer.ThrowCardList.Clear();
                    }
                }
            }
        }
        private void RefreshScreen()
        {
            Console.Clear();
            //DhumbalLogAddThrowlist(currentPlayer);
            ShowTitle();
            ShowGround();
            Console.WriteLine();
            Console.Write("           ");
            playerlist[0].DisplayeCardInHand();
            Console.WriteLine();
        }
        private void ShowGround()
        {
            int width = 30;
            int height = 10;
            string text = "";
            string turnindicator = "   ";
            foreach (var card in CardsOnGround[CardsOnGround.Count() - 1])
            {
                text += "   ";
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            // Print top side
            if (playerturn == playerlist[2].GetName)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                turnindicator = "<--";
            }
            Console.Write($"                       JACK {turnindicator}       ");
            Console.ResetColor();
            Console.WriteLine("                            Event log ");
            Console.Write("           " + new string('x', width));
            Console.WriteLine("                 ----------------------------------- ");
            turnindicator = "   ";

            // Print left and right sides with text
            for (int i = 0; i < height; i++)
            {
                if (i == height / 2)
                {
                    if (playerturn == playerlist[3].GetName)
                    {
                        turnindicator = "<--";
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write($"  MATT {turnindicator}");
                    Console.ResetColor();
                    Console.Write("*");
                    turnindicator = "   ";
                    int padding = (width - text.Length) / 2;
                    Console.Write(new string(' ', padding-4));
                    int j = 4;
                    foreach (var card in CardsOnGround[CardsOnGround.Count - 1])
                    {
                        Cardlogiccs.PrintCard(card);
                        j = j - 2;
                    }
                    Console.Write(new string(' ', width - text.Length - padding+j)) ;
                    Console.Write("*");
                    if (playerturn == playerlist[1].GetName)
                    {
                        turnindicator = "<--";
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    Console.Write($" BILL {turnindicator}");
                    Console.ResetColor();
                    turnindicator = "   ";
                }
                else
                {
                    Console.Write("          *");
                    Console.Write(new string(' ', width));
                    Console.Write("*");
                }
                if (Dhumballog.Count-1-i >=0)
                {
                    string space = "               ";
                    if (i == height / 2)
                    {
                        space = "      ";
                    }
                    Console.Write(space);
                    ShowLogLine(Dhumballog.Count - 1 - i);
                }
                else
                {
                    Console.WriteLine();
                }
            }
            // Print bottom side
            Console.Write("           "+new string('x', width));
            Console.WriteLine("                 ---------------------------------- ");
            if (playerturn == playerlist[0].GetName)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                turnindicator = "<--";
            }
            Console.WriteLine($"                       YOU {turnindicator}");
            Console.ResetColor();

        }
        private void StartPlaying()
        {
            Console.Clear();
            InitialCardInground();

            // start loop until game ends
            while (!gamecomplete)
            {

                //human player
                HumanPlayerLogic(playerlist[0]);
                RefreshScreen();
                // comp player
                for (int i = 1; i < playerlist.Count(); i++)
                {
                    playerturn = playerlist[i].GetName;
                    DhumbalLogAdd($"** {playerlist[i].GetName}'s turn **");
                    //cpu player turn
                    if (Cardlogiccs.GetTotalIntegerValue(playerlist[i].CardInHand) < 6)
                    {
                        Finisher = playerlist[i];
                        gamecomplete = true; break;
                    }

                    playerlist[i].cardsinInteger = Cardlogiccs.GetCardIntegerValue(playerlist[i].CardInHand);
                    playerlist[i].cardsinInteger.Sort();
                    Thread.Sleep(500);
                    //checkquads and trail
                    if (CardStrengthLogic.CheckColorRunCards(playerlist[i], 5, GameName))
                    {
                    }
                    else if (CardStrengthLogic.CheckXnumCards(playerlist[i], 4, GameName))
                    {
                    }
                    else if (CardStrengthLogic.CheckColorRunCards(playerlist[i], 4, GameName))
                    {

                    }
                    else if (CardStrengthLogic.CheckXnumCards(playerlist[i], 3, GameName))
                    {

                    }
                    else if (CardStrengthLogic.CheckColorRunCards(playerlist[i], 3, GameName))
                    {

                    }
                    else if (CardStrengthLogic.CheckXnumCards(playerlist[i], 2, GameName))
                    {

                    }
                    else
                    {
                        for (int j = 0; j < playerlist[i].cardsinInteger.Count; j++)//Ace to 1 value
                        {
                            if (playerlist[i].cardsinInteger[j] == 14)
                            {
                                playerlist[i].cardsinInteger[j] = 1;
                            }
                        }
                        int maximumnum = playerlist[i].cardsinInteger.Max();
                        var maximumnumcard = playerlist[i].CardInHand.FirstOrDefault(c => c.GetCardValue() == CardConversion.ConversionIntegertoValue(maximumnum));
                        playerlist[i].ThrowCardList.Add(item: maximumnumcard);
                    }
                    RefreshScreen();
                    Thread.Sleep(500);
                    HandleCPUpickthrow(playerlist[i]);
                    RefreshScreen();
                    Thread.Sleep(500);

                }
            }
        }
        private void HandleCPUpickthrow(Player.Player player)
        {
            List<Card> currentcardsonground = CardsOnGround[CardsOnGround.Count - 1];
            DhumbalLogAddThrowlist(player);
            player.ThrowCards();
            //Check ground card compatibility
            if (!CheckPickfromGroundorDeck(player, currentcardsonground))
            {//pick from ground
                carddeck.Draw(player);
                DhumbalLogAdd($"** {player.GetName} Picked from Deck **");
            }
            Cardlogiccs.CardInGroundThrowLogic(player.ThrowCardList, CardsOnGround);// add cards on ground
            player.ThrowCardList.Clear();
        }

        private bool CheckPickfromGroundorDeck(Player.Player player, List<Card> currentcardsonground)
        {
            foreach (var card in currentcardsonground)
            {
                if (player.HaveCardValue(card.GetCardValue()))
                {
                    player.PickSingleCard(card);
                    DhumbalLogAdd($"** {player.GetName} Picked from Ground **");
                    return true;
                }
                else if (Compatiblecolorrun(player, card))
                {
                    DhumbalLogAdd($"** {player.GetName} Picked from Ground **");
                    player.PickSingleCard(card);
                    return true;
                }
                else if ((Cardlogiccs.GetTotalIntegerValue(player.CardInHand) + CardConversion.ConversionValuetoInteger(card.GetCardValue())) < 6)
                {
                    DhumbalLogAdd($"** {player.GetName} Picked from Ground **");
                    player.PickSingleCard(card);
                    return true;
                }
            }
            return false;
        }

        private static bool Compatiblecolorrun(Player.Player player, Card card)
        {

            if (CardConversion.ConversionValuetoInteger(card.GetCardValue()) > 2 &&
                CardConversion.ConversionValuetoInteger(card.GetCardValue()) < 12)
            {
                int cardvalueint = CardConversion.ConversionValuetoInteger(card.GetCardValue());
                int previousvalueint = cardvalueint - 1;
                int prepreviousvalueint = cardvalueint - 2;
                int nextvalueint = cardvalueint + 1;
                int nextnextvalueint = cardvalueint + 2;
                if (player.HaveCard(card.GetCardType(), CardConversion.ConversionIntegertoValue(previousvalueint)) &&
                    player.HaveCard(card.GetCardType(), CardConversion.ConversionIntegertoValue(prepreviousvalueint)))
                {
                    return true;
                }
                else if (player.HaveCard(card.GetCardType(), CardConversion.ConversionIntegertoValue(previousvalueint)) &&
                    player.HaveCard(card.GetCardType(), CardConversion.ConversionIntegertoValue(nextvalueint)))
                {
                    return true;
                }
                else if (player.HaveCard(card.GetCardType(), CardConversion.ConversionIntegertoValue(nextvalueint)) &&
                    player.HaveCard(card.GetCardType(), CardConversion.ConversionIntegertoValue(nextnextvalueint)))
                {
                    return true;
                }
            }
            if (card.GetCardValue() == CardValue.Two)
            {
                if (player.HaveCard(card.GetCardType(), CardValue.Ace) && player.HaveCard(card.GetCardType(), CardValue.Three))
                {
                    return true;
                }
                if (player.HaveCard(card.GetCardType(), CardValue.Four) && player.HaveCard(card.GetCardType(), CardValue.Three))
                {
                    return true;
                }
            }
            if (card.GetCardValue() == CardValue.Ace)
            {
                if (player.HaveCard(card.GetCardType(), CardValue.Two) && player.HaveCard(card.GetCardType(), CardValue.Three))
                {
                    return true;
                }
            }
            if (card.GetCardValue() == CardValue.Queen)
            {
                if (player.HaveCard(card.GetCardType(), CardValue.King) && player.HaveCard(card.GetCardType(), CardValue.Jacks))
                {
                    return true;
                }
                if (player.HaveCard(card.GetCardType(), CardValue.Jacks) && player.HaveCard(card.GetCardType(), CardValue.Ten))
                {
                    return true;
                }
            }
            if (card.GetCardValue() == CardValue.King)
            {
                if (player.HaveCard(card.GetCardType(), CardValue.Queen) && player.HaveCard(card.GetCardType(), CardValue.Jacks))
                {
                    return true;
                }
            }

            return false;
        }
        private void ShowTitle()
        {
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("                           DHUMBAL GAME   ");
            Console.WriteLine("-------------------------------------------------------------------------------");
            Console.WriteLine("CONTROLS : PRESS LEFT RIGHT ARROW TO NAVIGATE CARDS, DOWNARROW TO CHOOSE CARD ");
            Console.WriteLine("           TO THROW, ENTER TO CONFIRM AND PRESS X TO SUBMIT CARD. ENJOY. - MAKER");
            Console.WriteLine("-------------------------------------------------------------------------------");

        }
        private void ShowLogLine(int i)
        {
            string indicator1 = "   ";
            string indicator2 = "   ";
            if (i == Dhumballog.Count - 1)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                indicator1 = "<--";
                indicator2 = "-->";
            }
            Console.WriteLine(" " +indicator2+" "+ Dhumballog[i].ToString().ToUpper() +" "+ indicator1);
            Console.ResetColor();
            indicator1 = "   ";
            indicator2 = "   ";
        }
        private void DhumbalLogAdd(string line)
        {
            if (Dhumballog.Count > 0)
            {
                if (Dhumballog[Dhumballog.Count - 1] != line)
                {
                    Dhumballog.Add(line);
                }
            }
            else
            {
                Dhumballog.Add(line);
            }
        }
        private void DhumbalLogAddThrowlist(Player.Player player)
        {
            string message = $"** {player.GetName} has thrown ";
            foreach (var card in player.ThrowCardList)
            {
                if (player.ThrowCardList[0] == card)
                {
                    message += $"{card.GetCardType()}{card.GetCardValue()}";
                }
                else
                {
                    message += $" and {card.GetCardType()}{card.GetCardValue()}";
                }
            }
            DhumbalLogAdd(message + " **");
        }
        private void InitialCardInground()
        {
            List<Card> temp = new List<Card>();
            temp.Add(carddeck.TotalDeck[carddeck.TotalDeck.Count - 1]);
            CardsOnGround.Add(temp);
            carddeck.TotalDeck.RemoveAt(carddeck.TotalDeck.Count - 1);
        }
    }
}
