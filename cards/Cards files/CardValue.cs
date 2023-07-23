using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Cards_files
{
    public enum CardValue
    {
        Ace,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jacks,
        Queen,
        King
    }
    public static class CardConversion
    {
        public static int ConversionValuetoInteger(CardValue item, bool greateracevalue = false)
        {
            switch (item)
            {
                case CardValue.Ace:
                    if (greateracevalue)
                    {
                        return 14;
                    }
                    return 1;
                case CardValue.Two:
                    return 2;
                case CardValue.Three:
                    return 3;
                case CardValue.Four:
                    return 4;
                case CardValue.Five:
                    return 5;
                case CardValue.Six:
                    return 6;
                case CardValue.Seven:
                    return 7;
                case CardValue.Eight:
                    return 8;
                case CardValue.Nine:
                    return 9;
                case CardValue.Ten:
                    return 10;
                case CardValue.Jacks:
                    return 11;
                case CardValue.Queen:
                    return 12;
                case CardValue.King:
                    return 13;
                default: return 0;
            }
        }
        public static CardValue ConversionIntegertoValue(int num)
        {
            switch (num)
            {
                case 1:
                    return CardValue.Ace;
                case 14:
                    return CardValue.Ace;
                case 2:
                    return CardValue.Two;
                case 3:
                    return CardValue.Three;
                case 4:
                    return CardValue.Four;
                case 5:
                    return CardValue.Five;
                case 6:
                    return CardValue.Six;
                case 7:
                    return CardValue.Seven;
                case 8:
                    return CardValue.Eight;
                case 9:
                    return CardValue.Nine;
                case 10:
                    return CardValue.Ten;
                case 11:
                    return CardValue.Jacks;
                case 12:
                    return CardValue.Queen;
                case 13:
                    return CardValue.King;
                default: return 0;
            }
        }

        public static string GetPrintedForm(Card card)
        {//"\u2665", "\u2666", "\u2663", "\u2660" }
            string returnvalue = "";
            switch (card.GetCardType())
            {
                case CardType.Spade:
                    returnvalue = "\u2660";
                    break;
                case CardType.Club:
                    returnvalue = "\u2663";
                    break;
                case CardType.Diamond:
                    returnvalue = "\u2666";
                    break;
                case CardType.Heart:
                    returnvalue = "\u2665";
                    break;
                default:
                    returnvalue = "no suits";
                    break;

            }

            switch (card.GetCardValue())
            {
                case CardValue.Ace:
                    returnvalue = returnvalue + "A ";
                    break;
                case CardValue.Two:
                    returnvalue = returnvalue + "2 ";
                    break;
                case CardValue.Three:
                    returnvalue = returnvalue + "3 ";
                    break;
                case CardValue.Four:
                    returnvalue = returnvalue + "4 ";
                    break;
                case CardValue.Five:
                    returnvalue = returnvalue + "5 ";
                    break;
                case CardValue.Six:
                    returnvalue = returnvalue + "6 ";
                    break;
                case CardValue.Seven:
                    returnvalue = returnvalue + "7 ";
                    break;
                case CardValue.Eight:
                    returnvalue = returnvalue + "8 ";
                    break;
                case CardValue.Nine:
                    returnvalue = returnvalue + "9 ";
                    break;
                case CardValue.Ten:
                    returnvalue = returnvalue + "10";
                    break;
                case CardValue.Jacks:
                    returnvalue = returnvalue + "J ";
                    break;
                case CardValue.Queen:
                    returnvalue = returnvalue + "Q ";
                    break;
                case CardValue.King:
                    returnvalue = returnvalue + "K ";
                    break;
                default: return returnvalue + "no value";
            }
            return returnvalue;
        }
    }
}
