using System.Collections.Generic;
using MiniChess.Core.Characters.Shared;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Conditions;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 마법공학 박격포 여학생 (명세 5).
    /// 스킬 2 지연 포격. 스킬 1 설치/해체: 미구현.
    /// </summary>
    public class MortarPiece : PieceModule
    {
        public const string PieceId = "MORTAR";
        public const string DelayedStrikeId = "MORTAR_DELAYED_STRIKE";

        public override string Id => PieceId;
        public override string Name => "마법공학 박격포 여학생";
        public override CharacterRole Role => CharacterRole.Control;
        public override string Skill1Id => null;
        public override string Skill2Id => DelayedStrikeId;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return DelayedStrike(tuning.Mortar.DelayedStrike);
        }

        /// <summary>
        /// 지연 포격: 시전 시 범위를 조준하고, 다음 내 턴 시작에 착탄해 범위 안 적에게 장판 피해(명세 5.3, 사용자 확정).
        /// 발사 턴에는 박격포가 이동/행동할 수 없다(두 턴을 묶는 대신 높은 피해). 착탄 위치는 조준한 칸에 고정.
        /// 착탄 전에 시전자가 죽으면 예약이 취소된다. 설치 상태 조건/충격탄 변형은 설치/해체 구현 시 추가.
        /// </summary>
        public static SkillDefinition DelayedStrike(DelayedStrikeTuning t)
        {
            return new SkillDefinition(
                DelayedStrikeId, "지연 포격", t.ApCost, SkillActionKind.Combat,
                SkillBuildUtil.AreaTargeting(t.Area, t.Range, CellRequirement.NotWall),
                SkillBuildUtil.Area(t.Area),
                new SkillEffect[] { new ScheduleStrikeEffect(t.Damage) },
                new SkillCondition[] { new MaxActiveStrikesCondition(DelayedStrikeId, t.MaxActive) });
        }
    }

    public class MortarTuning
    {
        public DelayedStrikeTuning DelayedStrike { get; } = new DelayedStrikeTuning();
    }

    /// <summary>지연 포격(명세 5.3).</summary>
    public class DelayedStrikeTuning
    {
        public int? ApCost { get; set; }

        /// <summary>조준 기준 칸을 지정할 수 있는 거리.</summary>
        public int? Range { get; set; }

        public AreaTuning Area { get; } = new AreaTuning();

        /// <summary>착탄 시 범위 안 적 각각에게 주는 장판 피해.</summary>
        public int? Damage { get; set; }

        /// <summary>시전자당 동시 예약 수.</summary>
        public int? MaxActive { get; set; }
    }
}
