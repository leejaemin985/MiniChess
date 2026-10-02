namespace MiniChess.Core.Data
{
    /// <summary>모든 유닛에 공통인 행동의 AP 비용.</summary>
    public class ActionCostData
    {
        /// <summary>이동 1칸당 AP.</summary>
        public int MoveCostPerCell { get; set; }

        /// <summary>기본 공격 1회 AP.</summary>
        public int BasicAttackCost { get; set; }
    }
}
