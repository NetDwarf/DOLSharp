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

			int actual = calc.CalcValue(living, EnduranceRegenID);

			int expected = 5;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_PlayerOutOfCombat_6BaseBuffAnd7ItemBonus_13()
		{
			var living = Create.FakePlayer();
			living.BaseBuffBonusCategory[EnduranceRegenID] = 6;
			living.ItemBonus[EnduranceRegenID] = 7;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, EnduranceRegenID);

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

			int actual = calc.CalcValue(living, EnduranceRegenID);

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

			int actual = calc.CalcValue(living, EnduranceRegenID);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty EnduranceRegenID => eProperty.EnduranceRegenerationRate;

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

			int actual = calc.CalcValue(living, PowerRegenID);

			int expected = 23;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5ItemBonus6BaseBuffBonus_16()
		{
			var living = Create.FakePlayer();
			living.isInCombat = true;
			living.ItemBonus[PowerRegenID] = 5;
			living.BaseBuffBonusCategory[PowerRegenID] = 6;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, PowerRegenID);

			int expected = 16;
			Assert.AreEqual(expected, actual);
		}

		private eProperty PowerRegenID => eProperty.PowerRegenerationRate;

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

			int actual = calc.CalcValue(living, HealthRegenID);

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

			int actual = calc.CalcValue(living, HealthRegenID);

			int expected = 15;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5ItemBonus6BaseBuffBonus_21()
		{
			var living = Create.FakePlayer();
			living.isInCombat = true;
			living.ItemBonus[HealthRegenID] = 5;
			living.BaseBuffBonusCategory[HealthRegenID] = 6;
			var calc = createCalculator();
			setRegenMultiplier(1);

			int actual = calc.CalcValue(living, HealthRegenID);

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

			int actual = calc.CalcValue(living, HealthRegenID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}
		private eProperty HealthRegenID => eProperty.HealthRegenerationRate;

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
