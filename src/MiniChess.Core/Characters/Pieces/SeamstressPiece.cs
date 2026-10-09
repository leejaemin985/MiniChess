using System.Collections.Generic;
using MiniChess.Core.Data;
using MiniChess.Core.Movement;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 공간 재봉사 (명세 11).
    /// 스킬 1 워프/위치 교환. 스킬 2 장애물 생성: 미구현.
    /// </summary>
    public class SeamstressPiece : PieceModule
    {
        public const string PieceId = "SEAMSTRESS";
        public const string WarpId = "SEAMSTRESS_WARP";

        public override string Id => PieceId;
        public override string Name => "공간 재봉사";
        public override CharacterRole Role => CharacterRole.Control;
        public override string Skill1Id => WarpId;
        public override string Skill2Id => null;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return Warp(tuning.Seamstress.Warp);
        }

        /// <summary>
        /// 워프/위치 교환: 사거리 안의 아군 둘(자신 포함 가능)의 자리를 맞바꾼다(명세 11.2, 사용자 확정).
        /// 외부 이동이라 이미 공격한 아군도 옮길 수 있고, 옮겨진 아군의 전투 행동은 회복되지 않는다.
        /// 도착 칸의 칸 효과는 발동한다. 이동이 막힌 유닛(속박된 자신 등)은 지정할 수 없다.
        /// </summary>
        public static SkillDefinition Warp(WarpTuning t)
        {
            return new SkillDefinition(
                WarpId, "워프 / 위치 교환", t.ApCost, SkillActionKind.Combat,
                new MultiUnitTargeting(TargetFilter.AllyOrSelf, t.Range, count: 2, movedBy: MoveKind.Swap),
                PatternArea.SingleCell,
                new SkillEffect[] { new SwapEffect() });
        }
    }

    public class SeamstressTuning
    {
        public WarpTuning Warp { get; } = new WarpTuning();
    }

    /// <summary>워프/위치 교환(명세 11.2).</summary>
    public class WarpTuning
    {
        public int? ApCost { get; set; }

        /// <summary>교환할 두 아군 각각이 시전자로부터 떨어질 수 있는 최대 거리.</summary>
        public int? Range { get; set; }
    }
}
