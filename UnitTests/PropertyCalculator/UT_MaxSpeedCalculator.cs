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

			int actual = movementSpeedCalculator.CalcValue(npc, eProperty.MaxSpeed);
			int expected = 200;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void CalcValue_NPCWith1250PermilleSpeedBuff_250()
		{
			var npc = Create.FakeNPC();
			var maxSpeed = new BonusType(eBonusType.MaxSpeed);
			var movementSpeedCalculator = createMovementSpeedCalculator();
			
			npc.Boni.Add(maxSpeed.Multiplier.Create(1250));

			int actual = movementSpeedCalculator.CalcValue(npc, eProperty.MaxSpeed);
			int expected = 250;
			Assert.AreEqual(expected, actual);
		}

		private static MaxSpeedCalculator createMovementSpeedCalculator()
		{
			return new MaxSpeedCalculator();
		}
	}
}
