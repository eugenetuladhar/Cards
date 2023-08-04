using cards.Cards_files;
using cards.Interface;
using cards.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        List<Player.Player> playerlist;
        public DhumbalGame()
        {
            carddeck = new CardCompleteDeck();
            CardsOnGround = new List<List<Card>>();
            GameName = CardGameType.DHUMBAL;
            MAX_NUM_PLAYERS = 4;
            NUM_CARDS_TO_DEAL = 5;
        }
        public void Run()
        {
            //New card instance
            Console.WriteLine();
            // Player logic
            playerlist = Cardlogiccs.GetPlayers(MAX_NUM_PLAYERS);

            //shuffle
            carddeck.Shuffle();

            //Deal
            Deal();

            //Start
            StartPlaying();

            //Display winner
            DetermineWinner();
        }

        private void DetermineWinner()
        {
            Cardlogiccs.ShowAllPlayersCard(playerlist);
            Console.ReadLine();
            //Display winners
            Console.WriteLine("**  Winner   **");
            Console.WriteLine();
        }

        private void Deal()
        {
            Cardlogiccs.Deal(playerlist, carddeck, NUM_CARDS_TO_DEAL, false);
            // display pick and throw cards
            foreach (var p in playerlist)
            {
                p.TurnONOFFpickthrowMessage = true;
            }
        }
        private void HumanPlayerLogic(Player.Player HumanPlayer)
        {
            // Sort the list based on Property1
            HumanPlayer.CardInHand.Sort((a, b) => a.GetCardType().CompareTo(b.GetCardType()));

            ConsoleKeyInfo keyInfo;// for arrow movement user interface
            int gapSize = 0;
            bool move = false;
            string symbol = "\u2191";
            
            bool menu = true;
            while (menu)
            {
                Console.Clear();
                Console.WriteLine("Your turn");
                ShowGround();
                List<Card> currentcardsonground = CardsOnGround[CardsOnGround.Count - 1];
                Console.WriteLine();
                Console.Write("     ");
                HumanPlayer.DisplayeCardInHand();
                Console.WriteLine();
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
                else if (keyInfo.Key == ConsoleKey.RightArrow && gapSize < 4)
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
                    if(Cardlogiccs.GetTotalIntegerValue(HumanPlayer.CardInHand)<6)
                    {
                        //determinewinner
                        gamecomplete= true;
                        menu = false;
                    }
                    else
                    {
                        Console.WriteLine("Not eligible to submit. Should be less than 6");
                    }
                }
                else if (keyInfo.Key == ConsoleKey.T)
                {
                    // throw card
                    Card cardtothrow = HumanPlayer.CardInHand[gapSize];
                    HumanPlayer.ThrowCard(cardtothrow);

                    //refresh screen logic
                    Console.Clear();
                    Console.WriteLine("Your turn");
                    ShowGround();
                    Console.WriteLine();
                    Console.Write("     ");
                    HumanPlayer.DisplayeCardInHand();
                    Console.WriteLine();

                    //extralogic
                    Cardlogiccs.CardInGroundThrowLogic(cardtothrow, CardsOnGround);
                    HumanPlayer.ShowPickThrowMessage(cardtothrow, THROW);

                    // pick from ground or deck
                    Console.Write( " Pick from: DECK ");
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
                            Console.Clear();
                            Console.WriteLine("Your turn");
                            ShowGround();
                            Console.WriteLine();
                            Console.Write("     ");
                            HumanPlayer.DisplayeCardInHand();
                            Console.WriteLine();

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

                            Console.WriteLine($"left arrow {gapSize}");
                        }
                        else if (keyInfo.Key == ConsoleKey.RightArrow && gapSize < currentcardsonground.Count())
                        {
                            gapSize++;
                            //refresh screen logic
                            Console.Clear();
                            Console.WriteLine("Your turn");
                            ShowGround();
                            Console.WriteLine();
                            Console.Write("     ");
                            HumanPlayer.DisplayeCardInHand();
                            Console.WriteLine();

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

                            Console.WriteLine($"right arrow {gapSize}");
                        }
                        else if (keyInfo.Key == ConsoleKey.Enter)
                        {
                            if (gapSize == 0)
                            {
                                Console.WriteLine(" Pick from Deck");
                                pickcomplete = true;
                                carddeck.Draw(HumanPlayer);
                                Console.ReadLine();
                            }else if(gapSize == 1)
                            {
                                Console.WriteLine(" Pick from Ground");
                                HumanPlayer.PickCard(currentcardsonground[0]);
                                Cardlogiccs.CardInGroundPickLogic(currentcardsonground[0], CardsOnGround);
                                pickcomplete = true;
                                Console.ReadLine();
                            }
                            else if (gapSize == 2)
                            {
                                Console.WriteLine(" Pick from Ground");
                                HumanPlayer.PickCard(currentcardsonground[1]);
                                Cardlogiccs.CardInGroundPickLogic(currentcardsonground[1], CardsOnGround);
                                pickcomplete = true;
                                Console.ReadLine();
                            }
                            else if (gapSize == 3)
                            {
                                Console.WriteLine(" Pick from Ground");
                                HumanPlayer.PickCard(currentcardsonground[2]);
                                Cardlogiccs.CardInGroundPickLogic(currentcardsonground[2], CardsOnGround);
                                pickcomplete = true;
                                Console.ReadLine();
                            }
                            else if (gapSize == 4)
                            {
                                Console.WriteLine(" Pick from Ground");
                                HumanPlayer.PickCard(currentcardsonground[3]);
                                Cardlogiccs.CardInGroundPickLogic(currentcardsonground[3], CardsOnGround);
                                pickcomplete = true;
                                Console.ReadLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }
                    }
                    gapSize = 0;
                    menu = false;
                }
            }
        }
        private void ShowGround()
        {
            Console.WriteLine("********* GROUND **********");
            Console.WriteLine();
            Console.Write("        ");
            foreach (var card in CardsOnGround[CardsOnGround.Count()-1])
            {
                Cardlogiccs.PrintCard(card);
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("********* GROUND **********");
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
                // comp player
                for (int i = 1; i < playerlist.Count(); i++)
                {
                    //cpu player turn
                    if (Cardlogiccs.GetTotalIntegerValue(playerlist[i].CardInHand)<6)
                    {
                        gamecomplete = true; break;
                    }
                }
            }

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
