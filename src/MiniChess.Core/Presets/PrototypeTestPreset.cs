using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;

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
                // [프로토타입] 명세 v0.1 3.2
                Ap = new ApRuleData
                {
                    MaxAp = 6,
                    StartAp = 4,
                    TurnRecoveryAp = 4,
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
                // [프로토타입] 명세 v0.1 3.5
                Capture = new CaptureRuleData
                {
                    RequiredTurnEndCount = 2,
                    ActiveFromStart = true,
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
                    "##.#.##",
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

        /// <summary>
        /// 플레이 테스트용 스킬 정의. 캐릭터 스킬은 구현되는 대로 여기에 추가하고,
        /// CreateCharacters 에서 해당 캐릭터의 SkillSlots 에 연결한다.
        /// </summary>
        public static SkillCatalog CreateSkills()
        {
            return new SkillCatalog();
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
