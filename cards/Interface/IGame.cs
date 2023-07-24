using cards.Cards_files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cards.Interface
{
    public interface IGame
    {
        CardGameType GameName { get; set; }
        int MAX_NUM_PLAYERS { get; set; }
        int NUM_CARDS_TO_DEAL { get; set; }
        void Run();
    }
}
