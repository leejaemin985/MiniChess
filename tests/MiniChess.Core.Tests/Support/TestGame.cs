using MiniChess.Core.Common;
using MiniChess.Core.Data;
using MiniChess.Core.Skills;
using MiniChess.Core.State;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Tests.Support
{
    /// <summary>
    /// 테스트용 경기 구성 헬퍼. 맵 문자열과 유닛 배치를 직접 지정해 Battle 상태로 시작한다.
    /// 규칙 수치는 v0.7 프로토타입 값(AP 6/4/4, 이동 1, 기본 공격 2)으로 고정한다.
    /// </summary>
    public class TestGame
    {
        private readonly List<(Team Team, Position Position, UnitBaseStats Stats)> _placements = new();
        private string[] _rows = EmptyRows(7, 7);
        private GameRuleData _rules = CreateRules();
        private SkillCatalog _skills = new();

        public static GameRuleData CreateRules()
        {
            return new GameRuleData
            {
                Ap = new ApRuleData { MaxAp = 6, StartAp = 4, TurnRecoveryAp = 4, CarryOver = true },
                ActionCost = new ActionCostData { MoveCostPerCell = 1, BasicAttackCost = 2 },
                Match = new MatchRuleData { UnitsPerTeam = 4, FirstTeam = Team.Player1 },
                Capture = new CaptureRuleData { RequiredTurnStartCount = 2, ActiveFromStart = true },
            };
        }

        public static UnitBaseStats Stats(string id = "test", int hp = 10, int attack = 3, int range = 1, params string[] skills)
        {
            return new UnitBaseStats { Id = id, Name = id, MaxHp = hp, Attack = attack, AttackRange = range, SkillIds = skills };
        }

        public static string[] EmptyRows(int width, int height)
        {
            return Enumerable.Repeat(new string('.', width), height).ToArray();
        }

        /// <summary>맵 문자열. 첫 줄이 가장 위(y = Height - 1).</summary>
        public TestGame WithMap(params string[] rows)
        {
            _rows = rows;
            return this;
        }

        public TestGame WithSkills(params SkillDefinition[] skills)
        {
            foreach (SkillDefinition skill in skills)
                _skills.Add(skill);
            return this;
        }

        public TestGame WithRules(Action<GameRuleData> configure)
        {
            configure(_rules);
            return this;
        }

        public TestGame Place(Team team, int x, int y, UnitBaseStats stats = null)
        {
            _placements.Add((team, new Position(x, y), stats ?? Stats()));
            return this;
        }

        /// <summary>경기를 만들고 Battle 을 시작한다. 반환된 유닛 배열은 Place 호출 순서와 같다.</summary>
        public (GameState State, Unit[] Units) Start()
        {
            var state = new GameState(_rules, new MapData { Name = "Test", Rows = _rows }, _skills);
            var units = new Unit[_placements.Count];

            for (int i = 0; i < _placements.Count; i++)
            {
                var (team, position, stats) = _placements[i];
                units[i] = state.CreateUnit(team, stats);
                if (!state.Board.TrySpawn(units[i], position))
                    throw new InvalidOperationException($"테스트 배치 실패: {position}");
            }

            TurnSystem.StartBattle(state);
            return (state, units);
        }
    }
}
