using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;
using DOL.GS.Keeps;
using NSubstitute;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_MaxHealthCalculator
	{
		[TestFixtureSetUp]
		public void init()
		{
			GameLiving.LoadCalculators();
			GS.ServerProperties.Properties.GAMENPC_HP_GAIN_PER_CON = 1;
		}

		[Test]
		public void CalcValue_L1PlayerWithOneConstitution_27()
		{
			var player = Create.FakePlayer();
			player.Level = 1;
			player.ChangeBaseStat((eStat)ConstitutionID, 1);
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 27;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L1PlayerWithElevenConstitution_28()
		{
			var player = Create.FakePlayer();
			player.Level = 1;
			player.ChangeBaseStat((eStat)ConstitutionID, 11);
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 28;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithOneConstitution_326()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 1);
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 326;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithElevenConstitution_386()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 11);
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 386;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithOneConstitutionAndOneItemHP_327()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 1);
			player.ItemBonus[MaxHealthID] = 1;
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 327;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithOneConstitutionAndScarsOfBattle_358()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 1);
			player.abilities.Add("Scars of Battle");
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 358;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWith50Constitution_620()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 50);
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 620;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWith60Constitution_650()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 60);
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 650;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithCLOneAndOneConstitution_366()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChampionLevel = 1;
			GS.ServerProperties.Properties.HPS_PER_CHAMPIONLEVEL = 40; //default
			player.ChangeBaseStat((eStat)ConstitutionID, 1);
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 366;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithWithTenExtraHPAndOneConstitution_359()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 1);
			player.ItemBonus[ExtraHPID] = 10;
			
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(player, MaxHealthID);

			var expected = 359;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_KeepComponent_Init_Zero()
		{
			var keepComponent = new GameKeepComponent();
			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(keepComponent, MaxHealthID);

			var expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_KeepComponent_WithKeepLevel5AndBaseLevel5_5000()
		{
			var keepComponent = new GameKeepComponent();
			var keep = new GameKeep();
			keep.DBKeep = new Database.DBKeep();
			keep.Level = 5;
			keep.BaseLevel = 5;
			keepComponent.Keep = keep;

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(keepComponent, MaxHealthID);

			var expected = 5000;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_KeepDoor_Init_Zero()
		{
			var keepDoor = new GameKeepDoor();

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(keepDoor, MaxHealthID);

			var expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_KeepDoor_WithKeepLevel5AndBaseLevel5_5000()
		{
			var keepDoor = new GameKeepDoor();
			var keep = new GameKeep();
			keep.DBKeep = new Database.DBKeep();
			keep.Level = 5;
			keep.BaseLevel = 5;
			keepDoor.Component = new GameKeepComponent();
			keepDoor.Component.Keep = keep;

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(keepDoor, MaxHealthID);

			var expected = 5000;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_LevelZero_21()
		{
			var npc = Create.FakeNPC();
			npc.Level = 0;

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(npc, MaxHealthID);

			var expected = 21;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_Level50_1971()
		{
			var npc = Create.FakeNPC();
			npc.Level = 50;

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(npc, MaxHealthID);

			var expected = 1971;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_Level50And50Constitution_2020()
		{
			var npc = Create.FakeNPC();
			npc.Level = 50;
			//has 1 Constitution on creation
			npc.ChangeBaseStat((eStat)ConstitutionID, 49);

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(npc, MaxHealthID);

			var expected = 2020;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPC_Level50And2000BaseBuffConstitution_2956()
		{
			var npc = Create.FakeNPC();
			npc.Level = 50;
			npc.BaseBuffBonusCategory[ConstitutionID] = 2000;

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(npc, MaxHealthID);

			var expected = 2956;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_GenericLiving_Level50_1970()
		{
			var living = Substitute.For<GameLiving>();
			living.Level = 50;

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(living, MaxHealthID);

			var expected = 1970;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_GenericLiving_Level50And20Constitution_1970()
		{
			var living = Substitute.For<GameLiving>();
			living.Level = 50;
			//has 1 Constitution on creation
			living.ChangeBaseStat((eStat)ConstitutionID, 19);

			var calc = createMaxHealthCalc();

			int actual = calc.CalcValue(living, MaxHealthID);

			var expected = 1970;
			Assert.AreEqual(expected, actual);
		}

		private eProperty ConstitutionID => eProperty.Constitution;
		private eProperty MaxHealthID => eProperty.MaxHealth;
		private eProperty ExtraHPID => eProperty.ExtraHP;

		private MaxHealthCalculator createMaxHealthCalc()
		{
			return new MaxHealthCalculator();
		}
	}
}
