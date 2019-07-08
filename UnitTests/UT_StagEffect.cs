﻿using DOL.GS;
using DOL.GS.Effects;
using DOL.GS.PropertyCalc;
using NSubstitute;
using NUnit.Framework;

namespace DOL.UnitTests.GameServer
{
	[TestFixture]
	class UT_StagEffect
	{
		[TestFixtureSetUp]
		public void init()
		{
			GS.ServerProperties.Properties.DISABLED_REGIONS = "";
			GS.ServerProperties.Properties.DISABLED_EXPANSIONS = "";
			GamePlayer.LoadCalculators();
		}

		[Test]
		public void Start_L50Player_EffectLevel5_LowerBoundMaxHealthIs505()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.CurrentRegion = new Region(new GameTimer.TimeManager("test"), new RegionData());
			var stagEffect = new StagEffect(5);
			setRandomDoubleTo(0);
			Assert.AreEqual(326, player.MaxHealth, "Player has different base health pool");

			stagEffect.Start(player);

			int actual = player.MaxHealth;
			int expected = 505;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Start_L50Player_EffectLevel5_UpperBoundMaxHealthIs537()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.CurrentRegion = new Region(new GameTimer.TimeManager("test"), new RegionData());
			var stagEffect = new StagEffect(5);
			setRandomDoubleTo(1);

			stagEffect.Start(player);

			int actual = player.MaxHealth;
			int expected = 537;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Start_L50PlayerAnd100BaseBuffHealthPool_LowerBoundMaxHealthIs605()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.CurrentRegion = new Region(new GameTimer.TimeManager("test"), new RegionData());
			var stagEffect = new StagEffect(5);
			player.BaseBuffBonusCategory[MaxHealthID] = 100;
			setRandomDoubleTo(0);

			stagEffect.Start(player);

			int actual = player.MaxHealth;
			int expected = 605;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Stop_L50Player_MaxHealthIs326()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.CurrentRegion = new Region(new GameTimer.TimeManager("test"), new RegionData());
			var stagEffect = new StagEffect(5);
			setRandomDoubleToDefault();

			stagEffect.Start(player);
			stagEffect.Stop();

			int actual = player.MaxHealth;
			int expected = 326;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Stop_L50PlayerAnd100BaseBuffHealthPool_426()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.CurrentRegion = new Region(new GameTimer.TimeManager("test"), new RegionData());
			var stagEffect = new StagEffect(5);
			player.BaseBuffBonusCategory[MaxHealthID] = 100;
			setRandomDoubleToDefault();
			Assert.AreEqual(426, player.MaxHealth, "Player has different base health pool");

			stagEffect.Start(player);
			stagEffect.Stop();

			int actual = player.MaxHealth;
			int expected = 426;
			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void Stop_L50PlayerWithScarsOfBattle_LowerBoundMaxHealthIs537()
		{
			var player = Create.FakePlayer();
			player.Level = 50;
			player.CurrentRegion = new Region(new GameTimer.TimeManager("test"), new RegionData());
			player.abilities.Add("Scars of Battle");
			var stagEffect = new StagEffect(5);
			setRandomDoubleTo(0);

			stagEffect.Start(player);

			int actual = player.MaxHealth;
			var expected = 537;
			Assert.AreEqual(expected, actual);
		}

		private eProperty MaxHealthID => eProperty.MaxHealth;

		private static void setRandomDoubleTo(double value)
		{
			var util = new ConstantRandomUtil(value);
			Util.LoadTestDouble(util);
		}

		private static void setRandomDoubleToDefault()
		{
			Util.LoadTestDouble(new Util());
		}

		private static IPropertyCalculator createMaxHealthCalc()
		{
			return new MaxHealthCalculator();
		}
	}
}
