using MiniChess.Core.State;

namespace MiniChess.Core.Actions
{
    public class AttackResult
    {
        public Unit Attacker { get; }
        public Unit Target { get; }
        public int Damage { get; }
        public bool TargetDied { get; }

        public AttackResult(Unit attacker, Unit target, int damage, bool targetDied)
        {
            Attacker = attacker;
            Target = target;
            Damage = damage;
            TargetDied = targetDied;
        }
    }
}
