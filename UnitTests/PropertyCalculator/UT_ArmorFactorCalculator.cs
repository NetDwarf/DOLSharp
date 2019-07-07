using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;
using DOL.GS.Keeps;
using DOL.Database;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_ArmorFactorCalculator
	{
		[Test]
		public void CalcValue_L25Player_30Item_25()
		{
			var player = Create.FakePlayer();
			player.Level = 25;
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.Item.Create(30));

			int actual = calc.CalcValue(player, ArmorFactorType.DatabaseID);

			int expected = 25;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50Player_100SpecBuff_93()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.SpecBuff.Create(100));

			int actual = calc.CalcValue(player, ArmorFactorType.DatabaseID);

			int expected = 93;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_12Debuff_Minus12()
		{
			var player = Create.FakePlayer();
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.Debuff.Create(12));

			int actual = calc.CalcValue(player, ArmorFactorType.DatabaseID);

			int expected = -12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Player_100ExtraBuff_Hundred()
		{
			var player = Create.FakePlayer();
			var calc = createCalculator();
			player.Boni.Add(ArmorFactorType.ExtraBuff.Create(100));

			int actual = calc.CalcValue(player, ArmorFactorType.DatabaseID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50NPC_604()
		{
			var player = Create.FakeNPC();
			player.Level = 50;
			var calc = createCalculator();

			int actual = calc.CalcValue(player, ArmorFactorType.DatabaseID);

			int expected = 604;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_BaseLevel50KeepDoor_50()
		{
			var keep = new GameKeep();
			keep.DBKeep = new DBKeep();
			keep.BaseLevel = 50;
			var keepDoor = new GameKeepDoor();
			keepDoor.Component = new GameKeepComponent();
			keepDoor.Component.AbstractKeep = keep;
			var calc = createCalculator();

			int actual = calc.CalcValue(keepDoor, ArmorFactorType.DatabaseID);
			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_BaseLevel50TowerDoor_25()
		{
			var tower = new GameKeepTower();
			tower.DBKeep = new DBKeep();
			tower.BaseLevel = 50;
			var keepDoor = new GameKeepDoor();
			keepDoor.Component = new GameKeepComponent();
			keepDoor.Component.AbstractKeep = tower;
			var calc = createCalculator();

			int actual = calc.CalcValue(keepDoor, ArmorFactorType.DatabaseID);
			int expected = 25;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArmorFactorType => new BonusType(eBonusType.ArmorFactor);

		private static IPropertyCalculator createCalculator()
		{
			return new ArmorFactorCalculator();
		}
	}
}
