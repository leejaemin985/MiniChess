using System.Collections.Generic;
using MiniChess.Core.Common;

namespace MiniChess.Core.Data
{
    /// <summary>
    /// 기획서 v0.7 기준 기본 데이터. 동작 확인용이며, 이후 JSON 등 외부 데이터로 대체한다.
    /// </summary>
    public static class DefaultGameData
    {
        public static GameRuleData CreateRules()
        {
            return new GameRuleData
            {
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
                Match = new MatchRuleData
                {
                    UnitsPerTeam = 4,
                    FirstTeam = Team.Player1,
                },
                Capture = new CaptureRuleData
                {
                    RequiredTurnEndCount = 2,
                    ActiveFromStart = true,
                },
            };
        }

        /// <summary>
        /// 기획서 3.1 테스트 맵. 벽 배치는 기획서 그대로이며,
        /// 시작 위치는 기획서에 없어 임의로 지정했다.
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

        /// <summary>팀별 초기 유닛 구성(유닛 Id 목록). 유닛 선택 기능이 생기기 전까지 하드코딩.</summary>
        public static string[] CreateStartingRoster()
        {
            return new[] { "warrior", "archer", "guardian", "assassin" };
        }

        /// <summary>임시 샘플 유닛. 능력치는 기획서에서 TBD 라 임의 값이다.</summary>
        public static List<UnitBaseStats> CreateSampleUnits()
        {
            return new List<UnitBaseStats>
            {
                new UnitBaseStats { Id = "warrior",  Name = "Warrior",  MaxHp = 12, Attack = 3, AttackRange = 1 },
                new UnitBaseStats { Id = "archer",   Name = "Archer",   MaxHp = 8,  Attack = 2, AttackRange = 3 },
                new UnitBaseStats { Id = "guardian", Name = "Guardian", MaxHp = 16, Attack = 2, AttackRange = 1 },
                new UnitBaseStats { Id = "assassin", Name = "Assassin", MaxHp = 7,  Attack = 4, AttackRange = 1 },
            };
        }
    }
}
