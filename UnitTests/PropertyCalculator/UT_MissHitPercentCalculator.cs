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
			player.Boni.Add(MissHitType.BaseBuff.Create(5));
			player.Boni.Add(MissHitType.SpecBuff.Create(4));
			player.Boni.Add(MissHitType.ExtraBuff.Create(3));

			int actual = calc.CalcValue(player, MissHitType.DatabaseID);

			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_5Debuff_Minus5()
		{
			var player = Create.FakePlayer();
			var calc = createMissHitCalculator();
			player.Boni.Add(MissHitType.Debuff.Create(5));

			int actual = calc.CalcValue(player, MissHitType.DatabaseID);

			int expected = -5;
			Assert.AreEqual(expected, actual);
		}

		private BonusType MissHitType => new BonusType(eBonusType.MissHit);

		private static IPropertyCalculator createMissHitCalculator()
		{
			return new MissHitPercentCalculator();
		}
	}
}