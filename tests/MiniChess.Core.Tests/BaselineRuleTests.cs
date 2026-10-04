using MiniChess.Core.Actions;
using MiniChess.Core.Common;
using MiniChess.Core.State;
using MiniChess.Core.Tests.Support;
using Xunit;

namespace MiniChess.Core.Tests
{
    /// <summary>기존(v0.7 프로토타입) 이동/공격/턴 동작을 고정하는 테스트. 리팩터링 중 회귀 확인용.</summary>
    public class BaselineRuleTests
    {
        #region Move

        [Fact]
        public void Move_StraightLine_SpendsApPerCell()
        {
            var (state, u) = new TestGame().Place(Team.Player1, 0, 0).Start();

            new MoveAction(u[0], new Position(0, 3)).Execute(state);

            Assert.Equal(new Position(0, 3), u[0].Position);
            Assert.Equal(1, state.GetPlayer(Team.Player1).Ap.Current);
        }

        [Fact]
        public void Move_Diagonal_IsRejected()
        {
            var (state, u) = new TestGame().Place(Team.Player1, 0, 0).Start();

            Assert.Equal(MoveFailReason.NotStraightLine, new MoveAction(u[0], new Position(1, 1)).Validate(state));
        }

        [Fact]
        public void Move_ThroughWall_IsRejected()
        {
            var (state, u) = new TestGame()
                .WithMap(".......", ".......", ".......", ".......", ".......", "#......", ".......")
                .Place(Team.Player1, 0, 0)
                .Start();

            Assert.Equal(MoveFailReason.PathBlocked, new MoveAction(u[0], new Position(0, 2)).Validate(state));
        }

        [Fact]
        public void Move_ThroughUnit_IsRejected()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player1, 0, 1)
                .Start();

            Assert.Equal(MoveFailReason.PathBlocked, new MoveAction(u[0], new Position(0, 2)).Validate(state));
        }

        [Fact]
        public void Move_BeyondAp_IsRejected()
        {
            var (state, u) = new TestGame().Place(Team.Player1, 0, 0).Start();

            Assert.Equal(MoveFailReason.NotEnoughAp, new MoveAction(u[0], new Position(0, 5)).Validate(state));
        }

        #endregion

        #region Attack

        [Fact]
        public void MoveThenAttack_IsAllowed()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player2, 0, 3)
                .Start();

            new MoveAction(u[0], new Position(0, 2)).Execute(state);

            Assert.Equal(AttackFailReason.None, new AttackAction(u[0], u[1]).Validate(state));
        }

        [Fact]
        public void AttackThenMove_IsRejected()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player2, 0, 1)
                .Start();

            new AttackAction(u[0], u[1]).Execute(state);

            Assert.Equal(MoveFailReason.AlreadyActed, new MoveAction(u[0], new Position(1, 0)).Validate(state));
        }

        [Fact]
        public void Attack_SecondTimeSameTurn_IsRejected()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player2, 0, 1, TestGame.Stats(hp: 20))
                .Start();

            new AttackAction(u[0], u[1]).Execute(state);

            Assert.Equal(AttackFailReason.AlreadyActed, new AttackAction(u[0], u[1]).Validate(state));
        }

        [Fact]
        public void Attack_DiagonalWithinChebyshevRange_IsAllowed()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player2, 1, 1)
                .Start();

            Assert.Equal(AttackFailReason.None, new AttackAction(u[0], u[1]).Validate(state));
        }

        [Fact]
        public void Attack_OverWall_IsAllowed()
        {
            var (state, u) = new TestGame()
                .WithMap(".......", ".......", ".......", ".......", ".......", "#......", ".......")
                .Place(Team.Player1, 0, 0, TestGame.Stats(range: 2))
                .Place(Team.Player2, 0, 2)
                .Start();

            Assert.Equal(AttackFailReason.None, new AttackAction(u[0], u[1]).Validate(state));
        }

        [Fact]
        public void Attack_OutOfRange_IsRejected()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player2, 0, 2)
                .Start();

            Assert.Equal(AttackFailReason.OutOfRange, new AttackAction(u[0], u[1]).Validate(state));
        }

        [Fact]
        public void Attack_DealsDamageAndSpendsAp()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0, TestGame.Stats(attack: 3))
                .Place(Team.Player2, 0, 1, TestGame.Stats(hp: 10))
                .Start();

            AttackResult result = new AttackAction(u[0], u[1]).Execute(state);

            Assert.Equal(3, result.Damage);
            Assert.Equal(7, u[1].Stats.CurrentHp);
            Assert.Equal(2, state.GetPlayer(Team.Player1).Ap.Current);
        }

        [Fact]
        public void Attack_KillingLastEnemy_EndsGame()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0, TestGame.Stats(attack: 5))
                .Place(Team.Player2, 0, 1, TestGame.Stats(hp: 5))
                .Start();

            AttackResult result = new AttackAction(u[0], u[1]).Execute(state);

            Assert.True(result.TargetDied);
            Assert.False(u[1].IsPlaced);
            Assert.True(state.IsGameOver);
            Assert.Equal(Team.Player1, state.Winner);
        }

        #endregion

        #region Turn

        [Fact]
        public void FirstTurn_UsesStartApOnly_ThenRecoversFromSecondTurn()
        {
            var (state, _) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player2, 6, 6)
                .Start();

            Assert.Equal(4, state.GetPlayer(Team.Player1).Ap.Current);

            new EndTurnAction(Team.Player1).Execute(state);
            Assert.Equal(4, state.GetPlayer(Team.Player2).Ap.Current);

            new EndTurnAction(Team.Player2).Execute(state);
            Assert.Equal(6, state.GetPlayer(Team.Player1).Ap.Current); // 4 + 4, 최대 6
        }

        [Fact]
        public void EndTurn_ByWrongTeam_IsRejected()
        {
            var (state, _) = new TestGame().Place(Team.Player1, 0, 0).Place(Team.Player2, 6, 6).Start();

            Assert.Equal(EndTurnFailReason.NotYourTurn, new EndTurnAction(Team.Player2).Validate(state));
        }

        [Fact]
        public void NextOwnTurn_AllowsActingAgain()
        {
            var (state, u) = new TestGame()
                .Place(Team.Player1, 0, 0)
                .Place(Team.Player2, 0, 1, TestGame.Stats(hp: 20))
                .Start();

            new AttackAction(u[0], u[1]).Execute(state);
            new EndTurnAction(Team.Player1).Execute(state);
            new EndTurnAction(Team.Player2).Execute(state);

            Assert.Equal(AttackFailReason.None, new AttackAction(u[0], u[1]).Validate(state));
        }

        #endregion
    }
}
