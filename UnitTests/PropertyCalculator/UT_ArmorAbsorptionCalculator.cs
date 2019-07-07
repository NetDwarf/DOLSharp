﻿using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_ArmorAbsorptionCalculator
	{
		[Test]
		public void CalcValue_Player_100Ability_50()
		{
			var player = Create.FakePlayer();
			var calc = createCalculator();
			player.Boni.Add(ArmorAbsorptionType.Ability.Create(100));

			int actual = calc.CalcValue(player, ArmorAbsorptionType.DatabaseID);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50NPC_17()
		{
			var npc = Create.FakeNPC();
			npc.Level = 50;
			var calc = createCalculator();

			int actual = calc.CalcValue(npc, ArmorAbsorptionType.DatabaseID);

			int expected = 17;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_LevelOneNPC_MinusTen()
		{
			var npc = Create.FakeNPC();
			npc.Level = 1;
			var calc = createCalculator();

			int actual = calc.CalcValue(npc, ArmorAbsorptionType.DatabaseID);

			int expected = -10;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArmorAbsorptionType => new BonusType(eBonusType.ArmorAbsorption);

		private static IPropertyCalculator createCalculator()
		{
			return new ArmorAbsorptionCalculator();
		}
	}
}
