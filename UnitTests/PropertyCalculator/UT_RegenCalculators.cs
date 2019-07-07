using DOL.GS;
using DOL.GS.PropertyCalc;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_EnduranceRegenerationRateCalculator
	{
		[TestFixtureSetUp]
		public void init()
		{
			setRoundingDown();
		}

		[Test]
		public void CalcValue_PlayerOutOfCombat_5()
		{
			var living = Create.FakePlayer();
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, EndurangeRegenType.DatabaseID);

			int expected = 5;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_PlayerOutOfCombat_6BaseBuffAnd7ItemBonus_13()
		{
			var living = Create.FakePlayer();
			living.Boni.Add(EndurangeRegenType.BaseBuff.Create(6));
			living.Boni.Add(EndurangeRegenType.Item.Create(7));
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, EndurangeRegenType.DatabaseID);

			int expected = 17;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_PlayerInCombat_One()
		{
			var living = Create.FakePlayer();
			living.isInCombat = true;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, EndurangeRegenType.DatabaseID);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_PlayerIsSprintingOutOfCombat_One()
		{
			var living = Create.FakePlayer();
			living.isSprinting = true;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, EndurangeRegenType.DatabaseID);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private BonusType EndurangeRegenType => new BonusType(eBonusType.EnduranceRegenerationRate);

		private IPropertyCalculator createCalculator()
		{
			var calc = new EnduranceRegenerationRateCalculator();
			return calc;
		}

		private void setRoundingDown()
		{
			Util.LoadTestDouble(new ConstantRandomUtil(1));
		}

		private void setRegenMultiplier(double value)
		{
			GS.ServerProperties.Properties.ENDURANCE_REGEN_RATE = value;
		}
	}

	[TestFixture]
	public class UT_PowerRegenerationRateCalculator
	{
		[TestFixtureSetUp]
		public void init()
		{
			setRoundingDown();
		}

		[Test]
		public void CalcValue_L50Player_OutOfCombat_23()
		{
			var living = Create.FakePlayer();
			living.Level = 50;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, PowerRegenType.DatabaseID);

			int expected = 23;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5ItemBonus6BaseBuffBonus_16()
		{
			var living = Create.FakePlayer();
			living.isInCombat = true;
			living.Boni.Add(PowerRegenType.Item.Create(5));
			living.Boni.Add(PowerRegenType.BaseBuff.Create(6));
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, PowerRegenType.DatabaseID);

			int expected = 16;
			Assert.AreEqual(expected, actual);
		}

		private BonusType PowerRegenType => new BonusType(eBonusType.PowerRegenerationRate);

		private IPropertyCalculator createCalculator()
		{
			var calc = new PowerRegenerationRateCalculator();
			return calc;
		}

		private void setRoundingDown()
		{
			Util.LoadTestDouble(new ConstantRandomUtil(1));
		}

		private void setRegenMultiplier(double value)
		{
			GS.ServerProperties.Properties.MANA_REGEN_RATE = value;
		}
	}

	[TestFixture]
	public class UT_HealthRegenerationRateCalculator
	{
		[TestFixtureSetUp]
		public void init()
		{
			setRoundingDown();
		}

		[Test]
		public void CalcValue_L50Player_OutOfCombat_30()
		{
			var living = Create.FakePlayer();
			living.Level = 50;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, HealthRegenType.DatabaseID);

			int expected = 30;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50NPC_InCombat_30()
		{
			var living = Create.FakeNPC();
			living.Level = 50;
			living.isInCombat = true;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, HealthRegenType.DatabaseID);

			int expected = 15;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5ItemBonus6BaseBuffBonus_21()
		{
			var living = Create.FakePlayer();
			living.isInCombat = true;
			living.Boni.Add(HealthRegenType.Item.Create(5));
			living.Boni.Add(HealthRegenType.BaseBuff.Create(6));
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, HealthRegenType.DatabaseID);

			int expected = 21;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_KeepDoor_1000MaxHealth_50()
		{
			var living = Create.FakeKeepDoor();
			living.maxHealth = 1000;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, HealthRegenType.DatabaseID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		private BonusType HealthRegenType => new BonusType(eBonusType.HealthRegenerationRate);

		private IPropertyCalculator createCalculator()
		{
			var calc = new HealthRegenerationRateCalculator();
			return calc;
		}

		private void setRoundingDown()
		{
			Util.LoadTestDouble(new ConstantRandomUtil(1));
		}

		private void setRegenMultiplier(double value)
		{
			GS.ServerProperties.Properties.HEALTH_REGEN_RATE = value;
		}
	}
}
