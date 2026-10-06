using System.Collections.Generic;
using MiniChess.Core.Characters.Shared;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Conditions;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;
using MiniChess.Core.Statuses.Library;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 소형 중화기 화염 딜러 (명세 10).
    /// 스킬 2 압축열탄. 스킬 1 화염지대: 미구현.
    /// </summary>
    public class FlamePiece : PieceModule
    {
        public const string PieceId = "FLAME";
        public const string CompressedShellId = "FLAME_COMPRESSED_SHELL";

        public override string Id => PieceId;
        public override string Name => "소형 중화기 화염 딜러";
        public override CharacterRole Role => CharacterRole.Combat;
        public override string Skill1Id => null;
        public override string Skill2Id => CompressedShellId;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return CompressedShell(tuning.Flame.CompressedShell);
        }

        /// <summary>압축열탄: 단일 대상 화상. 화상은 대상 소유자 Turn End 피해, 스택 대신 갱신(명세 10.3).</summary>
        public static SkillDefinition CompressedShell(CompressedShellTuning t)
        {
            var effects = new List<SkillEffect>();
            SkillBuildUtil.AddOptionalDamage(effects, t.ImpactDamage);
            effects.Add(new ApplyStatusEffect(StatusLibrary.Burn(t.BurnDamage, t.BurnTriggers)));

            return new SkillDefinition(
                CompressedShellId, "압축열탄", t.ApCost, SkillActionKind.Combat,
                new UnitTargeting(TargetFilter.Enemy, t.Range),
                new SingleCellArea(),
                effects,
                new SkillCondition[]
                {
                    new TuningRequirement(
                        (nameof(t.BurnDamage), t.BurnDamage),
                        (nameof(t.BurnTriggers), t.BurnTriggers)),
                });
        }
    }

    public class FlameTuning
    {
        public CompressedShellTuning CompressedShell { get; } = new CompressedShellTuning();
    }

    /// <summary>압축열탄(명세 10.3).</summary>
    public class CompressedShellTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }

        /// <summary>명중 즉시 피해. 0 이면 없음.</summary>
        public int? ImpactDamage { get; set; }

        /// <summary>화상 1회 피해.</summary>
        public int? BurnDamage { get; set; }

        /// <summary>화상 발동 횟수(대상 소유자 턴 종료 횟수).</summary>
        public int? BurnTriggers { get; set; }
    }
}
