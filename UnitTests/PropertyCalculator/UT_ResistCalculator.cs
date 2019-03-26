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

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_First, true);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_2AbilityResistAdded_One()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.Ability.Create(2, eProperty.Resist_First));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_First, true);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneItemResistAdded_One()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(Bonus.Item.Create(1, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body, true);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneBuffResistAdded_One()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.BaseBuff.Create(1, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body, true);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_8BuffAnd6DebuffBodyResist_2()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.BaseBuff.Create(8, eProperty.Resist_Body));
			npc.Boni.Add(Bonus.Debuff.Create(6, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body, true);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_6ItemAnd6Debuff_3()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Item.Create(6, eProperty.Resist_Body));
			player.Boni.Add(Bonus.Debuff.Create(6, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body, true);
			int expected = 3;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void HardCap_100AbilityBodyResist_CalcValueIs70()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.Ability.Create(100, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body, true);
			int expected = 70;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWith50Item_26()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Item.Create(50, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body, true);
			int expected = 26;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWith50ItemResistAnd5ItemResistOvercap_31()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Item.Create(50, eProperty.Resist_Body));
			player.Boni.Add(Bonus.Item.Create(5, eProperty.BodyResCapBonus));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body, true);
			int expected = 31;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWith50ItemResistAnd6ItemResistOvercap_31()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(Bonus.Item.Create(50, eProperty.Resist_Body));
			player.Boni.Add(Bonus.Item.Create(6, eProperty.BodyResCapBonus));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body, true);
			int expected = 31;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_15BaseBuffAnd15ExtraBuffResist_24()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(Bonus.ExtraBuff.Create(15, eProperty.Resist_Body));
			player.Boni.Add(Bonus.BaseBuff.Create(15, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body, true);
			int expected = 24;
			Assert.AreEqual(expected, actual);
		}

		private static ResistCalculator createResistCalculator()
		{
			return new ResistCalculator();
		}
	}
}
