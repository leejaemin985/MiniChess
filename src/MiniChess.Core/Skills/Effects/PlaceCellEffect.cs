using System;
using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.Effects;

namespace MiniChess.Core.Skills.Effects
{
    /// <summary>
    /// 범위의 각 칸에 칸 효과(장판/덫)를 설치한다. 칸마다 새 인스턴스를 만든다.
    /// 설치 규칙(벽/점령 칸 제한, 같은 레이어 덮어쓰기)은 CellEffectSystem 이 적용하며, 설치할 수 없는 칸은 건너뛴다.
    /// </summary>
    public class PlaceCellEffect : SkillEffect
    {
        /// <summary>칸 효과 생성기(시전 정보, 칸) → 효과. 효과 수치가 TBD 면 null 로 둔다.</summary>
        public Func<SkillContext, Position, ICellEffect> Factory { get; }

        public PlaceCellEffect(Func<SkillContext, Position, ICellEffect> factory)
        {
            Factory = factory;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Factory, path, nameof(Factory), issues);
        }

        public override void Apply(SkillContext context)
        {
            foreach (Position position in context.AffectedCells)
                CellEffectSystem.Place(context.State, position, Factory(context, position));
        }
    }
}
