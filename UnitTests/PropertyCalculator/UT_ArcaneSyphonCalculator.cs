using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_ArcaneSyphonCalculator
	{
		[Test]
		public void CalcValue_Player_30Item_25()
		{
			var player = Create.FakePlayer();
			var calc = createArcaneSyphonCalculator();
			player.ItemBonus[ArcaneSiphonID] = 30;

			int actual = calc.CalcValue(player, ArcaneSiphonID);

			int expected = 25;
			Assert.AreEqual(expected, actual);
		}
		
		private eProperty ArcaneSiphonID => eProperty.ArcaneSyphon;

		private static IPropertyCalculator createArcaneSyphonCalculator()
		{
			return new ArcaneSyphonCalculator();
		}
	}
}
