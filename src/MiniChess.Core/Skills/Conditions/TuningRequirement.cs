using System.Collections.Generic;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills.Conditions
{
    /// <summary>
    /// 부품에 직접 드러나지 않는 수치(상태 지속, 덫 피해 등)의 설정 여부만 보고하는 조건. 항상 만족한다.
    /// 누락 보고 위치를 수치 이름으로 남기기 위해 사용한다.
    /// </summary>
    public class TuningRequirement : SkillCondition
    {
        private readonly (string Name, object Value)[] _values;

        public TuningRequirement(params (string Name, object Value)[] values)
        {
            _values = values;
        }

        public override void CollectConfigIssues(string path, List<string> issues)
        {
            foreach ((string name, object value) in _values)
                Require(value, path, name, issues);
        }

        public override bool IsMet(GameState state, Unit caster) => true;
    }
}
