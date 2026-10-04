using System;

namespace MiniChess.Core.Statuses
{
    /// <summary>
    /// 상태효과 종류의 정의(데이터). 같은 정의를 여러 유닛의 효과 인스턴스가 공유한다.
    /// 무엇을 하는지는 Behavior 가, 언제 하고 언제 사라지는지는 Trigger/Decrement 가 정한다.
    /// </summary>
    public class StatusDefinition
    {
        public string Id { get; }

        /// <summary>남은 횟수의 초기값. Decrement 시점마다 1 줄고 0 이 되면 제거된다.</summary>
        public int Duration { get; }

        public StatusStackPolicy StackPolicy { get; }

        /// <summary>남은 횟수가 줄어드는 시점.</summary>
        public StatusTiming Decrement { get; }

        /// <summary>Behavior.OnTrigger 가 호출되는 시점. 반복 발동이 없는 상태면 null.</summary>
        public StatusTiming Trigger { get; }

        /// <summary>상태의 실제 동작. 표식처럼 존재 자체만 의미 있는 상태면 null.</summary>
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
