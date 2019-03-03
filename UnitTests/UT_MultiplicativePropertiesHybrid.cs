using DOL.GS.PropertyCalc;
using DOL.GS;
using NUnit.Framework;
	
namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_MultiplicativePropertiesHybrid
	{
		[Test]
		public void Get_Init_100Percent()
		{
			var property = createMultiplicativeProperty();

			double actual = property.Get((int)eProperty.Undefined);
			double expected = 1.0;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Set_125PercentMaxSpeed_SpeedIs125Percent()
		{
			var property = createMultiplicativeProperty();

			property.Set((int)eProperty.MaxSpeed, new object(), 1.25);

			double actual = property.Get((int)eProperty.MaxSpeed);
			double expected = 1.25;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Set_125PercentMaxSpeed_To200PercentMaxSpeed_SpeedIs250Percent()
		{
			var property = createMultiplicativeProperty();
			property.Set((int)eProperty.MaxSpeed, new object(), 2.00);

			property.Set((int)eProperty.MaxSpeed, new object(), 1.25);

			double actual = property.Get((int)eProperty.MaxSpeed);
			double expected = 2.5;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Remove_SetBonus_100Percent()
		{
			var property = createMultiplicativeProperty();
			var obj = new object();
			property.Set((int)eProperty.Undefined, obj, 1.25);

			property.Remove((int)eProperty.Undefined, obj);

			double actual = property.Get((int)eProperty.Undefined);
			double expected = 1.0;
			Assert.AreEqual(expected, actual, 0.001);
		}

		[Test]
		public void Remove_BonusWithWrongReference_Remains125Percent()
		{
			var property = createMultiplicativeProperty();
			property.Set((int)eProperty.Undefined, new object(), 1.25);

			property.Remove((int)eProperty.Undefined, new object());

			double actual = property.Get((int)eProperty.Undefined);
			double expected = 1.25;
			Assert.AreEqual(expected, actual, 0.001);
		}

		private static MultiplicativePropertiesHybrid createMultiplicativeProperty()
		{
			return new MultiplicativePropertiesHybrid();
		}
	}
}
