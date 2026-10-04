using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills
{
    /// <summary>스킬 실행 중 효과들에 전달되는 정보.</summary>
    public class SkillContext
    {
        public GameState State { get; }
        public Unit Caster { get; }
        public SkillDefinition Skill { get; }

        /// <summary>플레이어가 지정한 칸.</summary>
        public Position Target { get; }

        /// <summary>범위 계산 결과(효과가 적용되는 칸). 시전 시점에 한 번 계산한다.</summary>
        public IReadOnlyList<Position> AffectedCells { get; }

        public SkillContext(GameState state, Unit caster, SkillDefinition skill, Position target, IReadOnlyList<Position> affectedCells)
        {
            State = state;
            Caster = caster;
            Skill = skill;
            Target = target;
            AffectedCells = affectedCells;
        }
    }
}
