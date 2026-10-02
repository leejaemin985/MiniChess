using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Movement
{
    public class MovementResult
    {
        public Unit Unit { get; }
        public Position From { get; }

        /// <summary>실제로 이동을 마친 칸.</summary>
        public Position StoppedAt { get; }

        /// <summary>실제로 이동한 칸 수.</summary>
        public int CellsMoved { get; }

        /// <summary>요청한 경로 끝까지 가지 못하고 도중에 멈췄는지.</summary>
        public bool WasInterrupted { get; }

        public MovementResult(Unit unit, Position from, Position stoppedAt, int cellsMoved, bool wasInterrupted)
        {
            Unit = unit;
            From = from;
            StoppedAt = stoppedAt;
            CellsMoved = cellsMoved;
            WasInterrupted = wasInterrupted;
        }
    }
}
