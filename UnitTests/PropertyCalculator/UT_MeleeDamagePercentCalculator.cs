using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_MeleeDamagePercentCalculator
	{
		[Test]
		public void CalcValue_Init_Zero()
		{
			var player = Create.FakePlayer();
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, MeleeDamageID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeDamageBaseBuff_One()
		{
			var player = Create.FakePlayer();
			player.BaseBuffBonusCategory[MeleeDamageID] = 1;
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, MeleeDamageID);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_ElevenDamageDebuff_MinusTen()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.DebuffCategory[MeleeDamageID] = 11;
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, MeleeDamageID);

			int expected = -10;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeDamageViaItem_One()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.AbilityBonus[MeleeDamageID] = 1;
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, MeleeDamageID);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_ElevenMeleeDamageViaItem_90()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[MeleeDamageID] = 11;
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, MeleeDamageID);

			int expected = 10;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_9ItemAnd9BaseBuffMeleeDamage_18()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.BaseBuffBonusCategory[MeleeDamageID] = 9;
			player.ItemBonus[MeleeDamageID] = 9;
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, MeleeDamageID);

			int expected = 18;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_ElevenAbilityMeleeDamage_Eleven()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.AbilityBonus[MeleeDamageID] = 11;
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, MeleeDamageID);

			int expected = 11;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty MeleeDamageID => eProperty.MeleeDamage;

		private static MeleeDamagePercentCalculator createMeleeDamageCalculator()
		{
			return new MeleeDamagePercentCalculator();
		}
	}
}

