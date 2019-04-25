using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DbRepository
{
    public class Player
    {
        public int PlayerId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Score { get; set; }
    }
}
