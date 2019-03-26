using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public interface IBonusCompound
	{
		eProperty Type { get; }

		int Get(BonusCategory category);
		void Set(int value, ePropertyCategory category);

		void Add(int value, BonusCategory category);
		void Remove(int value, BonusCategory category);
	}

	public class BonusComponents : IBonusCompound
	{
		private int[] componentValues = new int[(int)ePropertyCategory.__Last + 1];
		private List<int> perMilleMultiplier = new List<int>();

		public eProperty Type { get; }

		public BonusComponents(eProperty property)
		{
			this.Type = property;
		}

		public void Add(int value, BonusCategory category)
		{
			if(!category.Equals(Bonus.Multiplier))
			{
				int componentIndex = (int)category.Name;
				componentValues[componentIndex] += value;
			}
			else
			{
				perMilleMultiplier.Add(value);
			}
		}

		public void Remove(int value, BonusCategory category)
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

		public virtual int Get(BonusCategory category)
		{
			if(category.Name == ePropertyCategory.Multiplier)
			{
				double result = 1.0;
				foreach(var perMilleMultiplicator in perMilleMultiplier)
				{
					result *= perMilleMultiplicator / 1000.0;
				}
				return (int)(result * 1000);
			}
			int componentIndex = (int)category.Name;
			return componentValues[componentIndex];
		}

		public void Set(int value, ePropertyCategory category)
		{
			if (category != ePropertyCategory.Multiplier)
			{
				int componentIndex = (int)category;
				componentValues[componentIndex] = value;
			}
			else
			{
				perMilleMultiplier = new List<int>();
				perMilleMultiplier.Add(value);
			}

		}

		private static readonly IBonusCompound nullProperty = new NullBonusComponents();
		public static IBonusCompound Dummy()
		{
			return nullProperty;
		}
	}

	public class NullBonusComponents : IBonusCompound
	{
		public eProperty Type { get { return eProperty.Undefined; } }
		
		public int Get(BonusCategory category)
		{
			if(category.Name == ePropertyCategory.Multiplier)
			{
				return 1000;
			}
			return 0;
		}

		public void Add(int value, BonusCategory category)
		{
			//do nothing
		}

		public void Remove(int value, BonusCategory category)
		{
			//do nothing
		}

		public void Set(int value, ePropertyCategory category)
		{
			//do nothing
		}

		public int GetEffective(BonusCategory category)
		{
			return Get(category);
		}
	}
}
