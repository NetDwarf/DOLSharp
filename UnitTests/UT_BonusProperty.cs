using NUnit.Framework;
using DOL.GS;
using System;
using System.Linq;
using System.Collections.Generic;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_BonusProperty
	{
		[Test]
		public void Base_Init_Zero()
		{
			var bonusProp = createBonusProperty();

			int actual = bonusProp.Base;
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneBase_BaseIsOne()
		{
			var bonusProp = createBonusProperty();

			bonusProp.Add(1, Bonus.Base);

			int actual = bonusProp.Base;
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_OneBaseToInit_BaseIsMinusOne()
		{
			var bonusProp = createBonusProperty();

			bonusProp.Remove(1, Bonus.Base);

			int actual = bonusProp.Base;
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneAbilityToInit_AbilityIsOne()
		{
			var bonusProp = createBonusProperty();

			bonusProp.Add(1, Bonus.Ability);

			int actual = bonusProp.Ability;
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_OneAbility_AbilityIsOne()
		{
			var bonusProp = createBonusProperty();

			bonusProp.Set(1, ePropertyCategory.Ability);

			int actual = bonusProp.Ability;
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private static BonusProperty createBonusProperty()
		{
			return new BonusProperty(Create.FakeNPC(), eProperty.Undefined);
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
