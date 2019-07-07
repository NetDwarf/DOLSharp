using NUnit.Framework;
using DOL.GS.PropertyCalc;
using DOL.GS;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	public class UT_ArcaneSyphoneCalculator
	{
		[Test]
		public void CalcValue_Player_30Item_25()
		{
			var player = Create.FakePlayer();
			var calc = createArcaneSyphonCalculator();
			player.Boni.Add(ArcaneSyphonType.Item.Create(30));

			int actual = calc.CalcValue(player, ArcaneSyphonType.DatabaseID);

			int expected = 25;
			Assert.AreEqual(expected, actual);
		}

		private BonusType ArcaneSyphonType => new BonusType(eBonusType.ArcaneSyphon);

		private static IPropertyCalculator createArcaneSyphonCalculator()
		{
			return new ArcaneSyphonCalculator();
		}
	}
}
