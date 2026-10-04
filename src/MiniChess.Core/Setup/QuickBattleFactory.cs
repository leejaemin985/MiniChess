using System;
using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.Presets;
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
        /// <param name="characters">사용 가능한 캐릭터 정의.</param>
        /// <param name="lineup">양 팀이 공통으로 쓰는 캐릭터 Id 구성.</param>
        /// <exception cref="GameConfigException">구성에 쓰인 캐릭터의 필수 수치가 비어 있거나 Id 가 없음.</exception>
        public static GameState Create(
            GameRuleData rules,
            MapData map,
            IReadOnlyList<CharacterDefinition> characters,
            IReadOnlyList<string> lineup)
        {
            List<UnitBaseStats> lineupStats = ResolveLineup(characters, lineup);

            var state = new GameState(rules, map);

            SpawnTeam(state, Team.Player1, lineupStats);
            SpawnTeam(state, Team.Player2, lineupStats);

            TurnSystem.StartBattle(state);
            return state;
        }

        /// <summary>플레이 테스트 프리셋(PrototypeTestPreset)으로 생성한다.</summary>
        public static GameState CreateDefault()
        {
            return Create(
                PrototypeTestPreset.CreateRules(),
                PrototypeTestPreset.CreateTestMap(),
                PrototypeTestPreset.CreateCharacters(),
                PrototypeTestPreset.CreateStartingLineup());
        }

        /// <summary>구성의 모든 캐릭터를 확인하고, 문제를 한 번에 모아 보고한다.</summary>
        private static List<UnitBaseStats> ResolveLineup(IReadOnlyList<CharacterDefinition> characters, IReadOnlyList<string> lineup)
        {
            var issues = new List<string>();
            var resolved = new List<UnitBaseStats>();

            foreach (string id in lineup)
            {
                CharacterDefinition definition = characters.FirstOrDefault(c => c.Id == id);
                if (definition == null)
                {
                    issues.Add($"캐릭터 Id '{id}' 없음");
                    continue;
                }

                List<string> missing = definition.GetMissingFields();
                if (missing.Count > 0)
                {
                    issues.AddRange(missing.Select(field => $"{field} 미설정"));
                    continue;
                }

                resolved.Add(definition.ToBaseStats());
            }

            if (issues.Count > 0)
                throw new GameConfigException(issues.Distinct().ToList());

            return resolved;
        }

        private static void SpawnTeam(GameState state, Team team, IReadOnlyList<UnitBaseStats> lineupStats)
        {
            IReadOnlyList<Position> spawnPositions = state.Board.GetSpawnPositions(team);
            if (spawnPositions.Count < lineupStats.Count)
                throw new InvalidOperationException($"{team} 시작 위치 {spawnPositions.Count}칸 < 유닛 {lineupStats.Count}개");

            for (int i = 0; i < lineupStats.Count; i++)
            {
                Unit unit = state.CreateUnit(team, lineupStats[i]);
                state.Board.TrySpawn(unit, spawnPositions[i]);
            }
        }
    }
}
