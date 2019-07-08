using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public class UncappedBoni
	{
		private List<IBonusCompound> bonusCompounds = new List<IBonusCompound>();

		public void Add(Bonus bonus)
		{
			if (bonus.Type.Equals(new BonusType(eBonusType.ManaPool)) && bonus.Part.Equals(Bonus.ItemOvercap))
			{
				var bonusCompound2 = GetBonusOf(new BonusType(eBonusType.ManaPoolPercent), true);
				bonusCompound2.Add(bonus.Value, bonus.Part);
			}
			if (bonus.Type.Equals(new BonusType(eBonusType.ManaPoolPercent)) && bonus.Part.Equals(Bonus.ItemOvercap))
			{
				var bonusCompound2 = GetBonusOf(new BonusType(eBonusType.ManaPool), true);
				bonusCompound2.Add(bonus.Value, bonus.Part);
			}
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
			if (bonus.Type.Equals(new BonusType(eBonusType.ManaPool)) && bonus.Part.Equals(Bonus.ItemOvercap))
			{
				var bonusCompound2 = GetBonusOf(new BonusType(eBonusType.ManaPoolPercent), true);
				bonusCompound2.Set(bonus.Value, bonus.Part);
			}
			if (bonus.Type.Equals(new BonusType(eBonusType.ManaPoolPercent)) && bonus.Part.Equals(Bonus.ItemOvercap))
			{
				var bonusCompound2 = GetBonusOf(new BonusType(eBonusType.ManaPool), true);
				bonusCompound2.Set(bonus.Value, bonus.Part);
			}
			var bonusCompound = GetBonusOf(bonus.Type, true);
			bonusCompound.Set(bonus.Value, bonus.Part);
		}
		
		public int RawValueOf(BonusComponent component)
		{
			return GetBonusOf(component.Type).Get(component.Part);
		}

		public void Clear(BonusPart part)
		{
			var allPropertyIDs = Enum.GetValues(typeof(eBonusType));
			foreach(var id in allPropertyIDs)
			{
				var bonusType = new BonusType((eBonusType)id);
				SetTo(bonusType.From(part).Create(0));
				if(part.Equals(Bonus.Item))
				{
					SetTo(bonusType.ItemOvercap.Create(0));
					SetTo(bonusType.Mythical.Create(0));
				}
			}
		}

		private IBonusCompound GetBonusOf(BonusType type)
		{
			return GetBonusOf(type, false);
		}

		private IBonusCompound GetBonusOf(BonusType type, bool createIfNotExists)
		{
			var bonusindex = bonusCompounds.FindIndex(s => s.Type.Equals(type));
			if (bonusindex < 0)
			{
				if (createIfNotExists)
				{
					var bonusCompound = new BonusCompound(type);
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
