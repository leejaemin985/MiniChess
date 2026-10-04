using MiniChess.Core.Capture;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Events
{
    /// <summary>팀의 점령 진행도가 바뀌었다(증가 또는 초기화).</summary>
    public class CaptureProgressChangedEvent : GameEvent
    {
        public Team Team { get; }
        public int Progress { get; }
        public int Required { get; }

        public CaptureProgressChangedEvent(Team team, int progress, int required)
        {
            Team = team;
            Progress = progress;
            Required = required;
        }
    }

    /// <summary>점령이 완료되었다. 점령 칸은 소멸하고 이 경기에서 더 이상 점령은 없다.</summary>
    public class CaptureCompletedEvent : GameEvent
    {
        public Team Team { get; }

        /// <summary>지급된 보상. 보상 풀이 비어 있으면 null.</summary>
        public CaptureReward Reward { get; }

        public CaptureCompletedEvent(Team team, CaptureReward reward)
        {
            Team = team;
            Reward = reward;
        }
    }

    /// <summary>유닛의 보호막 양이 바뀌었다(획득 또는 피해 흡수).</summary>
    public class UnitShieldChangedEvent : GameEvent
    {
        public Unit Unit { get; }
        public int ShieldAfter { get; }

        public UnitShieldChangedEvent(Unit unit, int shieldAfter)
        {
            Unit = unit;
            ShieldAfter = shieldAfter;
        }
    }
}
