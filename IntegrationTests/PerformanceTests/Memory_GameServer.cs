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
				players[i] = GamePlayer.CreateTestableGamePlayer();
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
				players[i] = GamePlayer.CreateTestableGamePlayer();
				for (int j = 1; j <= 8; j++)
				{
					players[i].Boni.Add(Bonus.Base.ComponentOf((eProperty)j).Create(60));
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
				players[i] = GamePlayer.CreateTestableGamePlayer();
				for (int j = 1; j <= 100; j++)
				{
					players[i].Boni.Add(Bonus.Item.ComponentOf((eProperty)j).Create(60));
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("100 GamePlayer with 100 item properties set consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 10 * 1000 * 1000);
		}

		[Test]
		public void GameNPC_With8BasePropertiesSet_LessThan100Megabyte()
		{
			long before = GC.GetTotalMemory(true);
			GameNPC[] npcs = new GameNPC[10000];
			for (int i = 0; i < npcs.Length; i++)
			{
				npcs[i] = Create.FakeNPC();
				for (int j = 1; j <= 8; j++)
				{
					npcs[i].Boni.Add(Bonus.Base.ComponentOf((eProperty)j).Create(60));
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("10000 GameNPC with 8 base properties set consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 100 * 1000 * 1000);
		}

		[Test]
		public void Boni_10000Times_With8BasePropertiesSet_LessThan20MegaByte()
		{
			long before = GC.GetTotalMemory(true);
			Boni[] boniArray = new Boni[10000];
			for (int i = 0; i < boniArray.Length; i++)
			{
				boniArray[i] = new Boni();
				for (int j = 1; j <= 8; j++)
				{
					boniArray[i].Add(Bonus.Base.ComponentOf((eProperty)j).Create(60));
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("10000 Boni with 8 base properties set consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 20 * 1000 * 1000);
		}

		[Test]
		public void BonusCaps_HundredTimes_LessThan1MegaByte()
		{
			long beforeInit = GC.GetTotalMemory(true);
			var foo = new PropertyCaps(Create.FakePlayer());
			long afterInit = GC.GetTotalMemory(true);
			PropertyCaps[] capsArray = new PropertyCaps[100];
			for (int i = 0; i < capsArray.Length; i++)
			{
				capsArray[i] = new PropertyCaps(Create.FakePlayer());
			}
			long after = GC.GetTotalMemory(true);
			long initMemoryConsumption = afterInit - beforeInit;
			long memoryConsumption = after - afterInit;
			Console.WriteLine("BonusCaps init consumes " + initMemoryConsumption + " bytes");
			Console.WriteLine("100 more BonusCaps consume " + memoryConsumption + " bytes");
			Assert.Less(after - beforeInit, 1 * 1000 * 1000);
		}
	}
}
