using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
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
        public Player GetPlayerByName(string name)
        {
            foreach(Player player in players)
            {
                if (player.Name == name)
                    return player;
            }
            return null;
        }
        public DiscordEmbedBuilder NewPlayer(DiscordUser user, DiscordEmbedBuilder embed)
        {
            if(user.Id != 277275957147074560)
            {
                embed.Title = "New User Detected";
                embed.Description = "Here is a flare to start you off. Use g/inventory again to view your inventory. Use g/help to view all the commands. ";
                Bot.PlayerDatabase.AddPlayer(new Player() { ID = Convert.ToInt64(user.Id), Name = user.Username, Image = user.AvatarUrl });
            }
            else
            {
                embed.Title = "Greetings Goldminer127";
                embed.Description = "Admin permissions granted.";
                Bot.PlayerDatabase.AddPlayer(new Player() { ID = Convert.ToInt64(user.Id), Name = user.Username, Image = user.AvatarUrl, AuthroizedAdmin = true, AuthroizedMod = true });
            }
            return embed;
        }
    }
}