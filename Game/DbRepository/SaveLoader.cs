using PlayerModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbRepository
{
    public static class SaveLoader
    {
        class PlayerContext : DbContext
        {
            internal PlayerContext() : base()
            { }

            public DbSet<Player> Players { get; set; }
        }



        //сохранит игрока в базу, если существует
        //создаст новую запись, если не существует
        public static Player Save(Player player)
        {
            using (PlayerContext _playerContext = new PlayerContext())
            {
                if (IsPlayerExist(player.Name))
                {
                    if (_playerContext.Players.FirstOrDefault(x => x.Name == player.Name && x.Password == player.Password) != null)
                    {
                        _playerContext.Players.FirstOrDefault(x => x.Name == player.Name && x.Password == player.Password).Score = player.Score;
                    }
                }
                else
                {
                    _playerContext.Players.Add(player);
                }
                _playerContext.SaveChanges();

                return Load(player.Name, player.Password);
            }
        }

        //возвращает игрока из базы, null если игрока не найдено
        public static Player Load(string PlayerName, string PlayerPassword)
        {
            using (PlayerContext _playerContext = new PlayerContext())
            {
                return _playerContext.Players.FirstOrDefault(x => x.Name == PlayerName && x.Password == PlayerPassword);
            }
        }

        public static bool IsPlayerExist(string playerName)
        {
            using (PlayerContext _playerContext = new PlayerContext())
            {
                var a = _playerContext.Players.Where(c => c.Name == playerName);
                if (a.Count() != 0)
                {
                    return true;
                }
                return false;
            }
        }
    }
}
