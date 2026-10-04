using MiniChess.Core.Common;
using MiniChess.Core.State;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Statuses
{
    /// <summary>유닛에 걸린 상태효과 한 개(인스턴스).</summary>
    public class StatusEffect
    {
        public StatusDefinition Definition { get; }
        public Unit Target { get; }

        /// <summary>건 유닛. 맵 효과 등 유닛이 아닌 출처면 null.</summary>
        public Unit Source { get; private set; }

        /// <summary>건 쪽의 팀. Source 가 없어도 시점 판정(SourceOwner)에 사용한다.</summary>
        public Team SourceTeam { get; private set; }

        public int Remaining { get; internal set; }

        /// <summary>부여(또는 갱신)된 경기 턴 번호.</summary>
        public int AppliedTurnNumber { get; private set; }

        internal StatusEffect(StatusDefinition definition, Unit target, Unit source, Team sourceTeam, int turnNumber)
        {
            Definition = definition;
            Target = target;
            Reapply(source, sourceTeam, turnNumber);
        }

        internal void Reapply(Unit source, Team sourceTeam, int turnNumber)
        {
            Source = source;
            SourceTeam = sourceTeam;
            Remaining = Definition.Duration;
            AppliedTurnNumber = turnNumber;
        }

        /// <summary>지금 턴 단계가 timing 에 해당하는지.</summary>
        internal bool Matches(StatusTiming timing, TurnContext context)
        {
            if (timing == null || timing.Step != context.Step)
                return false;

            Team ownerTeam = timing.Owner == TimingOwner.TargetOwner ? Target.Team : SourceTeam;
            if (ownerTeam != context.ActiveTeam)
                return false;

            return timing.IncludeApplicationTurn || context.State.TurnNumber != AppliedTurnNumber;
        }
    }
}
