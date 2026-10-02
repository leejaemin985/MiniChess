namespace MiniChess.Core.Data
{
    /// <summary>중앙 점령 목표 규칙.</summary>
    public class CaptureRuleData
    {
        /// <summary>점령 칸 위에서 자신의 Turn End 판정을 몇 번 만족해야 점령 완료인지.</summary>
        public int RequiredTurnEndCount { get; set; }

        /// <summary>게임 시작부터 점령 목표가 활성 상태인지.</summary>
        public bool ActiveFromStart { get; set; }
    }
}
