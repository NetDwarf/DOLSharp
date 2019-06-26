﻿using DOL.AI;
using DOL.Database;
using DOL.GS;
using DOL.GS.Keeps;
using NSubstitute;
using System.Collections.Generic;

namespace DOL.UnitTests.GameServer
{
    public class FakePlayer : GamePlayer
    {
        public int modifiedSpecLevel;
        public ICharacterClass characterClass;
        public int modifiedToHitBonus;
        public int modifiedSpellLevel;
        public int modifiedEffectiveLevel;
        private int totalConLostOnDeath;
		public bool isInCombat = false;
		public bool isSprinting = false;
		public List<string> abilities = new List<string>();

        public override ICharacterClass CharacterClass { get { return characterClass; } }

        public FakePlayer() : base(null, null) { }

		public override bool InCombat => isInCombat;
		public override bool IsSprinting => isSprinting;

		public override void LoadFromDatabase(DataObject obj)
        {
        }

        public override int GetModifiedSpecLevel(string keyName)
        {
            return modifiedSpecLevel;
        }

		public override int GetModified(eProperty property)
		{
			switch (property)
			{
				case eProperty.SpellLevel:
					return modifiedSpellLevel;
				case eProperty.ToHitBonus:
					return modifiedToHitBonus;
				case eProperty.LivingEffectiveLevel:
					return modifiedEffectiveLevel;
				default:
					return base.GetModified(property);
			}
		}

		public override string GetName(int article, bool firstLetterUppercase)
		{
			return "";
		}

		public override int TotalConstitutionLostAtDeath
        {
            get { return totalConLostOnDeath; }
            set { totalConLostOnDeath = value; }
        }

		public override bool HasAbility(string abilityName)
		{
			return abilities.Contains(abilityName);
		}

		public override int ChampionLevel { get; set; } = 0;
	}

	public class FakeNPC : GameNPC
	{
		public bool isInCombat = false;

		public FakeNPC() : base(Substitute.For<ABrain>()){}

		public override string GetName(int article, bool firstLetterUppercase)
		{
			return "FakeName";
		}

		public override bool InCombat => isInCombat;
	}

	public class FakeKeepDoor : GameKeepDoor
	{
		public int maxHealth = 0;
		public override int MaxHealth{ get { return maxHealth; } }
	}

	public class FakeRace : LivingRace
	{
		public override int GetResist(eResist resistID)
		{
			return 0;
		}
	}
}
