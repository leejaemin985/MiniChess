using System.Collections.Generic;
using MiniChess.Core.Combat;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Effects
{
    /// <summary>
    /// 범위 안 대상에게 보호막을 준다. 같은 보호막 Id 가 이미 있으면 중첩하지 않고 재충전한다.
    /// </summary>
    public class ApplyShieldEffect : SkillEffect
    {
        /// <summary>부여할 보호막의 Id. 다른 Id 의 보호막과는 합산된다.</summary>
        public string ShieldId { get; }

        /// <summary>보호막 양. [TBD 가능]</summary>
        public int? Amount { get; }

        public TargetFilter Filter { get; }

        public ApplyShieldEffect(string shieldId, int? amount, TargetFilter filter = TargetFilter.AllyOrSelf)
        {
            ShieldId = shieldId;
            Amount = amount;
            Filter = filter;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(ShieldId, path, nameof(ShieldId), issues);
            Require(Amount, path, nameof(Amount), issues);
        }

        public override void Apply(SkillContext context)
        {
            foreach (Unit target in GetUnitsInArea(context, Filter))
                DamageSystem.AddShield(context.State, target, ShieldId, Amount.Value, context.Caster);
        }
    }
}
