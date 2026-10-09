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

        /// <summary>플레이어가 지정한 칸 전부(지정 순).</summary>
        public IReadOnlyList<Position> Targets { get; }

        /// <summary>첫 번째 지정 칸. 범위의 기준이 된다.</summary>
        public Position Target => Targets[0];

        /// <summary>범위 계산 결과(효과가 적용되는 칸). 시전 시점에 한 번 계산한다.</summary>
        public IReadOnlyList<Position> AffectedCells { get; }

        public SkillContext(GameState state, Unit caster, SkillDefinition skill, IReadOnlyList<Position> targets, IReadOnlyList<Position> affectedCells)
        {
            State = state;
            Caster = caster;
            Skill = skill;
            Targets = targets;
            AffectedCells = affectedCells;
        }
    }
}
