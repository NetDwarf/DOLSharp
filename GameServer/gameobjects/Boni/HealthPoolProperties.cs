using DOL.GS.Keeps;
using System;

namespace DOL.GS
{
	public class PlayerHealthPool
	{
		private GamePlayer owner;

		public PlayerHealthPool(GamePlayer owner)
		{
			this.owner = owner;
		}

		public int Value
		{
			get
			{
				int constitution = owner.Boni.ValueOf(Bonus.Constitution) - 50;
				if (constitution < 0) constitution *= 2;

				double hpFromLevel = (owner.CharacterClass.BaseHP * owner.Level) / 50;
				int hpFromConstitutition = (int)hpFromLevel * constitution / 200;
				int hpFromChampionLevel = 20 + owner.ChampionLevel * ServerProperties.Properties.HPS_PER_CHAMPIONLEVEL;
				double artifactMultiplier = 1 + owner.GetModified(eProperty.ExtraHP) * 0.01;
				int hpBase = (int)Math.Round(artifactMultiplier * (hpFromLevel + hpFromConstitutition + hpFromChampionLevel));
				hpBase = Math.Max(1, hpBase);

				if (owner.HasAbility(Abilities.ScarsOfBattle) && owner.Level >= 40)
				{
					int levelbonus = Math.Min(owner.Level - 40, 10);
					hpBase = (int)(hpBase * (100 + levelbonus) * 0.01);
				}
				
				var healthPoolBonus = new BonusType(eBonusType.HealthPool);
				var minHealth = 1;
				return Math.Max(hpBase + owner.Boni.ValueOf(healthPoolBonus), minHealth);
			}
		}
	}

	public class KeepComponentHealthPool
	{
		GameKeepComponent owner;

		public KeepComponentHealthPool(GameKeepComponent keepComponent)
		{
			this.owner = keepComponent;
		}

		public int Value
		{
			get
			{
				GameKeepComponent keepComp = owner;

				if (keepComp.Keep != null)
					return (keepComp.Keep.EffectiveLevel(keepComp.Keep.Level) + 1) * keepComp.AbstractKeep.BaseLevel * 200;

				return 0;
			}
		}
	}

	public class KeepDoorHealthPool
	{
		GameKeepDoor owner;

		public KeepDoorHealthPool(GameKeepDoor keepDoor)
		{
			this.owner = keepDoor;
		}

		public int Value
		{
			get
			{
				GameKeepDoor keepdoor = owner;

				if (keepdoor.Component != null && keepdoor.Component.Keep != null)
				{
					return (keepdoor.Component.Keep.EffectiveLevel(keepdoor.Component.Keep.Level) + 1) * keepdoor.Component.AbstractKeep.BaseLevel * 200;
				}

				return 0;

				//todo : use material too to calculate maxhealth
			}
		}
	}

	public class NPCHealthPool
	{
		GameNPC owner;

		public NPCHealthPool(GameNPC owner)
		{
			this.owner = owner;
		}

		public int Value
		{
			get
			{
				var healthPoolBonus = new BonusType(eBonusType.HealthPool);
				var hpFromBaseBuffs = owner.Boni.RawValueOf(healthPoolBonus.BaseBuff);
				int hp = 0;

				if (owner.Level < 10)
				{
					hp = owner.Level * 20 + 20 + hpFromBaseBuffs;  // default
				}
				else
				{
					hp = (int)(50 + 11 * owner.Level + 0.548331 * owner.Level * owner.Level) + hpFromBaseBuffs;
					if (owner.Level < 25)
						hp += 20;
				}

				int basecon = (owner as GameNPC).Constitution;
				int conmod = 20; // at level 50 +75 con ~= +300 hit points

				// first adjust hitpoints based on base CON

				if (basecon != ServerProperties.Properties.GAMENPC_BASE_CON)
				{
					hp = Math.Max(1, hp + ((basecon - ServerProperties.Properties.GAMENPC_BASE_CON) * ServerProperties.Properties.GAMENPC_HP_GAIN_PER_CON));
				}

				// Now adjust for buffs

				// adjust hit points based on constitution difference from base con
				// modified from http://www.btinternet.com/~challand/hp_calculator.htm
				int conhp = hp + (conmod * owner.Level * (owner.GetModified(eProperty.Constitution) - basecon) / 250);

				conhp = Math.Min((int)(hp * 1.5), conhp);
				conhp = Math.Max(hp / 2, conhp);

				return conhp;
			}
		}
	}

	public class GenericLivingHealthPool
	{
		GameLiving owner;

		public GenericLivingHealthPool(GameLiving owner)
		{
			this.owner = owner;
		}

		public int Value
		{
			get
			{
				var healthPoolBonus = new BonusType(eBonusType.HealthPool);
				var hpFromBaseBuffs = owner.Boni.RawValueOf(healthPoolBonus.BaseBuff);
				if (owner.Level < 10)
				{
					return owner.Level * 20 + 20 + hpFromBaseBuffs;
				}
				else
				{
					int hp = (int)(50 + 11 * owner.Level + 0.548331 * owner.Level * owner.Level) + hpFromBaseBuffs;
					if (owner.Level < 25)
					{
						hp += 20;
					}
					return hp;
				}
			}
		}
	}
}
