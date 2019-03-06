using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public interface IBonusProperty
	{
		eProperty Type { get; }

		int Get(BonusCategory category);

		void Set(int value, ePropertyCategory category);

		void AddMultiplier(int perMilleValue);
		void RemoveMultiplier(int perMilleValue);
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
			int componentIndex = (int)category.Name;
			componentValues[componentIndex] += value;
		}

		public void Remove(int value, BonusCategory category)
		{
			Add(-1 * value, category);
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
			int componentIndex = (int)category;
			componentValues[componentIndex] = value;
		}

		public void AddMultiplier(int perMillevalue)
		{
			perMilleMultiplier.Add(perMillevalue);
		}

		public void RemoveMultiplier(int perMilleValue)
		{
			perMilleMultiplier.Remove(perMilleValue);
		}

		private static readonly IBonusProperty nullProperty = new NullProperty();
		public static IBonusProperty Dummy()
		{
			return nullProperty;
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

		public void Set(int value, ePropertyCategory category)
		{
			//do nothing
		}

		public void AddMultiplier(int perMilleValue)
		{
			//do nothing
		}

		public void RemoveMultiplier(int perMilleValue)
		{
			//do nothing
		}
	}
}
