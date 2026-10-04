using System;
using MiniChess.Core.Common;
using MiniChess.Core.State;
using MiniChess.Core.Statuses;

namespace MiniChess.Core.Capture
{
    /// <summary>
    /// 이후 경기 동안 그 팀의 기본 공격이 적중하면 대상에게 상태를 건다(예: 기본 공격에 화상 추가).
    /// </summary>
    public class BasicAttackStatusReward : CaptureReward
    {
        public StatusDefinition Status { get; }

        /// <summary>보상 식별자는 부여하는 상태의 Id 를 따른다(예: "ATTACK_BURN").</summary>
        public override string Id => $"ATTACK_{Status.Id}";

        public BasicAttackStatusReward(StatusDefinition status)
        {
            Status = status ?? throw new ArgumentNullException(nameof(status));
        }

        internal override void Grant(GameState state, Team team)
        {
            state.GetPlayer(team).AddBasicAttackStatus(Status);
        }
    }
}
