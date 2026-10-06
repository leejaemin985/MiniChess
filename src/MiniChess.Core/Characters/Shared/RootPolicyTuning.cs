namespace MiniChess.Core.Characters.Shared
{
    /// <summary>속박 공통 정책(사슬 속박과 속박 덫이 공유).</summary>
    public class RootPolicyTuning
    {
        /// <summary>속박 중 외부 강제 이동(넉백/워프 등)도 막는지(명세 12.3).</summary>
        public bool? BlocksExternalMoves { get; set; }
    }
}
