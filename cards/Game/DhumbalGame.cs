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
        private List<Card> CardsInGround { get; set; }
        CardCompleteDeck carddeck;
        private bool PICK = true;
        private bool THROW = false;
        public DhumbalGame()
        {
            carddeck = new CardCompleteDeck();
            CardsInGround = new List<Card>();
            GameName = CardGameType.DHUMBAL;
            MAX_NUM_PLAYERS = 4;
            NUM_CARDS_TO_DEAL = 5;
        }
        public void Run()
        {
            //New card instance
            Console.WriteLine();
            // Player logic
            List<Player.Player> playerlist = Cardlogiccs.GetPlayers(MAX_NUM_PLAYERS);

            //shuffle
            carddeck.Shuffle();

            //Deal
            Deal(playerlist, carddeck);

            //Start
            StartPlaying(playerlist);
        }
        private void Deal(List<Player.Player> list, CardCompleteDeck c)
        {
            Cardlogiccs.Deal(list, c, NUM_CARDS_TO_DEAL, false);
            // display pick and throw cards
            foreach (var p in list)
            {
                p.TurnONOFFpickthrowMessage = true;
            }
        }
        private void CardInGroundLogic(Card card, bool pickorthrow)
        {
            if (pickorthrow)
            {
                if (CardsInGround[CardsInGround.Count - 2] == card)
                {
                    CardsInGround.RemoveAt(CardsInGround.Count - 2);
                }
                else
                {
                    Console.Error.WriteLine("Invalid pick");
                }
            }
            else if (!pickorthrow)
            {
                CardsInGround.Add(card);
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
                Console.WriteLine("Your turn");
                ShowGround();
                Card cardonground = CardsInGround[CardsInGround.Count - 1];
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
                else if(keyInfo.Key == ConsoleKey.T)
                {
                    // throw card
                    Card cardtothrow = HumanPlayer.CardInHand[gapSize];
                    CardInGroundLogic(cardtothrow, THROW);
                    HumanPlayer.ThrowCard(cardtothrow);

                    // pick from ground or deck
                    Console.WriteLine(" Pick from Ground or Deck?");
                    Console.WriteLine(" G or D");
                    bool pickcomplete = false;
                    while (!pickcomplete)
                    {
                        keyInfo = Console.ReadKey(true);
                        
                        if (keyInfo.Key == ConsoleKey.D)
                        {
                            Console.WriteLine(" Pick from Deck");
                            pickcomplete = true;
                            carddeck.Draw(HumanPlayer);
                            Console.ReadLine();
                        }
                        else if (keyInfo.Key == ConsoleKey.G)
                        {
                            Console.WriteLine(" Pick from Ground");
                            HumanPlayer.PickCard(cardonground);
                            CardInGroundLogic(cardonground,PICK);
                            pickcomplete = true;
                            Console.ReadLine();
                        }
                        else
                        {
                            Console.WriteLine("Invalid input");
                        }
                    }
                    gapSize = 0;
                }
                Console.Clear();
            }
        }
        private void ShowGround()
        {
            Console.WriteLine("********* GROUND **********");
            Console.WriteLine();
            Console.Write("        ");
            Cardlogiccs.PrintCard( CardsInGround[CardsInGround.Count - 1]);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("********* GROUND **********");
        }
        private void StartPlaying(List<Player.Player> playerlist)
        {
            Console.Clear();
            InitialCardInground();

            // start loop until game ends

            //human player
            HumanPlayerLogic(playerlist[0]);
            // comp player

        }

        private void InitialCardInground()
        {
            CardsInGround.Add(carddeck.TotalDeck[carddeck.TotalDeck.Count - 1]);
            carddeck.TotalDeck.RemoveAt(carddeck.TotalDeck.Count - 1);
        }
    }
}
