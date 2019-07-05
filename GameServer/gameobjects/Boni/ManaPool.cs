namespace DOL.GS
{
	public class PlayerManaPool : IBonusProperty
	{
		private GamePlayer owner;

		public PlayerManaPool(GamePlayer owner)
		{
			this.owner = owner;
		}

		public int Value
		{
			get
			{
				eStat manaStatID = owner.CharacterClass.ManaStat;

				var bonusFactory = new BonusFactory();
				var manaBonusType = bonusFactory.CreateType((eProperty)manaStatID);
				if (manaStatID == eStat.UNDEFINED)
				{
					//Special handling for Vampiirs:
					/* There is no stat that affects the Vampiir's power pool or the damage done by its power based spells.
					 * The Vampiir is not a focus based class like, say, an Enchanter.
					 * The Vampiir is a lot more cut and dried than the typical casting class. 
					 * EDIT, 12/13/04 - I was told today that this answer is not entirely accurate.
					 * While there is no stat that affects the damage dealt (in the way that intelligence or piety affects how much damage a more traditional caster can do),
					 * the Vampiir's power pool capacity is intended to be increased as the Vampiir's strength increases.
					 * 
					 * This means that strength ONLY affects a Vampiir's mana pool
					 */
					if (owner.CharacterClass.ID == (int)eCharacterClass.Vampiir)
					{
						manaBonusType = Bonus.Strength;
					}
					else if (owner.Champion && owner.ChampionLevel > 0)
					{
						return owner.CalculateMaxMana(owner.Level, 0);
					}
					else
					{
						return 0;
					}
				}
				int manaStatBonus = owner.Boni.ValueOf(manaBonusType);
				int manaBase = owner.CalculateMaxMana(owner.Level, manaStatBonus);

				int itemBonus = owner.Boni.ValueOf(new BonusType(eBonusType.ManaPool));
				int poolBonus = owner.Boni.ValueOf(new BonusType(eBonusType.ManaPoolPercent));

				//Q: What exactly does the power pool % increase do?Does it increase the amount of power my cleric
				//can generate (like having higher piety)? Or, like the dex cap increase, do I have to put spellcraft points into power to make it worth anything?
				//A: I’m better off quoting Balance Boy directly here: ” Power pool is affected by
				//your acuity stat, +power bonus, the Ethereal Bond Realm ability, and your level.
				//The resulting power pool is adjusted by your power pool % increase bonus.
				return (int)((manaBase + itemBonus) * (1 + poolBonus * 0.01));
			}
		}
	}

	public class DefaultManaPool : IBonusProperty
	{
		public int Value => 1000000;
	}

	public class NPCManaPool : IBonusProperty
	{
		public int Value => 1000;
	}

	public class ManaPoolFactory
	{
		public static IBonusProperty Create(GameLiving owner)
		{
			if(owner is GamePlayer)
			{
				return new PlayerManaPool(owner as GamePlayer);
			}
			else if(owner is GameNPC)
			{
				return new NPCManaPool();
			}
			else
			{
				return new DefaultManaPool();
			}
		}
	}
}
