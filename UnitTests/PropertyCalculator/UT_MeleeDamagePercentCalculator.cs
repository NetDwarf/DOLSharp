using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_MeleeDamagePercentCalculator
	{
		[Test]
		public void CalcValue_NoMeleeDamageBoni_Zero()
		{
			var player = Create.FakePlayer();
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, eProperty.MeleeDamage);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeDamageBaseBuff_One()
		{
			var player = Create.FakePlayer();
			player.Boni.SetTo(Bonus.BaseBuff.Create(1, eProperty.MeleeDamage));
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, eProperty.MeleeDamage);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeDamageDebuff_MinusOne()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Debuff.Create(1, eProperty.MeleeDamage));
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, eProperty.MeleeDamage);

			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneMeleeDamageViaItem_One()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Item.Create(1, eProperty.MeleeDamage));
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, eProperty.MeleeDamage);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_ElevenMeleeDamageViaItem_90()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Item.Create(11, eProperty.MeleeDamage));
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, eProperty.MeleeDamage);

			int expected = 10;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_9ItemAnd9BaseBuffMeleeDamage_18()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.BaseBuff.Create(9, eProperty.MeleeDamage));
			player.Boni.SetTo(Bonus.Item.Create(9, eProperty.MeleeDamage));
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, eProperty.MeleeDamage);

			int expected = 18;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_ElevenAbilityMeleeDamage_Eleven()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Ability.Create(11, eProperty.MeleeDamage));
			var meleeDamageCalc = createMeleeDamageCalculator();

			int actual = meleeDamageCalc.CalcValue(player, eProperty.MeleeDamage);

			int expected = 11;
			Assert.AreEqual(expected, actual);
		}

		private static MeleeDamagePercentCalculator createMeleeDamageCalculator()
		{
			return new MeleeDamagePercentCalculator();
		}
	}
}

