using cards.Cards_files;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Utils
{
    public class CardCompareLogics
    {
        private static List<Player.Player> Winnerlist = new List<Player.Player>();
        public static List<Player.Player> GetWinnerlist()
        {
            return Winnerlist;
        }
        public static void ResetWinnerlist()
        {
            Winnerlist.Clear();
        }
        public static void CompareTwoHands(Player.Player player1, Player.Player player2)
        {
            if (CompareTrial(player1, player2)) { }
            else if (CompareColorSequence(player1, player2)) { }
            else if (CompareSequence(player1, player2)) { }
            else if (CompareColor(player1, player2)) { }
            else if (CompareDouble(player1, player2)) { }
            else
            {
                CompareCommon(player1, player2);
            }
        }

        private static void CompareCommon(Player.Player player1, Player.Player player2)
        {
            player1.cardsinInteger.Sort();
            player2.cardsinInteger.Sort();

            if (player1.cardsinInteger[2] == player2.cardsinInteger[2])
            {
                if (player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    if (player1.cardsinInteger[0] == player2.cardsinInteger[0])
                    {
                        player1.result = CardResult.DRAW;
                        player2.result = CardResult.DRAW;
                        Winnerlist.Add(player1);
                        Winnerlist.Add(player2);
                    }
                    else if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                    {
                        player1.result = CardResult.WIN;
                        player2.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player1);
                    }
                    else if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                    {
                        player2.result = CardResult.WIN;
                        player1.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player2);
                    }
                    else
                    {
                        Logger.LogMessage("error at common compare");
                    }
                }
                else if (player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                else if (player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                else
                {
                    Logger.LogMessage("error at common compare");

                }
            }
            else if (player1.cardsinInteger[2] > player2.cardsinInteger[2])
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
            }
            else if (player1.cardsinInteger[2] < player2.cardsinInteger[2])
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
            }
        }

        private static bool CompareDouble(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.Double && player2.strength == CardStrength.Double)
            {
                if (player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    int player1remainingcard, player2remainingcard;
                    if (player1.cardsinInteger[0] == player1.cardsinInteger[1]) { player1remainingcard = player1.cardsinInteger[2]; }
                    else { player1remainingcard = player1.cardsinInteger[0]; }
                    if (player2.cardsinInteger[0] == player2.cardsinInteger[1]) { player2remainingcard = player2.cardsinInteger[2]; }
                    else { player2remainingcard = player2.cardsinInteger[0]; }

                    if (player1remainingcard == player2.cardsinInteger[1])
                    {
                        player1.result = CardResult.DRAW;
                        player2.result = CardResult.DRAW;
                        Winnerlist.Add(player1);
                        Winnerlist.Add(player2);
                    }
                    if (player1remainingcard > player2remainingcard)
                    {
                        player1.result = CardResult.WIN;
                        player2.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player1);
                    }
                    if (player1remainingcard < player2remainingcard)
                    {
                        player2.result = CardResult.WIN;
                        player1.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player2);
                    }
                }
                if (player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.Double)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;
            }
            else if (player2.strength == CardStrength.Double)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareColor(Player.Player player1, Player.Player player2)
        {
            player1.cardsinInteger.Sort();
            player2.cardsinInteger.Sort();
            if (player1.strength == CardStrength.Color && player2.strength == CardStrength.Color)
            {
                if (player1.cardsinInteger[2] == player2.cardsinInteger[2] && player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    if (player1.cardsinInteger[0] == player2.cardsinInteger[0])
                    {
                        player1.result = CardResult.DRAW;
                        player2.result = CardResult.DRAW;
                        Winnerlist.Add(player1);
                        Winnerlist.Add(player2);
                    }
                    if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                    {
                        player1.result = CardResult.WIN;
                        player2.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player1);
                    }
                    if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                    {
                        player2.result = CardResult.WIN;
                        player1.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player2);
                    }
                    return true;
                }
                if (player1.cardsinInteger[2] == player2.cardsinInteger[2])
                {
                    if (player1.cardsinInteger[1] > player2.cardsinInteger[1])
                    {
                        player1.result = CardResult.WIN;
                        player2.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player1);
                    }

                    if (player1.cardsinInteger[1] < player2.cardsinInteger[1])
                    {
                        player2.result = CardResult.WIN;
                        player1.result = CardResult.LOSE;
                        Winnerlist.Clear();
                        Winnerlist.Add(player2);
                    }
                    return true;
                }
                
                if (player1.cardsinInteger[2] > player2.cardsinInteger[2])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                else if (player1.cardsinInteger[2] < player2.cardsinInteger[2])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.Color)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.Color)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareSequence(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.Sequence && player2.strength == CardStrength.Sequence)
            {
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    player1.result = CardResult.DRAW;
                    player2.result = CardResult.DRAW;
                    Winnerlist.Add(player1);
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.Sequence)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.Sequence)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareColorSequence(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.ColorSequence && player2.strength == CardStrength.ColorSequence)
            {
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] == player2.cardsinInteger[1])
                {
                    player1.result = CardResult.DRAW;
                    player2.result = CardResult.DRAW;
                    Winnerlist.Add(player1);
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] > player2.cardsinInteger[1])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0] && player1.cardsinInteger[1] < player2.cardsinInteger[1])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.ColorSequence)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.ColorSequence)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool CompareTrial(Player.Player player1, Player.Player player2)
        {
            if (player1.strength == CardStrength.Trial && player2.strength == CardStrength.Trial)
            {
                if (player1.cardsinInteger[0] == player2.cardsinInteger[0])
                {
                    player1.result = CardResult.DRAW;
                    player2.result = CardResult.DRAW;
                    Winnerlist.Add(player1);
                    Winnerlist.Add(player2);
                }
                if (player1.cardsinInteger[0] > player2.cardsinInteger[0])
                {
                    player1.result = CardResult.WIN;
                    player2.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player1);
                }
                if (player1.cardsinInteger[0] < player2.cardsinInteger[0])
                {
                    player2.result = CardResult.WIN;
                    player1.result = CardResult.LOSE;
                    Winnerlist.Clear();
                    Winnerlist.Add(player2);
                }
                return true;
            }
            else if (player1.strength == CardStrength.Trial)
            {
                player1.result = CardResult.WIN;
                player2.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player1);
                return true;

            }
            else if (player2.strength == CardStrength.Trial)
            {
                player2.result = CardResult.WIN;
                player1.result = CardResult.LOSE;
                Winnerlist.Clear();
                Winnerlist.Add(player2);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
