using System;
using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.Skills.Areas;
using MiniChess.Core.Skills.Targeting;
using MiniChess.Core.State;
using MiniChess.Core.Tests.Support;
using Xunit;

namespace MiniChess.Core.Tests
{
    public class AreaTests
    {
        private static readonly string[] ForwardLine = { "#", "#", "o" };

        private static (GameState State, Unit Caster) Setup(int x = 3, int y = 3)
        {
            var (state, u) = new TestGame().Place(Team.Player1, x, y).Start();
            return (state, u[0]);
        }

        private static HashSet<Position> Cells(params (int X, int Y)[] cells)
        {
            return cells.Select(c => new Position(c.X, c.Y)).ToHashSet();
        }

        #region AreaShape

        [Fact]
        public void Parse_FirstRowIsUp_AndExcludedAnchorIsNotACell()
        {
            AreaShape shape = AreaShape.Parse(ForwardLine);

            Assert.Equal(new[] { (0, 2), (0, 1) }, shape.Offsets);
        }

        [Fact]
        public void Parse_IncludedAnchorIsACell()
        {
            AreaShape shape = AreaShape.Parse(new[] { "##", "@#" });

            Assert.Equal(new[] { (0, 1), (1, 1), (0, 0), (1, 0) }, shape.Offsets);
        }

        [Fact]
        public void Parse_NullIsUnset()
        {
            Assert.Null(AreaShape.Parse(null));
        }

        [Theory]
        [InlineData("###")]      // 기준점 없음
        [InlineData("@#@")]      // 기준점 둘
        [InlineData("#@|#")]     // 줄 길이 불일치
        [InlineData("#@x")]      // 알 수 없는 문자
        public void Parse_InvalidGrid_Throws(string rows)
        {
            Assert.Throws<ArgumentException>(() => AreaShape.Parse(rows.Split('|')));
        }

        #endregion

        #region PatternArea

        [Fact]
        public void TargetAnchor_SpreadsFromSelectedCell_AndClipsToBoard()
        {
            var (state, caster) = Setup();
            var area = new PatternArea(AreaShape.Parse(new[] { "###", "#@#", "###" }), AreaAnchor.Target);

            IReadOnlyList<Position> cells = area.GetCells(state, caster, new Position(0, 0));

            Assert.Equal(Cells((0, 1), (1, 1), (0, 0), (1, 0)), cells.ToHashSet());
        }

        [Fact]
        public void CasterAnchor_SpreadsFromCaster_IgnoringSelectedCell()
        {
            var (state, caster) = Setup();
            var area = new PatternArea(AreaShape.Parse(new[] { "#o#" }), AreaAnchor.Caster);

            IReadOnlyList<Position> cells = area.GetCells(state, caster, new Position(0, 0));

            Assert.Equal(Cells((2, 3), (4, 3)), cells.ToHashSet());
        }

        [Theory]
        [InlineData(3, 4, new[] { 3, 4, 3, 5 })]   // 위
        [InlineData(4, 3, new[] { 4, 3, 5, 3 })]   // 오른쪽
        [InlineData(3, 2, new[] { 3, 2, 3, 1 })]   // 아래
        [InlineData(2, 3, new[] { 2, 3, 1, 3 })]   // 왼쪽
        public void CasterAnchor_Rotate_FacesSelectedDirection(int targetX, int targetY, int[] expected)
        {
            var (state, caster) = Setup();
            var area = new PatternArea(AreaShape.Parse(ForwardLine), AreaAnchor.Caster, rotate: true);

            IReadOnlyList<Position> cells = area.GetCells(state, caster, new Position(targetX, targetY));

            Assert.Equal(Cells((expected[0], expected[1]), (expected[2], expected[3])), cells.ToHashSet());
        }

        [Fact]
        public void Rotate_WithTargetAnchor_Throws()
        {
            Assert.Throws<ArgumentException>(() => new PatternArea(AreaShape.SingleCell, AreaAnchor.Target, rotate: true));
        }

        [Fact]
        public void UnsetShape_IsReportedAsConfigIssue()
        {
            var issues = new List<string>();
            new PatternArea(null, AreaAnchor.Target).CollectConfigIssues("SKILL.Area", issues);

            Assert.Contains("SKILL.Area.Shape 미설정", issues);
        }

        #endregion

        #region DirectionTargeting

        [Fact]
        public void DirectionTargeting_OffersOrthogonalNeighborsOnBoard()
        {
            var (state, caster) = Setup(0, 0);

            var candidates = new DirectionTargeting().GetCandidates(state, caster).ToHashSet();

            Assert.Equal(Cells((0, 1), (1, 0)), candidates);
        }

        #endregion
    }
}
