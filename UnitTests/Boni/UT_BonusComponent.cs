using NUnit.Framework;
using DOL.GS;
using System.Collections.Generic;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_BonusComponent
	{
		[Test]
		public void Equals_AnotherComponent_WithSameCategoryAndProperty_True()
		{
			var constitution = new BonusType(eProperty.Constitution);
			var component1 = new BonusComponent(Bonus.Base, constitution);
			var component2 = new BonusComponent(Bonus.Base, constitution);

			Assert.IsTrue(component1.Equals(component2));
		}

		[Test]
		public void Constructor_ItemConstitutionOvercap_ItemOvercapAndConstitution()
		{
			var constitutionCap = new BonusType(eProperty.ConCapBonus);

			var actual = new BonusComponent(Bonus.Item, constitutionCap);

			var expected = Bonus.Stat.Constitution.ItemOvercap;
			Assert.AreEqual(expected, actual);
		}
	}
}
