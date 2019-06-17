using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public interface IBonusCompound
	{
		eProperty Type { get; }

		int Get(BonusPart category);
		void Set(int value, BonusPart category);

		void Add(int value, BonusPart category);
		void Remove(int value, BonusPart category);
	}

	public class BonusCompound : IBonusCompound
	{
		private int[] componentValues = new int[(int)eBonusPart.__Last + 1];
		private List<int> perMilleMultiplier = new List<int>();

		public eProperty Type { get; }

		public BonusCompound(eProperty property)
		{
			this.Type = property;
		}

		public void Add(int value, BonusPart category)
		{
			if(!category.Equals(Bonus.Multiplier))
			{
				int componentIndex = (int)category.ID;
				componentValues[componentIndex] += value;
			}
			else
			{
				perMilleMultiplier.Add(value);
			}
		}

		public void Remove(int value, BonusPart category)
		{
			if (!category.Equals(Bonus.Multiplier))
			{
				Add(-1 * value, category);
			}
			else
			{
				bool successfullyRemoved = perMilleMultiplier.Remove(value);
				if (!successfullyRemoved) { throw new ArgumentOutOfRangeException(); }
			}
		}

		public virtual int Get(BonusPart category)
		{
			if(category.Equals(Bonus.Multiplier))
			{
				double result = 1.0;
				foreach(var perMilleMultiplicator in perMilleMultiplier)
				{
					result *= perMilleMultiplicator / 1000.0;
				}
				return (int)(result * 1000);
			}
			int componentIndex = (int)category.ID;
			return componentValues[componentIndex];
		}

		public void Set(int value, BonusPart category)
		{
			if (category.Equals(Bonus.Multiplier))
			{
				perMilleMultiplier = new List<int>();
				perMilleMultiplier.Add(value);
			}
			else
			{
				int componentIndex = (int)category.ID;
				componentValues[componentIndex] = value;
			}
		}

		private static readonly IBonusCompound nullProperty = new NullBonusCompound();
		public static IBonusCompound Dummy()
		{
			return nullProperty;
		}
	}

	public class NullBonusCompound : IBonusCompound
	{
		public eProperty Type { get { return eProperty.Undefined; } }
		
		public int Get(BonusPart category)
		{
			if(category.Equals(Bonus.Multiplier))
			{
				return 1000;
			}
			return 0;
		}

		public void Add(int value, BonusPart category)
		{
			//do nothing
		}

		public void Remove(int value, BonusPart category)
		{
			//do nothing
		}

		public void Set(int value, BonusPart category)
		{
			//do nothing
		}
	}
}
