using System;
using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Targeting
{
    /// <summary>칸 지정 조건.</summary>
    public enum CellRequirement
    {
        /// <summary>벽이 아닌 칸(유닛 유무 무관).</summary>
        NotWall,

        /// <summary>유닛을 놓을 수 있는 빈 칸(벽/유닛 없음).</summary>
        Empty,
    }

    /// <summary>사거리 안의 칸 하나를 지정한다(설치/소환/범위 중심 등). 시전자 칸은 제외한다.</summary>
    public class CellTargeting : SkillTargeting
    {
        /// <summary>사거리(칸). [TBD 가능]</summary>
        public int? Range { get; }

        public RangeShape Shape { get; }
        public CellRequirement Requirement { get; }

        public CellTargeting(int? range, CellRequirement requirement, RangeShape shape = RangeShape.Square)
        {
            Range = range;
            Requirement = requirement;
            Shape = shape;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Range, path, nameof(Range), issues);
        }

        public override IEnumerable<Position> GetCandidates(GameState state, Unit caster)
        {
            if (!caster.IsPlaced)
                yield break;

            foreach (Position position in RangeShapes.GetCells(state.Board, caster.Position.Value, Range.Value, Shape))
            {
                if (Satisfies(state.Board, position))
                    yield return position;
            }
        }

        private bool Satisfies(Board board, Position position)
        {
            switch (Requirement)
            {
                case CellRequirement.NotWall: return !board.GetCell(position).IsWall;
                case CellRequirement.Empty: return board.CanPlace(position);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
