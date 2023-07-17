﻿using cards.Cards_files;
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

            //testcode
            //playerlist[0].CardInHand.Clear();
            //playerlist[0].CardInHand.Add(new Card(CardType.Diamond, CardValue.Ten));
            //playerlist[0].CardInHand.Add(new Card(CardType.Club, CardValue.Jacks));
            //playerlist[0].CardInHand.Add(new Card(CardType.Diamond, CardValue.Queen));
            //playerlist[1].CardInHand.Clear();
            //playerlist[1].CardInHand.Add(new Card(CardType.Diamond, CardValue.Ace));
            //playerlist[1].CardInHand.Add(new Card(CardType.Club, CardValue.Queen));
            //playerlist[1].CardInHand.Add(new Card(CardType.Club, CardValue.King));
            //playerlist[2].CardInHand.Clear();
            //playerlist[2].CardInHand.Add(new Card(CardType.Club, CardValue.Four));
            //playerlist[2].CardInHand.Add(new Card(CardType.Diamond, CardValue.Five));
            //playerlist[2].CardInHand.Add(new Card(CardType.Club, CardValue.Six));
            //playerlist[3].CardInHand.Clear();
            //playerlist[3].CardInHand.Add(new Card(CardType.Spade, CardValue.Ace));
            //playerlist[3].CardInHand.Add(new Card(CardType.Spade, CardValue.King));
            //playerlist[3].CardInHand.Add(new Card(CardType.Spade, CardValue.Nine));
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
                CardStrengthLogic.InsertStrengthThreeCards(p);
                
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
    }
}
