using System;

namespace MiniChess.Core.Statuses
{
    /// <summary>
    /// 상태 종류의 정의(불변 데이터). 같은 정의를 여러 StatusEffect 가 공유하므로
    /// Behavior 에 유닛별 진행 값을 저장하면 안 된다.
    /// </summary>
    public class StatusDefinition
    {
        public string Id { get; }

        /// <summary>지속 횟수. 걸리거나 갱신될 때 StatusEffect.Remaining 의 시작값이 된다.</summary>
        public int Duration { get; }

        public StatusStackPolicy StackPolicy { get; }

        /// <summary>Remaining 이 1 줄어드는 시점.</summary>
        public StatusTiming Decrement { get; }

        /// <summary>Behavior.OnTrigger 가 호출되는 시점. 반복 발동이 없으면 null.</summary>
        public StatusTiming Trigger { get; }

        /// <summary>실제 동작. 표식처럼 존재만 의미 있는 상태면 null.</summary>
        public StatusBehavior Behavior { get; }

        public StatusDefinition(
            string id,
            int duration,
            StatusStackPolicy stackPolicy,
            StatusTiming decrement,
            StatusTiming trigger = null,
            StatusBehavior behavior = null)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("상태 Id 가 비어 있음", nameof(id));
            if (duration <= 0) throw new ArgumentOutOfRangeException(nameof(duration), $"상태 '{id}': Duration 은 1 이상");

            Id = id;
            Duration = duration;
            StackPolicy = stackPolicy;
            Decrement = decrement ?? throw new ArgumentNullException(nameof(decrement));
            Trigger = trigger;
            Behavior = behavior;
        }
    }
}
