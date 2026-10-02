namespace MiniChess.Core.Actions
{
    public enum MoveFailReason
    {
        None,
        NotBattlePhase,
        NotYourTurn,
        UnitNotOnBoard,
        AlreadyActed,

        /// <summary>상하좌우 직선이 아니거나 제자리.</summary>
        NotStraightLine,

        /// <summary>경로에 벽/유닛/범위 밖 칸이 있음.</summary>
        PathBlocked,

        NotEnoughAp,
    }
}
