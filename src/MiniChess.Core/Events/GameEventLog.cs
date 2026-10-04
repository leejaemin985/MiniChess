using System;
using System.Collections.Generic;

namespace MiniChess.Core.Events
{
    /// <summary>경기 전체의 이벤트 기록. 추가만 가능하다.</summary>
    public class GameEventLog
    {
        private readonly List<GameEvent> _events = new List<GameEvent>();

        public int Count => _events.Count;
        public IReadOnlyList<GameEvent> All => _events;

        internal void Record(GameEvent gameEvent)
        {
            if (gameEvent == null) throw new ArgumentNullException(nameof(gameEvent));

            gameEvent.Sequence = _events.Count;
            _events.Add(gameEvent);
        }

        /// <summary>startIndex 부터 끝까지의 이벤트(한 행동이 만든 이벤트를 잘라낼 때 사용).</summary>
        public IReadOnlyList<GameEvent> Since(int startIndex)
        {
            if (startIndex < 0 || startIndex > _events.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            return _events.GetRange(startIndex, _events.Count - startIndex);
        }
    }
}
