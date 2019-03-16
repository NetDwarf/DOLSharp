using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public interface IBonusProperty
	{
		eProperty Type { get; }

		int Get(BonusCategory category);

		void Add(int value, BonusCategory category);
		void Remove(int value, BonusCategory category);

		void Set(int value, ePropertyCategory category);
	}

	public class BonusProperty : IBonusProperty
	{
		private GameLiving owner;
		private int[] componentValues = new int[(int)ePropertyCategory.__Last + 1];
		private List<int> perMilleMultiplier = new List<int>();

		public eProperty Type { get; }

		public BonusProperty(GameLiving owner, eProperty property)
		{
			this.owner = owner;
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

		public int Get(BonusCategory category)
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
		
		private static readonly IBonusProperty nullProperty = new NullProperty();
		public static IBonusProperty Dummy()
		{
			return nullProperty;
		}
	}

	public class PlayerBonusProperty : BonusProperty
	{
		protected GamePlayer owner;

		public PlayerBonusProperty(GamePlayer owner, eProperty property) :base(owner, property)
		{
			this.owner = owner;
		}
	}

	public class NullProperty : IBonusProperty
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
	}
}
