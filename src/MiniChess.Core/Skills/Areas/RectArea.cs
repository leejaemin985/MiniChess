using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Areas
{
    /// <summary>
    /// 지정한 칸을 왼쪽 아래 모서리로 하는 Width×Height 직사각형(예: 2×2). 보드 밖 칸은 제외한다.
    /// [가정] 기준 칸을 왼쪽 아래로 둔다. 모양/기준점이 확정되면 바꾼다.
    /// </summary>
    public class RectArea : SkillArea
    {
        public int? Width { get; }
        public int? Height { get; }

        public RectArea(int? width, int? height)
        {
            Width = width;
            Height = height;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Width, path, nameof(Width), issues);
            Require(Height, path, nameof(Height), issues);
        }

        public override IReadOnlyList<Position> GetCells(GameState state, Unit caster, Position target)
        {
            var cells = new List<Position>();

            for (int y = target.Y; y < target.Y + Height.Value; y++)
            {
                for (int x = target.X; x < target.X + Width.Value; x++)
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
