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
            npc.BaseBuffBonusCategory[ConstitutionID] = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(npc, ConstitutionID);

            Assert.AreEqual(100, actual);
        }

        [Test]
        public void CalcValueFromBuffs_L50Player_With100BaseBuff_62()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[ConstitutionID] = 100;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(player, ConstitutionID);

            int expected = (int)(50 * 1.25);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValueFromBuffs_L50Player_With100SpecBuff_93()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[ConstitutionID] = 100;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(player, ConstitutionID);

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
            npc.BaseBuffBonusCategory[ConstitutionID] = 3;
            npc.SpecBuffBonusCategory[ConstitutionID] = 4;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(npc, ConstitutionID);

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
            player.ItemBonus[ConstitutionID] = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

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
			player.ItemBonus[ConstitutionMythicalID] = 100;
			player.ItemBonus[ConstitutionID] = 150;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(127, actual);
        }

        [Test]
        public void CalcValue_L50Player_With5MythicalCapAnd100ItemOvercap_106()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.ItemBonus[ConstitutionMythicalID] = 5;
			player.ItemBonus[ConstitutionItemOvercapID] = 100;
			player.ItemBonus[ConstitutionID] = 150;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(106, actual);
        }

        [Test]
        public void CalcValue_L50Player_With100ItemBonusAnd10ItemOvercap_85()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.ItemBonus[ConstitutionID] = 100;
			player.ItemBonus[ConstitutionItemOvercapID] = 10;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            Assert.AreEqual(85, actual);
        }
        [Test]
        public void CalcValue_ConstitutionFromNPC_With100Constitution_100()
        {
            var npc = Create.FakeNPC();
            npc.Constitution = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(npc, ConstitutionID);

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
            player.AbilityBonus[ConstitutionID] = 200;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            Assert.AreEqual(200, actual);
        }

        [Test]
        public void CalcValue_200Debuff_Return1()
        {
            var player = Create.FakePlayer();
            player.DebuffCategory[ConstitutionID] = 200;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            Assert.AreEqual(1, actual);
        }

        [Test]
        public void CalcValue_200AbilityBonusAnd50Debuff_200()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[ConstitutionID] = 200;
            player.DebuffCategory[ConstitutionID] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            Assert.AreEqual(200, actual);
        }

        [Test]
        public void CalcValue_70BuffBonusAnd50Debuff_20()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[ConstitutionID] = 70;
            player.DebuffCategory[ConstitutionID] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            Assert.AreEqual(20, actual);
        }

        [Test]
        public void CalcValue_70ItemConstitutionAnd50ConDebuff_45()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[ConstitutionID] = 70;
            player.DebuffCategory[ConstitutionID] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            int expected = 45;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_70BaseConsitutitonAnd50Debuff_45()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.ChangeBaseStat((eStat)ConstitutionID, 70);
			player.DebuffCategory[ConstitutionID] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            int expected = 45;
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_Constitution_70AbilityConsitutionAnd3ConLostOnDeath_67()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.AbilityBonus[ConstitutionID] = 70;
            player.TotalConstitutionLostAtDeath = 3;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);
            
            Assert.AreEqual(67, actual);
        }

        [Test]
        public void CalcValue_Dexterity_70DexAbilityAnd3ConLostOnDeath_70()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[DexterityID] = 70;
            player.TotalConstitutionLostAtDeath = 3;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, DexterityID);

            Assert.AreEqual(70, actual);
        }

        [Test]
        public void CalcValue_50AbilityAnd25PercentMultiplicator_12()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[ConstitutionID] = 50;
            player.BuffBonusMultCategory1.Set((int)ConstitutionID, new object(), 0.25);
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, ConstitutionID);

            Assert.AreEqual(12, actual);
        }
		#endregion

		private eProperty ConstitutionID => eProperty.Constitution;
		private eProperty ConstitutionItemOvercapID => eProperty.ConCapBonus;
		private eProperty ConstitutionMythicalID => eProperty.MythicalConCapBonus;
		private eProperty DexterityID => eProperty.Dexterity;

		private static IPropertyCalculator createStatCalculator()
        {
            return new StatCalculator();
        }
    }
}
