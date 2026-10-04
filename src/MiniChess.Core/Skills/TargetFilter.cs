using System;
using MiniChess.Core.State;

namespace MiniChess.Core.Skills
{
    /// <summary>시전자 기준으로 어떤 유닛을 대상으로 삼는지.</summary>
    public enum TargetFilter
    {
        Enemy,
        Ally,

        /// <summary>자신을 포함한 아군.</summary>
        AllyOrSelf,

        Self,

        /// <summary>자신을 포함한 모든 유닛.</summary>
        Any,
    }

    public static class TargetFilterExtensions
    {
        public static bool Matches(this TargetFilter filter, Unit caster, Unit unit)
        {
            if (unit == null)
                return false;

            bool isSelf = ReferenceEquals(caster, unit);
            bool isAlly = unit.Team == caster.Team;

            switch (filter)
            {
                case TargetFilter.Enemy: return !isAlly;
                case TargetFilter.Ally: return isAlly && !isSelf;
                case TargetFilter.AllyOrSelf: return isAlly;
                case TargetFilter.Self: return isSelf;
                case TargetFilter.Any: return true;
                default: throw new ArgumentOutOfRangeException(nameof(filter), filter, null);
            }
        }
    }
}
