using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Statuses.Library
{
    /// <summary>속박: 자발적 이동을 막는다. BlocksExternalMoves 면 외부 강제 이동도 막는다.</summary>
    public class RootBehavior : StatusBehavior
    {
        public bool BlocksExternalMoves { get; }

        public RootBehavior(bool blocksExternalMoves)
        {
            BlocksExternalMoves = blocksExternalMoves;
        }

        public override MoveBlockReason CheckMove(StatusContext context, Unit initiator, MoveKind kind, int cells)
        {
            bool voluntary = MovementControl.IsVoluntary(context.Target, initiator);
            return voluntary || BlocksExternalMoves ? MoveBlockReason.Rooted : MoveBlockReason.None;
        }
    }
}
