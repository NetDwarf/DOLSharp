using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_ResistCalculator
	{
		[Test]
		public void CalcValue_Init_Zero()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();

			int actual = resistCalc.CalcValue(npc, SomeResistID);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_2AbilityBonus_2()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.AbilityBonus[SomeResistID] = 2;

			int actual = resistCalc.CalcValue(npc, SomeResistID);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneItemBonus_One()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.ItemBonus[SomeResistID] = 1;

			int actual = resistCalc.CalcValue(player, SomeResistID);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneBaseBuffBonus_One()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.BaseBuffBonusCategory[SomeResistID] = 1;

			int actual = resistCalc.CalcValue(npc, SomeResistID);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_8BuffAnd6Debuff_2()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.BaseBuffBonusCategory[SomeResistID] = 8;
			npc.DebuffCategory[SomeResistID] = 6;

			int actual = resistCalc.CalcValue(npc, SomeResistID);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_6ItemAnd6Debuff_3()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[SomeResistID] = 6;
			player.DebuffCategory[SomeResistID] = 6;

			int actual = resistCalc.CalcValue(player, SomeResistID);
			int expected = 3;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_100AbilityBonus_HardcapAt70()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.AbilityBonus[SomeResistID] = 100;

			int actual = resistCalc.CalcValue(player, SomeResistID);
			int expected = 70;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWith50Item_26()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[SomeResistID] = 50;

			int actual = resistCalc.CalcValue(player, SomeResistID);
			int expected = 26;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With50ItemAnd5Mythical_31()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[SomeResistID] = 50;
			player.ItemBonus[SomeMythicalResistID] = 5;

			int actual = resistCalc.CalcValue(player, SomeResistID);
			int expected = 31;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With50ItemAnd6Mythical_31()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ItemBonus[SomeResistID] = 50;
			player.ItemBonus[SomeMythicalResistID] = 6;

			int actual = resistCalc.CalcValue(player, SomeResistID);
			int expected = 31;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_15BaseBuffAnd15ExtraBuff_24()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.BaseBuffBonusCategory[SomeResistID] = 15;
			player.BuffBonusCategory4[SomeResistID] = 15;

			int actual = resistCalc.CalcValue(player, SomeResistID);
			int expected = 24;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty SomeResistID => eProperty.Resist_First;
		private eProperty SomeMythicalResistID => eProperty.ResCapBonus_First;

		private static ResistCalculator createResistCalculator()
		{
			return new ResistCalculator();
		}
	}
}
