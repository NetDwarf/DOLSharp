using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public class Boni
	{
		private GameLiving owner;
		protected List<IBonusProperty> properties = new List<IBonusProperty>();

		public Boni(GameLiving owner)
		{
			this.owner = owner;
		}

		public void Add(Bonus bonus)
		{
			var bonusProperty = Get(bonus.Type, true);
			bonusProperty.Add(bonus.Value, new BonusCategory(bonus.Category));
		}

		public void Remove(Bonus bonus)
		{
			var bonusProperty = Get(bonus.Type, true);
			bonusProperty.Remove(bonus.Value, new BonusCategory(bonus.Category));
		}

		public void SetTo(Bonus bonus)
		{
			var bonusProperty = Get(bonus.Type, true);
			bonusProperty.Set(bonus.Value, bonus.Category);
		}

		public void AddMultiplier(int perMilleValue, eProperty property)
		{
			var bonusProperty = Get(property, true);
			bonusProperty.Add(perMilleValue, Bonus.Multiplier);
		}

		public void RemoveMultiplier(int perMilleValue, eProperty property)
		{
			var bonusProperty = Get(property);
			bonusProperty.Remove(perMilleValue, Bonus.Multiplier);
		}

		public int GetValueOf(BonusComponent component)
		{
			return Get(component.Property).Get(new BonusCategory(component.Category));
		}

		protected IBonusProperty Get(eProperty property)
		{
			return Get(property, false);
		}

		protected virtual IBonusProperty Get(eProperty property, bool createIfNotExists)
		{
			var propIndex = properties.FindIndex(s => s.Type == property);
			if (propIndex < 0)
			{
				if (createIfNotExists)
				{
					IBonusProperty bonusProperty;
					bonusProperty = new BonusProperty(owner, property);
					properties.Add(bonusProperty);
					return bonusProperty;
				}
				else
				{
					return BonusProperty.Dummy();
				}
			}
			return properties[propIndex];
		}

		public void Clear(BonusCategory category)
		{
			for (int i = 0; i <= (int)eProperty.MaxProperty; i++)
			{
				SetTo(new Bonus(0, category.Name, (eProperty)i));
			}
		}
	}

	public class PlayerBoni : Boni
	{
		private GamePlayer owner;

		public PlayerBoni(GamePlayer owner) : base(owner)
		{
			this.owner = owner;
		}

		protected override IBonusProperty Get(eProperty property, bool createIfNotExists)
		{
			var propIndex = properties.FindIndex(s => s.Type == property);
			if (propIndex < 0)
			{
				if (createIfNotExists)
				{
					IBonusProperty bonusProperty;
					bonusProperty = new PlayerBonusProperty(owner, property);
					properties.Add(bonusProperty);
					return bonusProperty;
				}
				else
				{
					return BonusProperty.Dummy();
				}
			}
			return properties[propIndex];
		}
	}
}
