using DOL.GS.PropertyCalc;
using System;

namespace DOL.GS
{
	public class BonusAggregator
	{
		private IPropertyIndexer AbilityBonus { get; } = new PropertyIndexer();
		private IPropertyIndexer ItemBonus { get; set; } = new PropertyIndexer();
		private IPropertyIndexer BaseBuffBonusCategory { get; } = new PropertyIndexer();
		private IPropertyIndexer SpecBuffBonusCategory { get; } = new PropertyIndexer();
		private IPropertyIndexer ExtraBuffBonusCategory { get; } = new PropertyIndexer();
		private IPropertyIndexer DebuffCategory { get; } = new PropertyIndexer();
		private IPropertyIndexer SpecDebuffCategory { get; } = new PropertyIndexer();

		public void ClearItemBonuses()
		{
			ItemBonus = new PropertyIndexer();
		}

		public IPropertyIndexer GetIndexerFor(eBonusSource source)
		{
			switch (source)
			{
				case eBonusSource.Ability: return AbilityBonus;
				case eBonusSource.Item: return ItemBonus;
				case eBonusSource.BaseBuff: return BaseBuffBonusCategory;
				case eBonusSource.SpecBuff: return SpecBuffBonusCategory;
				case eBonusSource.ExtraBuff: return ExtraBuffBonusCategory;
				case eBonusSource.Debuff: return DebuffCategory;
				case eBonusSource.SpecDebuff: return SpecBuffBonusCategory;
				default: throw new ArgumentException();
			}
		}
	}

	public enum eBonusSource
	{
		Ability,
		Item,
		BaseBuff,
		SpecBuff,
		ExtraBuff,
		Debuff,
		SpecDebuff,
	}
}
