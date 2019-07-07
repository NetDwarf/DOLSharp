using DOL.GS.Keeps;
using DOL.GS;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	public class UT_GameKeepGuard
	{
		[TestFixtureSetUp]
		public void init()
		{
			GS.ServerProperties.Properties.GAMENPC_BASE_CON = 1;
			GS.ServerProperties.Properties.GAMENPC_HP_GAIN_PER_CON = 1;
			GameLiving.LoadCalculators();
		}

		[Test]
		public void MaxHealth_LevelOne_44()
		{
			var guard = createKeepGuard();
			guard.Level = 1;

			int actual = guard.MaxHealth;

			int expected = 44;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void MaxHealth_Level50_2170()
		{
			var guard = createKeepGuard();
			guard.Level = 50;

			int actual = guard.MaxHealth;

			int expected = 2170;
			Assert.AreEqual(expected, actual);
		}

		private GameKeepGuard createKeepGuard()
		{
			return new GameKeepGuard();
		}
	}
}
