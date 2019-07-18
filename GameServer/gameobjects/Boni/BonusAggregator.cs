using DOL.GS.PropertyCalc;
using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public class BonusAggregator
	{
		private List<Bonus> bonuses = new List<Bonus>();

		public int GetValueOf(BonusComponent component)
		{
			var cachedBonus = bonuses.Find(s => component.Equals(s.Component));

			if (cachedBonus is null) { return 0; }
			return cachedBonus.Value;
		}

		public void Set(Bonus bonus)
		{
			var cachedBonusIndex = bonuses.FindIndex(s => s.Component.Equals(bonus.Component));
			bool noBonusFound = cachedBonusIndex == -1;
			bool resetBonus = bonus.Value == 0;

			if(resetBonus)
			{
				if (noBonusFound) { return; }
				else { bonuses.RemoveAt(cachedBonusIndex); }
			}
			else if (noBonusFound)
			{
				bonuses.Add(bonus);
			}
			else
			{
				bonuses[cachedBonusIndex] = bonus;
			}
		}

		public void ClearItemBonuses()
		{
			for(int i=0; i<byte.MaxValue; i++)
			{
				var bonusType = new BonusType((eProperty)i);
				Set(bonusType.Item.WithValue(0));
			}
		}

		public IPropertyIndexer GetIndexerFor(BonusSource source)
		{
			return new BonusAggregatorToIndexerAdapter(this, source);
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

		public Bonus WithValue(int value)
		{
			return new Bonus(this, value);
		}

		public override bool Equals(object obj)
		{
			if (obj is BonusComponent)
			{
				var component2 = obj as BonusComponent;
				bool isSameType = Type.Equals(component2.Type);
				bool isSameSource = Source.Equals(component2.Source);
				return isSameType && isSameSource;
			}
			return false;
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

		public override bool Equals(object obj)
		{
			if (obj is BonusType)
			{
				return (obj as BonusType).ID == this.ID;
			}
			return false;
		}
	}

	public class BonusSource
	{
		public eBonusSource ID { get; }

		public BonusSource(eBonusSource sourceID)
		{
			ID = sourceID;
		}

		public override bool Equals(object obj)
		{
			if(obj is BonusSource)
			{
				return (obj as BonusSource).ID == this.ID;
			}
			return false;
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
