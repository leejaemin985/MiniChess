using MiniChess.Core.Common;
using MiniChess.Core.Effects;

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

        Removed,
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
