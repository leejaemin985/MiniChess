using MiniChess.Core.Common;

namespace MiniChess.Core.Events
{
    public class TurnStartedEvent : GameEvent
    {
        public Team Team { get; }
        public int TurnNumber { get; }

        public TurnStartedEvent(Team team, int turnNumber)
        {
            Team = team;
            TurnNumber = turnNumber;
        }
    }

    public class TurnEndedEvent : GameEvent
    {
        public Team Team { get; }
        public int TurnNumber { get; }

        public TurnEndedEvent(Team team, int turnNumber)
        {
            Team = team;
            TurnNumber = turnNumber;
        }
    }

    public class GameEndedEvent : GameEvent
    {
        /// <summary>승리 팀. 무승부면 null.</summary>
        public Team? Winner { get; }

        public GameEndedEvent(Team? winner)
        {
            Winner = winner;
        }
    }
}
