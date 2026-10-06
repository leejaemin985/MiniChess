using System.Collections.Generic;
using MiniChess.Core.Data;
using MiniChess.Core.Effects.Zones;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Conditions;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 원예부 선배 수호자 (명세 6).
    /// 스킬 1 회복 초원. 스킬 2 단일 보호막: 미구현.
    /// </summary>
    public class GardenerPiece : PieceModule
    {
        public const string PieceId = "GARDENER";
        public const string HealingMeadowId = "GARDENER_HEALING_MEADOW";

        public override string Id => PieceId;
        public override string Name => "원예부 선배 수호자";
        public override CharacterRole Role => CharacterRole.Guardian;
        public override string Skill1Id => HealingMeadowId;
        public override string Skill2Id => null;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return HealingMeadow(tuning.Gardener.HealingMeadow);
        }

        /// <summary>
        /// 회복 초원: 고정 회복 장판. 장판 위 아군을 매 턴 시작에 회복(명세 6.2). 자기 칸 중심 설치 허용.
        /// </summary>
        public static SkillDefinition HealingMeadow(HealingMeadowTuning t)
        {
            int? heal = t.Heal;

            return new SkillDefinition(
                HealingMeadowId, "회복 초원", t.ApCost, SkillActionKind.Combat,
                new CellTargeting(t.Range, CellRequirement.NotWall, includeCasterCell: true),
                new SquareArea(t.Radius),
                new SkillEffect[]
                {
                    new PlaceZoneEffect(t.Lifetime, (zone, _) => new HealField(zone, heal.Value)),
                },
                new SkillCondition[]
                {
                    new TuningRequirement((nameof(t.Heal), t.Heal)),
                });
        }
    }

    public class GardenerTuning
    {
        public HealingMeadowTuning HealingMeadow { get; } = new HealingMeadowTuning();
    }

    /// <summary>회복 초원(명세 6.2).</summary>
    public class HealingMeadowTuning
    {
        public int? ApCost { get; set; }

        /// <summary>장판 중심을 지정할 수 있는 거리.</summary>
        public int? Range { get; set; }

        /// <summary>중심 기준 반경. [설계안] 3×3 = 1.</summary>
        public int? Radius { get; set; }

        /// <summary>턴 시작마다 회복량.</summary>
        public int? Heal { get; set; }

        /// <summary>장판 수명(설치한 쪽 소유자 턴 종료 횟수).</summary>
        public int? Lifetime { get; set; }
    }
}
