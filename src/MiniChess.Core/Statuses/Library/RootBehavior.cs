using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Statuses.Library
{
    /// <summary>
    /// 속박: 자발적 이동(일반 이동 및 자신을 이동시키는 스킬)을 막는다. 공격/스킬은 막지 않는다(기절 아님).
    /// 외부 강제 이동을 막을지는 별도 정책으로 지정한다(명세 12.3).
    /// </summary>
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
