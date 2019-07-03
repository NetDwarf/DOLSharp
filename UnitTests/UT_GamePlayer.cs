using DOL.GS;
using DOL.GS.PlayerClass;
using NUnit.Framework;
using System;

namespace DOL.UnitTests.GameServer
{
    [TestFixture]
    class UT_GamePlayer
    {
        [TestFixtureSetUp]
        public void init()
        {
            GameLiving.LoadCalculators();
        }

        [Test]
        public void Constitution_Level50PlayerWith100ConstBaseBuff_Return62()
        {
            var player = createPlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[eProperty.Constitution] = 100;

            int actual = player.Constitution;

            int expected = (int)(50 * 1.25);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Constitution_Level50PlayerWith100ConstSpecBuff_Return93()
        {
            var player = createPlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[eProperty.Constitution] = 100;

            int actual = player.Constitution;

            int expected = (int)(50 * 1.875);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Constitution_Level50Player100ConstFromItems_Return75()
        {
            var player = createPlayer();
            player.Level = 50;
            player.ItemBonus[eProperty.Constitution] = 100;

            int actual = player.Constitution;

            int expected = (int)(1.5 * 50);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith50AcuityFromItems_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.ItemBonus[eProperty.Acuity] = 50;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

        [Test]
        public void Constitution_Level50Player150ConAnd100MythicalConCap_Return127()
        {
            var player = createPlayer();
            player.Level = 50;
			player.Boni.Add(Bonus.Constitution.Mythical.Create(100));
			player.Boni.Add(Bonus.Constitution.Item.Create(150));

            int actual = player.Constitution;

            Assert.AreEqual(127, actual);
        }

        [Test]
        public void Constitution_Level50PlayerWith5MythicalConCap100ConCap_Return106()
        {
            var player = createPlayer();
            player.Level = 50;
			player.ItemBonus[eProperty.MythicalConCapBonus] = 5;
			player.ItemBonus[eProperty.ConCapBonus] = 100;
			player.ItemBonus[eProperty.Constitution] = 150;

            int actual = player.Constitution;

            Assert.AreEqual(106, actual);
        }

        [Test]
        public void Constitution_Level50Player60ConFromBuffsAnd50ConDebuff_Return10()
        {
            var player = createPlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[eProperty.Constitution] = 60;
            player.DebuffCategory[eProperty.Constitution] = 50;

            int actual = player.Constitution;

            Assert.AreEqual(10, actual);
        }

        [Test]
        public void Constitution_Level50Player30ConFromBuffsAnd30ConFromItemsAnd50ConDebuff_Return20()
        {
            var player = createPlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[eProperty.Constitution] = 30;
            player.ItemBonus[eProperty.Constitution] = 30;
            player.DebuffCategory[eProperty.Constitution] = 50;

            int actual = player.Constitution;

            Assert.AreEqual(20, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith50AcuityBuff_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.BaseBuffBonusCategory[eProperty.Acuity] = 50;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith200AcuityAnd30AcuCapEachFromItems_Return127()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
			player.Boni.Add(Bonus.Acuity.Item.Create(200));
			player.Boni.Add(Bonus.Acuity.ItemOvercap.Create(30));
			player.Boni.Add(Bonus.Acuity. Mythical.Create(30));

            int actual = player.Intelligence;

            Assert.AreEqual(127, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith30AcuityAnd20IntelligenceFromItems_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.ItemBonus[eProperty.Acuity] = 30;
            player.ItemBonus[eProperty.Intelligence] = 20;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

        [Test]
        public void Constitution_Level30AnimistWith200ConAnd20ConCapEachViaItems_Return81()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 30;
			player.Boni.Add(Bonus.Constitution.Item.Create(200));
			player.Boni.Add(Bonus.Constitution.ItemOvercap.Create(20));
			player.Boni.Add(Bonus.Constitution.Mythical.Create(20));

            int actual = player.Constitution;

            Assert.AreEqual(81, actual);
        }

		[Test]
		public void ChangeBaseStat_AddOneCon_PlayerHasOneConstitution()
		{
			var player = createPlayer();

			player.ChangeBaseStat(eStat.CON, 1);

			int actual = player.Constitution;
			Assert.AreEqual(1, actual);
		}

        [Test]
        public void Intelligence_L50AnimistWith30AcuityAnd20IntelligenceAbilityBonus_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.AbilityBonus[eProperty.Acuity] = 30;
            player.AbilityBonus[eProperty.Intelligence] = 20;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

		[Test]
		public void Piety_L50ClericWith30ItemAcuity_30()
		{
			var player = createPlayer(new ClassCleric());
			player.Level = 50;
			player.ItemBonus[eProperty.Acuity] = 30;

			int actual = player.Piety;

			Assert.AreEqual(30, actual);
		}

		[Test]
		public void Piety_L50ClericWith30SpecBuffAcuity_One()
		{
			var player = createPlayer(new ClassCleric());
			player.Level = 50;
			player.BaseBuffBonusCategory[eProperty.Acuity] = 30;

			int actual = player.Piety;

			Assert.AreEqual(1, actual);
		}

		[Test]
		public void CalculateMaxHealth_LevelOneAndOneConstitution_27()
		{
			var player = Create.FakePlayer();
			player.Level = 1;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(1));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 27;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_LevelOneAndElevenConstitution_28()
		{
			var player = Create.FakePlayer();
			player.Level = 1;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(11));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 28;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithOneConstitution_326()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(1));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 326;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_L50PlayerWithElevenConstitution_386()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(11));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 386;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50AndOneConstitution_OneItemHP_326()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(1));
			player.Boni.SetTo(Bonus.HealthPool.Item.Create(1));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 326;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50AndOneConstitution_WithScarsOfBattle_358()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(1));
			player.abilities.Add("Scars of Battle");

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 326;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50And50Constitution_620()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(50));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 620;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50And60Constitution_650()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(60));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 650;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50AndOneConstitution_CLOne_366()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.ChampionLevel = 1;
			GS.ServerProperties.Properties.HPS_PER_CHAMPIONLEVEL = 40; //default
			player.Boni.SetTo(Bonus.Constitution.Base.Create(1));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 366;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50AndOneConstitution_WithTenExtraHP_359()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(1));
			player.Boni.SetTo(new BonusType(eBonusType.ExtraHP).Item.Create(10));

			int actual = player.CalculateMaxHealth(player.Level, player.Boni.ValueOf(Bonus.Constitution));

			var expected = 359;
			Assert.AreEqual(expected, actual);
		}

		private static GamePlayer createPlayer()
        {
            return GamePlayer.CreateTestableGamePlayer();
        }

        private static GamePlayer createPlayer(ICharacterClass charClass)
        {
            return GamePlayer.CreateTestableGamePlayer(charClass);
        }
    }
}
