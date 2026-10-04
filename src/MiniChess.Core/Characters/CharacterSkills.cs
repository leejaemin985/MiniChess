using System.Collections.Generic;
using MiniChess.Core.Effects;
using MiniChess.Core.Effects.Zones;
using MiniChess.Core.Skills;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Conditions;
using MiniChess.Core.Skills.Effects;
using MiniChess.Core.Skills.Targeting;
using MiniChess.Core.Statuses;
using MiniChess.Core.Statuses.Library;

namespace MiniChess.Core.Characters
{
    /// <summary>
    /// 캐릭터 스킬 정의. 명세가 확정한 구조(지정 방식, 효과 종류, 발동 시점)는 여기서 고정하고,
    /// 수치는 모두 SkillTuning 에서 받는다. 수치가 비어 있으면 해당 스킬은 설정 누락으로 사용할 수 없다.
    /// 아직 구현하지 않은 스킬(돌진, 볼라, 분신, 화염지대 등)은 포함하지 않는다.
    /// </summary>
    public static class CharacterSkills
    {
        public static SkillCatalog CreateCatalog(SkillTuning tuning)
        {
            return new SkillCatalog()
                .Add(WarriorSmash(tuning.WarriorSmash))
                .Add(ArcherAimedShot(tuning.ArcherAimedShot))
                .Add(FlameCompressedShell(tuning.FlameCompressedShell))
                .Add(ChemistRootTrap(tuning.ChemistRootTrap, tuning.Root))
                .Add(ChemistPoisonGas(tuning.ChemistPoisonGas))
                .Add(ChainGuardChainBind(tuning.ChainGuardChainBind, tuning.Root))
                .Add(GardenerHealingMeadow(tuning.GardenerHealingMeadow));
        }

        /// <summary>
        /// 강타: 인접한 적에게 높은 피해를 주는 단일 공격. 부가 효과 없음(명세 7.2).
        /// [가정] "인접"은 사거리 1(체비셰프, 대각선 포함).
        /// </summary>
        public static SkillDefinition WarriorSmash(SmashTuning t)
        {
            return new SkillDefinition(
                SkillIds.WarriorSmash, "강타", t.ApCost, SkillActionKind.Combat,
                new UnitTargeting(TargetFilter.Enemy, 1),
                new SingleCellArea(),
                new SkillEffect[] { new DamageEffect(t.Damage) });
        }

        /// <summary>장거리 조준: 장거리 단일 공격. 조준 준비 턴/명중률 없음(명세 8.2).</summary>
        public static SkillDefinition ArcherAimedShot(AimedShotTuning t)
        {
            return new SkillDefinition(
                SkillIds.ArcherAimedShot, "장거리 조준", t.ApCost, SkillActionKind.Combat,
                new UnitTargeting(TargetFilter.Enemy, t.Range),
                new SingleCellArea(),
                new SkillEffect[] { new DamageEffect(t.Damage) });
        }

        /// <summary>압축열탄: 단일 대상 화상. 화상은 대상 소유자 Turn End 피해, 스택 대신 갱신(명세 10.3).</summary>
        public static SkillDefinition FlameCompressedShell(CompressedShellTuning t)
        {
            var effects = new List<SkillEffect>();
            AddOptionalDamage(effects, t.ImpactDamage);
            effects.Add(new ApplyStatusEffect(StatusLibrary.Burn(t.BurnDamage, t.BurnTriggers)));

            return new SkillDefinition(
                SkillIds.FlameCompressedShell, "압축열탄", t.ApCost, SkillActionKind.Combat,
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

        /// <summary>
        /// 속박 덫: 빈 칸에 덫 설치. 적이 밟으면 피해 + 속박, 진행 중 이동 정지(명세 9.2 [설계안]).
        /// 점령 칸 설치 불가는 CellEffectSystem 이 적용한다.
        /// </summary>
        public static SkillDefinition ChemistRootTrap(RootTrapTuning t, RootPolicyTuning root)
        {
            StatusDefinition rootStatus = StatusLibrary.Root(t.RootTurns, root.BlocksExternalMoves);
            int? damage = t.Damage;
            bool? singleUse = t.SingleUse;

            return new SkillDefinition(
                SkillIds.ChemistRootTrap, "속박 덫", t.ApCost, SkillActionKind.Combat,
                new CellTargeting(t.Range, CellRequirement.Empty, placeableLayer: CellEffectLayer.Trap),
                new SingleCellArea(),
                new SkillEffect[]
                {
                    new PlaceZoneEffect(t.Lifetime, (zone, _) => new RootTrap(zone, damage.Value, rootStatus, singleUse.Value)),
                },
                new SkillCondition[]
                {
                    new MaxActiveZonesCondition(SkillIds.ChemistRootTrap, t.MaxActive),
                    new TuningRequirement(
                        (nameof(t.Damage), t.Damage),
                        (nameof(t.RootTurns), t.RootTurns),
                        (nameof(t.SingleUse), t.SingleUse),
                        ("Root." + nameof(root.BlocksExternalMoves), root.BlocksExternalMoves)),
                });
        }

        /// <summary>독가스 설치: 작은 피해 장판 구역. 적 소유자 Turn End 에 피해(명세 9.3 [설계안]).</summary>
        public static SkillDefinition ChemistPoisonGas(PoisonGasTuning t)
        {
            int? damage = t.Damage;

            return new SkillDefinition(
                SkillIds.ChemistPoisonGas, "독가스 설치", t.ApCost, SkillActionKind.Combat,
                new CellTargeting(t.Range, CellRequirement.NotWall),
                new RectArea(t.Width, t.Height),
                new SkillEffect[]
                {
                    new PlaceZoneEffect(t.Lifetime, (zone, _) => new DamageField(zone, damage.Value)),
                },
                new SkillCondition[]
                {
                    new MaxActiveZonesCondition(SkillIds.ChemistPoisonGas, t.MaxActive),
                    new TuningRequirement((nameof(t.Damage), t.Damage)),
                });
        }

        /// <summary>
        /// 사슬 속박: 적 단일 대상에게 한 턴 속박(자발적 이동 불가, 공격 가능). 덫과 같은 속박 개념(명세 12.3).
        /// </summary>
        public static SkillDefinition ChainGuardChainBind(ChainBindTuning t, RootPolicyTuning root)
        {
            var effects = new List<SkillEffect>();
            AddOptionalDamage(effects, t.Damage);
            effects.Add(new ApplyStatusEffect(StatusLibrary.Root(1, root.BlocksExternalMoves)));

            return new SkillDefinition(
                SkillIds.ChainGuardChainBind, "사슬 속박", t.ApCost, SkillActionKind.Combat,
                new UnitTargeting(TargetFilter.Enemy, t.Range),
                new SingleCellArea(),
                effects,
                new SkillCondition[]
                {
                    new TuningRequirement(("Root." + nameof(root.BlocksExternalMoves), root.BlocksExternalMoves)),
                });
        }

        /// <summary>
        /// 회복 초원: 고정 회복 장판. 장판 위 아군을 매 턴 시작에 회복(명세 6.2). 자기 칸 중심 설치 허용.
        /// </summary>
        public static SkillDefinition GardenerHealingMeadow(HealingMeadowTuning t)
        {
            int? heal = t.Heal;

            return new SkillDefinition(
                SkillIds.GardenerHealingMeadow, "회복 초원", t.ApCost, SkillActionKind.Combat,
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

        /// <summary>피해 수치가 미설정이면 누락으로 보고되도록 넣고, 0 이면 피해 없음으로 생략한다.</summary>
        private static void AddOptionalDamage(List<SkillEffect> effects, int? damage)
        {
            if (damage != 0)
                effects.Add(new DamageEffect(damage));
        }
    }
}
