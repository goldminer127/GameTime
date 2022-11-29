﻿using System;
using System.Collections.Generic;
using System.Threading;
using GameTime.MultiplayerSessionModels;

namespace GameTime.Databases
{
    public class MutliplayerGameSessions
    {
        public Dictionary<string, GameSession> PublicSessions { get; private set; } = new Dictionary<string, GameSession>();
        public Dictionary<string, GameSession> PrivateSessions { get; private set; } = new Dictionary<string, GameSession>();
        public List<ulong> PlayersInGame { get; private set; } = new List<ulong>();
        public MutliplayerGameSessions()
        {

        }
        public MutliplayerGameSessions AddSession(GameSession session)
        {
            if(session.IsPrivate)
            {
                PrivateSessions.Add(session.SessionId, session);
            }
            else
            {
                PublicSessions.Add(session.SessionId, session);
            }
            return this;
        }
        public MutliplayerGameSessions RemoveSession(GameSession session)
        {
            if (session.IsPrivate)
            {
                PrivateSessions.Remove(session.SessionId);
            }
            else
            {
                PublicSessions.Remove(session.SessionId);
            }
            return this;
        }
        public MutliplayerGameSessions UpdateSession(GameSession session)
        {
            if(session.IsPrivate)
            {
                PrivateSessions[session.SessionId] = session;
            }
            else
            {
                PublicSessions[session.SessionId] = session;
            }
            return this;
        }
        public GameSession SearchOpenPublicSession(string gameType)
        {
            foreach(var session in PublicSessions)
            {
                if(session.Value.GameStatus == Status.Open && session.Value.SessionId.Contains(gameType))
                {
                    return session.Value;
                }
            }
            return null;
        }
        public GameSession SearchOpenPrivateSession(string id)
        {
            if (PrivateSessions.TryGetValue(id, out var session))
            {
                if(session.GameStatus == Status.Open)
                {
                    return session;
                }
            }
            return session;
        }
        public GameSession GetSession(string id)
        {
            if(!PublicSessions.TryGetValue(id, out GameSession session))
            {
                PrivateSessions.TryGetValue(id, out session);
            }
            return session;
        }
        public void RemoveSession(string id)
        {
            if(!PublicSessions.Remove(id))
                PrivateSessions.Remove(id);
        }
        public bool IsPublicEmpty()
        {
            foreach(var sessions in PublicSessions)
            {
                if(sessions.Value.GameStatus == Status.Open)
                {
                    return false;
                }
            }
            return true;
        }
        public void AddPlayerInSession(ulong id)
        {
            PlayersInGame.Add(id);
        }
        public bool PlayerInSession(ulong id)
        {
            return PlayersInGame.Contains(id);
        }
        public string GenerateGameId(string gameType)
        {
            var id = 0;
            for (int i = 0; i < 5; i++)
            {
                id = (id * 10) + new Random().Next(0, 10);
            }
            return gameType + id;
        }
    }
}
