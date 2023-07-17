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
using cards.Interface;
using Newtonsoft.Json.Linq;

namespace cards.Utils
{
    public class FlashLogic
    {
        
        public static void RunFlash() 
        {
            Cardlogiccs.CurrentGame = CardGameType.FLASH;
            //New card instance
            CardCompleteDeck carddeck = new CardCompleteDeck();
            Console.WriteLine("*** New Card deck created ***");
            // pleyer logic
            List<Player.Player> playerlist = new List<Player.Player>();
            playerlist = Cardlogiccs.GetPlayers();
            
            //shuffle
            carddeck.Shuffle();

            //Deal 
            FlashLogic.FlashDeal(playerlist, carddeck);
            Console.ReadLine();

            // Game Logic 
            FlashLogic.DetermineWinner(playerlist);
            Console.WriteLine("** Calculating Winner........ **");
            Console.ReadLine();
            //Display winners
            Console.WriteLine("**  Winner   **");
            Cardlogiccs.ShowAllPlayersCard(CardCompareLogics.GetWinnerlist());
            Console.ReadLine();
            Console.Clear();
        }
        public static void FlashDeal(List<Player.Player> list, CardCompleteDeck c)
        {
            int numberofcardstodeal = 3;
            Cardlogiccs.Deal(list, c, numberofcardstodeal,false);
        }
        public static void DetermineWinner(List<Player.Player> list)
        {
            foreach (var p in list)
            {
                p.cardsinInteger = Cardlogiccs.GetCardIntegerValue(p.CardInHand);
                if (CheckTrial(p.CardInHand))
                {
                    p.strength = CardStrength.Trial;
                }
                else if(CheckColor(p.CardInHand) && CheckRun(p.CardInHand))
                {
                    p.strength = CardStrength.ColorSequence;
                    p.cardsinInteger.Sort();
                }
                else if(CheckRun(p.CardInHand))
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
            Cardlogiccs.ShowAllPlayersCard(list);

            CalculateWinner(list);
            Console.WriteLine("");
            
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
                CardCompareLogics.CompareTwoHands(list[i], list[CurrentIndexWinner]);
                if (list[i].result == CardResult.WIN) 
                { 
                    CurrentIndexWinner = i; 
                }
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
            List<int> cardvalue = Cardlogiccs.GetCardIntegerValue(cardInHand);
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
    }
}
