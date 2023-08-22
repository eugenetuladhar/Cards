using cards.Game;
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
            while (mainmenu)
            {
                Console.WriteLine();
                Console.WriteLine(" CHOOSE GAME TO PLAY ?");
                Console.WriteLine();
                Console.WriteLine("    1 . FLASH");
                Console.WriteLine("    2 . KITTY");
                Console.WriteLine("    3 . DHUMBAL (Beta Version)");
                Console.WriteLine("    4 . CALLBREAK (Postponed)");
                Console.WriteLine("    0 . EXIT");
                value = Console.ReadLine();
                switch (value)
                {
                    case "1":
                        Console.Clear();
                        FlashGame f = new FlashGame();
                        Console.WriteLine($"** Welcome to {f.GameName} Game **");
                        f.Run();
                        Console.Clear();
                        break;
                    case "2":
                        Console.Clear();
                        KittyGame k = new KittyGame();
                        Console.WriteLine($"** Welcome to {k.GameName} Game **");
                        k.Run();
                        Console.Clear();
                        break;
                    case "3":
                        Console.Clear();
                        DhumbalGame d = new DhumbalGame();
                        Console.WriteLine($"** Welcome to {d.GameName} Game **");
                        d.Run();
                        Console.Clear();
                        break;
                    case "4":
                        Console.Clear();
                        CallBreakGame c = new CallBreakGame();
                        Console.WriteLine($"** Welcome to {c.GameName} Game **");
                        c.Run();
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
