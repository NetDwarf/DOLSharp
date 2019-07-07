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
			player.Boni.Add(ArcherySpeedBonus.Item.Create(5));
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedBonus.DatabaseID);

			int expected = 5;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_OnCreation_Zero()
		{
			var player = Create.FakeNPC();
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedBonus.DatabaseID);

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

			int actual = calc.CalcValue(player, ArcherySpeedBonus.DatabaseID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With15ItemBonus_Ten()
		{
			var player = Create.FakePlayer();
			player.Boni.Add(ArcherySpeedBonus.Item.Create(15));
			var calc = createArcherySpeedCalculator();

			int actual = calc.CalcValue(player, ArcherySpeedBonus.DatabaseID);

			int expected = 10;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArcherySpeedBonus => new BonusType(eBonusType.ArcherySpeed);

		private static IPropertyCalculator createArcherySpeedCalculator()
		{
			return new ArcherySpeedPercentCalculator();
		}
	}
}
