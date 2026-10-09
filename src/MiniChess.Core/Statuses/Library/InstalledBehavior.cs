using MiniChess.Core.Combat;
using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Statuses.Library
{
    /// <summary>
    /// 설치: 자발적 이동과 기본 공격을 막는다. 외부 강제 이동(워프 교환 등)은 막지 않는다.
    /// 걸릴 때 보호막을 주고, 풀릴 때(해체) 그 보호막을 제거한다.
    /// </summary>
    public class InstalledBehavior : StatusBehavior
    {
        public string ShieldId { get; }
        public int ShieldAmount { get; }

        public InstalledBehavior(string shieldId, int shieldAmount)
        {
            ShieldId = shieldId;
            ShieldAmount = shieldAmount;
        }

        public override void OnApplied(StatusContext context)
        {
            DamageSystem.AddShield(context.State, context.Target, ShieldId, ShieldAmount, context.Target);
        }

        public override void OnRemoved(StatusContext context)
        {
            DamageSystem.RemoveShield(context.State, context.Target, ShieldId);
        }

        public override bool BlocksBasicAttack(StatusContext context)
        {
            return true;
        }

        public override MoveBlockReason CheckMove(StatusContext context, Unit initiator, MoveKind kind, int cells)
        {
            return MovementControl.IsVoluntary(context.Target, initiator) ? MoveBlockReason.Installed : MoveBlockReason.None;
        }
    }
}
