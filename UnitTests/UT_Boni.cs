using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_Boni
	{
		[Test]
		public void ValueOf_Constitution_Init_AbilityConstitutionIsZero()
		{
			var boni = createBoni();

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetValueOf_Constitution_OneBase_AbilityConstitutionIsOne()
		{
			var boni = createBoni();

			boni.AbilityBoni[eProperty.Constitution] = 1;

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneAbilityConstitution_Init_AbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution.Create(1);

			boni.Add(bonusProp);

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_OneAbilityConstitution_Init_AbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Ability.Constitution.Create(1);

			boni.Remove(bonusProp);

			int actual = boni.GetValueOf(Bonus.Ability.Constitution);
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Clear_Ability_WithOneConstitutionAbility_AbilityConstitutionIsOne()
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

		[Test]
		public void GetProperty_Constitution_OneBaseConstitutionAdded_BaseIsOne()
		{
			var boni = createBoni();
			boni.Add(Bonus.Base.Constitution.Create(1));

			int actual = boni.GetProperty(eProperty.Constitution).Base;
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetProperty_Constitution_1dot25ConstitutionAdded_ConstitutionIs1dot25()
		{
			var boni = createBoni();
			boni.MultiplicativeBuff.Set((int)eProperty.Constitution, "", 1.25);

			double actual = boni.GetProperty(eProperty.Constitution).Multiplier;
			double expected = 1.25;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetProperty_Intelligence_OneBaseIntelligenceAdded_BaseIsOne()
		{
			var boni = createBoni();
			boni.Add(Bonus.Base.Intelligence.Create(1));

			int actual = boni.GetProperty(eProperty.Intelligence).Base;
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetProperty_Constitution_OneAbilityConstitutionAdded_AbilityIsOne()
		{
			var boni = createBoni();
			boni.Add(Bonus.Ability.Constitution.Create(1));

			int actual = boni.GetProperty(eProperty.Constitution).Ability;
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private Boni createBoni()
		{
			return new Boni(Create.FakeNPC());
		}
	}
}
