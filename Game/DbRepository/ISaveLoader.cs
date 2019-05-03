using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public interface ISaveLoader
    {
        Player Save(Player player);
        Player Load(string PlayerName, string PlayerPassword);
        bool IsPlayerExist(string playerName);
        void Remove(Player player);
    }
}
