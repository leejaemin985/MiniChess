namespace MiniChess.Core.State
{
    /// <summary>경기의 큰 흐름. 턴은 Battle 단계 안에서만 존재한다.</summary>
    public enum GamePhase
    {
        /// <summary>유닛 배치 단계.</summary>
        Setup,

        /// <summary>턴제 전투 단계.</summary>
        Battle,

        /// <summary>경기 종료.</summary>
        Ended,
    }
}
