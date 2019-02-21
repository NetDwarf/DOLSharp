using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_BonusComponent
	{
		[Test]
		public void Constructor_ItemConstitutionOvercap_ItemOvercapAndConstitution()
		{
			var component = new BonusComponent(ePropertyCategory.Item, eProperty.ConCapBonus);

			var actualCategory = component.Category;
			var expectedCategory = ePropertyCategory.ItemOvercap;
			var actualProperty = component.Property;
			var expectedProperty = eProperty.Constitution;
			Assert.AreEqual(expectedCategory, actualCategory);
			Assert.AreEqual(expectedProperty, actualProperty);
		}
	}
}
