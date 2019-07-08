using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;
using NSubstitute;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_NaturalResistCalculator
	{
		[Test]
		public void CalcValue_Init_Zero()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_TwoAbilityResistAdded_Two()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.AbilityBonus[NaturalResistID] = 2;

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_TwoItemResistAdded_Two()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[NaturalResistID] = 2;

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_TwoBuffResistAdded_Two()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.BaseBuffBonusCategory[NaturalResistID] = 2;

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_8BuffAnd6DebuffBodyResist_2()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.BaseBuffBonusCategory[NaturalResistID] = 8;
			player.DebuffCategory[NaturalResistID] = 6;

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With6ItemAnd6Debuff_3()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[NaturalResistID] = 6;
			player.DebuffCategory[NaturalResistID] = 6;

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 3;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With50Item_26()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[NaturalResistID] = 50;

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 26;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_50BaseBuffAnd50ExtraBuffResist_25()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.BaseBuffBonusCategory[NaturalResistID] = 50;
			player.BuffBonusCategory4[NaturalResistID] = 50;

			int actual = resistCalc.CalcValue(player, NaturalResistID);
			int expected = 25;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty NaturalResistID => eProperty.Resist_Natural;

		private static ResistNaturalCalculator createNaturalResistCalculator()
		{
			return new ResistNaturalCalculator();
		}
	}
}
