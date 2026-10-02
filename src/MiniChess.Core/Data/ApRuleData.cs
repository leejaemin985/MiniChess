namespace MiniChess.Core.Data
{
    /// <summary>플레이어 공용 AP 의 보유/회복 규칙.</summary>
    public class ApRuleData
    {
        /// <summary>보유 가능한 최대 AP. 회복 시 초과분은 버린다.</summary>
        public int MaxAp { get; set; }

        /// <summary>첫 행동 가능 턴의 시작 보유량.</summary>
        public int StartAp { get; set; }

        /// <summary>자기 턴 시작 시 회복량.</summary>
        public int TurnRecoveryAp { get; set; }

        /// <summary>남은 AP 를 다음 자기 턴으로 이월할지.</summary>
        public bool CarryOver { get; set; }
    }
}
