using System;
using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;

namespace MiniChess.Core.State
{
    public class PlayerState
    {
        /// <summary>죽은 유닛도 포함한다(부활 대상 보존). 보드에서만 제거된다.</summary>
        private readonly List<Unit> _units = new List<Unit>();

        public Team Team { get; }

        /// <summary>이 플레이어의 모든 유닛이 공유하는 AP.</summary>
        public ApPool Ap { get; }

        /// <summary>이 플레이어의 턴이 시작된 횟수. 첫 턴 판정(AP 회복 생략)에 사용.</summary>
        public int TurnsStarted { get; internal set; }

        public IReadOnlyList<Unit> Units => _units;
        public IEnumerable<Unit> AliveUnits => _units.Where(unit => unit.IsAlive);

        /// <summary>유닛이 1개 이상 있고, 그중 살아 있는 유닛이 하나도 없으면 전멸.</summary>
        public bool IsEliminated => _units.Count > 0 && !_units.Any(unit => unit.IsAlive);

        public PlayerState(Team team, ApPool ap)
        {
            Team = team;
            Ap = ap ?? throw new ArgumentNullException(nameof(ap));
        }

        internal void AddUnit(Unit unit)
        {
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            if (unit.Team != Team)
                throw new ArgumentException($"유닛 {unit.Id} 의 팀({unit.Team})이 플레이어 팀({Team})과 다름", nameof(unit));

            _units.Add(unit);
        }
    }
}
