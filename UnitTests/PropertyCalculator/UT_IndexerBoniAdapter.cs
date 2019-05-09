using DOL.GS;
using DOL.GS.PropertyCalc;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer.PropertyCalc
{
	[TestFixture]
	class UT_IndexerBoniAdapter
	{
		[Test]
		public void IndexGetter_Default_ReturnZero()
		{
			var propIndexer = createAbilityIndexerAdapter();

			int actual = propIndexer[0];

			Assert.AreEqual(0, actual);
		}

		[Test]
		public void IndexSetter_AddOneToInit_ReturnOne()
		{
			var propIndexer = createAbilityIndexerAdapter();

			propIndexer[0] = 1;
			int actual = propIndexer[0];

			Assert.AreEqual(1, actual);
		}

		private static IndexerBoniAdapter createAbilityIndexerAdapter()
		{
			var boni = new Boni();
			return new IndexerBoniAdapter(boni, Bonus.Ability);
		}
	}
}
