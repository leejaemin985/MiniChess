using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Targeting
{
    /// <summary>
    /// 방향 하나를 지정한다. 지정 칸은 시전자의 상하좌우 인접 칸(보드 안, 벽 포함)이며 방향만 의미가 있다.
    /// 회전하는 시전자 기준 범위(PatternArea.Rotate)와 함께 쓴다.
    /// </summary>
    public class DirectionTargeting : SkillTargeting
    {
        private static readonly (int X, int Y)[] Directions = { (0, 1), (1, 0), (0, -1), (-1, 0) };

        public override IEnumerable<Position> GetCandidates(GameState state, Unit caster)
        {
            if (!caster.IsPlaced)
                yield break;

            Position center = caster.Position.Value;
            foreach ((int x, int y) in Directions)
            {
                var position = new Position(center.X + x, center.Y + y);
                if (state.Board.IsInBounds(position))
                    yield return position;
            }
        }
    }
}
