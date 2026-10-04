using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.Statuses;

namespace MiniChess.Core.State
{
    public class Unit
    {
        private readonly List<StatusEffect> _statuses = new List<StatusEffect>();

        public int Id { get; }
        public Team Team { get; }
        public UnitStats Stats { get; }

        /// <summary>보드에 배치되지 않았으면 null. 변경은 Board 를 통해서만 한다.</summary>
        public Position? Position { get; internal set; }

        /// <summary>자기 소유자의 이번 턴 행동 기록(전투 행동 사용, 이동 잠금 등).</summary>
        public UnitTurnState TurnState { get; } = new UnitTurnState();

        /// <summary>소환물(분신 등)인지. 소환 시스템이 생기기 전까지는 항상 false.</summary>
        public bool IsSummon { get; internal set; }

        public bool IsAlive => Stats.IsAlive;
        public bool IsPlaced => Position.HasValue;

        public Unit(int id, Team team, IReadOnlyUnitBaseStats baseStats)
        {
            Id = id;
            Team = team;
            Stats = new UnitStats(baseStats);
        }

        public bool HasSkill(string skillId)
        {
            return skillId != null && Stats.Base.SkillIds.Contains(skillId);
        }

        /// <summary>걸려 있는 상태효과(부여 순). 변경은 StatusSystem 을 통해서만 한다.</summary>
        public IReadOnlyList<StatusEffect> Statuses => _statuses;

        public StatusEffect FindStatus(string definitionId)
        {
            return _statuses.FirstOrDefault(status => status.Definition.Id == definitionId);
        }

        public bool HasStatus(StatusEffect status)
        {
            return _statuses.Contains(status);
        }

        internal void AddStatus(StatusEffect status)
        {
            _statuses.Add(status);
        }

        internal bool RemoveStatus(StatusEffect status)
        {
            return _statuses.Remove(status);
        }

        /// <summary>소유 플레이어의 턴 시작 시 호출한다.</summary>
        internal void ResetTurnState()
        {
            TurnState.Reset();
        }
    }
}
