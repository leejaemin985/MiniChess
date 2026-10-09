using System;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Statuses
{
    /// <summary>시점을 누구의 턴 기준으로 셀지.</summary>
    public enum TimingOwner
    {
        /// <summary>상태가 걸린 유닛의 팀 턴.</summary>
        TargetOwner,

        /// <summary>상태를 건 쪽의 팀 턴.</summary>
        SourceOwner,
    }

    /// <summary>"누구의 턴, 어느 단계". 상태의 발동/감소 시점을 지정한다.</summary>
    public class StatusTiming
    {
        public TurnStep Step { get; }
        public TimingOwner Owner { get; }

        /// <summary>걸린 그 턴에도 반응할지. false 면 걸린 턴의 해당 단계는 건너뛴다.</summary>
        public bool IncludeApplicationTurn { get; }

        public StatusTiming(TurnStep step, TimingOwner owner, bool includeApplicationTurn)
        {
            if (step == TurnStep.StartApRecovery || step == TurnStep.StartCapture)
                throw new ArgumentException($"{step} 는 시스템 전용 단계라 효과 시점으로 쓸 수 없음", nameof(step));

            Step = step;
            Owner = owner;
            IncludeApplicationTurn = includeApplicationTurn;
        }
    }
}
