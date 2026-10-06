using System.Collections.Generic;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 궁수 / 볼라 사냥꾼 (명세 8).
    /// 스킬 1 장거리 조준. 스킬 2 볼라 투척: 미구현.
    /// </summary>
    public class ArcherPiece : PieceModule
    {
        public const string PieceId = "ARCHER";
        public const string AimedShotId = "ARCHER_AIMED_SHOT";

        public override string Id => PieceId;
        public override string Name => "궁수 / 볼라 사냥꾼";
        public override CharacterRole Role => CharacterRole.Combat;
        public override string Skill1Id => AimedShotId;
        public override string Skill2Id => null;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return AimedShot(tuning.Archer.AimedShot);
        }

        /// <summary>장거리 조준: 장거리 단일 공격. 조준 준비 턴/명중률 없음(명세 8.2).</summary>
        public static SkillDefinition AimedShot(AimedShotTuning t)
        {
            return new SkillDefinition(
                AimedShotId, "장거리 조준", t.ApCost, SkillActionKind.Combat,
                new UnitTargeting(TargetFilter.Enemy, t.Range),
                new SingleCellArea(),
                new SkillEffect[] { new DamageEffect(t.Damage) });
        }
    }

    public class ArcherTuning
    {
        public AimedShotTuning AimedShot { get; } = new AimedShotTuning();
    }

    /// <summary>장거리 조준(명세 8.2).</summary>
    public class AimedShotTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }
        public int? Damage { get; set; }
    }
}
