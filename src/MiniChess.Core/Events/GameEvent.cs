namespace MiniChess.Core.Events
{
    /// <summary>
    /// 코어 상태 변화 한 건의 기록. 발생 순서대로 GameEventLog 에 쌓인다.
    /// 코어는 기록만 하며, 표현 계층은 이를 읽어 연출을 재생할 수 있다.
    /// </summary>
    public abstract class GameEvent
    {
        /// <summary>경기 내 발생 순번(0부터). 로그에 기록될 때 정해진다.</summary>
        public int Sequence { get; internal set; } = -1;
    }
}
