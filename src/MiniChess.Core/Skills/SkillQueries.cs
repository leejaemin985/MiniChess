using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Actions;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills
{
    /// <summary>
    /// 표현 계층용 조회(스킬 버튼, 지정 가능 칸 하이라이트, 범위 미리보기). 상태를 바꾸지 않는다.
    /// </summary>
    public static class SkillQueries
    {
        /// <summary>유닛이 가진 스킬(슬롯 순). 경기 스킬 목록에 정의가 없는 Id 는 Definition 이 null.</summary>
        public static List<(string Id, SkillDefinition Definition)> GetSkills(GameState state, Unit unit)
        {
            return unit.Stats.Base.SkillIds
                .Select(id => (id, state.Skills.Find(id)))
                .ToList();
        }

        /// <summary>지금 실제로 사용할 수 있는 지정 칸. 사용 불가 상태면 빈 목록.</summary>
        public static List<Position> GetValidTargets(GameState state, Unit caster, string skillId)
        {
            if (UseSkillAction.ValidateUsable(state, caster, skillId) != SkillFailReason.None)
                return new List<Position>();

            return state.Skills.Find(skillId).Targeting.GetCandidates(state, caster).Distinct().ToList();
        }

        /// <summary>해당 칸을 지정했을 때 효과가 적용될 칸(범위 미리보기). 설정 오류가 있으면 빈 목록.</summary>
        public static IReadOnlyList<Position> GetAffectedCells(GameState state, Unit caster, string skillId, Position target)
        {
            SkillDefinition skill = state.Skills.Find(skillId);
            if (skill == null || skill.GetConfigIssues().Count > 0)
                return new List<Position>();

            return skill.Area.GetCells(state, caster, target);
        }
    }
}
