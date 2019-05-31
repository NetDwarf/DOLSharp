using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;
using DOL.GS.Spells;
using DOL.Database;
using DOL.GS.Effects;
using NSubstitute;
using static DOL.GS.GameObject;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_ArcherySpeedCalculator
	{
		[Test]
		public void CalcValue_Player_5ItemBonus_5()
		{
			var player = Create.FakePlayer();
			player.Boni.Add(ArcherySpeedBonus.Item.Create(5));
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedBonus.ID);

			int expected = 5;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_OnCreation_Zero()
		{
			var player = Create.FakeNPC();
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedBonus.ID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With2ItemBonus2Debuff_Zero()
		{
			var player = Create.FakePlayer();
			player.Boni.Add(ArcherySpeedBonus.Item.Create(2));
			player.Boni.Add(ArcherySpeedBonus.Debuff.Create(2));
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedBonus.ID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With15ItemBonus_Ten()
		{
			var player = Create.FakePlayer();
			player.Boni.Add(ArcherySpeedBonus.Item.Create(15));
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedBonus.ID);

			int expected = 10;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArcherySpeedBonus
		{
			get
			{
				return new BonusType(eProperty.ArcherySpeed);
			}
		}

		private static IPropertyCalculator createArcherySpeedCalculator()
		{
			return new ArcherySpeedPercentCalculator();
		}
	}

	[TestFixture]
	public class UT_SpellRangeCalculator
	{
		[Test]
		public void CalcValue_NPC_OnCreation_100()
		{
			var npc = Create.FakeNPC();
			var calc = createSpellRangeCalculator();

			int actual = calc.CalcValue(npc, SpellRangeBonus.ID);

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

			int actual = calc.CalcValue(player, SpellRangeBonus.ID);

			int expected = 110;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12ItemBonus_110()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Item.Create(12));

			int actual = calc.CalcValue(player, SpellRangeBonus.ID);

			int expected = 110;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12SpecBuffBonus_105()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.SpecBuff.Create(12));

			int actual = calc.CalcValue(player, SpellRangeBonus.ID);

			int expected = 105;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12Debuff_88()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Debuff.Create(12));

			int actual = calc.CalcValue(player, SpellRangeBonus.ID);

			int expected = 88;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12DebuffAnd25NearsightReductionSpell_91()
		{
			var player = Create.FakePlayer();
			player.ObjectState = eObjectState.Active;
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Debuff.Create(12));
			//apply spell
			var spell = Create.Spell();
			spell.Value = 25;
			spell = new Spell(spell, "NearsightReduction");
			var nearsightReduction = new NearsightReductionSpellHandler(player, spell, null);
			player.EffectList.Add(new GameSpellEffect(nearsightReduction, 1, 0));

			int actual = calc.CalcValue(player, SpellRangeBonus.ID);

			int expected = 91;
			Assert.AreEqual(expected, actual);
		}

		private BonusType SpellRangeBonus
		{
			get
			{
				return new BonusType(eProperty.SpellRange);
			}
		}

		private static IPropertyCalculator createSpellRangeCalculator()
		{
			return new SpellRangePercentCalculator();
		}
	}

	[TestFixture]
	public class UT_ArcheryRangeCalculator
	{
		[Test]
		public void CalcValue_Player_5ItemBonus5BaseBuffBonus_105()
		{
			var player = Create.FakePlayer();
			var calc = createArcheryRangeCalculator();
			player.Boni.Add(ArcheryRangeBonus.SpecBuff.Create(5));
			player.Boni.Add(ArcheryRangeBonus.Item.Create(5));

			int actual = calc.CalcValue(player, ArcheryRangeBonus.ID);

			int expected = 105;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12SpecBuffBonus_100()
		{
			var player = Create.FakePlayer();
			var calc = createArcheryRangeCalculator();
			player.Boni.Add(ArcheryRangeBonus.SpecBuff.Create(12));

			int actual = calc.CalcValue(player, ArcheryRangeBonus.ID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_TrueshotEffect_150()
		{
			var player = Create.FakePlayer();
			player.ObjectState = eObjectState.Active;
			var calc = createArcheryRangeCalculator();
			player.RangedAttackType = GameLiving.eRangedAttackType.Long;

			int actual = calc.CalcValue(player, ArcheryRangeBonus.ID);

			int expected = 150;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArcheryRangeBonus
		{
			get
			{
				return new BonusType(eProperty.ArcheryRange);
			}
		}

		private static IPropertyCalculator createArcheryRangeCalculator()
		{
			return new ArcheryRangePercentCalculator();
		}
	}
}
