namespace GameTime.MultiplayerSessionModels.Enums
{
    public enum MoveStatus
    {
        Success,
        InvalidTurn,
        InvalidArgs,
        InvalidMove,
        MoveErrored //A report will be generated if this occures
    }
}
