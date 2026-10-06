using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Statuses.Library
{
    /// <summary>
    /// 이동 거리 제한(볼라): 이번 턴 자발적 이동 칸 수의 누적이 MaxCells 를 넘는 이동을 막는다.
    /// 명령을 나누어도 누적으로 판정하므로 우회할 수 없다. 외부 강제 이동(넉백/당기기/다른 유닛의 워프)은 막지 않는다.
    /// 이동 완전 불가인 속박과는 다른 상태다(명세 8.3).
    /// </summary>
    public class DistanceLimitBehavior : StatusBehavior
    {
        /// <summary>이번 턴에 자발적으로 이동할 수 있는 최대 칸 수(누적).</summary>
        public int MaxCells { get; }

        public DistanceLimitBehavior(int maxCells)
        {
            MaxCells = maxCells;
        }

        public override MoveBlockReason CheckMove(StatusContext context, Unit initiator, MoveKind kind, int cells)
        {
            if (!MovementControl.IsVoluntary(context.Target, initiator))
                return MoveBlockReason.None;

            int movedThisTurn = context.Target.TurnState.VoluntaryCellsMoved;
            return movedThisTurn + cells > MaxCells ? MoveBlockReason.DistanceLimited : MoveBlockReason.None;
        }
    }
}
