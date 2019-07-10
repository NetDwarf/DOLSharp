using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;
using NSubstitute;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_MaxSpeedCalculator
	{
		[Test]
		public void CalcValue_DefaultNPCSpeed_200()
		{
			var npc = Create.FakeNPC();
			var movementSpeedCalculator = createMovementSpeedCalculator();

			int actual = movementSpeedCalculator.CalcValue(npc, MaxSpeedID);
			int expected = 200;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPCWith1250PermilleSpeedBuff_250()
		{
			var npc = Create.FakeNPC();
			var movementSpeedCalculator = createMovementSpeedCalculator();
			
			npc.BuffBonusMultCategory1.Set((int)MaxSpeedID, new object(), 1.25);

			int actual = movementSpeedCalculator.CalcValue(npc, MaxSpeedID);
			int expected = 250;
			Assert.AreEqual(expected, actual);
		}

		private eProperty MaxSpeedID => eProperty.MaxSpeed;

		private static MaxSpeedCalculator createMovementSpeedCalculator()
		{
			return new MaxSpeedCalculator();
		}
	}
}
