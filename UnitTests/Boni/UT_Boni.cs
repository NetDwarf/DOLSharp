using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_Boni
	{
		[Test]
		public void RawValueOf_SomeComponent_Init_Zero()
		{
			var boni = createBoni();

			int actual = boni.RawValueOf(SomeComponent);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_One_Init_RawValueIsOne()
		{
			var boni = createBoni();
			var bonus = SomeComponent.Create(1);

			boni.Add(bonus);

			int actual = boni.RawValueOf(SomeComponent);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_One_Init_RawValueIsMinusOne()
		{
			var boni = createBoni();
			var bonus = SomeComponent.Create(1);

			boni.Remove(bonus);

			int actual = boni.RawValueOf(SomeComponent);
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SetTo_One_RawValueIsOne()
		{
			var boni = createBoni();

			boni.SetTo(SomeComponent.Create(1));

			int actual = boni.RawValueOf(SomeComponent);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void SetTo_One_Added20Before_RawValueIsOne()
		{
			var boni = createBoni();
			var bonus = SomeComponent.Create(1);

			boni.Add(SomeComponent.Create(20));
			boni.SetTo(bonus);

			int actual = boni.RawValueOf(SomeComponent);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Clear_SomeBonusPart_WithOne_RawValueIsZero()
		{
			var boni = createBoni();
			var bonus = SomeComponent.Create(1);

			boni.Add(bonus);
			boni.Clear(SomeComponent.Part);

			int actual = boni.RawValueOf(SomeComponent);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Clear_Item_With12ItemOvercap_ItemOvercapIsZero()
		{
			var boni = createBoni();
			var someItemOvercapComponent = Bonus.Strength.ItemOvercap;
			var bonus = someItemOvercapComponent.Create(12);

			boni.Add(bonus);
			boni.Clear(Bonus.Item);

			int actual = boni.RawValueOf(someItemOvercapComponent);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		private BonusComponent SomeComponent => Bonus.Constitution.Ability;

		private Boni createBoni()
		{
			var owner = Create.FakeNPC();
			return new Boni(owner);
		}
	}
}
