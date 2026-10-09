using System.Collections.Generic;
using MiniChess.Core.Characters.Shared;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;
using MiniChess.Core.Statuses.Library;

namespace MiniChess.Core.Characters.Pieces
{
    /// <summary>
    /// 사슬 수호기사 (명세 12).
    /// 스킬 2 사슬 속박. 스킬 1 호위: 미구현.
    /// </summary>
    public class ChainGuardPiece : PieceModule
    {
        public const string PieceId = "CHAIN_GUARD";
        public const string ChainBindId = "CHAIN_GUARD_CHAIN_BIND";

        public override string Id => PieceId;
        public override string Name => "사슬 수호기사";
        public override CharacterRole Role => CharacterRole.Guardian;
        public override string Skill1Id => null;
        public override string Skill2Id => ChainBindId;

        public override IEnumerable<SkillDefinition> CreateSkills(SkillTuning tuning)
        {
            yield return ChainBind(tuning.ChainGuard.ChainBind, tuning.Root);
        }

        /// <summary>
        /// 사슬 속박: 적 단일 대상에게 한 턴 속박(자발적 이동 불가, 공격 가능). 덫과 같은 속박 개념(명세 12.3).
        /// </summary>
        public static SkillDefinition ChainBind(ChainBindTuning t, RootPolicyTuning root)
        {
            var effects = new List<SkillEffect>();
            SkillBuildUtil.AddOptionalDamage(effects, t.Damage);
            effects.Add(new ApplyStatusEffect(StatusLibrary.Root(1, root.BlocksExternalMoves)));

            return new SkillDefinition(
                ChainBindId, "사슬 속박", t.ApCost, SkillActionKind.Combat,
                new UnitTargeting(TargetFilter.Enemy, t.Range),
                new SingleCellArea(),
                effects,
                requiredTuning: SkillBuildUtil.Require(("Root." + nameof(root.BlocksExternalMoves), root.BlocksExternalMoves)));
        }
    }

    public class ChainGuardTuning
    {
        public ChainBindTuning ChainBind { get; } = new ChainBindTuning();
    }

    /// <summary>사슬 속박(명세 12.3). 지속은 "한 턴"으로 확정. 속박의 외부 이동 정책은 SkillTuning.Root 를 따른다.</summary>
    public class ChainBindTuning
    {
        public int? ApCost { get; set; }
        public int? Range { get; set; }

        /// <summary>피해. 0 이면 없음.</summary>
        public int? Damage { get; set; }
    }
}
