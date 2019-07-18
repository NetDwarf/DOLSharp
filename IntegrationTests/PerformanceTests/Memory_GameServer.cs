using NUnit.Framework;
using System;
using DOL.GS;
using DOL.UnitTests.GameServer;

namespace DOL.PerformanceTests.Memory
{
	[TestFixture]
	class GameServer
	{
		[Test]
		public void GamePlayer_100Times_Init_LessThanOneMegabyte()
		{
			long before = GC.GetTotalMemory(true);
			GamePlayer[] players = new GamePlayer[100];
			for (int i = 0; i < players.Length; i++)
			{
				players[i] = createPlayer();
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("100 GamePlayer take " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 10 * 1000 * 1000);
		}

		[Test]
		public void GamePlayer_100Times_WithEightBaseStatsSet_LessThanTenMegabyte()
		{
			long before = GC.GetTotalMemory(true);
			GamePlayer[] players = new GamePlayer[100];
			for (int i = 0; i < players.Length; i++)
			{
				players[i] = createPlayer();
				for (int j = 1; j <= 8; j++)
				{
					players[i].ChangeBaseStat((eStat)j, 60);
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("100 GamePlayer with 8 base properties set consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 10 * 1000 * 1000);
		}

		[Test]
		public void GamePlayer_100Times_WithHundredItemPropertiesSet_LessThanTenMegabyte()
		{
			long before = GC.GetTotalMemory(true);
			GamePlayer[] players = new GamePlayer[100];
			
			for (int i = 0; i < players.Length; i++)
			{
				players[i] = createPlayer();
				for (int j = 0; j < 100; j++)
				{
					players[i].ItemBonus[j] = 60;
				}

			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("100 GamePlayer with 100 different item bonuses added twice, consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 10 * 1000 * 1000);
		}

		[Test]
		public void GameNPC_10000Times_With8BasePropertiesSet_LessThan100Megabyte()
		{
			long before = GC.GetTotalMemory(true);
			GameNPC[] npcs = new GameNPC[10000];
			for (int i = 0; i < npcs.Length; i++)
			{
				npcs[i] = Create.FakeNPC();
				for (int j = 1; j <= 8; j++)
				{
					var bonusType = new BonusType((eProperty)j);
					npcs[i].ChangeBaseStat((eStat)j, 60);
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("10000 GameNPC with 8 base properties set consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 100 * 1000 * 1000);
		}

		private GamePlayer createPlayer()
		{
			return Create.FakePlayer();
		}
	}
}
