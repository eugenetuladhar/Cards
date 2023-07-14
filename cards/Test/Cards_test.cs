using cards.Cards_files;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace cards.Test
{
    [TestClass]
    public class Cards_test
    {
        [TestMethod]
        public void Shuffle_test()
        {
            var c = new CardCompleteDeck();
            var d = new CardCompleteDeck();
            d.Shuffle();
            bool test = true;

            for (int i = 0; i < 52; i++)
            {
                Console.WriteLine(i);
                Console.WriteLine(d.TotalDeck[i].GetCardType());
                Console.WriteLine(d.TotalDeck[i].GetCardValue());
                if (!d.HaveCard(c.TotalDeck[i]))
                {
                    test = false;
                }
            }
            Assert.IsTrue(test);
            Assert.IsTrue(d.TotalDeck.Count()== c.TotalDeck.Count());

        }
        [TestMethod]
        public void Removeat_test()
        {
            var c = new CardCompleteDeck();
            Card x = new Card(c.TotalDeck[51].GetCardType(), c.TotalDeck[51].GetCardValue());
            Console.WriteLine("Top card is : ");
            Console.WriteLine(x.GetCardType());
            Console.WriteLine(x.GetCardValue());
            c.RemoveAt(51);
            Assert.IsTrue(c.TotalDeck.Count()==51);
            Assert.IsFalse(c.HaveCard(x));

            c.AddCardOneDeck(x);
            Card y = new Card(c.TotalDeck[51].GetCardType(), c.TotalDeck[51].GetCardValue());
            Console.WriteLine("Top card is : ");
            Console.WriteLine(y.GetCardType());
            Console.WriteLine(y.GetCardValue());
            Assert.IsTrue(c.TotalDeck.Count() == 52);
            Assert.IsTrue(c.HaveCard(x));
        }
    }
}
