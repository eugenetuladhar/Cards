using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Cards_files
{
    public class Card
    {
        private CardType Type;
        private CardValue Number;
        public Card( CardType type, CardValue number)
        {
            Type = type;
            Number = number;
        }
        public CardType GetCardType()
        {
            return this.Type;
        }
        public CardValue GetCardValue()
        {
            return this.Number;
        }
    }
}
