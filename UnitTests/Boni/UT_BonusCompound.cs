using NUnit.Framework;
using DOL.GS;
using System;
using System.Linq;
using System.Collections.Generic;
using DOL.GS.PropertyCalc;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_BonusComponents
	{
		[Test]
		public void Base_Init_Zero()
		{
			var bonusCompound = createBonusCompound();

			int actual = bonusCompound.Get(Bonus.Base);
			int expected = 0;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneBase_BaseIsOne()
		{
			var bonusCompound = createBonusCompound();

			bonusCompound.Add(1, Bonus.Base);

			int actual = bonusCompound.Get(Bonus.Base);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Remove_OneBaseToInit_BaseIsMinusOne()
		{
			var bonusCompound = createBonusCompound();

			bonusCompound.Remove(1, Bonus.Base);

			int actual = bonusCompound.Get(Bonus.Base);
			int expected = -1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Add_OneAbility_AbilityIsOne()
		{
			var bonusCompound = createBonusCompound();

			bonusCompound.Add(1, Bonus.Ability);

			int actual = bonusCompound.Get(Bonus.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_OneAbility_AbilityIsOne()
		{
			var bonusCompound = createBonusCompound();

			bonusCompound.Set(1, Bonus.Ability);

			int actual = bonusCompound.Get(Bonus.Ability);
			int expected = 1;
			Assert.AreEqual(expected, actual);
		}

		private static BonusCompound createBonusCompound()
		{
			var undefinedBonusType = new BonusType(eBonusType.Undefined);
			return new BonusCompound(undefinedBonusType);
		}
	}

	[TestFixture]
	class UT_IndexerBoniAdapter
	{
		[Test]
		public void Set_StrCapBonusEPropertyTo12_StrengthItemcapBonusIs12()
		{
			var boni = new Boni(null);
			var indexer = new IndexerBoniAdapter(boni, Bonus.Item);

			indexer[eProperty.StrCapBonus] = 12;

			int actual = boni.RawValueOf(Bonus.Strength.ItemOvercap);
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_StrCapBonusEPropertyTo12_Is12()
		{
			var boni = new Boni(null);
			var indexer = new IndexerBoniAdapter(boni, Bonus.Item);

			indexer[eProperty.StrCapBonus] = 12;

			int actual = indexer[eProperty.StrCapBonus];
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_12MythicalSlashResist_MythicalSlashResistIs12()
		{
			var boni = new Boni(null);
			var indexer = new IndexerBoniAdapter(boni, Bonus.Item);
			var slashResist = new BonusType(eBonusType.Resist_Slash);

			indexer[eProperty.SlashResCapBonus] = 12;

			int actual = boni.RawValueOf(slashResist.Mythical);
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_12MythicalSlashResist_Is12()
		{
			var boni = new Boni(null);
			var indexer = new IndexerBoniAdapter(boni, Bonus.Item);

			indexer[eProperty.SlashResCapBonus] = 12;

			int actual = indexer[eProperty.SlashResCapBonus];
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_BaseIndexerOnConstitutionTo12_BaseConstitutionIs12()
		{
			var boni = new Boni(null);
			var indexer = new IndexerBoniAdapter(boni, Bonus.Base);

			indexer[eProperty.Constitution] = 12;

			int actual = boni.RawValueOf(Bonus.Constitution.Base);
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}
	}

	[TestFixture]
	class UT_eBonusPart
	{
		[Test]
		public void Last_EqualsBiggestValue()
		{
			var actual = eBonusPart.__Last;
			var expected = Enum.GetValues(typeof(eBonusPart)).Cast<eBonusPart>().Last<eBonusPart>();

			Assert.AreEqual(actual, expected);
		}
	}

	[TestFixture]
	class UT_BonusFactory
	{
		[Test]
		public void Create_UndefinedEProperty_ThrowArgumentException()
		{
			var bonusFactory = new BonusFactory();
			var undefined = 59;
			
			Assert.Throws<ArgumentException>(() => bonusFactory.CreateComponent((eProperty)59, Bonus.Item.ID));
		}
	}
}
