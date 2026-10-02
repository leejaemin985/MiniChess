using System;
using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.Effects;
using MiniChess.Core.State;

namespace MiniChess.Core.Movement
{
    /// <summary>
    /// 경로를 한 칸씩 진행하며 칸 효과를 발동시킨다. 걷기/넉백/당기기/워프 공통.
    /// 누가 이동을 시켰는지, AP 가 얼마인지는 알지 못한다.
    /// </summary>
    public static class MovementResolver
    {
        /// <summary>
        /// path 를 따라 유닛을 이동시킨다(현재 위치 제외, 도착 칸 포함).
        /// 워프는 도착 칸 하나만 담아 넘긴다.
        ///
        /// 다음 경우 그 자리에서 멈춘다.
        ///   - 다음 칸에 들어갈 수 없음(벽/유닛/범위 밖) → 들어가기 전 칸에서 멈춤
        ///   - 칸 효과가 Stop 을 반환 → 그 칸에서 멈춤
        ///   - 칸 효과로 유닛이 사망 → 그 칸에서 멈춤
        /// </summary>
        public static MovementResult Resolve(Board board, Unit unit, IReadOnlyList<Position> path)
        {
            if (board == null) throw new ArgumentNullException(nameof(board));
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            if (path == null) throw new ArgumentNullException(nameof(path));
            if (!unit.IsPlaced) throw new InvalidOperationException($"유닛 {unit.Id} 는 보드 위에 없음");

            Position from = unit.Position.Value;
            int cellsMoved = 0;
            bool stopRequested = false;

            foreach (Position next in path)
            {
                if (!board.CanPlace(next))
                    break;

                board.Relocate(unit, next);
                cellsMoved++;

                stopRequested = TriggerEnter(board, unit);
                if (stopRequested || !unit.IsAlive)
                    break;
            }

            if (cellsMoved > 0 && unit.IsAlive)
                TriggerStop(board, unit);

            bool wasInterrupted = cellsMoved < path.Count;
            return new MovementResult(unit, from, unit.Position ?? from, cellsMoved, wasInterrupted);
        }

        /// <summary>현재 칸의 모든 효과에 OnEnter 를 호출한다. 하나라도 Stop 이면 true.</summary>
        private static bool TriggerEnter(Board board, Unit unit)
        {
            CellEffectContext context = CreateContext(board, unit);
            bool stopRequested = false;

            // 발동 중 효과가 제거/교체될 수 있으므로 목록을 먼저 복사한다.
            foreach (ICellEffect effect in context.Cell.GetEffectsInOrder().ToList())
            {
                // 한 효과가 Stop 을 요청해도, 이미 들어온 칸의 나머지 효과는 모두 발동한다.
                if (effect.OnEnter(context) == CellEnterResult.Stop)
                    stopRequested = true;

                if (!unit.IsAlive)
                    break;
            }

            return stopRequested;
        }

        private static void TriggerStop(Board board, Unit unit)
        {
            CellEffectContext context = CreateContext(board, unit);

            foreach (ICellEffect effect in context.Cell.GetEffectsInOrder().ToList())
            {
                effect.OnStop(context);

                if (!unit.IsAlive)
                    break;
            }
        }

        private static CellEffectContext CreateContext(Board board, Unit unit)
        {
            BoardCell cell = board.GetCell(unit.Position.Value);
            return new CellEffectContext(board, cell, unit);
        }
    }
}
