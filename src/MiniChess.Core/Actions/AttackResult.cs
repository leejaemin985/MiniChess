using System.Collections.Generic;
using MiniChess.Core.Events;
using MiniChess.Core.State;

namespace MiniChess.Core.Actions
{
    public class AttackResult
    {
        public Unit Attacker { get; }
        public Unit Target { get; }

        /// <summary>실제로 대상 HP 에서 깎인 양.</summary>
        public int Damage { get; }
        public bool TargetDied { get; }

        /// <summary>이 공격으로 발생한 이벤트(발생 순).</summary>
        public IReadOnlyList<GameEvent> Events { get; }

        public AttackResult(Unit attacker, Unit target, int damage, bool targetDied, IReadOnlyList<GameEvent> events)
        {
            Attacker = attacker;
            Target = target;
            Damage = damage;
            TargetDied = targetDied;
            Events = events;
        }
    }
}
