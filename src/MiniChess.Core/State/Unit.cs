using MiniChess.Core.Common;
using MiniChess.Core.Data;

namespace MiniChess.Core.State
{
    public class Unit
    {
        public int Id { get; }
        public Team Team { get; }
        public UnitStats Stats { get; }

        /// <summary>보드에 배치되지 않았으면 null. 변경은 Board 를 통해서만 한다.</summary>
        public Position? Position { get; internal set; }

        /// <summary>자기 소유자의 이번 턴 행동 기록(전투 행동 사용, 이동 잠금 등).</summary>
        public UnitTurnState TurnState { get; } = new UnitTurnState();

        public bool IsAlive => Stats.IsAlive;
        public bool IsPlaced => Position.HasValue;

        public Unit(int id, Team team, IReadOnlyUnitBaseStats baseStats)
        {
            Id = id;
            Team = team;
            Stats = new UnitStats(baseStats);
        }

        /// <summary>소유 플레이어의 턴 시작 시 호출한다.</summary>
        internal void ResetTurnState()
        {
            TurnState.Reset();
        }
    }
}
