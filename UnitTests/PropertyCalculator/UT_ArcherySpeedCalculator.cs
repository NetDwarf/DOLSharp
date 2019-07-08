using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_ArcherySpeedCalculator
	{
		[Test]
		public void CalcValue_Player_5ItemBonus_5()
		{
			var player = Create.FakePlayer();
			player.ItemBonus[ArcherySpeedID] = 5;
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedID);

			int expected = 5;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_OnCreation_Zero()
		{
			var player = Create.FakeNPC();
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With2ItemBonus2Debuff_Zero()
		{
			var player = Create.FakePlayer();
			player.ItemBonus[ArcherySpeedID] = 2;
			player.DebuffCategory[ArcherySpeedID] = 2;
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With15ItemBonus_Ten()
		{
			var player = Create.FakePlayer();
			player.ItemBonus[ArcherySpeedID] = 15;
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedID);

			int expected = 10;
			Assert.AreEqual(expected, actual);
		}

		private eProperty ArcherySpeedID => eProperty.ArcherySpeed;

		private static IPropertyCalculator createArcherySpeedCalculator()
		{
			return new ArcherySpeedPercentCalculator();
		}
	}
}
