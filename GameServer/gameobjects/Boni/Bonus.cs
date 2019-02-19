namespace DOL.GS
{
	public class Bonus
	{
		public Bonus(int value, ePropertyCategory category, eProperty prop)
		{
			this.Value = value;
			this.Type = prop;
			this.Category = category;
		}

		public int Value { get; private set; }
		public eProperty Type { get; }
		public ePropertyCategory Category { get; }

		public static BonusCategory Base { get { return new BonusCategory(ePropertyCategory.Base); } }
		public static BonusCategory Ability { get { return new BonusCategory(ePropertyCategory.Ability); } }
		public static BonusCategory Item { get { return new BonusCategory(ePropertyCategory.Item); } }
		public static BonusCategory BaseBuff { get { return new BonusCategory(ePropertyCategory.BaseBuff); } }
		public static BonusCategory SpecBuff { get { return new BonusCategory(ePropertyCategory.SpecBuff); } }
		public static BonusCategory Extrabuff { get { return new BonusCategory(ePropertyCategory.ExtraBuff); } }
		public static BonusCategory Debuff { get { return new BonusCategory(ePropertyCategory.Debuff); } }
		public static BonusCategory SpecDebuff { get { return new BonusCategory(ePropertyCategory.SpecDebuff); } }

		public void Add(int value)
		{
			this.Value += value;
		}

		public void Remove(int value)
		{
			Add(-1 * value);
		}
	}
}
