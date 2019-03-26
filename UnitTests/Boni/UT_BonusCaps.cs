using NUnit.Framework;
using DOL.GS;

namespace DOL.UnitTests.GameServer.BonusCap
{
	[TestFixture]
	class UT_BonusCaps
	{
		[Test]
		public void Of_Undefined_NullCap()
		{
			var bonusCaps = createBonusCaps();

			var actual = bonusCaps.Of(Bonus.UndefinedType);

			Assert.That(actual, Is.TypeOf(typeof(NullCap)));
		}

		[Test]
		public void Of_Constitution_StatCap()
		{
			var bonusCaps = createBonusCaps();

			var actual = bonusCaps.Of(Bonus.Stat.Constitution);

			Assert.That(actual, Is.TypeOf(typeof(StatCap)));
		}

		[Test]
		public void Of_Dexterity_StatCap()
		{
			var bonusCaps = createBonusCaps();

			var actual = bonusCaps.Of(Bonus.Stat.Dexterity);

			Assert.That(actual, Is.TypeOf(typeof(StatCap)));
		}

		[Test]
		public void Of_HeatResist_StatCap()
		{
			var bonusCaps = createBonusCaps();

			var actual = bonusCaps.Of(Bonus.Resist.Heat);
			
			Assert.That(actual, Is.TypeOf(typeof(ResistCap)));
		}

		[Test]
		public void Of_Constitution_TwoPlayers_StatCapsAreNotSameObject()
		{
			var player1 = Create.FakePlayer();
			var player2 = Create.FakePlayer();
			var bonusCaps1 = createBonusCaps(player1);
			var bonusCaps2 = createBonusCaps(player2);

			var statCap1 = bonusCaps1.Of(Bonus.Stat.Constitution);
			var statCap2 = bonusCaps2.Of(Bonus.Stat.Constitution);

			Assert.AreNotSame(statCap1, statCap2);
		}

		private PropertyCaps createBonusCaps(GamePlayer owner)
		{
			return new PropertyCaps(owner);
		}

		private PropertyCaps createBonusCaps()
		{
			var owner = Create.FakePlayer();
			return new PropertyCaps(owner);
		}
	}

	[TestFixture]
	class UT_StatCap
	{
		[Test]
		public void For_Base_MaxInt()
		{
			var anything = Create.FakeNPC();
			var statCap = createStatCap(anything);

			int actual = statCap.For(Bonus.Base);

			int expected = int.MaxValue;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void For_Item_L50Player_75()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			var statCap = createStatCap(player);

			int actual = statCap.For(Bonus.Item);

			int expected = 75;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void For_Item_L40Player_60()
		{
			var player = Create.FakePlayer();
			player.Level = 40;
			var statCap = createStatCap(player);

			int actual = statCap.For(Bonus.Item);

			int expected = 60;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void For_ConstitutionBaseBuff_L50Player_62()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			var statCap = createStatCap(player);

			int actual = statCap.For(Bonus.BaseBuff);

			int expected = 62;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void For_ConstitutionBaseBuff_L40Player_50()
		{
			var player = Create.FakePlayer();
			player.Level = 40;
			var statCap = createStatCap(player);

			int actual = statCap.For(Bonus.BaseBuff);

			int expected = 50;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void For_ConstitutionSpecBuff_L40Player_50()
		{
			var player = Create.FakePlayer();
			player.Level = 40;
			var statCap = createStatCap(player);

			int actual = statCap.For(Bonus.SpecBuff);

			int expected = 75;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void For_ConstitutionItemOvercap_L40Player_21()
		{
			var player = Create.FakePlayer();
			player.Level = 40;
			var statCap = createStatCap(player);

			int actual = statCap.For(Bonus.ItemOvercap);

			int expected = 21;
			Assert.AreEqual(expected, actual);
		}

		private static StatCap createStatCap(GameLiving owner)
		{
			return new StatCap(owner);
		}
	}

	[TestFixture]
	class UT_ResistCap
	{
		[Test]
		public void For_Item_L40Player_21()
		{
			var player = Create.FakePlayer();
			player.Level = 40;
			var resistCap = createResistCap(player);

			int actual = resistCap.For(Bonus.Item);

			int expected = 21;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Buff_Player_24()
		{
			var player = Create.FakePlayer();
			var resistCap = createResistCap(player);

			int actual = resistCap.Buff;

			int expected = 24;
			Assert.AreEqual(expected, actual);
		}

		private static ResistCap createResistCap(GameLiving owner)
		{
			return new ResistCap(owner);
		}
	}
}
