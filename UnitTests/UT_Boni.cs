using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.Gameserver
{
	[TestFixture]
	class UT_Boni
	{
		[Test]
		public void ValueOf_Constitution_Init_PropertyAbilityConstitutionIsZero()
		{
			var boni = createBoni();

			int actual = boni.ValueOf(ePropertyCategory.Ability, eProperty.Constitution);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_Constitution_OneBase_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();
			
			boni.GetIndexer(ePropertyCategory.Ability)[eProperty.Constitution] = 1;

			int actual = boni.ValueOf(ePropertyCategory.Ability, eProperty.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneAbilityConstitution_Init_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution(1);

			boni.Add(bonusProp);

			int actual = boni.ValueOf(ePropertyCategory.Ability, eProperty.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_OneAbilityConstitution_Init_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution(1);

			boni.Remove(bonusProp);

			int actual = boni.ValueOf(ePropertyCategory.Ability, eProperty.Constitution);
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Clear_Ability_WithOneConstitutionAbility_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution(1);
			boni.Add(bonusProp);

			boni.Clear(ePropertyCategory.Ability);

			int actual = boni.ValueOf(ePropertyCategory.Ability, eProperty.Constitution);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		private Boni createBoni()
		{
			return new Boni();
		}
	}
}
