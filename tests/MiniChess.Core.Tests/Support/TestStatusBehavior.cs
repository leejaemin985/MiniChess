using MiniChess.Core.Movement;
using MiniChess.Core.State;
using MiniChess.Core.Statuses;

namespace MiniChess.Core.Tests.Support
{
    /// <summary>훅 동작을 람다로 지정하는 테스트용 상태 동작.</summary>
    public class TestStatusBehavior : StatusBehavior
    {
        public Action<StatusContext> Applied { get; init; }
        public Action<StatusContext> Removed { get; init; }
        public Action<StatusContext> Triggered { get; init; }
        public Func<StatusContext, Unit, MoveKind, int, MoveBlockReason> MoveCheck { get; init; }

        public int TriggerCount { get; private set; }
        public int RemovedCount { get; private set; }

        public override void OnApplied(StatusContext context) => Applied?.Invoke(context);

        public override void OnRemoved(StatusContext context)
        {
            RemovedCount++;
            Removed?.Invoke(context);
        }

        public override void OnTrigger(StatusContext context)
        {
            TriggerCount++;
            Triggered?.Invoke(context);
        }

        public override MoveBlockReason CheckMove(StatusContext context, Unit initiator, MoveKind kind, int cells)
        {
            return MoveCheck?.Invoke(context, initiator, kind, cells) ?? MoveBlockReason.None;
        }
    }
}
