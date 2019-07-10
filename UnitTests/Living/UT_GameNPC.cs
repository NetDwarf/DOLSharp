using NUnit.Framework;
using NSubstitute;
using DOL.GS;
using DOL.AI;

namespace DOL.UnitTests.GameServer
{
    [TestFixture]
    class UT_GameNPC
    {
		[TestFixtureSetUp]
		public void init()
		{
			GameLiving.LoadCalculators();
		}

		[Test]
        public void GetModified_GameNPCWith75Constitution_Return75()
        {
			var npc = createGenericNPC();
			npc.Constitution = 75;

            int actual = npc.GetModified(eProperty.Constitution);
            
            Assert.AreEqual(75, actual);
        }

		[Test]
		public void GetModified_Acuity_With75AbilityAcuityAndIntelligence_Zero()
		{
			var npc = createGenericNPC();
			npc.Intelligence = 75;
			npc.AbilityBonus[AcuityID] = 75;
			npc.AbilityBonus[IntelligenceID] = 75;

			int actual = npc.GetModified(AcuityID);

			Assert.AreEqual(0, actual);
		}

		[Test]
		public void GetModified_MeleeDamage_Has9AbilityMeleeDamage_9()
		{
			var npc = createGenericNPC();
			var meleeDamageID = eProperty.MeleeDamage;
			npc.AbilityBonus[meleeDamageID] = 9;

			int actual = npc.GetModified(meleeDamageID);

			Assert.AreEqual(9, actual);
		}

		[Test]
		public void GetModified_MatterResist_Has9AbilityResist_9()
		{
			var npc = createGenericNPC();
			npc.AbilityBonus[SomeResistID] = 9;

			int actual = npc.GetModified(SomeResistID);

			Assert.AreEqual(9, actual);
		}

		[Test]
		public void Constitution_Init_NPCHasOneConstitution()
		{
			var npc = createGenericNPC();

			int actual = npc.Constitution;
			Assert.AreEqual(1, actual);
		}

		[Test]
		public void ChangeBaseStat_OneConstitution_ConstitutionIs2()
		{
			var npc = createGenericNPC();

			npc.ChangeBaseStat(eStat.CON, 1);

			int actual = npc.Constitution;
			Assert.AreEqual(2, actual);
		}

		[Test]
		public void GetResistBase_FromMatterResist_Init_Zero()
		{
			var npc = createGenericNPC();

			int actual = npc.GetResistBase(eDamageType.Matter);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetResistBase_Matter_WithOneMatterBaseBuff_One()
		{
			var npc = createGenericNPC();
			npc.BaseBuffBonusCategory[eProperty.Resist_Matter] = 1;

			int actual = npc.GetResistBase(eDamageType.Matter);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetResistBase_Natural_WithOneNaturalBaseBuff_Zero()
		{
			var npc = createGenericNPC();
			npc.BaseBuffBonusCategory[eProperty.Resist_Natural] = 1;

			int actual = npc.GetResistBase(eDamageType.Natural);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ChanceToFumble_LevelOne_FivePercent()
		{
			var npc = createGenericNPC();
			npc.Level = 1;

			var actual = npc.ChanceToFumble;

			var expected = 0.05;
			Assert.AreEqual(expected, actual, 0.0001);
		}

		[Test]
		public void ChanceToFumble_Level50_OnePermille()
		{
			var npc = createGenericNPC();
			npc.Level = 50;

			var actual = npc.ChanceToFumble;

			var expected = 0.001;
			Assert.AreEqual(expected, actual, 0.0001);
		}

		[Test]
		public void ChanceToFumble_TenDebuff_TenPercent()
		{
			var npc = createGenericNPC();
			npc.DebuffCategory[FumbleChanceID] = 10;

			var actual = npc.ChanceToFumble;

			var expected = 0.10;
			Assert.AreEqual(expected, actual, 0.0001);
		}

		[Test]
		public void ChanceToFumble_TenAbility_TenPercent()
		{
			var npc = createGenericNPC();
			npc.AbilityBonus[FumbleChanceID] = 10;

			var actual = npc.ChanceToFumble;

			var expected = 0.10;
			Assert.AreEqual(expected, actual, 0.0001);
		}

		[Test]
		public void GetWeaponStat_With100StrengthBaseBuff_101()
		{
			var npc = createGenericNPC();
			
			npc.BaseBuffBonusCategory[eProperty.Strength] = 100;

			var actual = npc.GetWeaponStat(null);
			var expected = 101;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetArmorAF_UnsetSlot_With100BaseBuff_101()
		{
			var npc = createGenericNPC();
			
			npc.BaseBuffBonusCategory[ArmorFactorID] = 100;

			var actual = npc.GetArmorAF(eArmorSlot.NOTSET);
			var expected = 9;
			Assert.AreEqual(expected, actual, 0.0001);
		}

		[Test]
		public void MaxMana_Always_1000()
		{
			var npc = createGenericNPC();

			var actual = npc.MaxMana;

			var expected = 1000;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty AcuityID => eProperty.Acuity;
		private eProperty IntelligenceID => eProperty.Intelligence;
		private eProperty ArmorFactorID => eProperty.ArmorFactor;
		private eProperty BaseStatID => eProperty.Constitution;
		private eProperty SomeResistID => eProperty.Resist_Matter;
		private eProperty FumbleChanceID => eProperty.FumbleChance;

		private GameNPC createGenericNPC()
		{
			var brain = Substitute.For<ABrain>();
			var npc = new GameNPC(brain);
			return npc;
		}
	}
}
