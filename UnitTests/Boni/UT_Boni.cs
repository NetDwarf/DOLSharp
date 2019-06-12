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

			int actual = boni.RawValueOf(Bonus.Constitution.Ability);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetValueOf_Constitution_OneAbility_AbilityConstitutionIsOne()
		{
			var boni = createBoni();

			boni.SetTo(Bonus.Constitution.Ability.Create(1));

			int actual = boni.RawValueOf(Bonus.Constitution.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneAbilityConstitution_Init_AbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Constitution.Ability.Create(1);

			boni.Add(bonusProp);

			int actual = boni.RawValueOf(Bonus.Constitution.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_OneAbilityConstitution_Init_AbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Constitution.Ability.Create(1);

			boni.Remove(bonusProp);

			int actual = boni.RawValueOf(Bonus.Constitution.Ability);
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Clear_Ability_WithOneConstitutionAbility_AbilityConstitutionIsOne()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Constitution.Ability.Create(1);
			boni.Add(bonusProp);

			boni.Clear(Bonus.Ability);

			int actual = boni.RawValueOf(Bonus.Constitution.Ability);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_AbilityConstitution_Init_Zero()
		{
			var boni = createBoni();

			int actual = boni.RawValueOf(Bonus.Constitution.Ability);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_AbilityConstitution_AddedOneAbilityConstitution_One()
		{
			var boni = createBoni();

			boni.Add(Bonus.Constitution.Ability.Create(1));

			int actual = boni.RawValueOf(Bonus.Constitution.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_BaseConstitution_AddedOneBaseConstitution_One()
		{
			var boni = createBoni();

			boni.Add(Bonus.Constitution.Base.Create(1));

			int actual = boni.RawValueOf(Bonus.Constitution.Base);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ValueOf_DebuffHealthRegen_2HealthRegenSpecBuff_2()
		{
			var boni = createBoni();
			var healthRegen = new BonusType(eProperty.HealthRegenerationRate);

			boni.Add(healthRegen.SpecBuff.Create(2));

			int actual = boni.RawValueOf(healthRegen.Debuff);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		private Boni createBoni()
		{
			return new Boni();
		}
	}
}
