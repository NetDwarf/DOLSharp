/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System;

namespace DOL.GS.PropertyCalc
{
	[PropertyCalculator(eProperty.Stat_First, eProperty.Stat_Last)]
	public class StatCalculator : PropertyCalculator
    {
        public StatCalculator() { }

        public override int CalcValue(GameLiving living, eProperty property)
        {
            var boni = living.Boni;
			var bonusProperty = boni.GetProperty(property);
			
			int baseBonus = bonusProperty.Base;
			int abilityBonus = bonusProperty.Ability;
			int debuff = bonusProperty.Debuff;
			int deathConDebuff = 0;

            int itemBonus = CalcValueFromItems(living, property);
            int buffBonus = CalcValueFromBuffs(living, property);

			// Special cases:
			// 1) ManaStat (base stat + acuity, players only).
			// 2) As of patch 1.64: - Acuity - This bonus will increase your casting stat, 
			//    whatever your casting stat happens to be. If you're a druid, you should get an increase to empathy, 
			//    while a bard should get an increase to charisma.  http://support.darkageofcamelot.com/kb/article.php?id=540
			// 3) Constitution lost at death, only affects players.

			if (living is GamePlayer)
			{
				GamePlayer player = living as GamePlayer;
				var playerBoni = player.Boni;
				if (property == (eProperty)(player.CharacterClass.ManaStat))
				{
					if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
					{
						abilityBonus += playerBoni.GetProperty(eProperty.Acuity).Ability; //player.AbilityBonus[(int)eProperty.Acuity];
					}
				}

				deathConDebuff = player.TotalConstitutionLostAtDeath;
			}

			// Apply debuffs, 100% effectiveness for player buffs, 50% effectiveness
			// for item and base stats

			int unbuffedBonus = baseBonus + itemBonus;
			buffBonus -= Math.Abs(debuff);

			if (buffBonus < 0)
			{
				unbuffedBonus += buffBonus / 2;
				buffBonus = 0;
				if (unbuffedBonus < 0)
					unbuffedBonus = 0;
			}

			int stat = unbuffedBonus + buffBonus + abilityBonus;
			stat = (int)(stat * bonusProperty.Multiplier);

			stat -= (property == eProperty.Constitution)? deathConDebuff : 0;

			return Math.Max(1, stat);
        }

        public override int CalcValueFromBuffs(GameLiving living, eProperty property)
        {
			var boni = living.Boni;
			var bonusProperty = boni.GetProperty(property);

			int baseBuffBonus = bonusProperty.BaseBuff;
			int specBuffBonus = bonusProperty.SpecBuff;

            if (living is GamePlayer)
            {
                GamePlayer player = living as GamePlayer;
                if (property == (eProperty)(player.CharacterClass.ManaStat))
                    if (player.CharacterClass.ClassType == eClassType.ListCaster)
                        specBuffBonus += player.Boni.GetProperty(eProperty.Acuity).BaseBuff;
            }

            int baseBuffBonusCap = (living is GamePlayer) ? (int)(living.Level * 1.25) : Int16.MaxValue;
            int specBuffBonusCap = (living is GamePlayer) ? (int)(living.Level * 1.5 * 1.25) : Int16.MaxValue;

            baseBuffBonus = Math.Min(baseBuffBonus, baseBuffBonusCap);
            specBuffBonus = Math.Min(specBuffBonus, specBuffBonusCap);

            return baseBuffBonus + specBuffBonus;
        }

        public override int CalcValueFromItems(GameLiving living, eProperty property)
        {
			var boni = living.Boni;
			var bonusProperty = boni.GetProperty(property);

			int itemBonus = bonusProperty.Item;
            int itemBonusCap = (int)(living.Level * 1.5);
			int itemBonusCapIncrease = boni.GetProperty(eProperty.StatCapBonus_First - eProperty.Stat_First + property).Item;
			int itemBonusCapIncreaseCap = living.Level / 2 + 1;
			int MythicalitemBonusCapIncrease = boni.GetProperty(eProperty.MythicalStatCapBonus_First - eProperty.Stat_First + property).Item;
			int MythicalitemBonusCapIncreaseCap = 52;

			if (living is GamePlayer)
            {
				GamePlayer player = living as GamePlayer;

				if (property == (eProperty)player.CharacterClass.ManaStat)
				{
					if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
					{
						itemBonus += boni.GetProperty(eProperty.Acuity).Item;
						itemBonusCapIncrease += boni.GetProperty(eProperty.AcuCapBonus).Item;
						MythicalitemBonusCapIncrease += boni.GetProperty(eProperty.MythicalAcuCapBonus).Item;
					}
				}
			}

			itemBonusCapIncrease = Math.Min(itemBonusCapIncrease, itemBonusCapIncreaseCap);
			if (MythicalitemBonusCapIncrease + itemBonusCapIncrease > 52)
			{
				MythicalitemBonusCapIncrease = 52 - itemBonusCapIncrease;
			}

			int mythicalitemBonusCapIncrease = Math.Min(MythicalitemBonusCapIncrease, MythicalitemBonusCapIncreaseCap);
            return Math.Min(itemBonus, itemBonusCap + itemBonusCapIncrease + mythicalitemBonusCapIncrease);
        }
	}
}
