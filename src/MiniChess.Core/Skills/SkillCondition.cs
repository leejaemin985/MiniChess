using MiniChess.Core.State;

namespace MiniChess.Core.Skills
{
    /// <summary>
    /// 지정 대상과 무관한 사용 조건(예: "살아 있는 분신이 없을 때만 소환 가능").
    /// 조건을 만족하지 않으면 SkillFailReason.ConditionNotMet 이 된다.
    /// </summary>
    public abstract class SkillCondition : SkillComponent
    {
        public abstract bool IsMet(GameState state, Unit caster);
    }
}
