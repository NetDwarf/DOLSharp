using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_MissHitPercentCalculator
	{
		[Test]
		public void CalcValue_Player_5BaseBuff4SpecBuff3ExtraBuff_12()
		{
			var player = Create.FakePlayer();
			var calc = createMissHitCalculator();
			player.BaseBuffBonusCategory[MissHitID] = 5;
			player.SpecBuffBonusCategory[MissHitID] = 4;
			player.BuffBonusCategory4[MissHitID] = 3;

			int actual = calc.CalcValue(player, MissHitID);

			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5Debuff_Minus5()
		{
			var player = Create.FakePlayer();
			var calc = createMissHitCalculator();
			player.DebuffCategory[MissHitID] = 5;

			int actual = calc.CalcValue(player, MissHitID);

			int expected = -5;
			Assert.AreEqual(expected, actual);
		}

		private eProperty MissHitID => eProperty.MissHit;

		private static IPropertyCalculator createMissHitCalculator()
		{
			return new MissHitPercentCalculator();
		}
	}
}