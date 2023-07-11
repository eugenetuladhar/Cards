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
        public List<Card> FinalKittyHand = new List<Card>(); // for Kitty
        public List<CardStrength> kittyStrength = new List<CardStrength>(); //for kitty
        public CardStrength strength = new CardStrength();
        public CardResult result = new CardResult();
        public List<int> cardsinInteger = new List<int>();
        public double balance = new double();
        public Player(string name,double bal=0)
        {
            Name = name;
            strength = CardStrength.NotDecided;
            result = CardResult.NONE;
            balance = bal;
        }
        public string GetName
        {
            get { return Name; }
        }
        public void GetCardtoPrint()
        {
            string messagetoprint = "";
            foreach(Card card in CardInHand)
            {
                messagetoprint = messagetoprint+" *" + card.GetCardType() + card.GetCardNumber() + "*";
            }
            Console.WriteLine(messagetoprint);
        }
            
        public void PickCard(Card card,bool reveal=true)
        {
            this.CardInHand.Add(card);
            ShowPickThrowMessage(card, true,reveal);
        }
        public bool ThrowCard(Card card,bool reveal= true)
        {
            for (int i = 0; i < CardInHand.Count(); i++)
            {
                if (card.GetCardType() == CardInHand[i].GetCardType() &&
                    card.GetCardNumber() == CardInHand[i].GetCardNumber())
                {
                    CardInHand.RemoveAt(i);
                    ShowPickThrowMessage(card, false,reveal);
                    return true;
                }
            }
            return false;
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
                acard= ""+card.GetCardType() + card.GetCardNumber();
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
            balance = balance - sum;
            if (balance > 0) { balance = 0; }
        }
        public void AddBalance(double sum)
        {
            balance = balance + sum;
        }
    }
}
