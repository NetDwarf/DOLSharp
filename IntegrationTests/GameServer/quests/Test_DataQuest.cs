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
        DBDataQuest genericDataQuest = new DBDataQuest();
        FakeServer fakeServer;

        [SetUp]
        public void init()
        {
            genericDataQuest.ID = 23886;
            genericDataQuest.Name = "QuestName";
            genericDataQuest.StartType = 0;
            genericDataQuest.StartName = "StartNPC";
            genericDataQuest.StartRegionID = 27;
            genericDataQuest.AcceptText = "defense";
            genericDataQuest.Description = "description";
            genericDataQuest.SourceName = null;
            genericDataQuest.SourceText = "storyA||storyC|storyD|storyEnd";
            genericDataQuest.StepType = "0|4|6|6|5";
            genericDataQuest.StepText = "step1|step2|step3|step4|step5";
            genericDataQuest.StepItemTemplates = "item1|item2|item3|item4|item5";
            genericDataQuest.AdvanceText = "||important mission|warn|";
            genericDataQuest.TargetName = "tic;10|trick;9|truck;8|track;7|trock;6";
            genericDataQuest.TargetText = "foo|bar|baz|bork|fuu";
            genericDataQuest.CollectItemTemplate = "bring|me|this|darn|item";
            genericDataQuest.MaxCount = 1;
            genericDataQuest.MinLevel = 1;
            genericDataQuest.MaxLevel = 11;
            genericDataQuest.RewardMoney = "1|2|3|4|5";
            genericDataQuest.RewardXP = "6|7|8|9|10";
            genericDataQuest.OptionalRewardItemTemplates = null;
            genericDataQuest.FinalRewardItemTemplates = null;
            genericDataQuest.FinishText = "Done";
            genericDataQuest.QuestDependency = "DependingQuest";
            genericDataQuest.ClassType = null;
            genericDataQuest.AllowedClasses = "1|2|3";
            genericDataQuest.RewardCLXP = null;
            genericDataQuest.RewardRP = null;
            genericDataQuest.RewardBP = null;

            fakeServer = FakeServer.LoadAndReturn();
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
            var dataQuest = new DataQuestSpy(genericDataQuest);

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
            Assert.AreEqual(new List<string>() { "DependingQuest" }, dataQuest.SpyQuestDependency);
            Assert.AreEqual(new List<byte> { 1, 2, 3 }, dataQuest.SpyAllowedClasses);
            Assert.AreEqual("storyA", dataQuest.Story);
        }

        [Test]
        public void ParseQuest_GivenDBDataQuest_CompareStepData()
        {
            var dataQuest = new DataQuestSpy(genericDataQuest);

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
                Assert.AreEqual(new string[] { "", "", "important mission", "warn", "" }[index], dataQuest.SpyAdvanceText);
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
            var dataQuest = new DataQuestSpy(genericDataQuest, null);

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
        public void DataQuest_StartTypeIsCollectionAndPlayerGivesKillTaskItem_PlayerGainsOneExp()
        {
            var questNPCName = "foo";
            var questNPCRegion = 0;
            var dbDataQuest = NewDBDataQuest();
            var taskItemIDNB = "kill_task_item";
            dbDataQuest.StartType = (byte)DataQuest.eStartType.Collection;
            dbDataQuest.RewardXP = "1";
            dbDataQuest.TargetName = questNPCName + ";" + questNPCRegion;
            dbDataQuest.CollectItemTemplate = taskItemIDNB;
            var questItem = new ItemTemplate();
            questItem.Id_nb = taskItemIDNB;
            var player = new FakePlayerSpy();
            var invItem = new GameInventoryItem(questItem);
            invItem.OwnerID = "";
            var questNPC = new FakeNPC();
            questNPC.Name = questNPCName;
            var dataQuest = new DataQuestSpy(dbDataQuest, questNPC);
            player.AddQuest(dataQuest);
            questNPC.AddDataQuest(dataQuest);

            questNPC.ReceiveItem(player, invItem);

            Assert.Contains(GamePlayerEvent.ReceiveItem, dataQuest.SpyNotifyEvents);
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
            invItem.OwnerID = "";
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

        [Test]
        public void DataQuest_CurrentStepTypeIsDeliverFinishAndPlayerGivesCorrectItem_PlayerGainsOneExp()
        {
            var questNPCName = "foo";
            var questNPCRegion = 0;
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.StartType = (byte)DataQuest.eStartType.Standard;
            dbDataQuest.StepType = ((byte)DataQuest.eStepType.DeliverFinish).ToString();
            dbDataQuest.RewardXP = "1";
            dbDataQuest.TargetName = questNPCName + ";" + questNPCRegion;
            dbDataQuest.CollectItemTemplate = "the_item";
            var questItem = new ItemTemplate();
            questItem.Id_nb = "the_item";
            var player = new FakePlayerSpy();
            var invItem = new GameInventoryItem(questItem);
            invItem.OwnerID = "";
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

        [Test]
        public void DataQuest_StartTypeIsStandardAndPlayerWhispersCodeWord_PlayerAcquiresQuest()
        {
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.StartType = (byte)DataQuest.eStartType.Standard;
            dbDataQuest.AcceptText = "codeWord";
            var player = new FakePlayerSpy();
            var questNPC = new FakeNPC();
            var dataQuest = new DataQuestSpy(dbDataQuest);
            questNPC.AddDataQuest(dataQuest);

            player.Whisper(questNPC, "codeWord");

            Assert.Contains(GameLivingEvent.WhisperReceive, dataQuest.SpyNotifyEvents);
            Assert.AreEqual(1, player.QuestList.Count);
        }

        [Test]
        public void DataQuest_CurrentStepTypeIsWhisperFinishAndPlayerWhispersCodeWord_PlayerGainsOneExp()
        {
            var questNPCName = "foo";
            var questNPCRegion = 0;
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.StepType = ((byte)DataQuest.eStepType.WhisperFinish).ToString();
            dbDataQuest.RewardXP = "1";
            dbDataQuest.TargetName = questNPCName + ";" + questNPCRegion;
            dbDataQuest.AdvanceText = "codeWord";
            var player = new FakePlayerSpy();
            var questNPC = new FakeNPC();
            questNPC.Name = questNPCName;
            var dataQuest = new DataQuestSpy(player, null, dbDataQuest);
            player.AddQuest(dataQuest);

            dataQuest.Step = 1;
            player.Whisper(questNPC, "codeWord");

            Assert.Contains(GameLivingEvent.Whisper, dataQuest.SpyNotifyEvents);
            var actual = player.SpyLastExperienceGained;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void DataQuest_StartTypeIsKillCompleteAndPlayerKillsTaskMob_PlayerGainsOneExp()
        {
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.StartType = (byte)DataQuest.eStartType.KillComplete;
            dbDataQuest.RewardXP = "1";
            var player = new FakePlayerSpy();
            var taskMob = new FakeNPC();
            var dataQuest = new DataQuestSpy(dbDataQuest);
            taskMob.AddDataQuest(dataQuest);

            taskMob.Die(player);

            Assert.Contains(GameLivingEvent.Dying, dataQuest.SpyNotifyEvents);
            var actual = player.SpyLastExperienceGained;
            Assert.AreEqual(1, actual);
        }

        [Test]
        public void DataQuest_HasNoOptionalRewardAndPlayerChoosesNone_PlayerGainsOneExp()
        {
            var dbDataQuest = NewDBDataQuest();
            dbDataQuest.RewardXP = "1";
            var player = new FakePlayerSpy();
            var dataQuest = new DataQuestSpy(player, null, dbDataQuest);

            dataQuest.Step = 1;
            var internalQuestID = dbDataQuest.ID + 32767;
            dataQuest.Notify(GamePlayerEvent.QuestRewardChosen, player, new QuestRewardChosenEventArgs(-1, internalQuestID, 0, null));

            Assert.Contains(GamePlayerEvent.QuestRewardChosen, dataQuest.SpyNotifyEvents);
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
            public string SpyAdvanceText => AdvanceText;

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
