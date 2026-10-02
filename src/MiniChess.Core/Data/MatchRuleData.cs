using MiniChess.Core.Common;

namespace MiniChess.Core.Data
{
    /// <summary>경기 구성 규칙.</summary>
    public class MatchRuleData
    {
        /// <summary>팀당 유닛 수.</summary>
        public int UnitsPerTeam { get; set; }

        /// <summary>선공 팀.</summary>
        public Team FirstTeam { get; set; }
    }
}
