using NUnit.Framework;
using DOL.GS;
using DOL.GS.PropertyCalc;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_PropertyIndexerAdapter
	{
		[Test]
		public void IndexSet_ConstitutionAbilityToOne_GetValueOfIsOne()
		{
			var bonusAggregator = createBonusAggregator();
			var indexerAdapter = createIndexerAdapterFrom(bonusAggregator);

			indexerAdapter[eProperty.Constitution] = 1;

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void IndexGet_ConstitutionAbility_BonusSetToOne_One()
		{
			var bonusAggregator = createBonusAggregator();
			var indexerAdapter = createIndexerAdapterFrom(bonusAggregator);

			bonusAggregator.Set(Bonus.Constitution.Ability.WithValue(1));

			int actual = indexerAdapter[eProperty.Constitution];
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void IndexSet_ConstitutionAbilityToZero_GetValueOfIsZero()
		{
			var bonusAggregator = createBonusAggregator();
			var indexerAdapter = createIndexerAdapterFrom(bonusAggregator);

			bonusAggregator.Set(Bonus.Constitution.Ability.WithValue(0));

			int actual = indexerAdapter[eProperty.Constitution];
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		private BonusAggregator createBonusAggregator()
		{
			return new BonusAggregator();
		}

		private IPropertyIndexer createIndexerAdapterFrom(BonusAggregator bonusAggregator)
		{
			var someSource = Bonus.Ability;
			return new BonusAggregatorToIndexerAdapter(bonusAggregator, someSource);
		}
	}
}
