using DOL.GS.PropertyCalc;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
{
    [TestFixture]
    class UT_PropertyIndexer
    {
        [Test]
        public void IndexAccessor_UninitializedValue_ReturnZero()
        {
            var propIndexer = new PropertyIndexer();

            int actual = propIndexer[0];

            Assert.AreEqual(0, actual);
        }

        [Test]
        public void IndexAccessor_AddOneToUninitializedElement_ReturnOne()
        {
            var propIndexer = new PropertyIndexer();

            propIndexer[0] += 1;
            int actual = propIndexer[0];

            Assert.AreEqual(1, actual);
        }
    }
}
