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

			int actual = resistCalc.CalcValue(npc, SomeResistDatabaseID);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_2AbilityBonus_2()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(BodyResist.Ability.Create(2));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneItemBonus_One()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(BodyResist.Item.Create(1));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneBaseBuffBonus_One()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(BodyResist.BaseBuff.Create(1));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_8BuffAnd6Debuff_2()
		{
			var resistCalc = createResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(BodyResist.BaseBuff.Create(8));
			npc.Boni.Add(BodyResist.Debuff.Create(6));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_6ItemAnd6Debuff_3()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(BodyResist.Item.Create(6));
			player.Boni.Add(BodyResist.Debuff.Create(6));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body);
			int expected = 3;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_100AbilityBonus_HardcapAt70()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(BodyResist.Ability.Create(100));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body);
			int expected = 70;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWith50Item_26()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(BodyResist.Item.Create(50));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body);
			int expected = 26;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With50ItemAnd5Mythical_31()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(BodyResist.Item.Create(50));
			player.Boni.Add(BodyResist.Mythical.Create(5));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body);
			int expected = 31;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_With50ItemAnd6Mythical_31()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.Add(BodyResist.Item.Create(50));
			player.Boni.Add(BodyResist.Mythical.Create(6));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body);
			int expected = 31;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_15BaseBuffAnd15ExtraBuff_24()
		{
			var resistCalc = createResistCalculator();
			var player = Create.FakePlayer();
			player.Boni.Add(BodyResist.ExtraBuff.Create(15));
			player.Boni.Add(BodyResist.BaseBuff.Create(15));

			int actual = resistCalc.CalcValue(player, eProperty.Resist_Body);
			int expected = 24;
			Assert.AreEqual(expected, actual);
		}

		private BonusType BodyResist => new BonusType(eBonusType.Resist_Body);
		private eProperty SomeResistDatabaseID => eProperty.Resist_First;

		private static ResistCalculator createResistCalculator()
		{
			return new ResistCalculator();
		}
	}
}
