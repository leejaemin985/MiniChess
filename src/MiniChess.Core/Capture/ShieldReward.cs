using System;
using System.Linq;
using MiniChess.Core.Combat;
using MiniChess.Core.Common;
using MiniChess.Core.State;

namespace MiniChess.Core.Capture
{
    /// <summary>
    /// 점령 시점에 보드 위에 살아 있는 아군 유닛 모두에게 보호막을 준다. 보호막은 소모될 때까지 유지된다.
    /// </summary>
    public class ShieldReward : CaptureReward
    {
        public const string RewardId = "SHIELD";

        public int Amount { get; }

        public override string Id => RewardId;

        public ShieldReward(int amount)
        {
            if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));

            Amount = amount;
        }

        internal override void Grant(GameState state, Team team)
        {
            foreach (Unit unit in state.GetPlayer(team).Units.Where(u => u.IsAlive && u.IsPlaced).ToList())
                DamageSystem.AddShield(state, unit, Amount);
        }
    }
}
