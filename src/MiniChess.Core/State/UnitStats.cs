using System;
using MiniChess.Core.Data;

namespace MiniChess.Core.State
{
    /// <summary>
    /// 유닛의 스탯. 기본값(Base) 참조 + 게임 중 변하는 값(현재 HP 등)을 함께 관리한다.
    /// 외부에서는 Base 대신 이 클래스의 값을 읽는다(추후 버프/디버프가 여기서 반영됨).
    /// </summary>
    public class UnitStats
    {
        /// <summary>기본 능력치(같은 종류 유닛끼리 공유).</summary>
        public IReadOnlyUnitBaseStats Base { get; }

        public int CurrentHp { get; private set; }

        public int MaxHp => Base.MaxHp;
        public int Attack => Base.Attack;
        public int AttackRange => Base.AttackRange;

        /// <summary>남은 보호막. HP 보다 먼저 피해를 흡수하며, 소모될 때까지 유지된다.</summary>
        public int Shield { get; private set; }

        public bool IsAlive => CurrentHp > 0;

        public UnitStats(IReadOnlyUnitBaseStats baseStats)
        {
            Base = baseStats ?? throw new ArgumentNullException(nameof(baseStats));
            CurrentHp = MaxHp;
        }

        /// <summary>피해를 적용한다. HP 는 0 아래로 내려가지 않는다.</summary>
        internal void ApplyDamage(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

            CurrentHp = Math.Max(0, CurrentHp - amount);
        }

        internal void AddShield(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

            Shield += amount;
        }

        /// <summary>피해를 보호막으로 흡수한다. 흡수한 양을 반환한다.</summary>
        internal int AbsorbWithShield(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

            int absorbed = Math.Min(Shield, amount);
            Shield -= absorbed;
            return absorbed;
        }

        /// <summary>회복을 적용한다. HP 는 MaxHp 를 넘지 않는다.</summary>
        internal void Heal(int amount)
        {
            if (amount < 0) throw new ArgumentOutOfRangeException(nameof(amount));

            CurrentHp = Math.Min(MaxHp, CurrentHp + amount);
        }
    }
}
