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
			var constitution = eProperty.Constitution;
			var ability = eBonusSource.Ability;

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Ability);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetValueOf_ConstitutionAbility_AbilityIndexerSetToOneForConstitution_One()
		{
			var bonusAggregator = createBonusAggregator();
			var constitution = eProperty.Constitution;
			var ability = eBonusSource.Ability;

			bonusAggregator.GetIndexerFor(ability)[constitution] = 1;

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetValueOf_ConstitutionItem_ItemIndexerSetToOneForConstitution_One()
		{
			var bonusAggregator = createBonusAggregator();
			var constitution = eProperty.Constitution;
			var item = eBonusSource.Item;

			bonusAggregator.GetIndexerFor(item)[constitution] = 1;

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Item);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_ConstitutionItem2_GetValueOfConstitutionItemIs2()
		{
			var bonusAggregator = createBonusAggregator();
			var constitution = eProperty.Constitution;
			var item = eBonusSource.Item;
			
			bonusAggregator.Set(Bonus.Constitution.Item.Create(2));

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Item);
			int expected = 2;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_OneAbilityConstitution_GetValueOfAbilityConstitutionIsOne()
		{
			var bonusAggregator = createBonusAggregator();
			var constitution = eProperty.Constitution;
			var ability = eBonusSource.Ability;
			var bonus = Bonus.Constitution.Ability.Create(1);

			bonusAggregator.Set(bonus);

			int actual = bonusAggregator.GetValueOf(Bonus.Constitution.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GetValueOf_ConstitutionAbility_AbilityIndexerOfConstitutionSetToOne_One()
		{
			var bonusAggregator = createBonusAggregator();
			var bonus = Bonus.Constitution.Ability.Create(1);

			bonusAggregator.GetIndexerFor(bonus.Source.ID)[bonus.Type.ID] = bonus.Value;

			int actual = bonusAggregator.GetValueOf(bonus.Component);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private BonusAggregator createBonusAggregator() => new BonusAggregator();
	}
}
