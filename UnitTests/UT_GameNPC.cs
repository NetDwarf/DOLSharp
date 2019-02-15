using NUnit.Framework;
using NSubstitute;
using DOL.GS;
using DOL.AI;
using DOL.GS.PropertyCalc;

namespace DOL.UnitTests.Gameserver
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

		private static GameNPC createNPC()
		{
			var brain = Substitute.For<ABrain>();
			var npc = new GameNPC(brain);
			return npc;
		}
	}
}
