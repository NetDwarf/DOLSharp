using DOL.GS;
using DOL.GS.PropertyCalc;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
{
    [TestFixture]
    class UT_PropertyIndexer
    {
        [Test]
        public void IndexGetter_UninitializedValue_ReturnZero()
        {
            var propIndexer = new PropertyIndexer();

            int actual = propIndexer[0];

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void IndexSetter_AddOneToUninitializedElement_ReturnOne()
        {
            var propIndexer = new PropertyIndexer();

            propIndexer[0] += 1;
            int actual = propIndexer[0];

            Assert.AreEqual(1, actual);
        }
    }

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

		[Test]
		public void Clear_ConstitutionIsOne_ConstitutionIsZero()
		{
			var propIndexer = createAbilityIndexerAdapter();
			propIndexer[eProperty.Constitution] = 1;

			propIndexer.Clear();

			int actual = propIndexer[eProperty.Constitution];
			Assert.AreEqual(0, actual);
		}

		private static IndexerBoniAdapter createAbilityIndexerAdapter()
		{
			var boni = new Boni(null);
			return new IndexerBoniAdapter(boni, ePropertyCategory.Ability);
		}
	}
}
