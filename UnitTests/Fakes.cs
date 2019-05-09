using System;
using DOL.AI;
using DOL.Database;
using DOL.GS;

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

        public override ICharacterClass CharacterClass { get { return characterClass; } }

        public FakePlayer() : base(null, null) { }

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

		public override int TotalConstitutionLostAtDeath
        {
            get { return totalConLostOnDeath; }
            set { totalConLostOnDeath = value; }
        }
    }

	public class FakeRace : LivingRace
	{
		public override int GetResist(eResist resistID)
		{
			return 0;
		}
	}
}
