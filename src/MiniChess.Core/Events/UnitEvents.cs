using MiniChess.Core.Combat;
using MiniChess.Core.Common;
using MiniChess.Core.Movement;
using MiniChess.Core.State;

namespace MiniChess.Core.Events
{
    /// <summary>유닛이 한 칸 이동했다. 여러 칸 이동은 칸마다 기록된다(워프는 한 번).</summary>
    public class UnitMovedEvent : GameEvent
    {
        public Unit Unit { get; }
        public Position From { get; }
        public Position To { get; }
        public MoveKind Kind { get; }

        /// <summary>이동을 일으킨 유닛. 출처가 유닛이 아니면 null.</summary>
        public Unit Initiator { get; }

        public bool IsVoluntary => MovementControl.IsVoluntary(Unit, Initiator);

        public UnitMovedEvent(Unit unit, Position from, Position to, MoveKind kind, Unit initiator)
        {
            Unit = unit;
            From = from;
            To = to;
            Kind = kind;
            Initiator = initiator;
        }
    }

    /// <summary>유닛이 피해를 받았다.</summary>
    public class UnitDamagedEvent : GameEvent
    {
        /// <summary>피해를 준 유닛. 출처가 유닛이 아니면 null.</summary>
        public Unit Source { get; }
        public Unit Target { get; }
        public DamageType Type { get; }

        /// <summary>요청된 피해량(가로채기 전).</summary>
        public int RequestedAmount { get; }

        /// <summary>실제로 HP 에서 깎인 양.</summary>
        public int AppliedAmount { get; }

        public int HpAfter { get; }

        /// <summary>보호막이 흡수한 양.</summary>
        public int ShieldAbsorbed { get; }

        public UnitDamagedEvent(
            Unit source, Unit target, DamageType type, int requestedAmount, int appliedAmount, int hpAfter, int shieldAbsorbed = 0)
        {
            Source = source;
            Target = target;
            Type = type;
            RequestedAmount = requestedAmount;
            AppliedAmount = appliedAmount;
            HpAfter = hpAfter;
            ShieldAbsorbed = shieldAbsorbed;
        }
    }

    /// <summary>유닛이 회복했다.</summary>
    public class UnitHealedEvent : GameEvent
    {
        public Unit Source { get; }
        public Unit Target { get; }
        public int AppliedAmount { get; }
        public int HpAfter { get; }

        public UnitHealedEvent(Unit source, Unit target, int appliedAmount, int hpAfter)
        {
            Source = source;
            Target = target;
            AppliedAmount = appliedAmount;
            HpAfter = hpAfter;
        }
    }

    /// <summary>유닛이 사망해 보드에서 제거되었다.</summary>
    public class UnitDiedEvent : GameEvent
    {
        public Unit Unit { get; }

        /// <summary>사망한 칸.</summary>
        public Position Position { get; }

        public UnitDiedEvent(Unit unit, Position position)
        {
            Unit = unit;
            Position = position;
        }
    }

    /// <summary>유닛의 보호막 하나(Id 기준)의 양이 바뀌었다(부여/재충전 또는 피해 흡수). 0 이면 그 보호막은 사라졌다.</summary>
    public class UnitShieldChangedEvent : GameEvent
    {
        public Unit Unit { get; }

        /// <summary>바뀐 보호막의 Id.</summary>
        public string ShieldId { get; }

        /// <summary>이 Id 보호막의 바뀐 뒤 양.</summary>
        public int AmountAfter { get; }

        /// <summary>유닛의 모든 보호막 합(바뀐 뒤).</summary>
        public int ShieldAfter { get; }

        public UnitShieldChangedEvent(Unit unit, string shieldId, int amountAfter, int shieldAfter)
        {
            Unit = unit;
            ShieldId = shieldId;
            AmountAfter = amountAfter;
            ShieldAfter = shieldAfter;
        }
    }
}
