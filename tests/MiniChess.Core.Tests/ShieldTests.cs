using MiniChess.Core.Actions;
using MiniChess.Core.Combat;
using MiniChess.Core.Common;
using MiniChess.Core.Events;
using MiniChess.Core.State;
using MiniChess.Core.Tests.Support;
using Xunit;

namespace MiniChess.Core.Tests
{
    /// <summary>
    /// Id 로 구분되는 보호막: 다른 Id 는 합산, 같은 Id 는 재충전(중첩 없음), 흡수는 먼저 받은 것부터, 소멸 기한 없음.
    /// </summary>
    public class ShieldTests
    {
        private static (GameState State, Unit[] Units) Game(int attack = 3, int hp = 10)
        {
            return new TestGame()
                .Place(Team.Player1, 0, 0, TestGame.Stats(attack: attack))
                .Place(Team.Player2, 0, 1, TestGame.Stats(hp: hp))
                .Start();
        }

        [Fact]
        public void DifferentIds_Coexist_AndSum()
        {
            var (state, u) = Game();

            DamageSystem.AddShield(state, u[1], "A", 2, source: null);
            DamageSystem.AddShield(state, u[1], "B", 3, source: u[0]);

            Assert.Equal(5, u[1].Stats.Shield);
            Assert.Equal(new[] { "A", "B" }, u[1].Stats.Shields.Select(s => s.Id));
            Assert.Same(u[0], u[1].Stats.FindShield("B").Source);
        }

        [Fact]
        public void SameId_Recharges_InsteadOfStacking_AndMovesToBackOfOrder()
        {
            var (state, u) = Game();
            DamageSystem.AddShield(state, u[1], "A", 3, source: null);
            DamageSystem.AddShield(state, u[1], "B", 3, source: null);
            u[1].Stats.AbsorbWithShields(2); // A: 3 → 1

            DamageSystem.AddShield(state, u[1], "A", 3, source: null);

            Assert.Equal(6, u[1].Stats.Shield);
            Assert.Equal(new[] { "B", "A" }, u[1].Stats.Shields.Select(s => s.Id));
            Assert.Equal(3, u[1].Stats.FindShield("A").Amount);
        }

        [Fact]
        public void Damage_IsAbsorbedByFirstReceivedShieldFirst_AndDepletedShieldIsRemoved()
        {
            var (state, u) = Game(attack: 3);
            DamageSystem.AddShield(state, u[1], "A", 2, source: null);
            DamageSystem.AddShield(state, u[1], "B", 5, source: null);

            AttackResult result = new AttackAction(u[0], u[1]).Execute(state);

            Assert.Null(u[1].Stats.FindShield("A"));
            Assert.Equal(4, u[1].Stats.FindShield("B").Amount);
            Assert.Equal(10, u[1].Stats.CurrentHp);

            var changes = result.Events.OfType<UnitShieldChangedEvent>().ToList();
            Assert.Equal(new[] { ("A", 0, 5), ("B", 4, 4) }, changes.Select(e => (e.ShieldId, e.AmountAfter, e.ShieldAfter)));
            Assert.Equal(3, result.Events.OfType<UnitDamagedEvent>().Single().ShieldAbsorbed);
        }

        [Fact]
        public void DamageBeyondAllShields_GoesToHp()
        {
            var (state, u) = Game(attack: 6);
            DamageSystem.AddShield(state, u[1], "A", 1, source: null);
            DamageSystem.AddShield(state, u[1], "B", 2, source: null);

            new AttackAction(u[0], u[1]).Execute(state);

            Assert.Empty(u[1].Stats.Shields);
            Assert.Equal(10 - 3, u[1].Stats.CurrentHp);
        }

        [Fact]
        public void Shield_HasNoExpiry_AcrossTurns()
        {
            var (state, u) = Game();
            DamageSystem.AddShield(state, u[1], "A", 3, source: null);

            for (int i = 0; i < 6; i++)
                new EndTurnAction(state.CurrentTeam).Execute(state);

            Assert.Equal(3, u[1].Stats.Shield);
        }

        [Fact]
        public void AddShield_RecordsChangedEventWithIdAndTotal()
        {
            var (state, u) = Game();
            DamageSystem.AddShield(state, u[1], "A", 2, source: null);
            int start = state.Events.Count;

            DamageSystem.AddShield(state, u[1], "B", 3, source: null);

            var changed = Assert.Single(state.Events.Since(start).OfType<UnitShieldChangedEvent>());
            Assert.Equal(("B", 3, 5), (changed.ShieldId, changed.AmountAfter, changed.ShieldAfter));
        }
    }
}
