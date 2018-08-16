using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_BaseStatIndexer
	{
		[Test]
		public void ToShortArray_SetFirstValueToOne_FirstStatIndexOfIndexerIsOne()
		{
			var indexer = new BasePropertyIndexer();
			var indexerAsArray = indexer.ToShortArray();

			indexerAsArray[0] = 1;

			int actual = indexer[(int)eStat._First];
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void ToShortArray_SetLastValueToOne_LastStatIndexOfIndexerIsOne()
		{
			var indexer = new BasePropertyIndexer();
			var indexerAsArray = indexer.ToShortArray();

			indexerAsArray[indexerAsArray.Length - 1] = 1;

			int actual = indexer[(int)eStat._Last];
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}
	}
}
