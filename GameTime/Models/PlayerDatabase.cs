using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameTime.Models
{
    public class PlayerDatabase : IDisposable
    {
        private LiteDatabase database;
        private ILiteCollection<Player> colPlayers;
        private List<Player> players;

        public string Path { get; set; }
        public List<Player> Players => players;

        public PlayerDatabase(string path)
        {
            Path = path;
            database = new LiteDatabase(Path);
            colPlayers = database.GetCollection<Player>("players");
            players = colPlayers.FindAll().ToList();
        }

        public void Dispose()
        {
            database.Dispose();
        }

        public void AddPlayer(Player player)
        {
            colPlayers.Insert(player);
            colPlayers.EnsureIndex(x => x.ID);
            players = colPlayers.FindAll().ToList();
        }

        public void UpdatePlayer(Player player)
        {
            colPlayers.Update(player);
            colPlayers.EnsureIndex(x => x.ID);
            players = colPlayers.FindAll().ToList();
        }

        public void DeletePlayer(long id)
        {
            colPlayers.Delete(id);
            colPlayers.EnsureIndex(x => x.ID);
            players = colPlayers.FindAll().ToList();
        }

        public Player GetPlayerByID(long id)
        {
            return colPlayers.FindById(id);
        }
    }
}