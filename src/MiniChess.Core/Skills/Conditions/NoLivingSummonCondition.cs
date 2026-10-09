using System.Linq;
using MiniChess.Core.State;
using MiniChess.Core.Summons;

namespace MiniChess.Core.Skills.Conditions
{
    /// <summary>시전자의 소환물이 하나도 살아 있지 않을 때만 사용 가능(분신은 본체당 1개, 없어져야 재소환).</summary>
    public class NoLivingSummonCondition : SkillCondition
    {
        public override bool IsMet(GameState state, Unit caster)
        {
            return !SummonSystem.GetLivingSummons(state, caster).Any();
        }
    }
}
