using System;
using MiniChess.Core.State;

namespace MiniChess.Core.Movement
{
    /// <summary>
    /// 유닛 상태에 따른 이동 가능 여부 판정. 경로/칸 조건은 Board 가, AP 는 각 행동이 따로 검사한다.
    ///
    /// 자발적 이동(이동하는 유닛 = 이동을 일으킨 유닛)에만 행동 잠금을 적용한다.
    /// 외부 이동(넉백/당기기/다른 유닛의 워프)은 행동 잠금의 예외이며, 이미 사용한 전투 행동을 회복시키지 않는다.
    /// </summary>
    public static class MovementControl
    {
        public static bool IsVoluntary(Unit mover, Unit initiator)
        {
            return ReferenceEquals(mover, initiator);
        }

        /// <param name="mover">이동하는 유닛.</param>
        /// <param name="initiator">이동을 일으킨 유닛(자신이면 자발적 이동).</param>
        /// <param name="kind">이동 방식.</param>
        public static MoveBlockReason Check(GameState state, Unit mover, Unit initiator, MoveKind kind)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (mover == null) throw new ArgumentNullException(nameof(mover));

            if (!IsVoluntary(mover, initiator))
                return MoveBlockReason.None;

            UnitTurnState turn = mover.TurnState;
            if (turn.ActionsEnded) return MoveBlockReason.ActionsEnded;
            if (turn.VoluntaryMoveLocked) return MoveBlockReason.VoluntaryMoveLocked;

            return MoveBlockReason.None;
        }
    }
}
