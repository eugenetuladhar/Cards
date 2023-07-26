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
    public class CallBreakGame : IGame
    {
        public CardGameType GameName { get; set; }
        public int MAX_NUM_PLAYERS { get; set; }
        public int NUM_CARDS_TO_DEAL { get; set; }
        public CallBreakGame()
        {
            GameName = CardGameType.CALLBREAK;
            MAX_NUM_PLAYERS = 4;
            NUM_CARDS_TO_DEAL = 13;
        }
        public void Run()
        {
            //New card instance
            CardCompleteDeck carddeck = new CardCompleteDeck();
            Console.WriteLine();
            // pleyer logic
            List<Player.Player> playerlist = new List<Player.Player>();
            playerlist = Cardlogiccs.GetPlayers(MAX_NUM_PLAYERS,false);

            //shuffle
            carddeck.Shuffle();

            //Deal 
            Deal(playerlist, carddeck);

            //Startgame
            StartPlaying(playerlist);
        }
        private void Deal(List<Player.Player> list, CardCompleteDeck c)
        {
            Cardlogiccs.Deal(list, c, NUM_CARDS_TO_DEAL, false);
        }
        private void StartPlaying(List<Player.Player> playerlist)
        {
            Console.Clear();
            //human player
            HumanPlayerLogic(playerlist[0]);
        }

        private void HumanPlayerLogic(Player.Player HumanPlayer)
        {
            // Sort the list based on Property1
            HumanPlayer.CardInHand.Sort((a, b) => a.GetCardType().CompareTo(b.GetCardType()));

            Console.WriteLine("Your cards are as follow, Forcast number of hands");
            Console.WriteLine();
            HumanPlayer.DisplayeCardInHand();
            Console.WriteLine();
            bool menu = true;
            while(menu)
            {
                Console.WriteLine("Enter forcast number of hands:");

                try
                {
                    HumanPlayer.Forecast_num_of_hands = int.Parse(Console.ReadLine());

                }
                catch
                {
                    Console.WriteLine("Enter again !!");
                    continue;
                }
                if (HumanPlayer.Forecast_num_of_hands > 0 && HumanPlayer.Forecast_num_of_hands < 14)
                {
                    menu= false;
                }
                else
                {
                    Console.WriteLine("Enter again !!");
                }
            }
        }
    }
}
