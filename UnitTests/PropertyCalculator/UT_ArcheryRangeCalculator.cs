﻿using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;
using static DOL.GS.GameObject;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_ArcheryRangeCalculator
	{
		[Test]
		public void CalcValue_Player_5ItemBonus5BaseBuffBonus_105()
		{
			var player = Create.FakePlayer();
			var calc = createArcheryRangeCalculator();
			player.SpecBuffBonusCategory[ArcheryRangeID] = 5;
			player.ItemBonus[ArcheryRangeID] = 5;

			int actual = calc.CalcValue(player, ArcheryRangeID);

			int expected = 105;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12SpecBuffBonus_100()
		{
			var player = Create.FakePlayer();
			var calc = createArcheryRangeCalculator();
			player.SpecBuffBonusCategory[ArcheryRangeID] = 12;

			int actual = calc.CalcValue(player, ArcheryRangeID);

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

			int actual = calc.CalcValue(player, ArcheryRangeID);

			int expected = 150;
			Assert.AreEqual(expected, actual);
		}

		private eProperty ArcheryRangeID => eProperty.ArcheryRange;

		private static IPropertyCalculator createArcheryRangeCalculator()
		{
			return new ArcheryRangePercentCalculator();
		}
	}
}
