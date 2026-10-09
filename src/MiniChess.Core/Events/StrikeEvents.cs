using MiniChess.Core.Effects.Scheduled;

namespace MiniChess.Core.Events
{
    /// <summary>포격이 조준(예약)되었다. 표시 계층이 공개 여부를 정한다.</summary>
    public class StrikeScheduledEvent : GameEvent
    {
        public ScheduledStrike Strike { get; }

        public StrikeScheduledEvent(ScheduledStrike strike)
        {
            Strike = strike;
        }
    }

    /// <summary>예약 포격이 착탄했다. 이어서 범위 안 적의 피격 이벤트가 기록된다.</summary>
    public class StrikeLandedEvent : GameEvent
    {
        public ScheduledStrike Strike { get; }

        public StrikeLandedEvent(ScheduledStrike strike)
        {
            Strike = strike;
        }
    }

    /// <summary>시전자 사망으로 예약 포격이 취소되었다.</summary>
    public class StrikeCancelledEvent : GameEvent
    {
        public ScheduledStrike Strike { get; }

        public StrikeCancelledEvent(ScheduledStrike strike)
        {
            Strike = strike;
        }
    }
}
