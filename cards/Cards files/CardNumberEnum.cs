﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Cards_files
{
    public enum CardValue{
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
        public static int ConversionValuetoInteger(CardValue item)
        {
            switch (item)
            {
                case CardValue.Ace:
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
    }
}