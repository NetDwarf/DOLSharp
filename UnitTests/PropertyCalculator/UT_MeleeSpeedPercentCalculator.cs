using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_MeleeSpeedPercentCalculator
	{
		[Test]
		public void CalcValue_NoMeleeSpeedBoni_OneHundred()
		{
			var player = Create.FakePlayer();
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeSpeedBaseBuff_99()
		{
			var player = Create.FakePlayer();
			player.Boni.SetTo(Bonus.BaseBuff.Create(1, eProperty.MeleeSpeed));
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 99;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeSpeedDebuff_100()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Debuff.Create(1, eProperty.MeleeSpeed));
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 101;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeSpeedViaItem_99()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Item.Create(1, eProperty.MeleeSpeed));
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 99;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_ElevenMeleeSpeedViaItem_90()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Item.Create(11, eProperty.MeleeSpeed));
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 90;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneHundredMeleeSpeedBaseBuff_One()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.BaseBuff.Create(100, eProperty.MeleeSpeed));
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_9ItemAnd9BaseBuffMeleeSpeed_82()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.BaseBuff.Create(9, eProperty.MeleeSpeed));
			player.Boni.SetTo(Bonus.Item.Create(9, eProperty.MeleeSpeed));
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 82;
			Assert.AreEqual(expected, actual);
		}

		private static MeleeSpeedPercentCalculator createMeleeSpeedCalculator()
		{
			return new MeleeSpeedPercentCalculator();
		}
	}
}
