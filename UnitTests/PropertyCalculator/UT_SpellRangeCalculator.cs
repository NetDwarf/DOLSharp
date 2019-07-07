using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;
using DOL.GS.Spells;
using DOL.GS.Effects;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_SpellRangeCalculator
	{
		[Test]
		public void CalcValue_NPC_OnCreation_100()
		{
			var npc = Create.FakeNPC();
			var calc = createSpellRangeCalculator();

			int actual = calc.CalcValue(npc, SpellRangeBonus.DatabaseID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5ItemBonus5BaseBuffBonus_110()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.SpecBuff.Create(5));
			player.Boni.Add(SpellRangeBonus.Item.Create(5));

			int actual = calc.CalcValue(player, SpellRangeBonus.DatabaseID);

			int expected = 110;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12ItemBonus_110()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Item.Create(12));

			int actual = calc.CalcValue(player, SpellRangeBonus.DatabaseID);

			int expected = 110;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12SpecBuffBonus_105()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.SpecBuff.Create(12));

			int actual = calc.CalcValue(player, SpellRangeBonus.DatabaseID);

			int expected = 105;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12Debuff_88()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Debuff.Create(12));

			int actual = calc.CalcValue(player, SpellRangeBonus.DatabaseID);

			int expected = 88;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_120Debuff_Zero()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Debuff.Create(120));

			int actual = calc.CalcValue(player, SpellRangeBonus.DatabaseID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12DebuffAnd25NearsightReductionSpell_91()
		{
			var player = Create.FakePlayer();
			player.ObjectState = GameObject.eObjectState.Active;
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Debuff.Create(12));
			//apply spell
			var spell = Create.Spell();
			spell.Value = 25;
			spell = new Spell(spell, "NearsightReduction");
			var nearsightReduction = new NearsightReductionSpellHandler(player, spell, null);
			player.EffectList.Add(new GameSpellEffect(nearsightReduction, 1, 0));

			int actual = calc.CalcValue(player, SpellRangeBonus.DatabaseID);

			int expected = 91;
			Assert.AreEqual(expected, actual);
		}

		private BonusType SpellRangeBonus => new BonusType(eBonusType.SpellRange);

		private static IPropertyCalculator createSpellRangeCalculator()
		{
			return new SpellRangePercentCalculator();
		}
	}
}
