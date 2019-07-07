using DOL.GS;
using DOL.GS.PropertyCalc;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_IndexerBoniAdapter
	{
		[Test]
		public void Get_AtIndexZero_Zero()
		{
			var propIndexer = SomeIndexerAdapter;

			int actual = propIndexer[0];

			Assert.AreEqual(0, actual);
		}

		[Test]
		public void Set_IndexZeroToOne_GetAtIndexZeroIsOne()
		{
			var propIndexer = SomeIndexerAdapter;

			propIndexer[0] = 1;
			int actual = propIndexer[0];

			Assert.AreEqual(1, actual);
		}

		[Test]
		public void Set_StrCapBonusTo12_BoniStrengthItemOvercapBonusIs12()
		{
			var boni = createBoni();
			var indexer = createIndexerAdapter(boni, Bonus.Item);

			indexer[eProperty.StrCapBonus] = 12;

			int actual = boni.RawValueOf(Bonus.Strength.ItemOvercap);
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_StrCapBonusTo12_GetIs12()
		{
			var indexer = createIndexerAdapter(createBoni(), Bonus.Item);

			indexer[eProperty.StrCapBonus] = 12;

			int actual = indexer[eProperty.StrCapBonus];
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_12MythicalSlashResist_BoniMythicalSlashResistIs12()
		{
			var boni = createBoni();
			var indexer = createIndexerAdapter(boni, Bonus.Item);
			var slashResist = new BonusType(eBonusType.Resist_Slash);

			indexer[eProperty.SlashResCapBonus] = 12;

			int actual = boni.RawValueOf(slashResist.Mythical);
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_12MythicalSlashResist_GetIs12()
		{
			var indexer = new IndexerBoniAdapter(createBoni(), Bonus.Item);

			indexer[eProperty.SlashResCapBonus] = 12;

			int actual = indexer[eProperty.SlashResCapBonus];
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Set_BaseConstitutionTo12_BoniBaseConstitutionIs12()
		{
			var boni = createBoni();
			var indexer = createIndexerAdapter(boni, Bonus.Base);

			indexer[eProperty.Constitution] = 12;

			int actual = boni.RawValueOf(Bonus.Constitution.Base);
			int expected = 12;
			Assert.AreEqual(expected, actual);
		}

		private Boni createBoni()
		{
			var owner = Create.FakeNPC();
			return new Boni(owner);
		}

		private IPropertyIndexer createIndexerAdapter(Boni boni, BonusPart part)
		{
			return new IndexerBoniAdapter(boni, part);
		}

		private IPropertyIndexer SomeIndexerAdapter
		{
			get
			{
				var owner = Create.FakePlayer();
				var boni = new Boni(owner);
				return new IndexerBoniAdapter(boni, Bonus.Base);
			}
		}
	}
}
