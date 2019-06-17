using System.Collections.Generic;

namespace DOL.GS
{
	public class UncappedBoni
	{
		private List<IBonusCompound> bonusCompounds = new List<IBonusCompound>();

		public void Add(Bonus bonus)
		{
			var bonusCompound = GetBonusOf(bonus.Type, true);
			bonusCompound.Add(bonus.Value, bonus.Part);
		}

		public void Remove(Bonus bonus)
		{
			var bonusCompound = GetBonusOf(bonus.Type, true);
			bonusCompound.Remove(bonus.Value, bonus.Part);
		}

		public void SetTo(Bonus bonus)
		{
			var bonusCompound = GetBonusOf(bonus.Type, true);
			bonusCompound.Set(bonus.Value, bonus.Part);
		}
		
		public int RawValueOf(BonusComponent component)
		{
			return GetBonusOf(component.Type).Get(component.Part);
		}

		public void Clear(BonusPart part)
		{
			for (int i = 0; i <= (int)eProperty.MaxProperty; i++)
			{
				var bonusType = new BonusType((eBonusType)i);
				SetTo(new Bonus(0, part, bonusType));
			}
		}

		private IBonusCompound GetBonusOf(BonusType type)
		{
			return GetBonusOf(type, false);
		}

		private IBonusCompound GetBonusOf(BonusType type, bool createIfNotExists)
		{
			var bonusindex = bonusCompounds.FindIndex(s => s.Type == type.ID);
			if (bonusindex < 0)
			{
				if (createIfNotExists)
				{
					var bonusCompound = new BonusCompound(type.ID);
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
				return bonusCompounds[bonusindex];
			}
		}
	}
}
