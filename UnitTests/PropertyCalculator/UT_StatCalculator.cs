using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
    [TestFixture]
    public class UT_StatCalculator
    {
        #region CalcValueFromBuffs
        [Test]
        public void CalcValueFromBuffs_GameNPC_With100BaseBuff_100()
        {
            var npc = Create.FakeNPC();
            npc.BaseBuffBonusCategory[ConstitutionDatabaseID] = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(npc, ConstitutionDatabaseID);

            Assert.AreEqual(100, actual);
        }

        [Test]
        public void CalcValueFromBuffs_L50Player_With100BaseBuff_62()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[ConstitutionDatabaseID] = 100;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(player, ConstitutionDatabaseID);

            int expected = (int)(50 * 1.25);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValueFromBuffs_L50Player_With100SpecBuff_93()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[ConstitutionDatabaseID] = 100;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(player, ConstitutionDatabaseID);

            int expected = (int)(50 * 1.875);
            Assert.AreEqual(expected, actual);
        }

		[Test]
		public void CalcValueFromBuffs_IntelligenceFromL50Animist_With50AcuityBaseBuff_50()
		{
			var player = Create.FakePlayer(new CharacterClassAnimist());
			player.Level = 50;
			player.BaseBuffBonusCategory[eProperty.Acuity] = 50;
			var statCalc = createStatCalculator();

			int actual = statCalc.CalcValueFromBuffs(player, eProperty.Intelligence);
			Assert.AreEqual(50, actual);
		}

		[Test]
        public void CalcValueFromBuffs_BaseBuff3AndSpecBuff4_7()
        {
            var npc = Create.FakeNPC();
            npc.BaseBuffBonusCategory[ConstitutionDatabaseID] = 3;
            npc.SpecBuffBonusCategory[ConstitutionDatabaseID] = 4;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(npc, ConstitutionDatabaseID);

            int expected = 7;
            Assert.AreEqual(expected, actual);
        }
		#endregion
		
		#region CalcValue
		[Test]
        public void CalcValue_L50Player_With100ItemBonus_75()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[ConstitutionDatabaseID] = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            int expected = (int)(1.5 * 50);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_IntelligenceFromL50Animist_With50AcuityFromItems_50()
        {
            var player = Create.FakePlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.ItemBonus[eProperty.Acuity] = 50;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Intelligence);
            
            Assert.AreEqual(50, actual);
        }

        [Test]
        public void CalcValue_ConstitutionFromL50Player_With150ConAnd100MythicalConCap_127()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.Boni.Add(Bonus.Constitution.Mythical.Create(100));
			player.Boni.Add(Bonus.Constitution.Item.Create(150));
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(127, actual);
        }

        [Test]
        public void CalcValue_L50Player_With5MythicalCapAnd100ItemOvercap_106()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.Boni.Add(Bonus.Constitution.Mythical.Create(5));
			player.Boni.Add(Bonus.Constitution.ItemOvercap.Create(100));
			player.Boni.Add(Bonus.Constitution.Item.Create(150));
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(106, actual);
        }

        [Test]
        public void CalcValue_L50Player_With100ItemBonusAnd10ItemOvercap_85()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.ItemBonus[ConstitutionDatabaseID] = 100;
			player.ItemBonus[ConstitutionItemOvercapDatabseID] = 10;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            Assert.AreEqual(85, actual);
        }
        [Test]
        public void CalcValue_ConstitutionFromNPC_With100Constitution_100()
        {
            var npc = Create.FakeNPC();
            npc.Constitution = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(npc, ConstitutionDatabaseID);

            Assert.AreEqual(100, actual);
        }

        [Test]
        public void CalcValue_IntelligenceFromNPC_With100Intelligence_100()
        {
            var npc = Create.FakeNPC();
            npc.Intelligence = 100;
            var statCalc = createStatCalculator();

			int actual = statCalc.CalcValue(npc, eProperty.Intelligence);

            Assert.AreEqual(100, actual);
        }

        [Test]
        public void CalcValue_IntelligenceL50Animist_With50Acuity_50()
        {
            var player = Create.FakePlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.BaseBuffBonusCategory[eProperty.Acuity] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Intelligence);

            Assert.AreEqual(50, actual);
        }

		[Test]
        public void CalcValue_200AbilityBonus_200()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[ConstitutionDatabaseID] = 200;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            Assert.AreEqual(200, actual);
        }

        [Test]
        public void CalcValue_200Debuff_Return1()
        {
            var player = Create.FakePlayer();
            player.DebuffCategory[ConstitutionDatabaseID] = 200;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            Assert.AreEqual(1, actual);
        }

        [Test]
        public void CalcValue_200AbilityBonusAnd50Debuff_200()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[ConstitutionDatabaseID] = 200;
            player.DebuffCategory[ConstitutionDatabaseID] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            Assert.AreEqual(200, actual);
        }

        [Test]
        public void CalcValue_70BuffBonusAnd50Debuff_20()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[ConstitutionDatabaseID] = 70;
            player.DebuffCategory[ConstitutionDatabaseID] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            Assert.AreEqual(20, actual);
        }

        [Test]
        public void CalcValue_70ConItemBonusAnd50ConDebuff_45()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[ConstitutionDatabaseID] = 70;
            player.DebuffCategory[ConstitutionDatabaseID] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            int expected = 70 - (50 / 2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_70ConBaseStatAnd50ConDebuff_45()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(70));
			player.DebuffCategory[eProperty.Constitution] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            int expected = 45;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_Constitution_70ConBaseStatAnd3ConLostOnDeath_67()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.Boni.SetTo(Bonus.Constitution.Base.Create(70));
            player.TotalConstitutionLostAtDeath = 3;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);
            
            Assert.AreEqual(67, actual);
        }

        [Test]
        public void CalcValue_Dexterity_70DexAbilityAnd3ConLostOnDeath_70()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[eProperty.Dexterity] = 70;
            player.TotalConstitutionLostAtDeath = 3;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Dexterity);

            Assert.AreEqual(70, actual);
        }

        [Test]
        public void CalcValue_50AbilityAnd25PercentMultiplicator_12()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[ConstitutionDatabaseID] = 50;
            player.BuffBonusMultCategory1.Set((int)ConstitutionDatabaseID, new object(), 0.25);
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionDatabaseID);

            Assert.AreEqual(12, actual);
        }
		#endregion

		private eProperty ConstitutionDatabaseID => eProperty.Constitution;
		private eProperty ConstitutionItemOvercapDatabseID => eProperty.ConCapBonus;
		private eProperty ConstitutionMythicalDatabaseID => eProperty.MythicalConCapBonus;
		private eProperty DexterityDatabaseID => eProperty.Dexterity;

		private static IPropertyCalculator createStatCalculator()
        {
            return new StatCalculator();
        }
    }
}
