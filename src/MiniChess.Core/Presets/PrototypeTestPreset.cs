using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Capture;
using MiniChess.Core.Characters;
using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.Statuses.Library;

namespace MiniChess.Core.Presets
{
    /// <summary>
    /// 플레이 테스트용 프리셋. 확정 밸런스가 아니다.
    ///   - 규칙/맵: 기획 v0.7 의 [프로토타입] 값
    ///   - 캐릭터 수치: 명세상 TBD 이며 이 프리셋에서만 임시로 채운다
    /// 임시로 정한 값과 근거는 저장소 루트의 ASSUMPTIONS.md 에 기록한다. 값을 바꾸면 그 문서도 함께 갱신한다.
    /// </summary>
    public static class PrototypeTestPreset
    {
        public static GameRuleData CreateRules()
        {
            return new GameRuleData
            {
                // [가정] 명세 v0.1 3.2 의 [프로토타입] 값(6/4/4)에서 플레이 테스트를 위해 12/8/8 로 변경. 나머지는 [프로토타입] 값
                Ap = new ApRuleData
                {
                    MaxAp = 12,
                    StartAp = 8,
                    TurnRecoveryAp = 8,
                    CarryOver = true,
                },
                ActionCost = new ActionCostData
                {
                    MoveCostPerCell = 1,
                    BasicAttackCost = 2,
                },
                // [프로토타입] 명세 v0.1 3.1 (4 vs 4)
                Match = new MatchRuleData
                {
                    UnitsPerTeam = 4,
                    FirstTeam = Team.Player1,
                },
                // [프로토타입] 명세 v0.1 3.5. 판정 시점(자기 Turn Start)과 보상 종류는 사용자 확정, 보상 수치는 [가정]
                Capture = new CaptureRuleData
                {
                    RequiredTurnStartCount = 2,
                    ActiveFromStart = true,
                    RewardPool = new List<CaptureReward>
                    {
                        new ShieldReward(amount: 3),
                        new BasicAttackStatusReward(StatusLibrary.Burn(damagePerTrigger: 1, triggers: 2)),
                    },
                },
            };
        }

        /// <summary>
        /// 기획서 3.1 테스트 맵. 벽 배치는 기획서 그대로이며,
        /// 시작 위치는 기획서에 없어 임의로 지정했다. [가정]
        /// </summary>
        public static MapData CreateTestMap()
        {
            return new MapData
            {
                Name = "TestMap_7x7",
                Rows = new[]
                {
                    "2222222",
                    "2222222",
                    "...#...",
                    "#..C..#",
                    "...#...",
                    "1111111",
                    "1111111",
                },
            };
        }

        /// <summary>9개 캐릭터에 임시 수치를 채운 정의. [가정] ASSUMPTIONS.md 참고.</summary>
        public static List<CharacterDefinition> CreateCharacters()
        {
            var temporaryStats = new Dictionary<string, (int Hp, int Attack, int Range)>
            {
                { CharacterRoster.Scythe,     (10, 3, 1) },
                { CharacterRoster.Mortar,     (9,  2, 3) },
                { CharacterRoster.Gardener,   (16, 2, 1) },
                { CharacterRoster.Warrior,    (12, 3, 1) },
                { CharacterRoster.Archer,     (8,  2, 3) },
                { CharacterRoster.Chemist,    (9,  2, 2) },
                { CharacterRoster.Flame,      (9,  3, 2) },
                { CharacterRoster.Seamstress, (9,  2, 2) },
                { CharacterRoster.ChainGuard, (15, 2, 1) },
            };

            return CharacterRoster.Create()
                .Select(definition =>
                {
                    (int hp, int attack, int range) = temporaryStats[definition.Id];
                    definition.MaxHp = hp;
                    definition.Attack = attack;
                    definition.AttackRange = range;
                    return definition;
                })
                .ToList();
        }

        /// <summary>구현된 캐릭터 스킬을 임시 수치로 만든 스킬 목록.</summary>
        public static SkillCatalog CreateSkills()
        {
            return CharacterSkills.CreateCatalog(CreateSkillTuning());
        }

        /// <summary>스킬 임시 수치. [가정] 모두 명세상 TBD. ASSUMPTIONS.md 참고.</summary>
        public static SkillTuning CreateSkillTuning()
        {
            var t = new SkillTuning();

            t.Root.BlocksExternalMoves = false;

            t.WarriorSmash.ApCost = 3;
            t.WarriorSmash.Damage = 5;

            t.ArcherAimedShot.ApCost = 3;
            t.ArcherAimedShot.Range = 5;
            t.ArcherAimedShot.Damage = 3;

            t.FlameCompressedShell.ApCost = 3;
            t.FlameCompressedShell.Range = 3;
            t.FlameCompressedShell.ImpactDamage = 1;
            t.FlameCompressedShell.BurnDamage = 1;
            t.FlameCompressedShell.BurnTriggers = 2;

            t.ChemistRootTrap.ApCost = 3;
            t.ChemistRootTrap.Range = 2;
            t.ChemistRootTrap.Damage = 1;
            t.ChemistRootTrap.RootTurns = 1;
            t.ChemistRootTrap.MaxActive = 2;
            t.ChemistRootTrap.Lifetime = 3;
            t.ChemistRootTrap.SingleUse = true;

            t.ChemistPoisonGas.ApCost = 3;
            t.ChemistPoisonGas.Range = 3;
            t.ChemistPoisonGas.Width = 2;
            t.ChemistPoisonGas.Height = 2;
            t.ChemistPoisonGas.Damage = 1;
            t.ChemistPoisonGas.Lifetime = 2;
            t.ChemistPoisonGas.MaxActive = 1;

            t.ChainGuardChainBind.ApCost = 3;
            t.ChainGuardChainBind.Range = 2;
            t.ChainGuardChainBind.Damage = 0;

            t.GardenerHealingMeadow.ApCost = 4;
            t.GardenerHealingMeadow.Range = 2;
            t.GardenerHealingMeadow.Radius = 1;
            t.GardenerHealingMeadow.Heal = 1;
            t.GardenerHealingMeadow.Lifetime = 2;

            return t;
        }

        /// <summary>QuickBattle 에서 양 팀이 쓰는 4인 구성. [가정] 기존 샘플(전사/궁수/수호/암살)과 비슷한 역할 분포.</summary>
        public static string[] CreateStartingLineup()
        {
            return new[]
            {
                CharacterRoster.Warrior,
                CharacterRoster.Archer,
                CharacterRoster.Gardener,
                CharacterRoster.Scythe,
            };
        }
    }
}
