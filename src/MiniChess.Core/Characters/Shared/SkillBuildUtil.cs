using System.Collections.Generic;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Effects;

namespace MiniChess.Core.Characters.Shared
{
    /// <summary>캐릭터 스킬 정의에서 공통으로 쓰는 도우미.</summary>
    public static class SkillBuildUtil
    {
        /// <summary>피해 수치가 미설정이면 누락으로 보고되도록 넣고, 0 이면 피해 없음으로 생략한다.</summary>
        public static void AddOptionalDamage(List<SkillEffect> effects, int? damage)
        {
            if (damage != 0)
                effects.Add(new DamageEffect(damage));
        }

        /// <summary>SkillDefinition 의 requiredTuning 인자로 넘길 필수 수치 목록. (이름, 값) 순.</summary>
        public static (string Name, object Value)[] Require(params (string Name, object Value)[] values)
        {
            return values;
        }
    }
}
