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

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_TwoAbilityResistAdded_Two()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Ability.Create(2, eProperty.Resist_Natural));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_TwoItemResistAdded_Two()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Item.Create(2, eProperty.Resist_Natural));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_TwoBuffResistAdded_Two()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(Bonus.BaseBuff.Create(2, eProperty.Resist_Natural));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_8BuffAnd6DebuffBodyResist_2()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(Bonus.BaseBuff.Create(8, eProperty.Resist_Natural));
			player.Boni.Add(Bonus.Debuff.Create(6, eProperty.Resist_Natural));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With6ItemAnd6Debuff_3()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Item.Create(6, eProperty.Resist_Natural));
			player.Boni.Add(Bonus.Debuff.Create(6, eProperty.Resist_Natural));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 3;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With50Item_26()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Item.Create(50, eProperty.Resist_Natural));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 26;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_50BaseBuffAnd50ExtraBuffResist_25()
		{
			var resistCalc = createNaturalResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(Bonus.ExtraBuff.Create(50, eProperty.Resist_Natural));
			player.Boni.Add(Bonus.BaseBuff.Create(50, eProperty.Resist_Natural));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Natural);
			int expected = 25;
			Assert.AreEqual(expected, actual);
		}

		private static ResistNaturalCalculator createNaturalResistCalculator()
		{
			return new ResistNaturalCalculator();
		}
	}
}
