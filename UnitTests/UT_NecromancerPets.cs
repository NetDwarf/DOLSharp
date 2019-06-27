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

			int actual = necroPet.GetModified(Strength.DatabaseID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_Level50_110()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 50;

			int actual = necroPet.GetModified(Strength.DatabaseID);

			int expected = 110;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAndOwnerHas20ItemBonus_80()
		{
			var owner = Create.FakePlayer();
			owner.Level = 50;
			owner.Boni.Add(Strength.Item.Create(20));
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;

			int actual = necroPet.GetModified(Strength.DatabaseID);

			int expected = 80;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAnd20BuffBonus_80()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(Strength.BaseBuff.Create(20));

			int actual = necroPet.GetModified(Strength.DatabaseID);

			int expected = 80;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAnd20ItemBonus_60()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(Strength.Item.Create(20));

			int actual = necroPet.GetModified(Strength.DatabaseID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Strength_LevelZeroAnd20Debuff_50()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(Strength.Debuff.Create(20));

			int actual = necroPet.GetModified(Strength.DatabaseID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZero_60()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;

			int actual = necroPet.GetModified(Constitution.DatabaseID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_Level50_85()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 50;

			int actual = necroPet.GetModified(Constitution.DatabaseID);

			int expected = 85;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZeroAnd20BaseBuff_80()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(Constitution.BaseBuff.Create(20));

			int actual = necroPet.GetModified(Constitution.DatabaseID);

			int expected = 80;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZeroAnd20Item_60()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(Constitution.Item.Create(20));

			int actual = necroPet.GetModified(Constitution.DatabaseID);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_Constitution_LevelZeroAnd20Debuff_50()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(Constitution.Debuff.Create(20));

			int actual = necroPet.GetModified(Constitution.DatabaseID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealth_LevelZero_186()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;

			int actual = necroPet.GetModified(MaxHealth.DatabaseID);

			int expected = 186;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealth_Level50_1888()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 50;

			int actual = necroPet.GetModified(MaxHealth.DatabaseID);

			int expected = 1888;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealth_LevelZeroAndTenConstitution_186()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(MaxHealth.BaseBuff.Create(10));

			int actual = necroPet.GetModified(MaxHealth.DatabaseID);

			int expected = 186;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetModified_MaxHealth_20MaxHealthDebuff_176()
		{
			var owner = Create.FakePlayer();
			var necroPet = createNecroPet(owner);
			necroPet.Level = 0;
			necroPet.Boni.Add(MaxHealth.Debuff.Create(20));

			int actual = necroPet.GetModified(MaxHealth.DatabaseID);

			int expected = 176;
			Assert.AreEqual(expected, actual);
		}

		private BonusType Strength => Bonus.Strength;
		private BonusType Constitution => Bonus.Constitution;
		private BonusType MaxHealth => Bonus.HealthPool;

		private NecromancerPet createNecroPet(GamePlayer owner)
		{
			var brain = Substitute.For<ABrain, IControlledBrain>();
			(brain as IControlledBrain).GetLivingOwner().Returns(owner);
			return NecromancerPet.CreateTestableNecromancerPet(brain, 0, 0);
		}
	}
}
