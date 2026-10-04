using System.Collections.Generic;
using MiniChess.Core.Capture;

namespace MiniChess.Core.Data
{
    /// <summary>중앙 점령 목표 규칙. 판정 방식은 CaptureSystem 참고.</summary>
    public class CaptureRuleData
    {
        /// <summary>점령 칸 위에서 자기 턴 시작 판정을 연속으로 몇 번 만족해야 점령 완료인지.</summary>
        public int RequiredTurnStartCount { get; set; }

        /// <summary>게임 시작부터 점령 목표가 활성 상태인지.</summary>
        public bool ActiveFromStart { get; set; }

        /// <summary>점령 완료 시 이 중 하나를 무작위로 지급한다. 비어 있으면 보상 없이 점령만 완료된다.</summary>
        public List<CaptureReward> RewardPool { get; set; } = new List<CaptureReward>();
    }
}
