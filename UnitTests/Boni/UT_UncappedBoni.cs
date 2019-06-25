using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_UncappedBoni
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
		public void Clear_Ability_WithOneConstitutionAbility_AbilityConstitutionIsZero()
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
		public void Clear_Item_With12StrengthItemOvercap_StrengthItemOvercapIsZero()
		{
			var boni = createBoni();
			var bonusProp = Bonus.Strength.ItemOvercap.Create(12);
			boni.Add(bonusProp);

			boni.Clear(Bonus.Item);

			int actual = boni.RawValueOf(Bonus.Strength.ItemOvercap);
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

		private UncappedBoni createBoni()
		{
			return new UncappedBoni();
		}
	}
}
