﻿using DOL.Database;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DOL.GS.Quests
{

	public class DataQuestStep
	{
		public long RewardXP { get; set; } = 0;
		public long RewardCLXP { get; set; } = 0;
		public long RewardRP { get; set; } = 0;
		public long RewardBP { get; set; } = 0;
		public long RewardMoney { get; set; } = 0;
		public DataQuest.eStepType StepType { get; set; } = DataQuest.eStepType.Unknown;
		public string SourceText { get; set; } = "";
		public string TargetText { get; set; } = "";
		public string StepText { get; set; } = "";
		public string StepItemTemplate { get; set; } = "";
		public string AdvanceText { get; set; } = "";
		public string CollectItem { get; set; } = "";
		public string TargetName { get; set; } = "";
		public ushort TargetRegion { get; set; } = 0;
	}

	public class DataQuestDefinition
	{
		public List<byte> AllowedClasses { get; set; }
		public bool ShowIndicator { get; set; }
		public byte NumOptionalRewardsChoice { get; set; }
		public List<ItemTemplate> OptionalRewards { get; set; }
		public List<ItemTemplate> FinalRewards { get; internal set; }
		public List<string> QuestDependencies { get; internal set; }
		public string ClassType { get; internal set; }
		public string AdditionalData { get; internal set; }
	}

	internal class DataQuestParser
	{
		DBDataQuest dbDataQuest;

		public DataQuestDefinition DataQuestDefinition { get; private set; } = new DataQuestDefinition();
		public List<DataQuestStep> DataQuestSteps { get; private set; } = new List<DataQuestStep>();

		private DataQuestParser(DBDataQuest dbDataQuest)
		{
			this.dbDataQuest = dbDataQuest;
		}

		public static DataQuestParser Load(DBDataQuest dbDataQuest)
		{
			var parser = new DataQuestParser(dbDataQuest);
			parser.ParseSteps();
			parser.ParseDefinition();
			return parser;
		}

		private void ParseDefinition()
		{
			DataQuestDefinition.AllowedClasses = ConvertToListOf<byte>(dbDataQuest.AllowedClasses);
			DataQuestDefinition.ShowIndicator = !CheckOccurance(dbDataQuest.SourceName, "NO_INDICATOR");
			DataQuestDefinition.NumOptionalRewardsChoice = ParseNumOptional(dbDataQuest.OptionalRewardItemTemplates);
			var optionalRewardString = "";
			if (!string.IsNullOrEmpty(dbDataQuest.OptionalRewardItemTemplates))
			{
				optionalRewardString= dbDataQuest.OptionalRewardItemTemplates.Substring(1);
			}
			DataQuestDefinition.OptionalRewards = ParseItemTemplates(optionalRewardString);
			DataQuestDefinition.FinalRewards = ParseItemTemplates(dbDataQuest.FinalRewardItemTemplates);
			DataQuestDefinition.QuestDependencies = ParseArrayString(dbDataQuest.QuestDependency);
			var customQuestType = ParseArrayString(dbDataQuest.ClassType);
			DataQuestDefinition.ClassType = customQuestType.Count > 0 ? ParseArrayString(dbDataQuest.ClassType)[0] : ""; 
			DataQuestDefinition.AdditionalData = customQuestType.Count > 1 ? ParseArrayString(dbDataQuest.ClassType)[1] : "";

			this.DataQuestDefinition = DataQuestDefinition;
		}

		private void ParseSteps()
		{
			var moneyRewards = ConvertToListOf<long>(dbDataQuest.RewardMoney);
			var xpRewards = ConvertToListOf<long>(dbDataQuest.RewardXP);
			var clxpRewards = ConvertToListOf<long>(dbDataQuest.RewardCLXP);
			var rpRewards = ConvertToListOf<long>(dbDataQuest.RewardRP);
			var bpRewards = ConvertToListOf<long>(dbDataQuest.RewardBP);
			var stepTypes = ConvertToListOf<byte>(dbDataQuest.StepType);
			var sourceTexts = ParseArrayString(dbDataQuest.SourceText);
			var targetTexts = ParseArrayString(dbDataQuest.TargetText);
			var stepTexts = ParseArrayString(dbDataQuest.StepText);
			var stepItemTemplates = ParseArrayString(dbDataQuest.StepItemTemplates);
			var advanceTexts = ParseArrayString(dbDataQuest.AdvanceText);
			var collectItems = ParseArrayString(dbDataQuest.CollectItemTemplate);
			var targetNames = ParseArrayString<string>(dbDataQuest.TargetName, 0);
			var targetRegions = ParseArrayString<ushort>(dbDataQuest.TargetName, 1);

			var numberOfSteps = new int[] {stepTypes.Count, moneyRewards.Count, xpRewards.Count, clxpRewards.Count, rpRewards.Count,
				bpRewards.Count, targetTexts.Count, stepTexts.Count, targetNames.Count}.Max();
			if (numberOfSteps == 0)
			{
				this.DataQuestSteps.Add(new DataQuestStep());
			}
			else
			{
				for (int index = 0; index < numberOfSteps; index++)
				{
					var newStep = new DataQuestStep();

					newStep.RewardMoney = moneyRewards.ElementAtOrDefault(index);
					newStep.RewardXP = xpRewards.ElementAtOrDefault(index);
					newStep.RewardCLXP = clxpRewards.ElementAtOrDefault(index);
					newStep.RewardRP = rpRewards.ElementAtOrDefault(index);
					newStep.RewardBP = bpRewards.ElementAtOrDefault(index);
					newStep.StepType = (DataQuest.eStepType)stepTypes.ElementAtOrDefault(index);
					newStep.SourceText = sourceTexts.ElementAtOrDefault(index);
					newStep.TargetText = GetElementAtOrDefault(targetTexts, index, string.Empty);
					newStep.StepText = GetElementAtOrDefault(stepTexts, index, string.Empty);
					newStep.StepItemTemplate = GetElementAtOrDefault(stepItemTemplates, index, string.Empty);
					newStep.AdvanceText = GetElementAtOrDefault(advanceTexts, index, string.Empty);
					newStep.CollectItem = GetElementAtOrDefault(collectItems, index, string.Empty);
					newStep.TargetName = GetElementAtOrDefault(targetNames, index, string.Empty);
					newStep.TargetRegion = GetElementAtOrDefault(targetRegions, index, (ushort)0);

					DataQuestSteps.Add(newStep);
				}
			}

			this.DataQuestSteps = DataQuestSteps;
		}

		private bool CheckOccurance(string word, string triggerWord)
		{
			if (!string.IsNullOrEmpty(word) && word.ToUpper().Contains(triggerWord))
			{
				return true;
			}
			return false;
		}

		private byte ParseNumOptional(string input)
		{
			if (!string.IsNullOrEmpty(input))
			{
				return Convert.ToByte(input.Substring(0, 1));
			}
			return 0;
		}

		private List<ItemTemplate> ParseItemTemplates(string itemIDNBs)
		{
			var itemTemplates = new List<ItemTemplate>();
			foreach (var itemIDNB in ParseArrayString(itemIDNBs))
			{
				var item = GameServer.Database.FindObjectByKey<ItemTemplate>(itemIDNB);
				if (item != null)
				{
					itemTemplates.Add(item);
				}
			}
			return itemTemplates;
		}

		private void ParseItemsImpl()
		{
			var m_questDependencies = new List<string>();
			var m_classType = "";
			var AdditionalData = "";

			var m_dataQuest = dbDataQuest;

			string lastParse;
			string[] parse1;

			lastParse = m_dataQuest.QuestDependency;
			if (!string.IsNullOrEmpty(lastParse))
			{
				parse1 = lastParse.Split('|');
				foreach (string str in parse1)
				{
					if (str != "")
					{
						m_questDependencies.Add(str);
					}
				}
			}

			lastParse = m_dataQuest.ClassType;
			if (!string.IsNullOrEmpty(lastParse))
			{
				parse1 = lastParse.Split('|');
				m_classType = parse1[0];
				if (parse1.Length > 1)
					AdditionalData = parse1[1];
			}
		}

		private List<T> ConvertToListOf<T>(string arrayString)
		{
			var result = new List<T>();
			foreach (var element in ParseArrayString(arrayString))
			{
				if (element != string.Empty || typeof(T) == typeof(string))
				{
					result.Add((T)Convert.ChangeType(element, typeof(T)));
				}
				else
				{
					result.Add(default);
				}
			}
			return result;
		}

		private List<string> ParseArrayString(string arrayString)
		{
			List<string> result = new List<string>();
			if (arrayString != null)
			{
				string[] parse = arrayString.Split('|');
				for (int i = 0; i < parse.Length; i++)
				{
					result.Add(parse[i].Trim());
				}
			}
			return result;
		}

		private List<T> ParseArrayString<T>(string arrayString, int index)
		{
			var result = new List<T>();
			foreach (var element in ParseArrayString(arrayString))
			{
				var subelements = element.Split(';');
				if (index < subelements.Length)
				{
					result.Add((T)Convert.ChangeType(subelements[index], typeof(T)));
				}
				else
				{
					return result;
				}
			}
			return result;
		}

		private T GetElementAtOrDefault<T>(IList<T> enumerable, int index, T @default)
		{
			if (index >= enumerable.Count)
			{
				return @default;
			}
			return enumerable.ElementAt<T>(index);
		}
	}
}
