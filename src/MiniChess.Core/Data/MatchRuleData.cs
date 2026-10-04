using MiniChess.Core.Common;

namespace MiniChess.Core.Data
{
    /// <summary>전멸 판정에서 소환물(분신 등)을 어떻게 다룰지.</summary>
    public enum SummonEliminationPolicy
    {
        /// <summary>
        /// 미정. 소환물만 남는 상황이 실제로 생기면 승패를 자동 판정하지 않고 GameConfigException 을 낸다(명세 14).
        /// </summary>
        Undecided,

        /// <summary>소환물은 전멸 판정에서 제외한다(소환물만 남아도 전멸).</summary>
        ExcludeSummons,

        /// <summary>소환물도 살아 있는 유닛으로 센다.</summary>
        IncludeSummons,
    }

    /// <summary>경기 구성 규칙.</summary>
    public class MatchRuleData
    {
        /// <summary>팀당 유닛 수.</summary>
        public int UnitsPerTeam { get; set; }

        /// <summary>선공 팀.</summary>
        public Team FirstTeam { get; set; }

        /// <summary>
        /// [TBD] 소환물의 전멸 판정 포함 여부(명세 3.1, 14). 확정 전까지 Undecided.
        /// </summary>
        public SummonEliminationPolicy SummonElimination { get; set; } = SummonEliminationPolicy.Undecided;
    }
}
