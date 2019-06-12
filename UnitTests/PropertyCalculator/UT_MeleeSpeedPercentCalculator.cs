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
			player.Boni.SetTo(MeleeSpeed.BaseBuff.Create(1));
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
			player.Boni.SetTo(MeleeSpeed.Debuff.Create(1));
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
			player.Boni.SetTo(MeleeSpeed.Item.Create(1));
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
			player.Boni.SetTo(MeleeSpeed.Item.Create(11));
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
			player.Boni.SetTo(MeleeSpeed.BaseBuff.Create(100));
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
			player.Boni.SetTo(MeleeSpeed.BaseBuff.Create(9));
			player.Boni.SetTo(MeleeSpeed.Item.Create(9));
			MeleeSpeedPercentCalculator speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, eProperty.MeleeSpeed);

			int expected = 82;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_With2BaseBuff_98()
		{
			var npc = Create.FakeNPC();
			npc.Level = 50;
			npc.Boni.Add(MeleeSpeed.BaseBuff.Create(2));
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(npc, eProperty.MeleeSpeed);
			int expected = 98;
			Assert.AreEqual(expected, actual);
		}

		private BonusType MeleeSpeed => new BonusType(eProperty.MeleeSpeed);

		private static MeleeSpeedPercentCalculator createMeleeSpeedCalculator()
		{
			return new MeleeSpeedPercentCalculator();
		}
	}
}
