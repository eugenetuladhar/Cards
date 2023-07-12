using cards.Cards_files;
using cards.Player;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace cards.Utils
{
    public class FlashLogic
    {
        public static void RunFlash() 
        {
            //New card instance
            CardCompleteDeck carddeck = new CardCompleteDeck();
            Console.WriteLine("*** New Card deck created ***");
            // pleyer logic
            List<Player.Player> playerlist = new List<Player.Player>();
            bool t = true;
            int numofplayers=0;
            while (t)
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
                switch (numofplayers)
                {
                    case 2:
                        t=false;
                        break;
                    case 3:
                        t = false;
                        break;
                    case 4:
                        t = false;
                        break;
                    case 5:
                        t = false;
                        break;
                    default: 
                        Console.WriteLine(" Invalid number or too high ");
                        Console.WriteLine(" Again! ");
                        break;
                }
            }

            playerlist = Cardlogiccs.GetPlayers(numofplayers);
            
            //shuffle
            carddeck.Shuffle();

            //Deal 
            FlashLogic.FlashDeal(playerlist, carddeck);
            Console.ReadLine();

            // Game Logic 
            FlashLogic.DetermineWinner(playerlist);
            Console.ReadLine();
            Console.Clear();
        }

        private static List<Player.Player> Winnerlist= new List<Player.Player>();
        public static void FlashDeal(List<Player.Player> list, CardCompleteDeck c)
        {
            int numberofcardstodeal = 3;
            Cardlogiccs.Deal(list, c, numberofcardstodeal,false);
        }
        public static void DetermineWinner(List<Player.Player> list)
        {
            Cardlogiccs.ShowAllPlayersCard(list);
            foreach (var p in list)
            {
                if (CheckTrial(p.CardInHand))
                {
                    p.strength = CardStrength.Trial;
                    p.cardsinInteger =GetCardValue(p.CardInHand);
                }
                else if(CheckColor(p.CardInHand) && CheckRun(p.CardInHand))
                {
                    p.strength = CardStrength.ColorSequence;
                    p.cardsinInteger = GetCardValue(p.CardInHand);
                    p.cardsinInteger.Sort();
                }
                else if(CheckRun(p.CardInHand))
                {
                    p.strength = CardStrength.Sequence;
                    p.cardsinInteger = GetCardValue(p.CardInHand);
                    p.cardsinInteger.Sort();

                }
                else if (CheckColor(p.CardInHand))
                {
                    p.strength = CardStrength.Color;
                    p.cardsinInteger = GetCardValue(p.CardInHand);
                    p.cardsinInteger.Sort();

                }
                else if (CheckJoot(p.CardInHand))
                {
                    p.strength = CardStrength.Double;
                    p.cardsinInteger = GetCardValue(p.CardInHand);
                    p.cardsinInteger.Sort();

                }
                else
                {
                    p.strength = CardStrength.Common;
                    p.cardsinInteger = GetCardValue(p.CardInHand);
                    p.cardsinInteger.Sort();

                }
            }
            CalculateWinner(list);
            Console.WriteLine("");
            Console.WriteLine("** Calculating Winner........ **");
            Console.ReadLine();
            foreach (var player in Winnerlist)
            {
                string hashave = "has";
                if (player.Checkplayer_you())
                {
                    hashave = "have";
                }
                Console.WriteLine($"{player.GetName} {hashave} won with {player.strength}");
                player.GetCardtoPrint();
            }
        }

        private static void CalculateWinner(List<Player.Player> list)
        {
            
            int CurrentIndexWinner = 0;
            for (int i = 0; i < list.Count(); i++)
            {
                for (int j = 0; j < list[i].cardsinInteger.Count(); j++)//Ace to 14 value
                {
                    if (list[i].cardsinInteger[j] == 1)
                    {
                        list[i].cardsinInteger[j] = 14;
                    }
                }
                if (i == 0)
                {
                    continue;
                }
                CompareTwoHands(list[i], list[CurrentIndexWinner]);
                if (list[i].result == CardResult.WIN) 
                { 
                    CurrentIndexWinner = i; 
                }
            }
        }

        private static void CompareTwoHands(Player.Player player1, Player.Player player2)
        {
            if (CompareTrial(player1, player2)) { }
            else if (CompareColorSequence(player1, player2)) { }
            else if (CompareSequence(player1, player2)) { }
            else if (CompareColor(player1, player2)) { }
            else if (CompareDouble(player1, player2)) { }
            else
            {
                CompareCommon(player1, player2);
            }
        }

        private static void CompareCommon(Player.Player player1, Player.Player player2)
        {
            player1.cardsinInteger.Sort();
            player2.cardsinInteger.Sort();

            if (player1.cardsinInteger[2] == player2.cardsinInteger[2])
            {
                if (player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    if (player1.cardsinInteger[0] == player2.cardsinInteger[0])
                    {
                        player1.result = CardResult.DRAW;
                        player2.result = CardResult.DRAW;
                        Winnerlist.Add(player1);
                        Winnerlist.Add(player2);
                    }
                    else if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                    {
                        player1.result = CardResult.WIN;
                        player2.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player1);
                    }
                    else if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                    {
                        player2.result = CardResult.WIN;
                        player1.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player2);
                    }
                    else
                    {
                        Logger.LogMessage("error at common compare");
                    }
                }
                else if (player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                else if (player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                else
                {
                    Logger.LogMessage("error at common compare");

                }
            }
            else if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
            }
            else if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
            }
        }

        private static bool CompareDouble(Player.Player player1, Player.Player player2)
        {
            if(player1.strength==CardStrength.Double && player2.strength == CardStrength.Double)
            {
                if (player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    int player1remainingcard,player2remainingcard;
                    if (player1.cardsinInteger[0] == player1.cardsinInteger[1]) { player1remainingcard = player1.cardsinInteger[2]; }
                    else { player1remainingcard = player1.cardsinInteger[0]; }
                    if (player2.cardsinInteger[0] == player2.cardsinInteger[1]) { player2remainingcard = player2.cardsinInteger[2]; }
                    else { player2remainingcard = player2.cardsinInteger[0]; }

                    if(player1remainingcard== player2.cardsinInteger[1])
                    {
                        player1.result = CardResult.DRAW;
                        player2.result = CardResult.DRAW;
                        Winnerlist.Add(player1);
                        Winnerlist.Add(player2);
                    }
                    if (player1remainingcard > player2remainingcard)
                    {
                        player1.result = CardResult.WIN;
                        player2.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player1);
                    }
                    if (player1remainingcard < player2remainingcard)
                    {
                        player2.result = CardResult.WIN;
                        player1.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player2);
                    }
                }
                if (player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result= CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if(player1.strength == CardStrength.Double)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;
            }
            else if (player2.strength == CardStrength.Double)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareColor(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.Color && player2.strength == CardStrength.Color)
            {
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {    if (player1.cardsinInteger[2] == player2.cardsinInteger[2])
                    {
                        player1.result = CardResult.DRAW;
                        player2.result = CardResult.DRAW;
                        Winnerlist.Add(player1);
                        Winnerlist.Add(player2);
                    }
                    if (player1.cardsinInteger[2] > player2.cardsinInteger[2])
                    {
                        player1.result = CardResult.WIN;
                        player2.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player1);
                    }
                    if (player1.cardsinInteger[2] < player2.cardsinInteger[2])
                    {
                        player2.result = CardResult.WIN;
                        player1.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player2);
                    }
                    return true;
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.Color)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.Color)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareSequence(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.Sequence && player2.strength == CardStrength.Sequence)
            {
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    player1.result = CardResult.DRAW;
                    player2.result = CardResult.DRAW;
                    Winnerlist.Add(player1);
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.Sequence)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.Sequence)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareColorSequence(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.ColorSequence && player2.strength == CardStrength.ColorSequence)
            {
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0]&& player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    player1.result = CardResult.DRAW;
                    player2.result = CardResult.DRAW;
                    Winnerlist.Add(player1);
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.ColorSequence)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.ColorSequence)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareTrial(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.Trial && player2.strength == CardStrength.Trial)
            {
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0])
                {
                    player1.result = CardResult.DRAW;
                    player2.result = CardResult.DRAW;
                    Winnerlist.Add(player1);
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.Trial)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.Trial)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else 
            { 
                return false; 
            }
        }

        public static bool CheckTrial(List<Card> cardInHand)
        {
            if (cardInHand[0].GetCardValue() == cardInHand[1].GetCardValue() &&
                cardInHand[0].GetCardValue() == cardInHand[2].GetCardValue())
            {

                return true;    
            }
            return false;
        }
        public static bool CheckColor(List<Card> cardInHand)
        {
            if (cardInHand[0].GetCardType() == cardInHand[1].GetCardType() &&
                cardInHand[0].GetCardType() == cardInHand[2].GetCardType())
            {

                return true;
            }
            return false;
        }
        public static bool CheckJoot(List<Card> cardInHand)
        {
            if(CheckTrial(cardInHand)) 
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
        public static bool CheckRun(List<Card> cardInHand)
        {
            if (CheckTrial(cardInHand) || CheckJoot(cardInHand))
            {
                return false;
            }
            List<int> cardvalue = GetCardValue(cardInHand);
            cardvalue.Sort();
            if (cardvalue[0] == cardvalue[1]-1 && cardvalue[2] == cardvalue[1] + 1)
            {
                return true;
            }
            if (cardvalue[0] == 1 && cardvalue[1]==12 && cardvalue[2] == 13)
            {
                return true;
            }
            return false; 
        }

        private static List<int> GetCardValue(List<Card> cardInHand)
        {
            List<int> listvalue = new List<int>();
            foreach (var item in cardInHand)
            {
                var temp = item.GetCardValue();
                listvalue.Add(CardConversion.ConversionValuetoInteger(temp));
            }
            return listvalue;
        }
    }
}
