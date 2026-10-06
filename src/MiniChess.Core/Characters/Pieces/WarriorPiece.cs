using System.Collections.Generic;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 대검 전사 (명세 7).
    /// 스킬 1 강타. 스킬 2 돌진: 미구현.
    /// </summary>
    public class WarriorPiece : PieceModule
    {
        public const string PieceId = "WARRIOR";
        public const string SmashId = "WARRIOR_SMASH";

        public override string Id => PieceId;
        public override string Name => "대검 전사";
        public override CharacterRole Role => CharacterRole.Combat;
        public override string Skill1Id => SmashId;
        public override string Skill2Id => null;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return Smash(tuning.Warrior.Smash);
        }

        /// <summary>
        /// 강타: 인접한 적에게 높은 피해를 주는 단일 공격. 부가 효과 없음(명세 7.2).
        /// [가정] "인접"은 사거리 1(체비셰프, 대각선 포함).
        /// </summary>
        public static SkillDefinition Smash(SmashTuning t)
        {
            return new SkillDefinition(
                SmashId, "강타", t.ApCost, SkillActionKind.Combat,
                new UnitTargeting(TargetFilter.Enemy, 1),
                new SingleCellArea(),
                new SkillEffect[] { new DamageEffect(t.Damage) });
        }
    }

    public class WarriorTuning
    {
        public SmashTuning Smash { get; } = new SmashTuning();
    }

    /// <summary>강타(명세 7.2).</summary>
    public class SmashTuning
    {
        public int? ApCost { get; set; }
        public int? Damage { get; set; }
    }
}
