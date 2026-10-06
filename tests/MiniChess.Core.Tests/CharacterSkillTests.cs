using MiniChess.Core.Actions;
using MiniChess.Core.Capture;
using MiniChess.Core.Characters;
using MiniChess.Core.Characters.Pieces;
using MiniChess.Core.Combat;
using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.Effects;
using MiniChess.Core.Effects.Zones;
using MiniChess.Core.Events;
using MiniChess.Core.Movement;
using MiniChess.Core.Presets;
using MiniChess.Core.Skills;
using MiniChess.Core.State;
using MiniChess.Core.Statuses.Library;
using MiniChess.Core.Tests.Support;
using Xunit;

namespace MiniChess.Core.Tests
{
    /// <summary>
    /// 구현된 캐릭터 스킬 동작 검증. 수치는 PrototypeTestPreset 의 임시값을 사용한다(규칙은 TestGame 의 AP 6/4/4).
    /// </summary>
    public class CharacterSkillTests
    {
        private static SkillDefinition[] PresetSkills() =>
            PieceModules.CreateSkillCatalog(PrototypeTestPreset.CreateSkillTuning()).All.ToArray();

        private static TestGame Game() => new TestGame().WithSkills(PresetSkills());

        private static UnitBaseStats With(string skill, int hp = 10) => TestGame.Stats(skill, hp: hp, skills: skill);

        private static UnitBaseStats With(string skill1, string skill2) => TestGame.Stats("dual", skills: new[] { skill1, skill2 });

        private static void EndTurn(GameState state) => new EndTurnAction(state.CurrentTeam).Execute(state);

        private static SkillResult Use(GameState state, Unit caster, string skill, int x, int y) =>
            new UseSkillAction(caster, skill, new Position(x, y)).Execute(state);

        private static SkillFailReason Check(GameState state, Unit caster, string skill, int x, int y) =>
            new UseSkillAction(caster, skill, new Position(x, y)).Validate(state);

        #region Config

        [Fact]
        public void PresetTuning_MakesAllImplementedSkillsUsable()
        {
            SkillDefinition[] skills = PresetSkills();

            Assert.Equal(8, skills.Length);
            Assert.All(skills, s => Assert.Empty(s.GetConfigIssues()));
        }

        [Fact]
        public void EmptyTuning_ReportsEveryMissingValueByName()
        {
            SkillDefinition[] skills = PieceModules.CreateSkillCatalog(new SkillTuning()).All.ToArray();

            Assert.All(skills, s => Assert.NotEmpty(s.GetConfigIssues()));

            List<string> trap = skills.Single(s => s.Id == ChemistPiece.RootTrapId).GetConfigIssues();
            Assert.Contains("CHEMIST_ROOT_TRAP.ApCost 미설정", trap);
            Assert.Contains("CHEMIST_ROOT_TRAP.Conditions[1].SingleUse 미설정", trap);
            Assert.Contains("CHEMIST_ROOT_TRAP.Conditions[1].Root.BlocksExternalMoves 미설정", trap);
        }

        [Fact]
        public void RosterSkillSlots_ReferToImplementedSkills()
        {
            var ids = PresetSkills().Select(s => s.Id).ToHashSet();
            var slotted = PieceModules.CreateDefinitions().SelectMany(c => c.SkillSlots).Where(id => id != null).ToList();

            Assert.Equal(8, slotted.Count);
            Assert.All(slotted, id => Assert.Contains(id, ids));
        }

        #endregion

        #region Single target

        [Fact]
        public void WarriorSmash_HitsAdjacentEnemyOnly()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 2, 2, With(WarriorPiece.SmashId))
                .Place(Team.Player2, 3, 3)
                .Place(Team.Player2, 4, 2)
                .Start();

            Assert.Equal(SkillFailReason.InvalidTarget, Check(state, u[0], WarriorPiece.SmashId, 4, 2));

            Use(state, u[0], WarriorPiece.SmashId, 3, 3);

            Assert.Equal(5, u[1].Stats.CurrentHp);
        }

        [Fact]
        public void ArcherAimedShot_ReachesItsRange()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 0, 0, With(ArcherPiece.AimedShotId))
                .Place(Team.Player2, 5, 5)
                .Place(Team.Player2, 6, 6)
                .Start();

            Assert.Equal(SkillFailReason.InvalidTarget, Check(state, u[0], ArcherPiece.AimedShotId, 6, 6));

            Use(state, u[0], ArcherPiece.AimedShotId, 5, 5);

            Assert.Equal(7, u[1].Stats.CurrentHp);
        }

        // 명세 14: "압축열탄 화상" → 대상 소유자 Turn End 에 피해
        [Fact]
        public void FlameCompressedShell_ImpactThenBurnOnTargetOwnersTurnEnds()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 0, 0, With(FlamePiece.CompressedShellId))
                .Place(Team.Player2, 2, 2)
                .Start();

            Use(state, u[0], FlamePiece.CompressedShellId, 2, 2);
            Assert.Equal(9, u[1].Stats.CurrentHp);

            EndTurn(state); // P1 종료: 화상 없음
            Assert.Equal(9, u[1].Stats.CurrentHp);

            EndTurn(state); // P2 종료: 화상 1회
            Assert.Equal(8, u[1].Stats.CurrentHp);

            EndTurn(state);
            EndTurn(state); // P2 종료: 화상 2회째, 만료
            Assert.Equal(7, u[1].Stats.CurrentHp);
            Assert.Empty(u[1].Statuses);
        }

        [Fact]
        public void FlameCompressedShell_ReapplyRefreshesInsteadOfStacking()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 0, 0, With(FlamePiece.CompressedShellId))
                .Place(Team.Player1, 1, 0, With(FlamePiece.CompressedShellId))
                .Place(Team.Player2, 2, 2, TestGame.Stats(hp: 20))
                .WithRules(r => r.Ap.StartAp = 6)
                .Start();

            Use(state, u[0], FlamePiece.CompressedShellId, 2, 2);
            Use(state, u[1], FlamePiece.CompressedShellId, 2, 2);

            Assert.Single(u[2].Statuses);
            EndTurn(state);
            EndTurn(state);
            Assert.Equal(17, u[2].Stats.CurrentHp); // 즉시 1 + 1, 화상 1 (중첩 없음)
        }

        [Fact]
        public void ChainBind_RootsTargetForItsNextTurn_ButAllowsAttack()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 0, 0, With(ChainGuardPiece.ChainBindId))
                .Place(Team.Player2, 1, 1, TestGame.Stats(hp: 20))
                .Start();

            Use(state, u[0], ChainGuardPiece.ChainBindId, 1, 1);
            EndTurn(state);

            Assert.Equal(MoveFailReason.Rooted, new MoveAction(u[1], new Position(1, 2)).Validate(state));
            Assert.Equal(AttackFailReason.None, new AttackAction(u[1], u[0]).Validate(state));

            EndTurn(state);
            EndTurn(state);
            Assert.Equal(MoveFailReason.None, new MoveAction(u[1], new Position(1, 2)).Validate(state));
        }

        #endregion

        #region Root trap

        [Fact]
        public void RootTrap_StopsEnemyMove_DamagesRoots_AndIsConsumed()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 0, 0, With(ChemistPiece.RootTrapId))
                .Place(Team.Player2, 0, 4)
                .Start();

            Use(state, u[0], ChemistPiece.RootTrapId, 0, 2);
            EndTurn(state);

            MovementResult move = new MoveAction(u[1], new Position(0, 1)).Execute(state);

            Assert.Equal(new Position(0, 2), move.StoppedAt);
            Assert.Equal(9, u[1].Stats.CurrentHp);
            Assert.NotNull(u[1].FindStatus(StatusLibrary.RootId));
            Assert.Null(state.Board.GetCell(new Position(0, 2)).GetEffect(CellEffectLayer.Trap));
            Assert.Equal(MoveFailReason.Rooted, new MoveAction(u[1], new Position(1, 2)).Validate(state));
        }

        // 자기 턴에 덫을 밟으면 남은 턴 + 다음 자기 턴까지 속박 (부여 턴 종료는 세지 않음)
        [Fact]
        public void RootTrap_TriggeredOnOwnTurn_RootsThroughNextOwnTurn()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 0, 0, With(ChemistPiece.RootTrapId))
                .Place(Team.Player2, 0, 4)
                .Start();
            Use(state, u[0], ChemistPiece.RootTrapId, 0, 2);
            EndTurn(state);
            new MoveAction(u[1], new Position(0, 1)).Execute(state);

            EndTurn(state); // P2 종료 (부여 턴, 세지 않음)
            EndTurn(state); // P1 종료
            Assert.Equal(MoveFailReason.Rooted, new MoveAction(u[1], new Position(1, 2)).Validate(state));

            EndTurn(state); // P2 종료: 만료
            EndTurn(state);
            Assert.Equal(MoveFailReason.None, new MoveAction(u[1], new Position(1, 2)).Validate(state));
        }

        [Fact]
        public void RootTrap_IsNotTriggeredByAllies()
        {
            var (state, u) = Game()
                .WithRules(r => r.Ap.StartAp = 6)
                .Place(Team.Player1, 0, 0, With(ChemistPiece.RootTrapId))
                .Place(Team.Player1, 1, 4)
                .Start();
            Use(state, u[0], ChemistPiece.RootTrapId, 1, 2);

            MovementResult move = new MoveAction(u[1], new Position(1, 1)).Execute(state);

            Assert.Equal(new Position(1, 1), move.StoppedAt);
            Assert.NotNull(state.Board.GetCell(new Position(1, 2)).GetEffect(CellEffectLayer.Trap));
        }

        // 명세 14: "점령 타일에 덫 생성" → 거부 (지정 단계에서 제외)
        [Fact]
        public void RootTrap_CannotTargetCaptureTile()
        {
            var (state, u) = Game()
                .WithMap(".......", ".......", ".......", ".......", "C......", ".......", ".......")
                .Place(Team.Player1, 0, 0, With(ChemistPiece.RootTrapId))
                .Start();

            Assert.Equal(SkillFailReason.InvalidTarget, Check(state, u[0], ChemistPiece.RootTrapId, 0, 2));
            Assert.DoesNotContain(new Position(0, 2), SkillQueries.GetValidTargets(state, u[0], ChemistPiece.RootTrapId));
        }

        [Fact]
        public void RootTrap_RespectsMaxActive_AndExpiresAfterLifetime()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 3, 3, With(ChemistPiece.RootTrapId))
                .Place(Team.Player2, 6, 6)
                .Start();

            Use(state, u[0], ChemistPiece.RootTrapId, 3, 5); // 턴 1
            EndTurn(state); EndTurn(state);
            Use(state, u[0], ChemistPiece.RootTrapId, 3, 1); // 턴 3
            EndTurn(state); EndTurn(state);

            Assert.Equal(SkillFailReason.ConditionNotMet, Check(state, u[0], ChemistPiece.RootTrapId, 5, 3)); // 턴 5: 최대 2개

            // 첫 덫(수명 3): 턴 3, 5, 7 의 P1 종료에 감소 → 턴 7 종료에 만료
            EndTurn(state); EndTurn(state);
            Assert.NotNull(state.Board.GetCell(new Position(3, 5)).GetEffect(CellEffectLayer.Trap));
            EndTurn(state);
            Assert.Null(state.Board.GetCell(new Position(3, 5)).GetEffect(CellEffectLayer.Trap));
            Assert.Contains(state.Events.All, e => e is CellEffectRemovedEvent r && r.Reason == CellEffectRemoveReason.Expired);
        }

        #endregion

        #region Fields

        [Fact]
        public void PoisonGas_DamagesEnemyOnItsTurnEnd_ForLifetime_AndAllowsOneZone()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 0, 0, With(ChemistPiece.PoisonGasId))
                .Place(Team.Player2, 3, 3)
                .Start();

            SkillResult result = Use(state, u[0], ChemistPiece.PoisonGasId, 2, 2);
            Assert.Equal(new[] { new Position(2, 2), new Position(3, 2), new Position(2, 3), new Position(3, 3) }, result.AffectedCells);

            EndTurn(state); // P1 종료: 적 소유자 턴 아님
            Assert.Equal(10, u[1].Stats.CurrentHp);
            EndTurn(state); // P2 종료: 1
            Assert.Equal(9, u[1].Stats.CurrentHp);

            Assert.Equal(SkillFailReason.ConditionNotMet, Check(state, u[0], ChemistPiece.PoisonGasId, 0, 2));

            EndTurn(state); // P1 종료: 수명 2 → 1
            EndTurn(state); // P2 종료: 2
            Assert.Equal(8, u[1].Stats.CurrentHp);
            EndTurn(state); // P1 종료: 수명 0, 전체 제거
            Assert.Null(state.Board.GetCell(new Position(2, 2)).GetEffect(CellEffectLayer.AreaEffect));
            EndTurn(state); // P2 종료 → P1 턴
            Assert.Equal(SkillFailReason.None, Check(state, u[0], ChemistPiece.PoisonGasId, 0, 2));
        }

        // 명세 14: "턴 시작 회복 초원" → 지정한 Turn Start 에만 회복
        [Fact]
        public void HealingMeadow_HealsAlliesAtTheirTurnStart()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 1, 1, With(GardenerPiece.HealingMeadowId))
                .Place(Team.Player1, 2, 2)
                .Place(Team.Player2, 0, 0, TestGame.Stats(hp: 10))
                .Start();
            DamageSystem.Apply(state, new DamageRequest(null, u[1], 5, DamageType.Direct));
            DamageSystem.Apply(state, new DamageRequest(null, u[2], 5, DamageType.Direct));

            Use(state, u[0], GardenerPiece.HealingMeadowId, 1, 1); // 자기 칸 중심 3×3
            EndTurn(state); // P1 종료: 회복 없음
            Assert.Equal(5, u[1].Stats.CurrentHp);

            EndTurn(state); // P2 종료 → P1 시작: 아군 회복, 적은 회복 안 됨
            Assert.Equal(6, u[1].Stats.CurrentHp);
            Assert.Equal(5, u[2].Stats.CurrentHp);
        }

        // 명세 14: "3×3 회복지대 일부에 독가스 설치" → 겹치는 타일의 회복지대가 교체됨
        [Fact]
        public void PoisonGas_OverwritesOverlappingMeadowTiles_OthersRemain()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 1, 1, With(GardenerPiece.HealingMeadowId))
                .Place(Team.Player1, 5, 5, With(ChemistPiece.PoisonGasId))
                .Place(Team.Player2, 6, 0)
                .Start();
            Use(state, u[0], GardenerPiece.HealingMeadowId, 1, 1); // (0..2, 0..2)
            EndTurn(state); EndTurn(state);

            Use(state, u[1], ChemistPiece.PoisonGasId, 2, 2); // (2..3, 2..3)

            Assert.IsType<DamageField>(state.Board.GetCell(new Position(2, 2)).GetEffect(CellEffectLayer.AreaEffect));
            Assert.IsType<HealField>(state.Board.GetCell(new Position(0, 0)).GetEffect(CellEffectLayer.AreaEffect));
            Assert.IsType<HealField>(state.Board.GetCell(new Position(2, 1)).GetEffect(CellEffectLayer.AreaEffect));
        }

        #endregion

        #region Shield

        [Fact]
        public void SingleShield_ShieldsAlly_SpendsAp_AndUsesCombatAction()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 1, 1, With(GardenerPiece.SingleShieldId))
                .Place(Team.Player1, 3, 3)
                .Place(Team.Player2, 6, 6)
                .Start();

            Use(state, u[0], GardenerPiece.SingleShieldId, 3, 3);

            Assert.Equal(3, u[1].Stats.FindShield(GardenerPiece.SingleShieldId).Amount);
            Assert.Same(u[0], u[1].Stats.FindShield(GardenerPiece.SingleShieldId).Source);
            Assert.Equal(1, state.GetPlayer(Team.Player1).Ap.Current);
            Assert.True(u[0].TurnState.CombatActionUsed);
        }

        [Fact]
        public void SingleShield_CanTargetSelf_ButNotEnemy()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 1, 1, With(GardenerPiece.SingleShieldId))
                .Place(Team.Player2, 2, 2)
                .Start();

            Assert.Equal(SkillFailReason.InvalidTarget, Check(state, u[0], GardenerPiece.SingleShieldId, 2, 2));

            Use(state, u[0], GardenerPiece.SingleShieldId, 1, 1);
            Assert.Equal(3, u[0].Stats.Shield);
        }

        [Fact]
        public void SingleShield_RecastOnSameTarget_RechargesWithoutStacking()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 1, 1, With(GardenerPiece.SingleShieldId))
                .Place(Team.Player2, 6, 6)
                .Start();
            Use(state, u[0], GardenerPiece.SingleShieldId, 1, 1);
            DamageSystem.Apply(state, new DamageRequest(null, u[0], 2, DamageType.Direct)); // 3 → 1
            EndTurn(state); EndTurn(state);

            Use(state, u[0], GardenerPiece.SingleShieldId, 1, 1);

            Assert.Equal(3, u[0].Stats.Shield);
            Assert.Single(u[0].Stats.Shields);
        }

        [Fact]
        public void SingleShield_AddsToShieldWithDifferentId()
        {
            var (state, u) = Game()
                .Place(Team.Player1, 1, 1, With(GardenerPiece.SingleShieldId))
                .Place(Team.Player2, 6, 6)
                .Start();
            DamageSystem.AddShield(state, u[0], ShieldReward.ShieldId, 3, source: null);

            Use(state, u[0], GardenerPiece.SingleShieldId, 1, 1);

            Assert.Equal(6, u[0].Stats.Shield);
            Assert.Equal(new[] { ShieldReward.ShieldId, GardenerPiece.SingleShieldId }, u[0].Stats.Shields.Select(s => s.Id));
        }

        #endregion
    }
}
