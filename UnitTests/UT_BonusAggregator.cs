using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_BonusAggregator
	{
		[Test]
		public void GetValueOf_ConstitutionAbility_Init_Zero()
		{
			var bonusAggregator = createBonusAggregator();

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Ability);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetValueOf_ConstitutionItem_SetToOne_One()
		{
			var bonusAggregator = createBonusAggregator();

			bonusAggregator.Set(Bonus.Constitution.Item.WithValue(1));

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Item);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_ConstitutionItem2_GetValueOfIs2()
		{
			var bonusAggregator = createBonusAggregator();
			
			bonusAggregator.Set(Bonus.Constitution.Item.WithValue(2));

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Item);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_OneAbilityConstitution_GetValueOfIsOne()
		{
			var bonusAggregator = createBonusAggregator();
			var bonus = Bonus.Constitution.Ability.WithValue(1);

			bonusAggregator.Set(bonus);

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_ExistingBonusFromTwoToOne_GetValueOfIsOne()
		{
			var bonusAggregator = createBonusAggregator();
			var bonusComponent = Bonus.Constitution.Ability;

			bonusAggregator.Set(bonusComponent.WithValue(2));
			bonusAggregator.Set(bonusComponent.WithValue(1));

			int actual = bonusAggregator.GetValueOf(bonusComponent);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private BonusAggregator createBonusAggregator() => new BonusAggregator();
	}
}
