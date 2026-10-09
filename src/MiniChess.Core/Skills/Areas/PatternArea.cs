using System;
using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Areas
{
    /// <summary>범위가 퍼지는 기준점.</summary>
    public enum AreaAnchor
    {
        /// <summary>플레이어가 선택한 칸 기준.</summary>
        Target,

        /// <summary>시전자 위치 기준.</summary>
        Caster,
    }

    /// <summary>
    /// 모양(AreaShape)과 기준점으로 정하는 범위. 보드 밖 칸은 제외하고, 벽 칸은 포함한다(효과가 각자 판단).
    /// Rotate 면 시전자 → 선택 칸 방향(상하좌우)으로 모양을 회전한다. 시전자 기준에서만 쓴다.
    /// </summary>
    public class PatternArea : SkillArea
    {
        /// <summary>선택한 칸 하나.</summary>
        public static readonly PatternArea SingleCell = new PatternArea(AreaShape.SingleCell, AreaAnchor.Target);

        /// <summary>[TBD 가능]</summary>
        public AreaShape Shape { get; }

        public AreaAnchor Anchor { get; }
        public bool Rotate { get; }

        public PatternArea(AreaShape shape, AreaAnchor anchor, bool rotate = false)
        {
            if (rotate && anchor != AreaAnchor.Caster)
                throw new ArgumentException("회전은 시전자 기준 범위에서만 쓸 수 있음", nameof(rotate));

            Shape = shape;
            Anchor = anchor;
            Rotate = rotate;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Shape, path, nameof(Shape), issues);
        }

        public override IReadOnlyList<Position> GetCells(GameState state, Unit caster, Position target)
        {
            var cells = new List<Position>();
            Position origin = Anchor == AreaAnchor.Caster ? caster.Position.Value : target;

            int facingX = 0, facingY = 1;
            if (Rotate && !TryGetFacing(origin, target, out facingX, out facingY))
                return cells;

            foreach ((int dx, int dy) in Shape.GetOffsets(facingX, facingY))
            {
                var position = new Position(origin.X + dx, origin.Y + dy);
                if (state.Board.IsInBounds(position))
                    cells.Add(position);
            }

            return cells;
        }

        /// <summary>from → to 가 상하좌우 직선이면 그 방향의 단위 벡터.</summary>
        private static bool TryGetFacing(Position from, Position to, out int x, out int y)
        {
            x = Math.Sign(to.X - from.X);
            y = Math.Sign(to.Y - from.Y);
            return (x == 0) != (y == 0);
        }
    }
}
