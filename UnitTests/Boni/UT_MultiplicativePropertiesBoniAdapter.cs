using DOL.GS.PropertyCalc;
using DOL.GS;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_MultiplicativePropertiesBoniAdapter
	{
		[Test]
		public void Get_Init_100Percent()
		{
			var multiplicativeIndexer = createMultiplicativeBoniAdapter();

			double actual = multiplicativeIndexer.Get((int)eProperty.Undefined);
			double expected = 1.0;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Set_125PercentMaxSpeed_SpeedIs125Percent()
		{
			var multiplicativeIndexer = createMultiplicativeBoniAdapter();

			multiplicativeIndexer.Set((int)eProperty.MaxSpeed, staticReferenceObject, 1.25);

			double actual = multiplicativeIndexer.Get((int)eProperty.MaxSpeed);
			double expected = 1.25;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Set_125PercentMaxSpeed_To200PercentMaxSpeed_SpeedIs250Percent()
		{
			var multiplicativeIndexer = createMultiplicativeBoniAdapter();
			multiplicativeIndexer.Set((int)SomeBonusTypeDatabaseID, staticReferenceObject, 2.00);
			var secondStaticObject = new object();
			multiplicativeIndexer.Set((int)SomeBonusTypeDatabaseID, secondStaticObject, 1.25);

			double actual = multiplicativeIndexer.Get((int)eProperty.MaxSpeed);
			double expected = 2.5;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Remove_SetBonus_100Percent()
		{
			var multiplicativeIndexer = createMultiplicativeBoniAdapter();
			multiplicativeIndexer.Set((int)SomeBonusTypeDatabaseID, staticReferenceObject, 1.25);

			multiplicativeIndexer.Remove((int)SomeBonusTypeDatabaseID, staticReferenceObject);

			double actual = multiplicativeIndexer.Get((int)eProperty.Undefined);
			double expected = 1.0;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Remove_BonusWithWrongReference_Remains125Percent()
		{
			var multiplicativeIndexer = createMultiplicativeBoniAdapter();
			multiplicativeIndexer.Set((int)SomeBonusTypeDatabaseID, staticReferenceObject, 1.25);
			var anotherObject = new object();

			multiplicativeIndexer.Remove((int)SomeBonusTypeDatabaseID, anotherObject);

			double actual = multiplicativeIndexer.Get((int)SomeBonusTypeDatabaseID);
			double expected = 1.25;
			Assert.AreEqual(expected, actual, 0.001);
		}

		private static readonly object staticReferenceObject = new object();
		private eProperty SomeBonusTypeDatabaseID => eProperty.MaxSpeed;

		private static IMultiplicativeProperties createMultiplicativeBoniAdapter()
		{
			var boni = new Boni(Create.FakeNPC());
			return new MultiplicativePropertiesBoniAdapter(boni);
		}
	}
}
