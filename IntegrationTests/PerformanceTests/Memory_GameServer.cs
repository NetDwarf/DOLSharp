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
					var bonusType = new BonusType((eProperty)j);
					players[i].Boni.Add(bonusType.Base.Create(60));
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
					var bonusType = new BonusType((eProperty)j);
					players[i].Boni.Add(bonusType.Item.Create(60));
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("100 GamePlayer with 100 item properties set consume " + memoryConsumption + " bytes");
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
					npcs[i].Boni.Add(bonusType.Base.Create(60));
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
			UncappedBoni[] boniArray = new UncappedBoni[10000];
			for (int i = 0; i < boniArray.Length; i++)
			{
				boniArray[i] = new UncappedBoni();
				for (int j = 1; j <= 8; j++)
				{
					var bonusType = new BonusType((eProperty)j);
					boniArray[i].Add(bonusType.Base.Create(60));
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("10000 Boni with 8 base properties set consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 20 * 1000 * 1000);
		}

		[Test]
		public void Boni_100Times_WithHundredItemPropertiesSet_LessThan2Megabyte()
		{
			long before = GC.GetTotalMemory(true);
			UncappedBoni[] boniArray = new UncappedBoni[100];
			for (int i = 0; i < boniArray.Length; i++)
			{
				boniArray[i] = new UncappedBoni();
				for (int j = 1; j <= 100; j++)
				{
					var bonusType = new BonusType((eProperty)j);
					boniArray[i].Add(bonusType.Item.Create(60));
				}
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("100 Boni with 100 item properties set consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 2 * 1000 * 1000);
		}

		[Test]
		public void Bonus_1000Times_LessThan80kb()
		{
			long before = GC.GetTotalMemory(true);
			Bonus[] boniArray = new Bonus[1000];
			for (int i = 0; i < boniArray.Length; i++)
			{
				boniArray[i] = Bonus.Strength.Base.Create(5);
			}
			long after = GC.GetTotalMemory(true);
			long memoryConsumption = after - before;
			Console.WriteLine("1000 Bonus(es) consume " + memoryConsumption + " bytes");
			Assert.Less(memoryConsumption, 80 * 1000);
		}
	}
}
