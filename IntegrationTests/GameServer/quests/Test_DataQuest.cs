using DOL.Database;
using DOL.Events;
using DOL.GS;
using DOL.GS.Quests;
using DOL.UnitTests.Gameserver;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace DOL.Integration.GameServer
{
    [TestFixture]
    class Test_DataQuest
    {
        DBDataQuest dbDataQuest = new DBDataQuest();

        [SetUp]
        public void init()
        {
            dbDataQuest.ID = 23886;
            dbDataQuest.Name = "QuestName";
            dbDataQuest.StartType = 0;
            dbDataQuest.StartName = "StartNPC";
            dbDataQuest.StartRegionID = 27;
            dbDataQuest.AcceptText = "defense";
            dbDataQuest.Description = "description";
            dbDataQuest.SourceName = null;
            dbDataQuest.SourceText = "storyA||storyC|storyD|storyEnd";
            dbDataQuest.StepType = "0|4|6|6|5";
            dbDataQuest.StepText = "step1|step2|step3|step4|step5";
            dbDataQuest.StepItemTemplates = "item1|item2|item3|item4|item5";
            dbDataQuest.AdvanceText = "||important mission|warn|";
            dbDataQuest.TargetName = "tic;10|trick;9|truck;8|track;7|trock;6";
            dbDataQuest.TargetText = "foo|bar|baz|bork|fuu";
            dbDataQuest.CollectItemTemplate = "bring|me|this|darn|item";
            dbDataQuest.MaxCount = 1;
            dbDataQuest.MinLevel = 1;
            dbDataQuest.MaxLevel = 11;
            dbDataQuest.RewardMoney = "1|2|3|4|5";
            dbDataQuest.RewardXP = "6|7|8|9|10";
            dbDataQuest.OptionalRewardItemTemplates = null;
            dbDataQuest.FinalRewardItemTemplates = null;
            dbDataQuest.FinishText = "Welcome, young <Class>. You look a bit out of breath! What business brings you to our village?\r\n\r\nYou tell the Veteran Guard that you were sent to warn him about the attack on the tower.\r\n\r\nYes - I can see theres something going on over there. I assumed it was a training exercise. I commend your bravery. You show great promise, here is some coin. Theres a merchant named Miraveth standing next to the well, and she has a cloak for sale that will suit you nicely. Well, my friend, I think it is safe to say you are no longer in need of training. It has been an honor, and I hope you will consider staying around to assist the town further in its time of need. Speak to Artigan, in the upper part of Fintain, if you have more questions. If you ever wish to leave this place, speak with the Channeler here in town. They can send you to the capital at anytime.";
            dbDataQuest.QuestDependency = "Basics of Combat (Hibernia)";
            dbDataQuest.ClassType = null;
            dbDataQuest.AllowedClasses = "1|2|3";
            dbDataQuest.RewardCLXP = null;
            dbDataQuest.RewardRP = null;
            dbDataQuest.RewardBP = null;

            FakeServer.LoadAndReturn();
            GS.ServerProperties.Properties.DISABLED_REGIONS = "";
            GS.ServerProperties.Properties.DISABLED_EXPANSIONS = "";
        }

        [TearDown]
        public void Final()
        {
            GS.GameServer.LoadTestDouble(null);
        }

        [Test]
        public void ParseQuest_GivenDBDataQuest_SetData()
        {
            var dataQuest = new DataQuestSpy(dbDataQuest);

            Assert.AreEqual(23886, dataQuest.ID);
            Assert.AreEqual("QuestName", dataQuest.Name);
            Assert.AreEqual(DataQuest.eStartType.Standard, dataQuest.StartType);
            Assert.AreEqual("description", dataQuest.Description);
            Assert.AreEqual(null, dataQuest.SpySourceName);
            Assert.AreEqual(1, dataQuest.MaxQuestCount);
            Assert.AreEqual(1, dataQuest.Level);
            Assert.AreEqual(11, dataQuest.MaxLevel);
            Assert.AreEqual("", dataQuest.OptionalRewards);
            Assert.AreEqual("", dataQuest.FinalRewards);
            Assert.AreEqual(new List<string>() { "Basics of Combat (Hibernia)" }, dataQuest.SpyQuestDependency);
            Assert.AreEqual(new List<byte> { 1, 2, 3 }, dataQuest.SpyAllowedClasses);
            Assert.AreEqual("storyA", dataQuest.Story);
        }

        [Test]
        public void ParseQuest_GivenDBDataQuest_CompareStepData()
        {
            var dataQuest = new DataQuestSpy(dbDataQuest);

            var stepCount = 5;
            for (int i = 1; i <= stepCount; i++)
            {
                dataQuest.Step = i;
                int index = i - 1;
                Assert.AreEqual(new List<byte>() { 0, 4, 6, 6, 5 }[index], (byte)dataQuest.StepType, "Failed at Step " + dataQuest.Step);
                Assert.AreEqual(new List<long>() { 1, 2, 3, 4, 5 }[index], dataQuest.SpyRewardMoney);
                Assert.AreEqual(new long[] { 6, 7, 8, 9, 10 }[index], dataQuest.SpyRewardXP);
                Assert.AreEqual(0, dataQuest.SpyRewardBP);
                Assert.AreEqual(new string[] { "tic", "trick", "truck", "track", "trock" }[index], dataQuest.TargetName);
                Assert.AreEqual(new long[] { 10, 9, 8, 7, 6 }[index], dataQuest.TargetRegion);
                Assert.AreEqual(new string[] { "foo", "bar", "baz", "bork", "fuu" }[index], dataQuest.SpyTargetText);
                Assert.AreEqual(new string[] { "storyA", "", "storyC", "storyD", "storyEnd" }[index], dataQuest.SpySourceText);
                Assert.AreEqual(new string[] { "step1", "step2", "step3", "step4", "step5"}[index], dataQuest.StepTexts[index]);
                Assert.AreEqual(new string[] { "item1", "item2", "item3", "item4", "item5" }[index], dataQuest.SpyStepItemTemplate);
                Assert.AreEqual(new string[] { "bring", "me", "this", "darn", "item" }[index], dataQuest.SpyCollectItemTemplate);
            }
        }

        [Test]
        public void ParseSearchArea_GenericSearchQuest()
        {
            var dbDataQuest = new DBDataQuest();
            dbDataQuest.SourceName = "SEARCH;2;text;3;4;5;6;7";

            var dataQuest = new DataQuestSpy(dbDataQuest, null);

            var actual = dataQuest.SpyAllQuestSearchAreas[0];
            Assert.AreEqual(typeof(DataQuest), actual.Value.QuestType);
            Assert.AreEqual(0, actual.Key);
            Assert.AreEqual(2, actual.Value.Step);
            Assert.AreEqual(4, actual.Value.X);
            Assert.AreEqual(5, actual.Value.Y);
            Assert.AreEqual(6, actual.Value.Radius);
            Assert.AreEqual(7, actual.Value.SearchSeconds);
            Assert.AreEqual("text", actual.Value.PopupText);

            Assert.AreEqual(1, dataQuest.SpyAllQuestSearchAreas.Count);
        }

        [Test]
        public void ParseSearchArea_GenericSearchStartQuest()
        {
            var dbDataQuest = new DBDataQuest();
            dbDataQuest.SourceName = "SEARCHSTART;itemtemplate;text;3;4;5;6;7";

            var dataQuest = new DataQuestSpy(dbDataQuest, null);

            var actual = dataQuest.SpyAllQuestSearchAreas[0];
            Assert.AreEqual(typeof(DataQuest), actual.Value.QuestType);
            Assert.AreEqual(0, actual.Key);
            Assert.AreEqual("itemtemplate", dataQuest.SpySearchStartItemTemplate);
            Assert.AreEqual(4, actual.Value.X);
            Assert.AreEqual(5, actual.Value.Y);
            Assert.AreEqual(6, actual.Value.Radius);
            Assert.AreEqual(7, actual.Value.SearchSeconds);
            Assert.AreEqual("text", actual.Value.PopupText);

            Assert.AreEqual(1, dataQuest.SpyAllQuestSearchAreas.Count);
        }

        [Test]
        public void NumSearchAreas_Init_Zero()
        {
            var dataQuest = new DataQuestSpy(dbDataQuest, null);

            Assert.AreEqual(0, dataQuest.SpyAllQuestSearchAreas.Count);
        }

        [Test]
        public void NumSearchAreas_TwoGenericSearchStartQuest_2()
        {
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.SourceName = "SEARCHSTART;itemtemplate;text;3;4;5;6;7|SEARCHSTART;itemtemplate;text;3;4;5;6;7";

            var dataQuest = new DataQuestSpy(dbDataQuest, null);

            Assert.AreEqual(2, dataQuest.SpyAllQuestSearchAreas.Count);
        }

        [Test]
        public void DataQuest_StartTypeIsInteractCompleteAndPlayerInteractsWithQuestNPC_PlayerGainsOneExpTotal()
        {
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.StartType = (byte)DataQuest.eStartType.InteractComplete;
            dbDataQuest.RewardXP = "1";
            var dataQuest = new DataQuestSpy(dbDataQuest);
            var player = new FakePlayerSpy();
            var npc = new FakeNPC();
            npc.AddDataQuest(dataQuest);

            npc.Interact(player);

            Assert.Contains(GameObjectEvent.Interact, dataQuest.SpyNotifyEvents);
            var actual = player.SpyLastExperienceGained;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void DataQuest_CurrentStepTypeIsInteractionFinishedAndPlayerInteractsWithQuestNPC_PlayerGainsOneExp()
        {
            var questNPCName = "foo";
            var questNPCRegion = 0;
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.StartType = (byte)DataQuest.eStartType.Standard;
            dbDataQuest.StepType = ((byte)DataQuest.eStepType.InteractFinish).ToString();
            dbDataQuest.RewardXP = "1";
            dbDataQuest.TargetName = questNPCName + ";" + questNPCRegion;
            var player = new FakePlayerSpy();
            var questNPC = new FakeNPC();
            questNPC.Name = questNPCName;
            var dataQuest = new DataQuestSpy(player, null, dbDataQuest);
            player.AddQuest(dataQuest);

            dataQuest.Step = 1;
            questNPC.Interact(player);

            Assert.Contains(GameObjectEvent.InteractWith, dataQuest.SpyNotifyEvents);
            var actual = player.SpyLastExperienceGained;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void DataQuest_CurrentStepTypeIsCollectFinishAndPlayerGivesCorrectItem_PlayerGainsOneExp()
        {
            var questNPCName = "foo";
            var questNPCRegion = 0;
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.StartType = (byte)DataQuest.eStartType.Standard;
            dbDataQuest.StepType = ((byte)DataQuest.eStepType.CollectFinish).ToString();
            dbDataQuest.RewardXP = "1";
            dbDataQuest.TargetName = questNPCName + ";" + questNPCRegion;
            dbDataQuest.CollectItemTemplate = "the_item";
            var questItem = new ItemTemplate();
            questItem.Id_nb = "the_item";
            var player = new FakePlayerSpy();
            var invItem = new GameInventoryItem(questItem);
            invItem.OwnerID = "doesNotMatter";
            var questNPC = new FakeNPC();
            questNPC.Name = questNPCName;
            var dataQuest = new DataQuestSpy(player, null, dbDataQuest);
            player.AddQuest(dataQuest);

            dataQuest.Step = 1;
            player.Notify(GamePlayerEvent.GiveItem, player, new GiveItemEventArgs(player, questNPC, invItem));

            Assert.Contains(GamePlayerEvent.GiveItem, dataQuest.SpyNotifyEvents);
            var actual = player.SpyLastExperienceGained;
            Assert.AreEqual(1, actual);
        }

        private DBDataQuest NewDBDataQuest() => new DBDataQuest();

        private class DataQuestSpy : DataQuest
        {
            public DataQuestSpy(DBDataQuest dbDataQuest) : base(dbDataQuest) { m_charQuest = new CharacterXDataQuest(); }
            public DataQuestSpy(DBDataQuest dbDataQuest, GameObject startingObject) : base(dbDataQuest, startingObject) { m_charQuest = new CharacterXDataQuest(); }
            public DataQuestSpy(GamePlayer questingPlayer, GameObject sourceObject, DBDataQuest dataQuest) : base(questingPlayer, sourceObject, dataQuest, null) 
            { 
                m_charQuest = new CharacterXDataQuest(); 
                m_charQuest.IsPersisted = true; 
            }

            public string SpySourceName => SourceName;
            public List<string> SpyQuestDependency => m_questDependencies;
            public List<byte> SpyAllowedClasses => m_allowedClasses;
            public List<KeyValuePair<int, QuestSearchArea>> SpyAllQuestSearchAreas => questSearchAreas;

            public long SpyRewardMoney => RewardMoney;
            public long SpyRewardXP => RewardXP;
            public long SpyRewardBP => RewardBP;
            public long SpyRewardRP => RewardRP;
            public long SpyRewardCLXP => RewardCLXP;
            public string SpySourceText => SourceText;
            public string SpyTargetText => TargetText;
            public string SpyStepItemTemplate => StepItemTemplate;
            public string SpyCollectItemTemplate => CollectItemTemplate;
            public string SpySearchStartItemTemplate => m_searchStartItemTemplate;

            public List<DOLEvent> SpyNotifyEvents { get; private set; } = new List<DOLEvent>();

            public override void Notify(DOLEvent e, object sender, EventArgs args)
            {
                SpyNotifyEvents.Add(e);
                base.Notify(e, sender, args);
            }
        }

        private class FakePlayerSpy : FakePlayer
        {
            public long SpyLastExperienceGained { get; private set; }

            public override void GainExperience(eXPSource xpSource, long expTotal, long expCampBonus, long expGroupBonus, long expOutpostBonus, bool sendMessage, bool allowMultiply, bool notify)
            {
                SpyLastExperienceGained = expTotal;
            }
        }
    }
}
