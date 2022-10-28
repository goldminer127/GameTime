namespace GameTime.Connect4Models
{
    public class Player
    {
        public ulong PlayerId { get; private set; }
        public ulong ChannelId { get; private set; }
        public Player(ulong playerId, ulong channelId)
        {
            PlayerId = playerId;
            ChannelId = channelId;
        }
    }
}
