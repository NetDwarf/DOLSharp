using NUnit.Framework;
using DOL.GS;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_BonusComponents
	{
		[Test]
		public void Base_Init_Zero()
		{
			var bonusProp = createBonusComponents();

			int actual = bonusProp.Get(Bonus.Base);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneBase_BaseIsOne()
		{
			var bonusProp = createBonusComponents();

			bonusProp.Add(1, Bonus.Base);

			int actual = bonusProp.Get(Bonus.Base);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_OneBaseToInit_BaseIsMinusOne()
		{
			var bonusProp = createBonusComponents();

			bonusProp.Remove(1, Bonus.Base);

			int actual = bonusProp.Get(Bonus.Base);
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneAbilityToInit_AbilityIsOne()
		{
			var bonusProp = createBonusComponents();

			bonusProp.Add(1, Bonus.Ability);

			int actual = bonusProp.Get(Bonus.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_OneAbility_AbilityIsOne()
		{
			var bonusProp = createBonusComponents();

			bonusProp.Set(1, Bonus.Ability);

			int actual = bonusProp.Get(Bonus.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private static BonusCompound createBonusComponents()
		{
			return new BonusCompound(eProperty.Undefined);
		}
	}

	[TestFixture]
	class UT_ePropertyCategory
	{
		[Test]
		public void Last_EqualsBiggestValue()
		{
			var actual = ePropertyCategory.__Last;
			var expected = Enum.GetValues(typeof(ePropertyCategory)).Cast<ePropertyCategory>().Last<ePropertyCategory>();

			Assert.AreEqual(actual, expected);
		}
	}
}
