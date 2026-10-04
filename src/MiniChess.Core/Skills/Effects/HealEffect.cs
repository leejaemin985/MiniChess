using System.Collections.Generic;
using MiniChess.Core.Combat;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Effects
{
    /// <summary>범위 안 대상을 회복한다.</summary>
    public class HealEffect : SkillEffect
    {
        /// <summary>회복량. [TBD 가능]</summary>
        public int? Amount { get; }

        public TargetFilter Filter { get; }

        public HealEffect(int? amount, TargetFilter filter = TargetFilter.AllyOrSelf)
        {
            Amount = amount;
            Filter = filter;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Amount, path, nameof(Amount), issues);
        }

        public override void Apply(SkillContext context)
        {
            foreach (Unit target in GetUnitsInArea(context, Filter))
                DamageSystem.Heal(context.State, context.Caster, target, Amount.Value);
        }
    }
}
