using NUnit.Framework;
using NSubstitute;
using DOL.GS;
using DOL.AI;
using DOL.AI.Brain;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_NecromancerPet
	{
		[TestFixtureSetUp]
		public void init()
		{
			GameLiving.LoadCalculators();
		}

		[Test]
		public void GetModified_Strength_LevelZero_60()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;

			int actual = necroPet.GetModified(StrengthID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_Level50_110()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 50;

			int actual = necroPet.GetModified(StrengthID);

			int expected = 110;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAndOwnerHas20ItemBonus_80()
		{
			var owner = Create.FakePlayer();
			owner.Level = 50;
			owner.ItemBonus[StrengthID] = 20;
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;

			int actual = necroPet.GetModified(StrengthID);

			int expected = 80;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAnd20BuffBonus_80()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.BaseBuffBonusCategory[StrengthID] = 20;

			int actual = necroPet.GetModified(StrengthID);

			int expected = 80;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAnd20ItemBonus_60()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.ItemBonus[StrengthID] = 20;

			int actual = necroPet.GetModified(StrengthID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAnd20Debuff_50()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.DebuffCategory[StrengthID] = 20;

			int actual = necroPet.GetModified(StrengthID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZero_60()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;

			int actual = necroPet.GetModified(ConstitutionID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_Level50_85()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 50;

			int actual = necroPet.GetModified(ConstitutionID);

			int expected = 85;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZeroAnd20BaseBuff_80()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.BaseBuffBonusCategory[ConstitutionID] = 20;

			int actual = necroPet.GetModified(ConstitutionID);

			int expected = 80;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZeroAnd20Item_60()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.ItemBonus[ConstitutionID] = 20;

			int actual = necroPet.GetModified(ConstitutionID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZeroAnd20Debuff_50()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.DebuffCategory[ConstitutionID] = 20;

			int actual = necroPet.GetModified(ConstitutionID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealth_LevelZero_186()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;

			int actual = necroPet.GetModified(MaxHealthID);

			int expected = 186;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealthLevel50_Init_1888()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 50;

			int actual = necroPet.GetModified(MaxHealthID);

			int expected = 1888;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealthLevelZero_WithTenMaxHealth_186()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.BaseBuffBonusCategory[MaxHealthID] = 10;

			int actual = necroPet.GetModified(MaxHealthID);

			int expected = 186;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealth_20MaxHealthDebuff_176()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.DebuffCategory[MaxHealthID] = 20;

			int actual = necroPet.GetModified(MaxHealthID);

			int expected = 176;
			Assert.AreEqual(expected, actual);
		}

		private eProperty StrengthID => eProperty.Strength;
		private eProperty ConstitutionID => eProperty.Constitution;
		private eProperty MaxHealthID => eProperty.MaxHealth;

		private NecromancerPet createNecroPet(GamePlayer owner)
		{
			var brain = Substitute.For<ABrain, IControlledBrain>();
			(brain as IControlledBrain).GetLivingOwner().Returns(owner);
			var necroPet = new TestableNecromancerPet();
			necroPet.SetOwnBrain(brain);
			return necroPet;
		}

		private class TestableNecromancerPet : NecromancerPet
		{
			public TestableNecromancerPet() : base(null, 0, 0) { }

			protected override bool LoadEquipmentTemplate(string templateID)
			{
				return true;
			}
		}
	}
}
