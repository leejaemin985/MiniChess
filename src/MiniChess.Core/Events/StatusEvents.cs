using MiniChess.Core.Statuses;

namespace MiniChess.Core.Events
{
    /// <summary>상태효과가 새로 걸리거나 갱신되었다.</summary>
    public class StatusAppliedEvent : GameEvent
    {
        public StatusEffect Status { get; }

        /// <summary>기존 효과를 갱신한 것이면 true.</summary>
        public bool Refreshed { get; }

        public StatusAppliedEvent(StatusEffect status, bool refreshed)
        {
            Status = status;
            Refreshed = refreshed;
        }
    }

    public enum StatusRemoveReason
    {
        Expired,
        Replaced,
        TargetDied,

        /// <summary>상태를 건 유닛이 사망(RemoveOnSourceDeath 인 상태만).</summary>
        SourceDied,

        Removed,
    }

    /// <summary>상태효과가 제거되었다.</summary>
    public class StatusRemovedEvent : GameEvent
    {
        public StatusEffect Status { get; }
        public StatusRemoveReason Reason { get; }

        public StatusRemovedEvent(StatusEffect status, StatusRemoveReason reason)
        {
            Status = status;
            Reason = reason;
        }
    }
}
