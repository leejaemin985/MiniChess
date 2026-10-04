namespace MiniChess.Core.Data
{
    /// <summary>팀 구성용 주 역할 분류. 스킬의 다른 기능을 금지하는 규칙이 아니다.</summary>
    public enum CharacterRole
    {
        /// <summary>전투형.</summary>
        Combat,

        /// <summary>수호형.</summary>
        Guardian,

        /// <summary>제어형.</summary>
        Control,
    }
}
