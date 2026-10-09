using System.Collections.Generic;
using MiniChess.Core.Events;
using MiniChess.Core.Statuses;

namespace MiniChess.Core.Skills.Effects
{
    /// <summary>
    /// 시전자에게 상태를 켜고 끈다(설치/해체처럼 같은 버튼이 상태에 따라 바뀌는 스킬).
    /// 걸려 있으면 제거하고, 없으면 건다.
    /// </summary>
    public class ToggleStatusEffect : SkillEffect
    {
        /// <summary>켜고 끌 상태. 수치가 TBD 면 null.</summary>
        public StatusDefinition Status { get; }

        public ToggleStatusEffect(StatusDefinition status)
        {
            Status = status;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Status, path, nameof(Status), issues);
        }

        public override void Apply(SkillContext context)
        {
            StatusEffect existing = context.Caster.FindStatus(Status.Id);
            if (existing != null)
                StatusSystem.Remove(context.State, existing, StatusRemoveReason.Removed);
            else
                StatusSystem.Apply(context.State, Status, context.Caster, context.Caster);
        }
    }
}
