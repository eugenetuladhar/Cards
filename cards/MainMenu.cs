﻿using cards.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards
{
    public class MainMenu
    {
        public static void ExecuteCode()
        {
            bool mainmenu = true;
            string value = "";
            string playagain = "n";
            while (mainmenu)
            {
                if (playagain != "y")
                {
                    Console.WriteLine(" Which game to play ?");
                    Console.WriteLine(" 1 . Flash");
                    Console.WriteLine(" 2 . Kitty (Under Construction)");
                    Console.WriteLine(" 0 . Exit");
                    value = Console.ReadLine();
                }
                switch (value)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("** Flash Game **");
                        FlashLogic.RunFlash();
                        Console.WriteLine("** PLAY AGAIN? (y/n) **");
                        playagain = Console.ReadLine();
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        Console.WriteLine("** Kitty Game **");
                        KittyLogic.RunKitty();
                        Console.WriteLine("** PLAY AGAIN? (y/n) **");
                        playagain = Console.ReadLine();
                        Console.Clear();
                        break;
                    case "0":
                        Console.WriteLine("** Exit **");
                        mainmenu = false;
                        break;
                    default:
                        Console.Clear();
                        Console.WriteLine("** Invalid value **");
                        break;
                }
            }
        }
    }
}
