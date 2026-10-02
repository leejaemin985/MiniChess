using System;
using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.State;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Setup
{
    /// <summary>
    /// 배치 단계를 건너뛰고 바로 Battle 을 시작하는 테스트용 팩토리.
    /// 각 팀의 유닛을 시작 위치 후보 칸에 앞에서부터 순서대로 배치한다.
    /// </summary>
    public static class QuickBattleFactory
    {
        public static GameState Create(
            GameRuleData rules,
            MapData map,
            IReadOnlyList<IReadOnlyUnitBaseStats> unitCatalog,
            IReadOnlyList<string> roster)
        {
            var state = new GameState(rules, map);

            SpawnTeam(state, Team.Player1, unitCatalog, roster);
            SpawnTeam(state, Team.Player2, unitCatalog, roster);

            TurnSystem.StartBattle(state);
            return state;
        }

        /// <summary>기본 데이터(규칙, 테스트 맵, 샘플 유닛, 하드코딩 구성)로 생성한다.</summary>
        public static GameState CreateDefault()
        {
            return Create(
                DefaultGameData.CreateRules(),
                DefaultGameData.CreateTestMap(),
                DefaultGameData.CreateSampleUnits(),
                DefaultGameData.CreateStartingRoster());
        }

        private static void SpawnTeam(
            GameState state,
            Team team,
            IReadOnlyList<IReadOnlyUnitBaseStats> unitCatalog,
            IReadOnlyList<string> roster)
        {
            IReadOnlyList<Position> spawnPositions = state.Board.GetSpawnPositions(team);
            if (spawnPositions.Count < roster.Count)
                throw new InvalidOperationException($"{team} 시작 위치 {spawnPositions.Count}칸 < 유닛 {roster.Count}개");

            for (int i = 0; i < roster.Count; i++)
            {
                IReadOnlyUnitBaseStats baseStats = FindStats(unitCatalog, roster[i]);
                Unit unit = state.CreateUnit(team, baseStats);
                state.Board.TrySpawn(unit, spawnPositions[i]);
            }
        }

        private static IReadOnlyUnitBaseStats FindStats(IReadOnlyList<IReadOnlyUnitBaseStats> unitCatalog, string unitId)
        {
            IReadOnlyUnitBaseStats stats = unitCatalog.FirstOrDefault(s => s.Id == unitId);
            return stats ?? throw new InvalidOperationException($"unit id '{unitId}' 없음");
        }
    }
}
