using System;
using MiniChess.Core.State;

namespace MiniChess.Core.Combat
{
    /// <summary>피해 한 건의 요청. DamageSystem 으로만 처리한다.</summary>
    public class DamageRequest
    {
        /// <summary>피해를 주는 유닛. 출처가 유닛이 아니면(맵 효과 등) null.</summary>
        public Unit Source { get; }
        public Unit Target { get; }
        public int Amount { get; }
        public DamageType Type { get; }

        public DamageRequest(Unit source, Unit target, int amount, DamageType type)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

            Source = source;
            Target = target ?? throw new ArgumentNullException(nameof(target));
            Amount = amount;
            Type = type;
        }
    }
}
