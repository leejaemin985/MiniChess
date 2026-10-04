using System;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Statuses
{
    /// <summary>시점의 기준이 되는 소유자.</summary>
    public enum TimingOwner
    {
        /// <summary>효과를 받은 유닛의 소유자 턴.</summary>
        TargetOwner,

        /// <summary>효과를 건 쪽의 소유자 턴.</summary>
        SourceOwner,
    }

    /// <summary>
    /// "누구의 턴, 어느 단계"에 반응하는지. 발동 시점과 지속시간 감소 시점을 각각 이 값으로 지정한다(명세 13.4).
    /// </summary>
    public class StatusTiming
    {
        public TurnStep Step { get; }
        public TimingOwner Owner { get; }

        /// <summary>
        /// 부여된 바로 그 턴에 같은 시점이 아직 남아 있을 때 반응할지.
        /// false 면 부여된 턴의 해당 시점은 건너뛴다(예: 내 턴에 건 효과가 내 턴 종료에 바로 1 소모되지 않게).
        /// </summary>
        public bool IncludeApplicationTurn { get; }

        public StatusTiming(TurnStep step, TimingOwner owner, bool includeApplicationTurn)
        {
            if (step == TurnStep.StartApRecovery)
                throw new ArgumentException("StartApRecovery 는 시스템 전용 단계라 효과 시점으로 쓸 수 없음", nameof(step));

            Step = step;
            Owner = owner;
            IncludeApplicationTurn = includeApplicationTurn;
        }
    }
}
