using DOL.GS;
using DOL.GS.PropertyCalc;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
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
			var boni = new Boni(null);
			return new IndexerBoniAdapter(boni, ePropertyCategory.Ability);
		}
	}

	[TestFixture]
	class UT_BoniMultiplicativeAdapter
	{
		[Test]
		public void Get_Init_100Percent()
		{
			var boniAdapter = createBoniAdapter();

			var actual = boniAdapter.Get((int)eProperty.Undefined);
			var expected = 1.0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_125Percent_Get125Percent()
		{
			var boniAdapter = createBoniAdapter();

			boniAdapter.Set((int)eProperty.Undefined, new object(), 1.25);

			var actual = boniAdapter.Get((int)eProperty.Undefined);
			var expected = 1.25;
			Assert.AreEqual(expected, actual);
		}

		private static MultiplicativePropertiesBoniAdapter createBoniAdapter()
		{
			var boni = new Boni(null);
			return new MultiplicativePropertiesBoniAdapter(boni);
		}
	}
}
