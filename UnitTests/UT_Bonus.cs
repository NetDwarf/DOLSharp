using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_Bonus
	{
		[Test]
		public void Add_One_ToOneBaseConsitutionBonus_BonusValueIsTwo()
		{
			var bonus = Bonus.Base.Constitution.Create(1);

			bonus.Add(1);

			int actual = bonus.Value;
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_One_OfOneBaseConsitutionBonus_BonusValueIsZero()
		{
			var bonus = Bonus.Base.Constitution.Create(1);

			bonus.Remove(1);

			int actual = bonus.Value;
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}
	}
}
