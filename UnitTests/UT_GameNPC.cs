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
			npc.Boni.SetTo(Bonus.Constitution.Base.Create(75));

            int actual = npc.GetModified(eProperty.Constitution);
            
            Assert.AreEqual(75, actual);
        }

		[Test]
		public void GetModified_Acuity_75BaseAcuityAndIntelligenceAdded_Zero()
		{
			var npc = createGenericNPC();
			npc.Boni.Add(Bonus.Acuity.Base.Create(75));
			npc.Boni.Add(Bonus.Intelligence.Base.Create(75));

			int actual = npc.GetModified(eProperty.Acuity);

			Assert.AreEqual(0, actual);
		}

		[Test]
		public void GetModified_MeleeDamage_Has9AbilityMeleeDamage_9()
		{
			var npc = createGenericNPC();
			var meleeDamage = new BonusType(eProperty.MeleeDamage);
			npc.Boni.Add(meleeDamage.Ability.Create(9));

			int actual = npc.GetModified(eProperty.MeleeDamage);

			Assert.AreEqual(9, actual);
		}

		[Test]
		public void GetModified_MatterResist_Has9ItemMatterResist_9()
		{
			var npc = createGenericNPC();
			var matterResist = new BonusType(eProperty.Resist_Matter);
			npc.Boni.Add(matterResist.Ability.Create(9));

			int actual = npc.GetModified(eProperty.Resist_Matter);

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
		public void ChangeBaseStat_AddOneCon_NPCHasTwoConstitution()
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
		public void GetResistBase_FromMatter_NPCHasOneMatterBaseBuff_One()
		{
			var npc = createGenericNPC();
			var matterResist = new BonusType(eProperty.Resist_Matter);
			npc.Boni.Add(matterResist.BaseBuff.Create(1));

			int actual = npc.GetResistBase(eDamageType.Matter);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetResistBase_FromNatural_NPCHasOneNaturalBaseBuff_Zero()
		{
			var npc = createGenericNPC();
			var naturalResist = new BonusType(eProperty.Resist_Natural);
			npc.Boni.Add(naturalResist.BaseBuff.Create(1));

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
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void ChanceToFumble_Level50_OnePermille()
		{
			var npc = createGenericNPC();
			npc.Level = 50;

			var actual = npc.ChanceToFumble;

			var expected = 0.001;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void ChanceToFumble_TenDebuff_TenPercent()
		{
			var npc = createGenericNPC();
			npc.Boni.Add(FumbleChance.Debuff.Create(10));

			var actual = npc.ChanceToFumble;

			var expected = 0.10;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void ChanceToFumble_TenAbility_TenPercent()
		{
			var npc = createGenericNPC();
			npc.Boni.Add(FumbleChance.Ability.Create(10));

			var actual = npc.ChanceToFumble;

			var expected = 0.10;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void GetWeaponStat_Add100StrengthBaseBuff_101()
		{
			var npc = createGenericNPC();

			npc.Boni.Add(Bonus.Strength.BaseBuff.Create(100));

			var actual = npc.GetWeaponStat(null);
			var expected = 101;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetArmorAF_Add100BaseBuff_101()
		{
			var npc = createGenericNPC();

			npc.Boni.Add(ArmorFactor.BaseBuff.Create(100));

			var actual = npc.GetArmorAF(eArmorSlot.NOTSET);
			var expected = 9;
			Assert.AreEqual(expected, actual, 0.001);
		}

		private BonusType FumbleChance => new BonusType(eBonusType.FumbleChance);
		private BonusType ArmorFactor => new BonusType(eBonusType.ArmorFactor);

		private static GameNPC createGenericNPC()
		{
			var brain = Substitute.For<ABrain>();
			var npc = new GameNPC(brain);
			npc.LivingRace = new FakeRace();
			return npc;
		}
	}
}
