using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Statuses
{
    /// <summary>상태효과 훅에 전달되는 정보.</summary>
    public class StatusContext
    {
        public GameState State { get; }
        public StatusEffect Status { get; }

        public Unit Target => Status.Target;

        public StatusContext(GameState state, StatusEffect status)
        {
            State = state;
            Status = status;
        }
    }

    /// <summary>
    /// 상태효과의 실제 동작. 필요한 훅만 재정의한다.
    /// 피해 감소/보호막처럼 피해 처리에 끼어드는 상태는 OnApplied/OnRemoved 에서 IDamageInterceptor 를 등록/해제한다.
    /// </summary>
    public abstract class StatusBehavior
    {
        public virtual void OnApplied(StatusContext context) { }

        public virtual void OnRemoved(StatusContext context) { }

        /// <summary>정의의 Trigger 시점에 호출된다(DoT 피해, 턴 시작 회복 등).</summary>
        public virtual void OnTrigger(StatusContext context) { }

        /// <summary>이 상태가 대상 유닛의 이동을 막는지(속박, 이동 거리 제한 등).</summary>
        /// <param name="initiator">이동을 일으킨 유닛. 대상 자신이면 자발적 이동.</param>
        /// <param name="cells">이번 이동의 칸 수(경로 길이).</param>
        public virtual MoveBlockReason CheckMove(StatusContext context, Unit initiator, MoveKind kind, int cells)
        {
            return MoveBlockReason.None;
        }
    }
}
