using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills
{
    /// <summary>
    /// 플레이어가 무엇을 지정할 수 있는지. 지정 대상은 칸 하나(Position)로 표현한다.
    /// 설정 오류가 없는 상태에서만 호출된다.
    /// </summary>
    public abstract class SkillTargeting : SkillComponent
    {
        /// <summary>지금 지정할 수 있는 칸 전부.</summary>
        public abstract IEnumerable<Position> GetCandidates(GameState state, Unit caster);

        public virtual bool IsValid(GameState state, Unit caster, Position target)
        {
            return GetCandidates(state, caster).Contains(target);
        }
    }

    /// <summary>사거리 모양.</summary>
    public enum RangeShape
    {
        /// <summary>체비셰프 거리(대각선 포함 정사각형). 공격/스킬 사거리의 기본.</summary>
        Square,

        /// <summary>상하좌우 직선 위의 칸만.</summary>
        Orthogonal,
    }

    internal static class RangeShapes
    {
        /// <summary>center 기준 range 이내의 보드 위 칸(center 제외).</summary>
        public static IEnumerable<Position> GetCells(Board board, Position center, int range, RangeShape shape)
        {
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = -range; dy <= range; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;
                    if (shape == RangeShape.Orthogonal && dx != 0 && dy != 0)
                        continue;

                    var position = new Position(center.X + dx, center.Y + dy);
                    if (board.IsInBounds(position))
                        yield return position;
                }
            }
        }
    }
}
