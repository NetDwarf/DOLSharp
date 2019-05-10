using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;
using DOL.GS.PlayerClass;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
    [TestFixture]
    public class UT_StatCalculator
    {
        #region CalcValueFromBuffs
        [Test]
        public void CalcValueFromBuffs_GameNPCWith100ConstBaseBuff_Return100()
        {
            var npc = Create.FakeNPC();
            npc.BaseBuffBonusCategory[eProperty.Constitution] = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(npc, eProperty.Constitution);

            Assert.AreEqual(100, actual);
        }

        [Test]
        public void CalcValueFromBuffs_Level50PlayerWith100ConstBaseBuff_Return62()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.BaseBuffBonusCategory[eProperty.Constitution] = 100;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(player, eProperty.Constitution);

            int expected = (int)(50 * 1.25);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValueFromBuffs_Level50PlayerWith100ConstSpecBuff_ReturnCapAt93()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[eProperty.Constitution] = 100;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(player, eProperty.Constitution);

            int expected = (int)(50 * 1.875);
            Assert.AreEqual(expected, actual);
        }

		[Test]
		public void CalcValueFromBuffs_Level50AnimistIntelligence_With50AcuityFromBuffs_50()
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
            npc.BaseBuffBonusCategory[eProperty.Constitution] = 3;
            npc.SpecBuffBonusCategory[eProperty.Constitution] = 4;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValueFromBuffs(npc, eProperty.Constitution);

            int expected = 7;
            Assert.AreEqual(expected, actual);
        }
		#endregion
		
		#region CalcValue
		[Test]
        public void CalcValue_L50Player100ItemBonus_75()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[eProperty.Constitution] = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            int expected = (int)(1.5 * 50);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_Intelligence_L50AnimistWith50AcuityFromItems_50()
        {
            var player = Create.FakePlayer(new CharacterClassAnimist());
            player.Level = 50;
            player.ItemBonus[eProperty.Acuity] = 50;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Intelligence);
            
            Assert.AreEqual(50, actual);
        }

        [Test]
        public void CalcValue_L50Player150ConAnd100MythicalConCap_127()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[eProperty.MythicalConCapBonus] = 100;
            player.ItemBonus[eProperty.Constitution] = 150;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(127, actual);
        }

        [Test]
        public void CalcValue_L50PlayerWith5MythicalCapAnd100CapBonus_106()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[eProperty.MythicalConCapBonus] = 5;
            player.ItemBonus[eProperty.ConCapBonus] = 100;
            player.ItemBonus[eProperty.Constitution] = 150;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(106, actual);
        }

        [Test]
        public void CalcValue_L50Player100ItemBonusAnd10CapBonus_85()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[eProperty.Constitution] = 100;
            player.ItemBonus[eProperty.ConCapBonus] = 10;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(85, actual);
        }
        [Test]
        public void CalcValue_NPCWith100Constitution_100()
        {
            var npc = Create.FakeNPC();
            npc.Constitution = 100;
			var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(npc, eProperty.Constitution);

            Assert.AreEqual(100, actual);
        }

        [Test]
        public void CalcValue_NPCWith100Intelligence_100()
        {
            var npc = Create.FakeNPC();
            npc.Intelligence = 100;
            var statCalc = createStatCalculator();

			int actual = statCalc.CalcValue(npc, eProperty.Intelligence);

            Assert.AreEqual(100, actual);
        }

        [Test]
        public void CalcValue_Intelligence_L50AnimistWith50Acuity_50()
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
            player.AbilityBonus[eProperty.Constitution] = 200;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(200, actual);
        }

        [Test]
        public void CalcValue_200Debuff_Return1()
        {
            var player = Create.FakePlayer();
            player.DebuffCategory[eProperty.Constitution] = 200;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(1, actual);
        }

        [Test]
        public void CalcValue_200AbilityBonusAnd50Debuff_200()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[eProperty.Constitution] = 200;
            player.DebuffCategory[eProperty.Constitution] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(200, actual);
        }

        [Test]
        public void CalcValue_70BuffBonusAnd50Debuff_20()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.SpecBuffBonusCategory[eProperty.Constitution] = 70;
            player.DebuffCategory[eProperty.Constitution] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            Assert.AreEqual(20, actual);
        }

        [Test]
        public void CalcValue_70ConItemBonusAnd50ConDebuff_45()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
            player.ItemBonus[eProperty.Constitution] = 70;
            player.DebuffCategory[eProperty.Constitution] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            int expected = 70 - (50 / 2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_70ConBaseStatAnd50ConDebuff_45()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.Boni.SetTo(Bonus.Base.Constitution.Create(70));
			player.DebuffCategory[eProperty.Constitution] = 50;
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Constitution);

            int expected = 70 - (50 / 2);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CalcValue_Constitution_70ConBaseStatAnd3ConLostOnDeath_67()
        {
            var player = Create.FakePlayer();
            player.Level = 50;
			player.Boni.SetTo(Bonus.Base.Constitution.Create(70));
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
        public void CalcValue_50AbilityBonusAnd25PercentMultiplicator_12()
        {
            var player = Create.FakePlayer();
            player.AbilityBonus[eProperty.Dexterity] = 50;
            player.BuffBonusMultCategory1.Set((int)eProperty.Dexterity, new object(), 0.25);
            var statCalc = createStatCalculator();

            int actual = statCalc.CalcValue(player, eProperty.Dexterity);

            Assert.AreEqual(12, actual);
        }
		#endregion

		public static IPropertyCalculator createStatCalculator()
        {
            return new StatCalculator();
        }
    }
}
