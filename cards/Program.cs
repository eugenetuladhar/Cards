// See https://aka.ms/new-console-template for more information
using cards.Cards_files;
using cards.Player;
using cards.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

Console.WriteLine("Test Cards");

bool mainmenu = true;
string value="";
string playagain = "N";
while (mainmenu)
{
    if(playagain!="y")
    { 
    Console.WriteLine(" Which game to play ?");
    Console.WriteLine(" 1 . Flash");
    Console.WriteLine(" 2 . Kitty");
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
            break;
        case "2":
            Console.Clear();
            Console.WriteLine("** Flash Game **");
            KittyLogic.RunKitty();
            Console.WriteLine("** PLAY AGAIN? (y/n) **");
            playagain = Console.ReadLine();
            break;
        case "0":
            Console.WriteLine("** Exit **");
            mainmenu= false;
            break;
        default:
            Console.Clear();
            Console.WriteLine("** Invalid value **");
            break;
    }
}

