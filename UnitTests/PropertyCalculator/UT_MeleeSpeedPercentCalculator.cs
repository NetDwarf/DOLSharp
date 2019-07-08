using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_MeleeSpeedPercentCalculator
	{
		[Test]
		public void CalcValue_Init_100()
		{
			var player = Create.FakePlayer();
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, MeleeSpeedID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeSpeedBaseBuff_99()
		{
			var player = Create.FakePlayer();
			player.BaseBuffBonusCategory[MeleeSpeedID] = 1;
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, MeleeSpeedID);

			int expected = 99;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeSpeedDebuff_100()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.DebuffCategory[MeleeSpeedID] = 1;
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, MeleeSpeedID);

			int expected = 101;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeSpeedViaItem_99()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[MeleeSpeedID] = 1;
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, MeleeSpeedID);

			int expected = 99;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_ElevenMeleeSpeedViaItem_90()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[MeleeSpeedID] = 11;
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, MeleeSpeedID);

			int expected = 90;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneHundredMeleeSpeedBaseBuff_One()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.BaseBuffBonusCategory[MeleeSpeedID] = 100;
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, MeleeSpeedID);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_9ItemAnd9BaseBuffMeleeSpeed_82()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.BaseBuffBonusCategory[MeleeSpeedID] = 9;
			player.ItemBonus[MeleeSpeedID] = 9;
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(player, MeleeSpeedID);

			int expected = 82;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_With2BaseBuff_98()
		{
			var npc = Create.FakeNPC();
			npc.Level = 50;
			npc.BaseBuffBonusCategory[MeleeSpeedID] = 2;
			var speedCalc = createMeleeSpeedCalculator();

			int actual = speedCalc.CalcValue(npc, MeleeSpeedID);
			int expected = 98;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty MeleeSpeedID => eProperty.MeleeSpeed;

		private static MeleeSpeedPercentCalculator createMeleeSpeedCalculator()
		{
			return new MeleeSpeedPercentCalculator();
		}
	}
}
