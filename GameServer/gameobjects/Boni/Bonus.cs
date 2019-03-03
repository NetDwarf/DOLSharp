namespace DOL.GS
{
	public class Bonus
	{
		public Bonus(int value, ePropertyCategory category, eProperty prop)
		{
			var component = new BonusComponent(category, prop);
			this.Type = component.Property;
			this.Category = component.Category;
			this.Value = value;
		}

		public int Value { get;}
		public eProperty Type { get; }
		public ePropertyCategory Category { get; }

		public static BonusCategory Base { get { return new BonusCategory(ePropertyCategory.Base); } }
		public static BonusCategory Ability { get { return new BonusCategory(ePropertyCategory.Ability); } }
		public static BonusCategory Item { get { return new BonusCategory(ePropertyCategory.Item); } }
		public static BonusCategory ItemOvercap { get { return new BonusCategory(ePropertyCategory.ItemOvercap); } }
		public static BonusCategory Mythical { get { return new BonusCategory(ePropertyCategory.Mythical); } }
		public static BonusCategory BaseBuff { get { return new BonusCategory(ePropertyCategory.BaseBuff); } }
		public static BonusCategory SpecBuff { get { return new BonusCategory(ePropertyCategory.SpecBuff); } }
		public static BonusCategory ExtraBuff { get { return new BonusCategory(ePropertyCategory.ExtraBuff); } }
		public static BonusCategory Debuff { get { return new BonusCategory(ePropertyCategory.Debuff); } }
		public static BonusCategory SpecDebuff { get { return new BonusCategory(ePropertyCategory.SpecDebuff); } }
		public static BonusCategory Multiplier { get { return new BonusCategory(ePropertyCategory.Multiplier); } }
	}
}
