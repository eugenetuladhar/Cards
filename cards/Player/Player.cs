using cards.Cards_files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Player
{
    public class Player
    {
        private string Name = "Default Name";
        public List<Card> CardInHand = new List<Card>();
        public CardStrength strength = new CardStrength();
        public CardResult result = new CardResult();
        public List<int> cardsinInteger = new List<int>();
        public double Balance = new double();

        public List<Card> FinalKittyHand = new List<Card>(); // for Kitty
        public List<CardStrength> kittyStrength = new List<CardStrength>(); //for kitty
        public List<CardResult> Kittyresult = new List<CardResult>();//for kitty
        public bool TurnONOFFpickthrowMessage = false;
        public Player(string name,double bal=0)
        {
            Name = name;
            strength = CardStrength.NotDecided;
            result = CardResult.NONE;
            Balance = bal;
        }
        public string GetName
        {
            get { return Name; }
        }
        public void DisplayeCardInHand()
        {
            string messagetoprint = "";
            //if (iskitty && FinalKittyHand.Any())
            //{
            //    CardInHand = FinalKittyHand;
            //}
            foreach(Card card in CardInHand)
            {
                messagetoprint = messagetoprint+" *" + card.GetCardType() + card.GetCardValue() + "*";
            }
            Console.WriteLine(messagetoprint);
        }
            
        public void PickCard(Card card,bool reveal=true)
        {
            this.CardInHand.Add(card);
            if (TurnONOFFpickthrowMessage)
            {
                ShowPickThrowMessage(card, true, reveal);
            }
        }
        public bool ThrowCard(Card card,bool reveal= true)
        {
            for (int i = 0; i < CardInHand.Count(); i++)
            {
                if (card.GetCardType() == CardInHand[i].GetCardType() &&
                    card.GetCardValue() == CardInHand[i].GetCardValue())
                {
                    CardInHand.RemoveAt(i);
                    if (TurnONOFFpickthrowMessage)
                    {
                        ShowPickThrowMessage(card, false, reveal);
                    }
                    return true;
                }
            }
            return false;
        }
        public void MovetoFinalKittyHand(Card card)
        {
            Remove(card);
            FinalKittyHand.Add(card);
        }
        public void Reset()
        {
            CardInHand.Clear();
        }
        public void ShowPickThrowMessage(Card card,bool pickorthrow,bool reveal=true)
        {
            string acard,hashave="has";
            if(reveal)
            {
                acard= ""+card.GetCardType() + card.GetCardValue();
            }
            else
            {
                acard = "card";
            }
            if (Checkplayer_you())
            {
                hashave = "have";
            }

            if (pickorthrow)
            {
                Console.WriteLine($"****{Name} {hashave} picked {acard} ****");
            }
            else
            {
                Console.WriteLine($"****{Name} {hashave} thrown {acard} ****");

            }
        }
        public bool Checkplayer_you()
        {
            if (Name == "You")
            {
                return true;
            }
            return false;
        }
        public void DeductBalance(double sum)
        {
            Balance = Balance - sum;
            if (Balance > 0) { Balance = 0; }
        }
        public void AddBalance(double sum)
        {
            Balance = Balance + sum;
        }
        public void RemoveAt(int index)
        {
            CardInHand.RemoveAt(index);
        }
        public void Remove(Card card)
        {
            for (int i = 0; i < CardInHand.Count(); i++)
            {
                if (card.GetCardType() == CardInHand[i].GetCardType() &&
                    card.GetCardValue() == CardInHand[i].GetCardValue())
                {
                    CardInHand.RemoveAt(i);
                }
            }
        }
    }
}
