using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Areas
{
    /// <summary>지정한 칸 하나.</summary>
    public class SingleCellArea : SkillArea
    {
        public override IReadOnlyList<Position> GetCells(GameState state, Unit caster, Position target)
        {
            return new[] { target };
        }
    }
}
