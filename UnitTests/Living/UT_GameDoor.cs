using DOL.GS.Keeps;
using DOL.GS;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	public class UT_GameDoor
	{
		[TestFixtureSetUp]
		public void init()
		{
			GameLiving.LoadCalculators();
		}

		[Test]
		public void MaxHealth_LevelOne_200()
		{
			var guard = createDoor();
			guard.Level = 1;

			int actual = guard.MaxHealth;

			int expected = 200;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void MaxHealth_Level50_9850()
		{
			var guard = createDoor();
			guard.Level = 50;

			int actual = guard.MaxHealth;

			int expected = 9850;
			Assert.AreEqual(expected, actual);
		}

		private GameDoor createDoor()
		{
			return new GameDoor();
		}
	}
}
