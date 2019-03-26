using System.Collections.Generic;

namespace DOL.GS
{
	public class Boni
	{
		private List<IBonusCompound> bonusCompounds = new List<IBonusCompound>();

		public void Add(Bonus bonus)
		{
			var bonusProperty = GetBonusComponentsOf(bonus.Type, true);
			bonusProperty.Add(bonus.Value, new BonusCategory(bonus.Category));
		}

		public void Remove(Bonus bonus)
		{
			var bonusProperty = GetBonusComponentsOf(bonus.Type, true);
			bonusProperty.Remove(bonus.Value, new BonusCategory(bonus.Category));
		}

		public void SetTo(Bonus bonus)
		{
			var bonusProperty = GetBonusComponentsOf(bonus.Type, true);
			bonusProperty.Set(bonus.Value, bonus.Category);
		}
		
		public int RawValueOf(BonusComponent component)
		{
			return GetBonusComponentsOf(component.Property).Get(new BonusCategory(component.Category));
		}

		public void Clear(BonusCategory category)
		{
			for (int i = 0; i <= (int)eProperty.MaxProperty; i++)
			{
				SetTo(new Bonus(0, category.Name, (eProperty)i));
			}
		}

		private IBonusCompound GetBonusComponentsOf(eProperty property)
		{
			return GetBonusComponentsOf(property, false);
		}

		private IBonusCompound GetBonusComponentsOf(eProperty property, bool createIfNotExists)
		{
			var propIndex = bonusCompounds.FindIndex(s => s.Type == property);
			if (propIndex < 0)
			{
				if (createIfNotExists)
				{
					var bonusProperty = new BonusComponents(property);
					bonusCompounds.Add(bonusProperty);
					return bonusProperty;
				}
				else
				{
					return BonusComponents.Dummy();
				}
			}
			else
			{
				return bonusCompounds[propIndex];
			}
		}
	}
}
