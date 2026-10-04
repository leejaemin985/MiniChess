using System;
using System.Collections.Generic;
using System.Linq;
using MiniChess.Core.Common;
using MiniChess.Core.Events;
using MiniChess.Core.Movement;
using MiniChess.Core.State;
using MiniChess.Core.Turns;

namespace MiniChess.Core.Statuses
{
    /// <summary>상태효과의 부여/제거/턴 단계 처리.</summary>
    public static class StatusSystem
    {
        /// <summary>
        /// 대상에게 상태를 건다. 같은 Id 가 이미 있으면 정의의 StackPolicy 를 따른다.
        /// 적용(또는 갱신)된 효과를 반환하며, 무시되었거나 대상이 보드 위에 없으면 null.
        /// </summary>
        internal static StatusEffect Apply(GameState state, StatusDefinition definition, Unit target, Unit source, Team sourceTeam)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (definition == null) throw new ArgumentNullException(nameof(definition));
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (!target.IsAlive || !target.IsPlaced)
                return null;

            StatusEffect existing = target.FindStatus(definition.Id);
            if (existing != null)
            {
                switch (definition.StackPolicy)
                {
                    case StatusStackPolicy.Ignore:
                        return null;

                    case StatusStackPolicy.Refresh:
                        existing.Reapply(source, sourceTeam, state.TurnNumber);
                        state.Events.Record(new StatusAppliedEvent(existing, refreshed: true));
                        return existing;

                    case StatusStackPolicy.Replace:
                        Remove(state, existing, StatusRemoveReason.Replaced);
                        break;
                }
            }

            var status = new StatusEffect(definition, target, source, sourceTeam, state.TurnNumber);
            target.AddStatus(status);
            state.Events.Record(new StatusAppliedEvent(status, refreshed: false));
            definition.Behavior?.OnApplied(new StatusContext(state, status));
            return status;
        }

        internal static StatusEffect Apply(GameState state, StatusDefinition definition, Unit target, Unit source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source), "출처 유닛이 없으면 sourceTeam 을 직접 지정");

            return Apply(state, definition, target, source, source.Team);
        }

        internal static void Remove(GameState state, StatusEffect status, StatusRemoveReason reason)
        {
            if (!status.Target.RemoveStatus(status))
                return;

            state.Events.Record(new StatusRemovedEvent(status, reason));
            status.Definition.Behavior?.OnRemoved(new StatusContext(state, status));
        }

        /// <summary>사망한 유닛의 상태를 모두 제거한다.</summary>
        internal static void RemoveAll(GameState state, Unit unit, StatusRemoveReason reason)
        {
            foreach (StatusEffect status in unit.Statuses.ToList())
                Remove(state, status, reason);
        }

        /// <summary>
        /// 턴 단계 처리. 보드 위 유닛을 Id 순으로, 각 유닛의 상태를 부여 순으로 처리한다.
        /// 한 상태는 같은 단계에서 발동(Trigger) 후 감소(Decrement) 순서로 처리된다.
        /// [정책] 같은 단계 안의 처리 순서(유닛 Id → 부여 순)는 명세 TBD 에 대한 임시 결정이다.
        /// </summary>
        internal static void RunStep(TurnContext context)
        {
            GameState state = context.State;

            foreach (Unit unit in GetPlacedUnitsById(state))
            {
                foreach (StatusEffect status in unit.Statuses.ToList())
                {
                    if (state.IsGameOver)
                        return;

                    if (!unit.IsAlive)
                        break;

                    // 앞선 상태의 처리로 제거되었을 수 있다.
                    if (!unit.HasStatus(status))
                        continue;

                    if (status.Matches(status.Definition.Trigger, context))
                        status.Definition.Behavior?.OnTrigger(new StatusContext(state, status));

                    if (!unit.IsAlive || !unit.HasStatus(status))
                        continue;

                    if (status.Matches(status.Definition.Decrement, context))
                    {
                        status.Remaining--;
                        if (status.Remaining <= 0)
                            Remove(state, status, StatusRemoveReason.Expired);
                    }
                }
            }
        }

        /// <summary>대상 유닛에 걸린 상태들이 이 이동을 막는지. 처음으로 막는 이유를 반환한다.</summary>
        internal static MoveBlockReason CheckMove(GameState state, Unit mover, Unit initiator, MoveKind kind, int cells)
        {
            foreach (StatusEffect status in mover.Statuses)
            {
                StatusBehavior behavior = status.Definition.Behavior;
                if (behavior == null)
                    continue;

                MoveBlockReason reason = behavior.CheckMove(new StatusContext(state, status), initiator, kind, cells);
                if (reason != MoveBlockReason.None)
                    return reason;
            }

            return MoveBlockReason.None;
        }

        private static IEnumerable<Unit> GetPlacedUnitsById(GameState state)
        {
            return state.GetPlayer(Team.Player1).Units
                .Concat(state.GetPlayer(Team.Player2).Units)
                .Where(unit => unit.IsPlaced)
                .OrderBy(unit => unit.Id)
                .ToList();
        }
    }
}
