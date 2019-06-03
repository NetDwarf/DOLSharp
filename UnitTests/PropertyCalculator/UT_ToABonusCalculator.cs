using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;
using DOL.GS.Spells;
using DOL.Database;
using DOL.GS.Effects;
using NSubstitute;
using static DOL.GS.GameObject;
using DOL.GS.Keeps;

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

		private BonusType ArcherySpeedBonus => new BonusType(eProperty.ArcherySpeed);

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
		public void CalcValue_Player_120Debuff_Zero()
		{
			var player = Create.FakePlayer();
			var calc = createSpellRangeCalculator();
			player.Boni.Add(SpellRangeBonus.Debuff.Create(120));

			int actual = calc.CalcValue(player, SpellRangeBonus.ID);

			int expected = 0;
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

		private BonusType SpellRangeBonus => new BonusType(eProperty.SpellRange);

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

		private BonusType ArcheryRangeBonus => new BonusType(eProperty.ArcheryRange);

		private static IPropertyCalculator createArcheryRangeCalculator()
		{
			return new ArcheryRangePercentCalculator();
		}
	}

	[TestFixture]
	public class UT_MissHitPercentCalculator
	{
		[Test]
		public void CalcValue_Player_5BaseBuff4SpecBuff3ExtraBuff_12()
		{
			var player = Create.FakePlayer();
			var calc = createMissHitCalculator();
			player.Boni.Add(MissHitType.BaseBuff.Create(5));
			player.Boni.Add(MissHitType.SpecBuff.Create(4));
			player.Boni.Add(MissHitType.ExtraBuff.Create(3));

			int actual = calc.CalcValue(player, MissHitType.ID);

			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5Debuff_Minus5()
		{
			var player = Create.FakePlayer();
			var calc = createMissHitCalculator();
			player.Boni.Add(MissHitType.Debuff.Create(5));

			int actual = calc.CalcValue(player, MissHitType.ID);

			int expected = -5;
			Assert.AreEqual(expected, actual);
		}

		private BonusType MissHitType => new BonusType(eProperty.MissHit);

		private static IPropertyCalculator createMissHitCalculator()
		{
			return new MissHitPercentCalculator();
		}
	}

	[TestFixture]
	public class UT_ArcaneSyphoneCalculator
	{
		[Test]
		public void CalcValue_Player_30Item_25()
		{
			var player = Create.FakePlayer();
			var calc = createArcaneSyphonCalculator();
			player.Boni.Add(ArcaneSyphonType.Item.Create(30));

			int actual = calc.CalcValue(player, ArcaneSyphonType.ID);

			int expected = 25;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArcaneSyphonType => new BonusType(eProperty.ArcaneSyphon);

		private static IPropertyCalculator createArcaneSyphonCalculator()
		{
			return new ArcaneSyphonCalculator();
		}
	}

	[TestFixture]
	public class UT_ArmorFactorCalculator
	{
		[Test]
		public void CalcValue_L25Player_30Item_25()
		{
			var player = Create.FakePlayer();
			player.Level = 25;
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.Item.Create(30));

			int actual = calc.CalcValue(player, ArmorFactorType.ID);

			int expected = 25;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_100SpecBuff_93()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.SpecBuff.Create(100));

			int actual = calc.CalcValue(player, ArmorFactorType.ID);

			int expected = 93;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12Debuff_Minus12()
		{
			var player = Create.FakePlayer();
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.Debuff.Create(12));

			int actual = calc.CalcValue(player, ArmorFactorType.ID);

			int expected = -12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_100ExtraBuff_Hundred()
		{
			var player = Create.FakePlayer();
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.ExtraBuff.Create(100));

			int actual = calc.CalcValue(player, ArmorFactorType.ID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50NPC_604()
		{
			var player = Create.FakeNPC();
			player.Level = 50;
			var calc = createCalculator();

			int actual = calc.CalcValue(player, ArmorFactorType.ID);

			int expected = 604;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_BaseLevel50KeepDoor_50()
		{
			var keep = new GameKeep();
			keep.DBKeep = new DBKeep();
			keep.BaseLevel = 50;
			var keepDoor = new GameKeepDoor();
			keepDoor.Component = new GameKeepComponent();
			keepDoor.Component.AbstractKeep = keep;
			var calc = createCalculator();

			int actual = calc.CalcValue(keepDoor, ArmorFactorType.ID);
			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_BaseLevel50TowerDoor_25()
		{
			var keep = new GameKeepTower();
			keep.DBKeep = new DBKeep();
			keep.BaseLevel = 50;
			var keepDoor = new GameKeepDoor();
			keepDoor.Component = new GameKeepComponent();
			keepDoor.Component.AbstractKeep = keep;
			var calc = createCalculator();

			int actual = calc.CalcValue(keepDoor, ArmorFactorType.ID);
			int expected = 25;
			Assert.AreEqual(expected, actual);
		}


		private BonusType ArmorFactorType => new BonusType(eProperty.ArmorFactor);

		private static IPropertyCalculator createCalculator()
		{
			return new ArmorFactorCalculator();
		}
	}

	[TestFixture]
	public class UT_ArmorAbsorptionCalculator
	{
		[Test]
		public void CalcValue_Player_100Ability_50()
		{
			var player = Create.FakePlayer();
			var calc = createCalculator();
			player.Boni.Add(ArmorAbsorptionType.Ability.Create(100));

			int actual = calc.CalcValue(player, ArmorAbsorptionType.ID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50NPC_17()
		{
			var npc = Create.FakeNPC();
			npc.Level = 50;
			var calc = createCalculator();

			int actual = calc.CalcValue(npc, ArmorAbsorptionType.ID);

			int expected = 17;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_LevelOneNPC_MinusTen()
		{
			var npc = Create.FakeNPC();
			npc.Level = 1;
			var calc = createCalculator();

			int actual = calc.CalcValue(npc, ArmorAbsorptionType.ID);

			int expected = -10;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArmorAbsorptionType => new BonusType(eProperty.ArmorAbsorption);

		private static IPropertyCalculator createCalculator()
		{
			return new ArmorAbsorptionCalculator();
		}
	}
}
