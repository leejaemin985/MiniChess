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

        /// <summary>
        /// 이번 턴에 이동을 제외한 자발적 행동(기본 공격, 스킬 등)을 했는지.
        /// true 면 추가 행동과 자발적 이동이 불가하다. 넉백/워프 같은 강제 이동은 별개.
        /// </summary>
        public bool HasActed { get; private set; }

        public bool IsAlive => Stats.IsAlive;
        public bool IsPlaced => Position.HasValue;

        public Unit(int id, Team team, IReadOnlyUnitBaseStats baseStats)
        {
            Id = id;
            Team = team;
            Stats = new UnitStats(baseStats);
        }

        internal void MarkActed()
        {
            HasActed = true;
        }

        /// <summary>소유 플레이어의 턴 시작 시 호출한다.</summary>
        internal void ResetTurnState()
        {
            HasActed = false;
        }
    }
}
