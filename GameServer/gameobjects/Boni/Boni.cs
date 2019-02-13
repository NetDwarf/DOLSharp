using DOL.GS.PropertyCalc;
using System;

namespace DOL.GS
{
	public class Boni
	{
		private PropertyIndexer m_abilityBonus = new PropertyIndexer();
		private PropertyIndexer m_itemBonus = new PropertyIndexer();
		private PropertyIndexer m_buff1Bonus = new PropertyIndexer();
		private PropertyIndexer m_buff2Bonus = new PropertyIndexer();
		private PropertyIndexer m_debuffBonus = new PropertyIndexer();
		private PropertyIndexer m_buff4Bonus = new PropertyIndexer();
		private PropertyIndexer m_specDebuffBonus = new PropertyIndexer();

		public int ValueOf(ePropertyCategory category, eProperty property)
		{
			return GetIndexer(category)[property];
		}

		public void Add(Bonus bonusProp)
		{
			var indexer = GetIndexer(bonusProp.Category);
			indexer[bonusProp.Property] += bonusProp.Value;
		}

		public void Remove(Bonus bonusProp)
		{
			var malus = new Bonus(-1 * bonusProp.Value, bonusProp.Property, bonusProp.Category);
			Add(malus);
		}

		public PropertyIndexer GetIndexer(ePropertyCategory category)
		{
			switch (category)
			{
				case ePropertyCategory.Ability:
					return m_abilityBonus;
				case ePropertyCategory.Item:
					return m_itemBonus;
				case ePropertyCategory.BaseBuff:
					return m_buff1Bonus;
				case ePropertyCategory.SpecBuff:
					return m_buff2Bonus;
				case ePropertyCategory.ExtraBuff:
					return m_buff4Bonus;
				case ePropertyCategory.Debuff:
					return m_debuffBonus;
				case ePropertyCategory.SpecDebuff:
					return m_specDebuffBonus;
				default:
					throw new ArgumentException();
			}
		}

		public void Clear(ePropertyCategory category)
		{
			var indexer = GetIndexer(category);
			indexer.Clear();
		}
	}

	public class Bonus
	{
		public Bonus(int value, eProperty prop,ePropertyCategory category)
		{
			this.Value = value;
			this.Property = prop;
			this.Category = category;
		}

		public int Value { get; }
		public eProperty Property { get; }
		public ePropertyCategory Category { get; }

		public static PropertyCategory Ability { get { return new PropertyCategory(ePropertyCategory.Ability); } }
		public static PropertyCategory Item { get { return new PropertyCategory(ePropertyCategory.Item); } }
		public static PropertyCategory BaseBuff { get { return new PropertyCategory(ePropertyCategory.BaseBuff); } }
		public static PropertyCategory SpecBuff { get { return new PropertyCategory(ePropertyCategory.SpecBuff); } }
		public static PropertyCategory Extrabuff { get { return new PropertyCategory(ePropertyCategory.ExtraBuff); } }
		public static PropertyCategory Debuff { get { return new PropertyCategory(ePropertyCategory.Debuff); } }
		public static PropertyCategory SpecDebuff { get { return new PropertyCategory(ePropertyCategory.SpecDebuff); } }
	}

	public class PropertyCategory
	{
		public PropertyCategory(ePropertyCategory category)
		{
			this.Category = category;
		}

		public ePropertyCategory Category { get; }

		public Bonus Constitution(int value) { return new Bonus(value, eProperty.Constitution, Category); }

		public Bonus Bonus(int value, eProperty property)
		{
			return new Bonus(value, property, Category);
		}
	}

	public enum ePropertyCategory
	{
		Ability,
		Item,
		BaseBuff,
		SpecBuff,
		ExtraBuff,
		SpecDebuff,
		Debuff,
	}
}
