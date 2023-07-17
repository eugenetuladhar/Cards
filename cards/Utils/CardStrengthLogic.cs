using cards.Cards_files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Utils
{
    public class CardStrengthLogic
    {
        public static void InsertStrengthThreeCards(Player.Player p)
        {
            p.cardsinInteger = Cardlogiccs.GetCardIntegerValue(p.CardInHand);
            if (CheckTrial(p.CardInHand))
            {
                p.strength = CardStrength.Trial;
            }
            else if (CheckColor(p.CardInHand) && CheckRun(p.CardInHand))
            {
                p.strength = CardStrength.ColorSequence;
                p.cardsinInteger.Sort();
            }
            else if (CheckRun(p.CardInHand))
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
        private static bool CheckTrial(List<Card> cardInHand)
        {
            if (cardInHand[0].GetCardValue() == cardInHand[1].GetCardValue() &&
                cardInHand[0].GetCardValue() == cardInHand[2].GetCardValue())
            {

                return true;
            }
            return false;
        }
        private static bool CheckColor(List<Card> cardInHand)
        {
            if (cardInHand[0].GetCardType() == cardInHand[1].GetCardType() &&
                cardInHand[0].GetCardType() == cardInHand[2].GetCardType())
            {

                return true;
            }
            return false;
        }
        private static bool CheckJoot(List<Card> cardInHand)
        {
            if (CheckTrial(cardInHand))
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
        private static bool CheckRun(List<Card> cardInHand)
        {
            if (CheckTrial(cardInHand) || CheckJoot(cardInHand))
            {
                return false;
            }
            List<int> cardvalue = Cardlogiccs.GetCardIntegerValue(cardInHand);
            cardvalue.Sort();
            if (cardvalue[0] == cardvalue[1] - 1 && cardvalue[2] == cardvalue[1] + 1)
            {
                return true;
            }
            if (cardvalue[0] == 1 && cardvalue[1] == 12 && cardvalue[2] == 13)
            {
                return true;
            }
            return false;
        }
    }
}
