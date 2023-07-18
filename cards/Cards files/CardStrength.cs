using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Cards_files
{
    public enum CardStrength
    {
        Quad,       // For Kitty
        Trial,
        ColorSequence,
        Sequence,
        Color,
        Double,
        Common,
        NotDecided,
    }
    public class CardStrengthUtils
    {
        public static int GetPriorty(CardStrength cs)
        {
            switch (cs)
            {
                case CardStrength.Quad:
                    return 7;
                case CardStrength.Trial:
                    return 6;
                case CardStrength.ColorSequence:
                    return 5;
                case CardStrength.Sequence:
                    return 4;
                case CardStrength.Color:
                    return 3;
                case CardStrength.Double:
                    return 2;
                case CardStrength.Common:
                    return 1;
                case CardStrength.NotDecided:
                    return -1;
                default:
                    return -1;
            }
        }
    }
}
