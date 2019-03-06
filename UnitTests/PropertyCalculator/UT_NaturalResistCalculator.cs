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
			var npc = Create.FakeNPC();

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_First);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneAbilityResistAdded_One()
		{
			var resistCalc = createNaturalResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.Ability.Create(1, eProperty.Resist_First));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_First);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneItemResistAdded_One()
		{
			var resistCalc = createNaturalResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.Item.Create(1, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_OneBuffResistAdded_One()
		{
			var resistCalc = createNaturalResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.BaseBuff.Create(1, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_8BuffAnd6DebuffBodyResist_2()
		{
			var resistCalc = createNaturalResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.BaseBuff.Create(8, eProperty.Resist_Body));
			npc.Boni.Add(Bonus.Debuff.Create(6, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_6ItemAnd6Debuff_3()
		{
			var resistCalc = createNaturalResistCalculator();
			var npc = Create.FakeNPC();
			npc.Level = 50;
			npc.Boni.Add(Bonus.Item.Create(6, eProperty.Resist_Body));
			npc.Boni.Add(Bonus.Debuff.Create(6, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 3;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50NPCWith50Item_26()
		{
			var resistCalc = createNaturalResistCalculator();
			var npc = Create.FakeNPC();
			npc.Level = 50;
			npc.Boni.Add(Bonus.Item.Create(50, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 26;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_50BaseBuffAnd50ExtraBuffResist_25()
		{
			var resistCalc = createNaturalResistCalculator();
			var npc = Create.FakeNPC();
			npc.Boni.Add(Bonus.ExtraBuff.Create(50, eProperty.Resist_Body));
			npc.Boni.Add(Bonus.BaseBuff.Create(50, eProperty.Resist_Body));

			int actual = resistCalc.CalcValue(npc, eProperty.Resist_Body);
			int expected = 25;
			Assert.AreEqual(expected, actual);
		}

		private static ResistNaturalCalculator createNaturalResistCalculator()
		{
			return new ResistNaturalCalculator();
		}
	}
}
