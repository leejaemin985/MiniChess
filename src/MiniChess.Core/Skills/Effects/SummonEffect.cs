using System;
using System.Collections.Generic;
using MiniChess.Core.Data;
using MiniChess.Core.Statuses;
using MiniChess.Core.Summons;

namespace MiniChess.Core.Skills.Effects
{
    /// <summary>
    /// 지정 칸에 시전자의 소환물을 만든다. 소환물의 능력치와 기본 공격 적중 시 상태는 정의 시점에 정한다.
    /// </summary>
    public class SummonEffect : SkillEffect
    {
        /// <summary>소환물 능력치. 수치가 TBD 면 null.</summary>
        public IReadOnlyUnitBaseStats BaseStats { get; }

        /// <summary>소환물의 기본 공격이 적중하면 거는 상태.</summary>
        public IReadOnlyList<StatusDefinition> OnHitStatuses { get; }

        public SummonEffect(IReadOnlyUnitBaseStats baseStats, IReadOnlyList<StatusDefinition> onHitStatuses = null)
        {
            BaseStats = baseStats;
            OnHitStatuses = onHitStatuses ?? Array.Empty<StatusDefinition>();
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(BaseStats, path, nameof(BaseStats), issues);
            for (int i = 0; i < OnHitStatuses.Count; i++)
                Require(OnHitStatuses[i], path, $"{nameof(OnHitStatuses)}[{i}]", issues);
        }

        public override void Apply(SkillContext context)
        {
            SummonSystem.Summon(context.State, context.Caster, BaseStats, context.Target, OnHitStatuses);
        }
    }
}
