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

		public int GetValueOf(BonusComponent component)
		{
			return GetIndexerFor(component.Source.ID)[component.Type.ID];
		}

		public void Set(Bonus bonus)
		{
			GetIndexerFor(bonus.Source.ID)[bonus.Type.ID] = bonus.Value;
		}

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

	public class Bonus
	{
		public BonusType Type => Component.Type;
		public BonusSource Source => Component.Source;
		public BonusComponent Component { get; }
		public int Value { get; }

		public Bonus(BonusComponent component, int value)
		{
			Component = component;
			Value = value;
		}

		public static BonusSource Ability => new BonusSource(eBonusSource.Ability);
		public static BonusSource Item => new BonusSource(eBonusSource.Item);
		public static BonusSource BaseBuff => new BonusSource(eBonusSource.BaseBuff);
		public static BonusSource SpecBuff => new BonusSource(eBonusSource.SpecBuff);
		public static BonusSource ExtraBuff => new BonusSource(eBonusSource.ExtraBuff);
		public static BonusSource Debuff => new BonusSource(eBonusSource.Debuff);
		public static BonusSource SpecDebuff => new BonusSource(eBonusSource.SpecDebuff);

		public static BonusType Constitution => new BonusType(eProperty.Constitution);
	}

	public class BonusComponent
	{
		public BonusType Type { get; }
		public BonusSource Source { get; }

		public BonusComponent(BonusType type, BonusSource source)
		{
			Type = type;
			Source = source;
		}

		public Bonus Create(int value)
		{
			return new Bonus(this, value);
		}
	}

	public class BonusType
	{
		public eProperty ID { get; }

		public BonusType(eProperty typeID)
		{
			ID = typeID;
		}

		public BonusComponent Ability => new BonusComponent(this, Bonus.Ability);
		public BonusComponent Item => new BonusComponent(this, Bonus.Item);
		public BonusComponent BaseBuff => new BonusComponent(this, Bonus.BaseBuff);
		public BonusComponent SpecBuff => new BonusComponent(this, Bonus.SpecBuff);
		public BonusComponent ExtraBuff => new BonusComponent(this, Bonus.ExtraBuff);
		public BonusComponent Debuff => new BonusComponent(this, Bonus.Debuff);
		public BonusComponent SpecDebuff => new BonusComponent(this, Bonus.SpecDebuff);
	}

	public class BonusSource
	{
		public eBonusSource ID { get; }

		public BonusSource(eBonusSource sourceID)
		{
			ID = sourceID;
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
