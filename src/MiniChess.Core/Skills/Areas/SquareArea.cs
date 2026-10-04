using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Areas
{
    /// <summary>지정한 칸을 중심으로 한 정사각형(반경 1 = 3×3). 벽 칸도 포함한다(효과가 각자 판단).</summary>
    public class SquareArea : SkillArea
    {
        /// <summary>중심에서의 반경. [TBD 가능]</summary>
        public int? Radius { get; }

        public SquareArea(int? radius)
        {
            Radius = radius;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Radius, path, nameof(Radius), issues);
        }

        public override IReadOnlyList<Position> GetCells(GameState state, Unit caster, Position target)
        {
            int radius = Radius.Value;
            var cells = new List<Position>();

            for (int y = target.Y - radius; y <= target.Y + radius; y++)
            {
                for (int x = target.X - radius; x <= target.X + radius; x++)
                {
                    var position = new Position(x, y);
                    if (state.Board.IsInBounds(position))
                        cells.Add(position);
                }
            }

            return cells;
        }
    }
}
