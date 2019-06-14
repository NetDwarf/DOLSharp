using System.Collections.Generic;

namespace DOL.GS
{
	public class UncappedBoni
	{
		private List<IBonusCompound> bonusCompounds = new List<IBonusCompound>();

		public void Add(Bonus bonus)
		{
			var bonusCompound = GetBonusOf(bonus.Type.ID, true);
			bonusCompound.Add(bonus.Value, bonus.Category);
		}

		public void Remove(Bonus bonus)
		{
			var bonusCompound = GetBonusOf(bonus.Type.ID, true);
			bonusCompound.Remove(bonus.Value, bonus.Category);
		}

		public void SetTo(Bonus bonus)
		{
			var bonusCompound = GetBonusOf(bonus.Type.ID, true);
			bonusCompound.Set(bonus.Value, bonus.Category);
		}
		
		public int RawValueOf(BonusComponent component)
		{
			return GetBonusOf(component.Type.ID).Get(component.Category);
		}

		public void Clear(BonusCategory category)
		{
			for (int i = 0; i <= (int)eProperty.MaxProperty; i++)
			{
				var bonusType = new BonusType((eProperty)i);
				SetTo(new Bonus(0, category, bonusType));
			}
		}

		private IBonusCompound GetBonusOf(eProperty property)
		{
			return GetBonusOf(property, false);
		}

		private IBonusCompound GetBonusOf(eProperty property, bool createIfNotExists)
		{
			var propIndex = bonusCompounds.FindIndex(s => s.Type == property);
			if (propIndex < 0)
			{
				if (createIfNotExists)
				{
					var bonusCompound = new BonusCompound(property);
					bonusCompounds.Add(bonusCompound);
					return bonusCompound;
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
