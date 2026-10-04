namespace MiniChess.Core.Statuses
{
    /// <summary>같은 상태(Definition Id)가 이미 걸린 대상에게 다시 부여할 때의 처리.</summary>
    public enum StatusStackPolicy
    {
        /// <summary>기존 효과의 남은 횟수를 처음 값으로 되돌리고 부여자/부여 턴을 갱신한다.</summary>
        Refresh,

        /// <summary>기존 효과를 제거하고 새 효과로 교체한다.</summary>
        Replace,

        /// <summary>기존 효과를 유지하고 새 부여는 무시한다.</summary>
        Ignore,
    }
}
