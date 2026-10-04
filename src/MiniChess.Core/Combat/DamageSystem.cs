using System;
using System.Linq;
using MiniChess.Core.Events;
using MiniChess.Core.State;

namespace MiniChess.Core.Combat
{
    /// <summary>
    /// 모든 피해/회복의 단일 처리 경로.
    /// 처리 순서: 가로채기(Order 순) → HP 적용 → 이벤트 기록 → 사망·승패 판정.
    /// </summary>
    public static class DamageSystem
    {
        /// <summary>피해를 적용한다. 보드 위에 살아 있는 대상만 처리하며, 그 외에는 아무것도 하지 않는다.</summary>
        internal static DamageOutcome Apply(GameState state, DamageRequest request)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (request == null) throw new ArgumentNullException(nameof(request));

            Unit target = request.Target;
            if (!target.IsAlive || !target.IsPlaced)
                return DamageOutcome.None;

            int amount = request.Amount;
            foreach (IDamageInterceptor interceptor in state.DamageInterceptors.OrderBy(i => i.Order))
                amount = Math.Max(0, interceptor.Intercept(state, request, amount));

            int hpBefore = target.Stats.CurrentHp;
            target.Stats.ApplyDamage(amount);
            int applied = hpBefore - target.Stats.CurrentHp;

            state.Events.Record(new UnitDamagedEvent(
                request.Source, target, request.Type, request.Amount, applied, target.Stats.CurrentHp));

            bool killed = DeathSystem.HandleIfDead(state, target);
            return new DamageOutcome(applied, killed);
        }

        /// <summary>회복을 적용한다. 보드 위에 살아 있는 대상만 처리한다.</summary>
        internal static int Heal(GameState state, Unit source, Unit target, int amount)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (target == null) throw new ArgumentNullException(nameof(target));
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

            if (!target.IsAlive || !target.IsPlaced)
                return 0;

            int hpBefore = target.Stats.CurrentHp;
            target.Stats.Heal(amount);
            int applied = target.Stats.CurrentHp - hpBefore;

            state.Events.Record(new UnitHealedEvent(source, target, applied, target.Stats.CurrentHp));
            return applied;
        }
    }

    public readonly struct DamageOutcome
    {
        public static readonly DamageOutcome None = new DamageOutcome(0, false);

        /// <summary>실제로 HP 에서 깎인 양.</summary>
        public int AppliedAmount { get; }

        /// <summary>이 피해로 대상이 사망했는지.</summary>
        public bool Killed { get; }

        public DamageOutcome(int appliedAmount, bool killed)
        {
            AppliedAmount = appliedAmount;
            Killed = killed;
        }
    }
}
