namespace MiniChess.Core.State
{
    /// <summary>
    /// 유닛이 가진 보호막 하나. 보호막 Id 로 구분하며 소멸 기한은 없다(모두 흡수되면 사라진다).
    /// 같은 Id 는 중첩되지 않고, 다른 Id 끼리는 함께 존재하며 양이 합산된다. 변경은 UnitStats 를 통해서만 한다.
    /// </summary>
    public class ShieldInstance
    {
        /// <summary>보호막 종류 식별자(예: 스킬 Id, 점령 보상 Id).</summary>
        public string Id { get; }

        public int Amount { get; internal set; }

        /// <summary>보호막을 준 유닛. 유닛이 아닌 출처(점령 보상 등)면 null.</summary>
        public Unit Source { get; }

        internal ShieldInstance(string id, int amount, Unit source)
        {
            Id = id;
            Amount = amount;
            Source = source;
        }
    }
}
