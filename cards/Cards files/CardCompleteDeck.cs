﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using cards.Player;
namespace cards.Cards_files
{
    public class CardCompleteDeck
    {
        public List<Card> TotalDeck = new();

        private readonly List<CardValue> listOfCardNumbers = new() { CardValue.Ace,CardValue.Two,CardValue.Three,CardValue.Four,
                                                                    CardValue.Five,CardValue.Six,CardValue.Seven,CardValue.Eight,
                                                                     CardValue.Nine,CardValue.Ten,CardValue.Jacks,CardValue.Queen,
                                                                      CardValue.King}; 
        private readonly List<CardType> listOfCardType = new() { CardType.Club,CardType.Heart,CardType.Spade,CardType.Diamond};
        public CardCompleteDeck() {
            CreateNewDeck();
        }

        private void CreateNewDeck()
        {
            foreach (var type in listOfCardType)
            {

                foreach (var num in listOfCardNumbers)
                {
                    Card card = new Card(type, num);
                    TotalDeck.Add(card);
                }
            }
        }
        public void Shuffle()
        {
            Random random= new Random();

            for (int i = 0; i < TotalDeck.Count(); i++)
            {
                var randomIndex = random.Next(TotalDeck.Count);
                var temp = TotalDeck[i];
                TotalDeck[i] = TotalDeck[randomIndex];
                TotalDeck[randomIndex] = temp;                
            }
        }
        public bool HaveCard(Card card)
        {
            return HaveCard(card.GetCardType(),card.GetCardValue());
        }
        public bool HaveCard(CardType cardType,CardValue cardvalue)
        {
            bool have = false;
            for (int i = 0; i < TotalDeck.Count(); i++)
            {
                if (cardType == TotalDeck[i].GetCardType() &&
                    cardvalue == TotalDeck[i].GetCardValue())
                {
                    have = true;
                }
            }
            return have;
        }
        public void RemoveAt(int index)
        {
            TotalDeck.RemoveAt(index);
        }
        public bool Remove(Card card)
        {
            for (int i = 0; i < TotalDeck.Count(); i++)
            {
                if (card.GetCardType() == TotalDeck[i].GetCardType() &&
                    card.GetCardValue() == TotalDeck[i].GetCardValue())
                {
                    TotalDeck.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public void AddCardOneDeck(Card card)
        {
            if(TotalDeck.Count<=52 && !HaveCard(card))
            {
                TotalDeck.Add(card);
            }
        }
        public void Reset()
        {
            TotalDeck.Clear();
            CreateNewDeck();
        }
        public void Draw(Player.Player p,bool reveal=true)
        {
            p.PickSingleCard(TotalDeck[TotalDeck.Count-1],reveal);
            RemoveAt(TotalDeck.Count-1);
        }
    }
}
