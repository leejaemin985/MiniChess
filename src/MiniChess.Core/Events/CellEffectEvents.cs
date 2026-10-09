using MiniChess.Core.Common;
using MiniChess.Core.Effects;
using MiniChess.Core.Effects.Zones;
using MiniChess.Core.State;

namespace MiniChess.Core.Events
{
    public class CellEffectPlacedEvent : GameEvent
    {
        public Position Position { get; }
        public ICellEffect Effect { get; }

        public CellEffectPlacedEvent(Position position, ICellEffect effect)
        {
            Position = position;
            Effect = effect;
        }
    }

    public enum CellEffectRemoveReason
    {
        /// <summary>같은 레이어에 새 효과가 설치되어 덮어써짐.</summary>
        Replaced,

        /// <summary>수명 종료.</summary>
        Expired,

        /// <summary>발동 후 소모(일회용 덫 등).</summary>
        Consumed,

        /// <summary>공격으로 파괴(장애물).</summary>
        Destroyed,

        Removed,
    }

    /// <summary>장애물이 기본 공격을 받았다. 남은 횟수가 0 이면 이어서 파괴(제거) 이벤트가 기록된다.</summary>
    public class ObstacleHitEvent : GameEvent
    {
        public Unit Attacker { get; }
        public Position Position { get; }
        public Obstacle Obstacle { get; }
        public int HitsRemaining { get; }

        public ObstacleHitEvent(Unit attacker, Position position, Obstacle obstacle, int hitsRemaining)
        {
            Attacker = attacker;
            Position = position;
            Obstacle = obstacle;
            HitsRemaining = hitsRemaining;
        }
    }

    public class CellEffectRemovedEvent : GameEvent
    {
        public Position Position { get; }
        public ICellEffect Effect { get; }
        public CellEffectRemoveReason Reason { get; }

        public CellEffectRemovedEvent(Position position, ICellEffect effect, CellEffectRemoveReason reason)
        {
            Position = position;
            Effect = effect;
            Reason = reason;
        }
    }
}
