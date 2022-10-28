using System.Collections.Generic;
using GameTime.MultiplayerSessionModels;

namespace GameTime.Databases
{
    public class MutliplayerGameSessions
    {
        public Dictionary<string, GameSession> PublicSessions { get; private set; } = new Dictionary<string, GameSession>();
        public Dictionary<string, GameSession> PrivateSessions { get; private set; } = new Dictionary<string, GameSession>();
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
        public GameSession SearchOpenPublicSession()
        {
            foreach(var session in PublicSessions)
            {
                if(session.Value.GameStatus == Status.Open)
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
    }
}
