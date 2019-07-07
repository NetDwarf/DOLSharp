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

			int actual = calc.CalcValue(living, EnduranceDatabaseID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_100()
		{
			var player = Create.FakePlayer();
			var calc = createFatigueCalculator();

			int actual = calc.CalcValue(player, EnduranceDatabaseID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With5ItemEnduranceBonus_105()
		{
			var player = Create.FakePlayer();
			player.Boni.Add(Endurance.Item.Create(5));
			var calc = createFatigueCalculator();

			int actual = calc.CalcValue(player, EnduranceDatabaseID);

			int expected = 105;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_With100ItemEnduranceBonus_115()
		{
			var player = Create.FakePlayer();
			player.Boni.Add(Endurance.Item.Create(100));
			var calc = createFatigueCalculator();

			int actual = calc.CalcValue(player, EnduranceDatabaseID);

			int expected = 115;
			Assert.AreEqual(expected, actual);
		}

		private eProperty EnduranceDatabaseID => eProperty.Fatigue;
		private BonusType Endurance => new BonusType(eBonusType.Endurance);

		private FatigueCalculator createFatigueCalculator()
		{
			return new FatigueCalculator();
		}
	}
}
