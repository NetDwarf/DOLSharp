using DOL.Database;
using DOL.GS;
using DOL.GS.PlayerClass;
using NUnit.Framework;
using System.Collections.Generic;

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
            player.BaseBuffBonusCategory[ConstitutionID] = 100;

            int actual = player.Constitution;

            int expected = (int)(50 * 1.25);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Constitution_Level50PlayerWith100ConstSpecBuff_Return93()
        {
            var player = createPlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[ConstitutionID] = 100;

            int actual = player.Constitution;

            int expected = (int)(50 * 1.875);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Constitution_Level50Player100ConstFromItems_Return75()
        {
            var player = createPlayer();
            player.Level = 50;
            player.ItemBonus[ConstitutionID] = 100;

            int actual = player.Constitution;

            int expected = (int)(1.5 * 50);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith50AcuityFromItems_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.ItemBonus[AcuityID] = 50;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

        [Test]
        public void Constitution_Level50Player150ConAnd100MythicalConCap_Return127()
        {
            var player = createPlayer();
            player.Level = 50;
			player.ItemBonus[eProperty.MythicalConCapBonus] = 100;
			player.ItemBonus[ConstitutionID] = 150;

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
			player.ItemBonus[ConstitutionID] = 150;

            int actual = player.Constitution;

            Assert.AreEqual(106, actual);
        }

        [Test]
        public void Constitution_Level50Player60ConFromBuffsAnd50ConDebuff_Return10()
        {
            var player = createPlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[ConstitutionID] = 60;
            player.DebuffCategory[ConstitutionID] = 50;

            int actual = player.Constitution;

            Assert.AreEqual(10, actual);
        }

        [Test]
        public void Constitution_Level50Player30ConFromBuffsAnd30ConFromItemsAnd50ConDebuff_Return20()
        {
            var player = createPlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[ConstitutionID] = 30;
            player.ItemBonus[ConstitutionID] = 30;
            player.DebuffCategory[ConstitutionID] = 50;

            int actual = player.Constitution;

            Assert.AreEqual(20, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith50AcuityBuff_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.BaseBuffBonusCategory[AcuityID] = 50;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith200AcuityAnd30AcuCapEachFromItems_Return127()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
			player.ItemBonus[AcuityID] = 200;
			player.ItemBonus[eProperty.AcuCapBonus] = 30;
			player.ItemBonus[eProperty.MythicalAcuCapBonus] = 30;

            int actual = player.Intelligence;

            Assert.AreEqual(127, actual);
        }

        [Test]
        public void Intelligence_Level50AnimistWith30AcuityAnd20IntelligenceFromItems_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.ItemBonus[AcuityID] = 30;
            player.ItemBonus[eProperty.Intelligence] = 20;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

        [Test]
        public void Constitution_Level30AnimistWith200ConAnd20ConCapEachViaItems_Return81()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 30;
			player.ItemBonus[ConstitutionID] = 200;
			player.ItemBonus[ConstitutionOvercapID] = 20;
			player.ItemBonus[ConstitutionMythicalCapID] = 20;

            int actual = player.Constitution;

            Assert.AreEqual(81, actual);
        }

		[Test]
		public void ChangeBaseStat_AddOneCon_PlayerHasOneConstitution()
		{
			var player = createPlayer();

			player.ChangeBaseStat((eStat)ConstitutionID, 1);

			int actual = player.Constitution;
			Assert.AreEqual(1, actual);
		}

        [Test]
        public void Intelligence_L50AnimistWith30AcuityAnd20IntelligenceAbilityBonus_Return50()
        {
            var player = createPlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.AbilityBonus[AcuityID] = 30;
            player.AbilityBonus[eProperty.Intelligence] = 20;

            int actual = player.Intelligence;

            Assert.AreEqual(50, actual);
        }

		[Test]
		public void Piety_L50ClericWith30ItemAcuity_30()
		{
			var player = createPlayer(new ClassCleric());
			player.Level = 50;
			player.ItemBonus[AcuityID] = 30;

			int actual = player.Piety;

			Assert.AreEqual(30, actual);
		}

		[Test]
		public void Piety_L50ClericWith30SpecBuffAcuity_One()
		{
			var player = createPlayer(new ClassCleric());
			player.Level = 50;
			player.BaseBuffBonusCategory[AcuityID] = 30;

			int actual = player.Piety;

			Assert.AreEqual(1, actual);
		}

		[Test]
		public void CalculateMaxHealth_LevelOneAndZeroConstitution_26()
		{
			var player = createPlayer();
			player.Level = 1;
			var constitutionValue = 0;

			int actual = player.CalculateMaxHealth(player.Level, constitutionValue);

			var expected = 26;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_L50PlayerAndZeroConstitution_320()
		{
			var player = createPlayer();
			player.Level = 50;
			var constitutionValue = 0;

			int actual = player.CalculateMaxHealth(player.Level, constitutionValue);

			var expected = 320;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50And50Constitution_620()
		{
			var player = createPlayer();
			player.Level = 50;
			var constitutionValue = 50;

			int actual = player.CalculateMaxHealth(player.Level, constitutionValue);

			var expected = 620;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50And60Constitution_650()
		{
			var player = createPlayer();
			player.Level = 50;
			var constitutionValue = 60;

			int actual = player.CalculateMaxHealth(player.Level, constitutionValue);

			var expected = 650;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50AndZeroConstitution_CLOne_360()
		{
			var player = createPlayer();
			player.Level = 50;
			player.ChampionLevel = 1;
			GS.ServerProperties.Properties.HPS_PER_CHAMPIONLEVEL = 40; //default
			var constitutionValue = 0;

			int actual = player.CalculateMaxHealth(player.Level, constitutionValue);

			var expected = 360;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalculateMaxHealth_Level50AndZeroConstitution_WithTenExtraHP_352()
		{
			var player = createPlayer();
			player.Level = 50;
			player.ItemBonus[eProperty.ExtraHP] = 10;
			var constitutionValue = 0;

			int actual = player.CalculateMaxHealth(player.Level, constitutionValue);

			var expected = 352;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void MaxHealth_Level50AndOneConstitutionAndWithScarsOfBattle_358()
		{
			var player = createPlayer();
			player.Level = 50;
			player.AbilityBonus[ConstitutionID] = 1;
			player.abilities.Add("Scars of Battle");

			int actual = player.MaxHealth;

			var expected = 358;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void MaxMana_GenericPlayer_Zero()
		{
			var player = createPlayer();

			var actual = player.MaxMana;

			var expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void MaxMana_LevelOneAnimist_5()
		{
			var player = createPlayer(new CharacterClassAnimist());

			var actual = player.MaxMana;

			var expected = 5;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void MaxEndurance_Init_100()
		{
			var player = createPlayer();

			var actual = player.MaxEndurance;

			var expected = 100;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void MaxEndurance_5ItemEndurance_105()
		{
			var player = createPlayer();
			player.ItemBonus[eProperty.Fatigue] = 5;

			var actual = player.MaxEndurance;

			var expected = 105;
			Assert.AreEqual(expected, actual);
		}

		private eProperty ConstitutionID => eProperty.Constitution;
		private eProperty ConstitutionOvercapID => eProperty.ConCapBonus;
		private eProperty ConstitutionMythicalCapID => eProperty.MythicalConCapBonus;
		private eProperty AcuityID => eProperty.Acuity;

		private class TestablePlayer : GamePlayer
		{
			public List<string> abilities = new List<string>();
			public TestablePlayer(GameClient client = null, DOLCharacters dbChar = null) : base(client, dbChar)
			{

			}

			public TestablePlayer(ICharacterClass characterClass) : this()
			{
				CharacterClass = characterClass;
			}

			public override ICharacterClass CharacterClass { get; } = new DefaultCharacterClass();
			public override int ChampionLevel { get; set; } = 0;

			public override void LoadFromDatabase(DataObject obj) { /*do nothing*/ }

			public override bool HasAbility(string abilityName)
			{
				return abilities.Contains(abilityName);
			}
		}

		private static TestablePlayer createPlayer()
        {
			return createPlayer(new DefaultCharacterClass());
        }

        private static TestablePlayer createPlayer(ICharacterClass charClass)
        {
			return new TestablePlayer(charClass);
        }
    }
}
