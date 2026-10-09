using System.Collections.Generic;
using MiniChess.Core.Characters.Shared;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 낫 / 그림자 분신 전투원 (명세 4).
    /// 스킬 1 일자 베기. 스킬 2 그림자 분신: 미구현.
    /// </summary>
    public class ScythePiece : PieceModule
    {
        public const string PieceId = "SCYTHE";
        public const string SlashId = "SCYTHE_SLASH";

        public override string Id => PieceId;
        public override string Name => "낫 / 그림자 분신 전투원";
        public override CharacterRole Role => CharacterRole.Combat;
        public override string Skill1Id => SlashId;
        public override string Skill2Id => null;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return Slash(tuning.Scythe.Slash);
        }

        /// <summary>
        /// 일자 베기: 사거리 안에서 지정한 칸 기준의 일자 범위 안 적 전원에게 피해(명세 4.2, 사용자 확정: 선택 칸 기준 가로 4칸, 방향 회전).
        /// 벽은 공격을 막지 않는다. 저주 표식 추가 피해는 그림자 분신 구현 후 붙인다.
        /// </summary>
        public static SkillDefinition Slash(SlashTuning t)
        {
            return new SkillDefinition(
                SlashId, "일자 베기", t.ApCost, SkillActionKind.Combat,
                SkillBuildUtil.AreaTargeting(t.Area, t.Range, CellRequirement.NotWall),
                SkillBuildUtil.Area(t.Area),
                new SkillEffect[] { new DamageEffect(t.Damage) });
        }
    }

    public class ScytheTuning
    {
        public SlashTuning Slash { get; } = new SlashTuning();
    }

    /// <summary>일자 베기(명세 4.2).</summary>
    public class SlashTuning
    {
        public int? ApCost { get; set; }

        /// <summary>범위 기준 칸을 지정할 수 있는 거리.</summary>
        public int? Range { get; set; }

        public AreaTuning Area { get; } = new AreaTuning();

        /// <summary>범위 안 적 각각에게 주는 피해.</summary>
        public int? Damage { get; set; }
    }
}
