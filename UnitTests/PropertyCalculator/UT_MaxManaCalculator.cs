using DOL.GS;
using DOL.GS.PlayerClass;
using DOL.GS.PropertyCalc;
using NSubstitute;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_MaxManaCalculator
	{
		[TestFixtureSetUp]
		public void init()
		{
			GameLiving.LoadCalculators();
		}

		[Test]
		public void CalcValue_Door_1000000()
		{
			var living = new GameDoor();
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(living, MaxManaID);

			int expected = 1000000;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_LevelOneGenericPlayer_Zero()
		{
			var player = Create.FakePlayer();
			player.Level = 1;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_LevelOneAnimist_5()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 1;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 5;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_201()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 201;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_WithOneItemManaPoint_202()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.ItemBonus[MaxManaID] = 1;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 202;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_WithHundredItemManaPoints_227()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.ItemBonus[MaxManaID] = 100;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 227;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_WithHundredItemManaPointsAndHundredOvercap_253()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.ItemBonus[MaxManaID] = 100;
			player.ItemBonus[MaxManaPercentOvercapID] = 100;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 253;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_WithHundredItemManaPercent_251()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.ItemBonus[MaxManaPercentID] = 100;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 251;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_WithHundredItemManaPercentAndHundredManaPoolOvercap_351()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.ItemBonus[MaxManaPercentID] = 100;
			player.ItemBonus[MaxManaPercentOvercapID] = 100;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 351;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_WithHundredItemManaPointsAndHundredPercentOvercap_253()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.ItemBonus[MaxManaID] = 100;
			player.ItemBonus[MaxManaPercentOvercapID] = 100;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 253;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_With50Acuity_250()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			var acuityID = eProperty.Acuity;
			player.ItemBonus[acuityID] = 50;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 250;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Animist_With20ManaPoolAbility_221()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.AbilityBonus[MaxManaID] = 20;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 221;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50PlayerAndChampionLevelOne_100()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChampionLevel = 1;
			player.Champion = true;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50PlayerAndChampionLevel5_100()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChampionLevel = 5;
			player.Champion = true;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Vampiir_201()
		{
			var player = Create.FakePlayer(new ClassVampiir());
			player.Level = 50;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 201;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Vampiir_WithHundredItemManaPoolPoints_227()
		{
			var player = Create.FakePlayer(new ClassVampiir());
			player.Level = 50;
			player.ItemBonus[MaxManaID] = 100;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 227;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_Level50Vampiir_With50Strength_250()
		{
			var player = Create.FakePlayer(new ClassVampiir());
			player.Level = 50;
			var strengthID = eProperty.Strength;
			player.ItemBonus[strengthID] = 50;
			var calc = createMaxManaCalculator();

			int actual = calc.CalcValue(player, MaxManaID);

			int expected = 250;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty MaxManaID => eProperty.MaxMana;
		private eProperty MaxManaPercentID => eProperty.PowerPool;
		private eProperty MaxManaPercentOvercapID => eProperty.PowerPoolCapBonus;

		private static MaxManaCalculator createMaxManaCalculator()
		{
			return new MaxManaCalculator();
		}
	}
}
