using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Conditions
{
    /// <summary>시전자가 이 스킬로 조준해 착탄을 기다리는 예약 포격 수가 한도 미만일 때만 사용 가능.</summary>
    public class MaxActiveStrikesCondition : SkillCondition
    {
        public string SkillId { get; }

        /// <summary>동시에 유지할 수 있는 예약 수. [TBD 가능]</summary>
        public int? Max { get; }

        public MaxActiveStrikesCondition(string skillId, int? max)
        {
            SkillId = skillId;
            Max = max;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            Require(Max, path, nameof(Max), issues);
        }

        public override bool IsMet(GameState state, Unit caster)
        {
            return state.ScheduledStrikes.Count(s => s.Source == caster && s.SkillId == SkillId) < Max.Value;
        }
    }
}
