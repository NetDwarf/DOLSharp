using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_FatigueCalculator
	{
		[Test]
		public void CalcValue_NPC_100()
		{
			var living = Create.FakeNPC();
			var calc = createFatigueCalculator();

			int actual = calc.CalcValue(living, EnduranceID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_100()
		{
			var player = Create.FakePlayer();
			var calc = createFatigueCalculator();

			int actual = calc.CalcValue(player, EnduranceID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With5ItemEnduranceBonus_105()
		{
			var player = Create.FakePlayer();
			player.ItemBonus[EnduranceID] = 5;
			var calc = createFatigueCalculator();

			int actual = calc.CalcValue(player, EnduranceID);

			int expected = 105;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With100ItemEnduranceBonus_115()
		{
			var player = Create.FakePlayer();
			player.ItemBonus[EnduranceID] = 100;
			var calc = createFatigueCalculator();

			int actual = calc.CalcValue(player, EnduranceID);

			int expected = 115;
			Assert.AreEqual(expected, actual);
		}

		private eProperty EnduranceID => eProperty.Fatigue;

		private FatigueCalculator createFatigueCalculator()
		{
			return new FatigueCalculator();
		}
	}
}
