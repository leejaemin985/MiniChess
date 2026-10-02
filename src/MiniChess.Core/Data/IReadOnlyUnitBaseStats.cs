namespace MiniChess.Core.Data
{
    /// <summary>유닛 기본 능력치의 읽기 전용 뷰. 게임 로직은 이 인터페이스로만 참조한다.</summary>
    public interface IReadOnlyUnitBaseStats
    {
        string Id { get; }
        string Name { get; }
        int MaxHp { get; }

        /// <summary>기본 공격 피해량.</summary>
        int Attack { get; }

        /// <summary>기본 공격 사거리(칸).</summary>
        int AttackRange { get; }
    }
}
