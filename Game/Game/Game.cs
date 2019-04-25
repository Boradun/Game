using DbRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Game
    {
        public Player Player1 { get; set; }
        public Player Player2 { get; set; }
        public int MinNumber { get; set; }
        public int MaxNumber { get; set; }
        public int SecretNumber { get; set; }

    }
}
