using System.Collections.Generic;

namespace DOL.GS
{
	public class Boni
	{
		private List<IBonusCompound> bonusCompounds = new List<IBonusCompound>();

		public void Add(Bonus bonus)
		{
			var bonusCompound = GetBonusComponentsOf(bonus.Type.ID, true);
			bonusCompound.Add(bonus.Value, bonus.Category);
		}

		public void Remove(Bonus bonus)
		{
			var bonusCompound = GetBonusComponentsOf(bonus.Type.ID, true);
			bonusCompound.Remove(bonus.Value, bonus.Category);
		}

		public void SetTo(Bonus bonus)
		{
			var bonusCompound = GetBonusComponentsOf(bonus.Type.ID, true);
			bonusCompound.Set(bonus.Value, bonus.Category);
		}
		
		public int RawValueOf(BonusComponent component)
		{
			return GetBonusComponentsOf(component.Type.ID).Get(component.Category);
		}

		public void Clear(BonusCategory category)
		{
			for (int i = 0; i <= (int)eProperty.MaxProperty; i++)
			{
				var bonusType = new BonusType((eProperty)i);
				SetTo(new Bonus(0, category, bonusType));
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
					var bonusProperty = new BonusCompound(property);
					bonusCompounds.Add(bonusProperty);
					return bonusProperty;
				}
				else
				{
					return BonusCompound.Dummy();
				}
			}
			else
			{
				return bonusCompounds[propIndex];
			}
		}
	}
}
