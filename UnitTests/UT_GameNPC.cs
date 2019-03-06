using NUnit.Framework;
using NSubstitute;
using DOL.GS;
using DOL.AI;
using System;

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
			var npc = createNPC();
			npc.Boni.SetTo(Bonus.Base.Constitution.Create(75));

            int actual = npc.GetModified(eProperty.Constitution);
            
            Assert.AreEqual(75, actual);
        }

		[Test]
		public void Constitution_Init_NPCHasOneConstitution()
		{
			var npc = createNPC();

			int actual = npc.Constitution;
			Assert.AreEqual(1, actual);
		}

		[Test]
		public void ChangeBaseStat_AddOneCon_NPCHasTwoConstitution()
		{
			var npc = createNPC();

			npc.ChangeBaseStat(eStat.CON, 1);

			int actual = npc.Constitution;
			Assert.AreEqual(2, actual);
		}

		[Test]
		public void GetResistBase_FromMatterResist_Init_Zero()
		{
			var npc = createNPC();

			int actual = npc.GetResistBase(eDamageType.Matter);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetResistBase_FromMatter_NPCHasOneMatterBaseBuff_One()
		{
			var npc = createNPC();
			npc.Boni.SetTo(Bonus.BaseBuff.Create(1, eProperty.Resist_Matter));

			int actual = npc.GetResistBase(eDamageType.Matter);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetResistBase_FromMatter_NPCHasOneItemMatterResist_One()
		{
			var npc = createNPC();
			npc.Boni.SetTo(Bonus.Item.Create(1, eProperty.Resist_Matter));

			int actual = npc.GetResistBase(eDamageType.Matter);

			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetResistBase_FromNatural_NPCHasOneNaturalBaseBuff_Zero()
		{
			var npc = createNPC();
			npc.Boni.SetTo(Bonus.BaseBuff.Create(1, eProperty.Resist_Natural));

			int actual = npc.GetResistBase(eDamageType.Natural);

			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		private static GameNPC createNPC()
		{
			var brain = Substitute.For<ABrain>();
			var npc = new GameNPC(brain);
			return npc;
		}
	}
}
