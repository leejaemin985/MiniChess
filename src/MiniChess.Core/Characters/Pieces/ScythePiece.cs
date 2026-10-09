using System.Collections.Generic;
using MiniChess.Core.Characters.Shared;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Conditions;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;
using MiniChess.Core.Statuses;
using MiniChess.Core.Statuses.Library;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 낫 / 그림자 분신 전투원 (명세 4).
    /// 스킬 1 일자 베기, 스킬 2 그림자 분신.
    /// 흐름: 분신 소환 → 분신 전진/공격으로 저주 표식 → 본체 일자 베기 또는 기본 공격으로 표식 소모 + 추가 피해.
    /// </summary>
    public class ScythePiece : PieceModule
    {
        public const string PieceId = "SCYTHE";
        public const string SlashId = "SCYTHE_SLASH";
        public const string ShadowCloneId = "SCYTHE_SHADOW_CLONE";
        public const string CloneUnitId = "SCYTHE_CLONE";

        public override string Id => PieceId;
        public override string Name => "낫 / 그림자 분신 전투원";
        public override CharacterRole Role => CharacterRole.Combat;
        public override string Skill1Id => SlashId;
        public override string Skill2Id => ShadowCloneId;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return Slash(tuning.Scythe.Slash);
            yield return ShadowClone(tuning.Scythe.ShadowClone);
        }

        /// <summary>
        /// 일자 베기: 사거리 안에서 지정한 칸 기준의 일자 범위 안 적 전원에게 피해(명세 4.2, 사용자 확정: 선택 칸 기준 가로 4칸, 방향 회전).
        /// 벽은 공격을 막지 않는다. 직접 피해라 이 본체의 저주 표식을 소모하고 추가 피해를 준다.
        /// </summary>
        public static SkillDefinition Slash(SlashTuning t)
        {
            return new SkillDefinition(
                SlashId, "일자 베기", t.ApCost, SkillActionKind.Combat,
                SkillBuildUtil.AreaTargeting(t.Area, t.Range, CellRequirement.NotWall),
                SkillBuildUtil.Area(t.Area),
                new SkillEffect[] { new DamageEffect(t.Damage) });
        }

        /// <summary>
        /// 그림자 분신: 본체 상하좌우 인접 빈 칸에 분신을 소환한다(명세 4.3, 사용자 확정).
        /// 본체당 1개(살아 있으면 재소환 불가), 소환한 턴에 본체는 행동 종료, 분신은 그 턴부터 팀 AP 로 이동/공격 가능.
        /// 분신의 기본 공격은 피해 0 이며 적중 시 저주 표식을 건다. 본체가 죽으면 분신도 사라진다.
        /// 분신은 점령과 전멸 판정에서 제외된다.
        /// </summary>
        public static SkillDefinition ShadowClone(ShadowCloneTuning t)
        {
            StatusDefinition mark = StatusLibrary.CurseMark(t.MarkTurns, t.MarkBonusDamage);

            return new SkillDefinition(
                ShadowCloneId, "그림자 분신", t.ApCost, SkillActionKind.Combat,
                new CellTargeting(1, CellRequirement.Empty, RangeShape.Orthogonal),
                PatternArea.SingleCell,
                new SkillEffect[] { new SummonEffect(CloneStats(t), new[] { mark }) },
                new SkillCondition[] { new NoLivingSummonCondition() },
                endsCasterActions: true);
        }

        /// <summary>분신 능력치. 기본 공격 피해는 0 으로 고정(명세 4.3 확정).</summary>
        private static UnitBaseStats CloneStats(ShadowCloneTuning t)
        {
            if (t.CloneHp == null || t.CloneAttackRange == null)
                return null;

            return new UnitBaseStats
            {
                Id = CloneUnitId,
                Name = "그림자 분신",
                MaxHp = t.CloneHp.Value,
                Attack = 0,
                AttackRange = t.CloneAttackRange.Value,
            };
        }
    }

    public class ScytheTuning
    {
        public SlashTuning Slash { get; } = new SlashTuning();
        public ShadowCloneTuning ShadowClone { get; } = new ShadowCloneTuning();
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

    /// <summary>그림자 분신과 저주 표식(명세 4.3, 4.5).</summary>
    public class ShadowCloneTuning
    {
        /// <summary>소환 AP.</summary>
        public int? ApCost { get; set; }

        /// <summary>분신 HP. [설계 의도] 1~2.</summary>
        public int? CloneHp { get; set; }

        /// <summary>분신 기본 공격 사거리.</summary>
        public int? CloneAttackRange { get; set; }

        /// <summary>저주 표식 지속(대상 소유자 턴 종료 횟수). [설계 의도] 2~3.</summary>
        public int? MarkTurns { get; set; }

        /// <summary>본체가 표식 대상을 때릴 때 추가 피해.</summary>
        public int? MarkBonusDamage { get; set; }
    }
}
