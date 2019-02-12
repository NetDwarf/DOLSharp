using DOL.GS.PropertyCalc;
using System;

namespace DOL.GS
{
	public class Boni
	{
		private IPropertyIndexer m_abilityBonus = new PropertyIndexer();
		private IPropertyIndexer m_itemBonus = new PropertyIndexer();
		private IPropertyIndexer m_buff1Bonus = new PropertyIndexer();
		private IPropertyIndexer m_buff2Bonus = new PropertyIndexer();
		private IPropertyIndexer m_debuffBonus = new PropertyIndexer();
		private IPropertyIndexer m_buff4Bonus = new PropertyIndexer();
		private IPropertyIndexer m_specDebuffBonus = new PropertyIndexer();

		public IPropertyIndexer GetIndexer(eBonusCategory category)
		{
			switch (category)
			{
				case eBonusCategory.Ability:
					return m_abilityBonus;
				case eBonusCategory.Item:
					return m_itemBonus;
				case eBonusCategory.BaseBuff:
					return m_buff1Bonus;
				case eBonusCategory.SpecBuff:
					return m_buff2Bonus;
				case eBonusCategory.ExtraBuff:
					return m_buff4Bonus;
				case eBonusCategory.Debuff:
					return m_debuffBonus;
				case eBonusCategory.SpecDebuff:
					return m_specDebuffBonus;
				default:
					throw new ArgumentException();
			}
		}

		public void Clear(eBonusCategory category)
		{
			var indexer = GetIndexer(category);
			indexer = new PropertyIndexer();
		}
	}

	public class BonusIndexer : IPropertyIndexer
	{
		private eBonusCategory category;
		private Boni bonuses;

		public BonusIndexer(eBonusCategory category, Boni bonuses)
		{
			this.bonuses = bonuses;
			this.category = category;
		}

		public int this[int index]
		{
			get
			{
				return bonuses.GetIndexer(category)[index];
			}
			set
			{
				bonuses.GetIndexer(category)[index] = value;
			}
		}

		public int this[eProperty index]
		{
			get
			{
				return this[(int)index];
			}
			set
			{
				this[(int)index] = value;
			}
		}
	}

	public enum eBonusCategory
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
