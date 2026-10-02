using System;
using System.Collections.Generic;
using MiniChess.Core.Common;
using MiniChess.Core.Data;

namespace MiniChess.Core.State
{
    /// <summary>한 경기의 전체 상태. 단계/턴 진행 등 동작은 별도 시스템이 담당한다.</summary>
    public class GameState
    {
        private readonly Dictionary<Team, PlayerState> _players;
        private int _nextUnitId = 1;

        public GameRuleData Rules { get; }
        public Board Board { get; }

        public GamePhase Phase { get; internal set; }

        /// <summary>현재 턴을 진행 중인 팀. Battle 단계에서만 의미가 있다.</summary>
        public Team CurrentTeam { get; internal set; }

        /// <summary>
        /// 경기 전체 턴 번호. Battle 시작 시 1 이 되며 한 플레이어의 턴이 끝날 때마다 1 증가.
        /// Battle 이전에는 0.
        /// </summary>
        public int TurnNumber { get; internal set; }

        /// <summary>승리한 팀. 경기가 끝나지 않았으면 null.</summary>
        public Team? Winner { get; internal set; }

        public bool IsGameOver => Phase == GamePhase.Ended;
        public PlayerState CurrentPlayer => GetPlayer(CurrentTeam);

        public GameState(GameRuleData rules, MapData map)
        {
            Rules = rules ?? throw new ArgumentNullException(nameof(rules));
            Board = new Board(map);

            _players = new Dictionary<Team, PlayerState>
            {
                { Team.Player1, CreatePlayer(Team.Player1, rules.Ap) },
                { Team.Player2, CreatePlayer(Team.Player2, rules.Ap) },
            };

            Phase = GamePhase.Setup;
            CurrentTeam = rules.Match.FirstTeam;
            TurnNumber = 0;
        }

        public PlayerState GetPlayer(Team team)
        {
            return _players[team];
        }

        /// <summary>새 유닛을 만들어 해당 팀 플레이어에 추가한다. 보드 배치는 별도로 한다.</summary>
        internal Unit CreateUnit(Team team, IReadOnlyUnitBaseStats baseStats)
        {
            var unit = new Unit(_nextUnitId++, team, baseStats);
            GetPlayer(team).AddUnit(unit);
            return unit;
        }

        /// <summary>게임 시작 시 AP 는 StartAp 만 적용한다. 회복은 각 플레이어의 다음 턴부터.</summary>
        private static PlayerState CreatePlayer(Team team, ApRuleData apRule)
        {
            var ap = new ApPool(apRule.MaxAp, apRule.StartAp);
            return new PlayerState(team, ap);
        }
    }
}
