using System;
using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.Events;
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

        /// <summary>이 이동으로 발생한 이벤트(발생 순). 이동을 실행한 행동이 채운다.</summary>
        public IReadOnlyList<GameEvent> Events { get; internal set; } = Array.Empty<GameEvent>();

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
