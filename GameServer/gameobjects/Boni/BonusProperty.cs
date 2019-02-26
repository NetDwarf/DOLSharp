namespace DOL.GS
{
	public class BonusProperty : IBonusProperty
	{
		GameLiving owner;
		private int[] componentValues = new int[(int)ePropertyCategory.__Last + 1];

		public eProperty Type { get; }

		public int Base { get { return Get(ePropertyCategory.Base); } }
		public int Ability { get { return Get(ePropertyCategory.Ability); } }
		public int Item { get { return Get(ePropertyCategory.Item); } }
		public int ItemOvercap { get { return Get(ePropertyCategory.ItemOvercap); } }
		public int Mythical { get { return Get(ePropertyCategory.Mythical); } }
		public int BaseBuff { get { return Get(ePropertyCategory.BaseBuff); } }
		public int SpecBuff { get { return Get(ePropertyCategory.SpecBuff); } }
		public int ExtraBuff { get { return Get(ePropertyCategory.ExtraBuff); } }
		public int Debuff { get { return Get(ePropertyCategory.Debuff); } }
		public int SpecDebuff { get { return Get(ePropertyCategory.SpecDebuff); } }
		public double Multiplier { get; private set; }

		public BonusProperty(GameLiving owner, eProperty property)
		{
			this.owner = owner;
			this.Type = property;
		}

		public void Add(int value, BonusCategory category)
		{
			int componentIndex = (int)category.Name;
			componentValues[componentIndex] += value;
		}

		public void Remove(int value, BonusCategory category)
		{
			Add(-1 * value, category);
		}

		public void SetMultiplier(double multiplier)
		{
			this.Multiplier = multiplier;
		}

		public int Get(ePropertyCategory category)
		{
			return Get(new BonusCategory(category));
		}

		public int Get(BonusCategory category)
		{
			int componentIndex = (int)category.Name;
			return componentValues[componentIndex];
		}

		public void Set(int value, ePropertyCategory category)
		{
			int componentIndex = (int)category;
			componentValues[componentIndex] = value;
		}

		private static readonly IBonusProperty nullProperty = new NullProperty();
		public static IBonusProperty Dummy()
		{
			return nullProperty;
		}
	}

	public interface IBonusProperty
	{
		eProperty Type { get; }

		int Get(BonusCategory category);

		void Set(int value, ePropertyCategory category);
	}

	public class NullProperty : IBonusProperty
	{
		public eProperty Type { get { return eProperty.Undefined; } }

		public int Get(BonusCategory category)
		{
			return 0;
		}

		public void Set(int value, ePropertyCategory category)
		{

		}
	}
}
