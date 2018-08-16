using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_Boni
	{
		[Test]
		public void ValueOf_Constitution_Init_PropertyAbilityConstitutionIsZero()
		{
			var boni = createBoni();

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_Constitution_OneBase_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();

			boni.AbilityBoni[eProperty.Constitution] = 1;

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneAbilityConstitution_Init_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution.Create(1);

			boni.Add(bonusProp);

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_OneAbilityConstitution_Init_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution.Create(1);

			boni.Remove(bonusProp);

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Clear_Ability_WithOneConstitutionAbility_PropertyAbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution.Create(1);
			boni.Add(bonusProp);

			boni.Clear(Bonus.Ability);

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_AbilityConstitution_Init_Zero()
		{
			var boni = createBoni();

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_AbilityConstitution_AddedOneAbilityConstitution_One()
		{
			var boni = createBoni();

			boni.Add(Bonus.Ability.Constitution.Create(1));

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_BaseConstitution_AddedOneBaseConstitution_One()
		{
			var boni = createBoni();

			boni.Add(Bonus.Base.Constitution.Create(1));

			int actual = boni.GetValueOf(Bonus.Base.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private Boni createBoni()
		{
			return new Boni();
		}
	}
}
