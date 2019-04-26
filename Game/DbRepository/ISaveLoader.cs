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
