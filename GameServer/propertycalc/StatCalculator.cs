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

			int baseBonus = boni.RawValueOf(Bonus.Base.ComponentOf(property));
			int abilityBonus = GetCombinedBonusValue(living, Bonus.Ability.ComponentOf(property));
			int debuff = boni.RawValueOf(Bonus.Debuff.ComponentOf(property));

            int itemBonus = CalcValueFromItems(living, property);
            int buffBonus = CalcValueFromBuffs(living, property);
			
			// Apply debuffs, 100% effectiveness for player buffs, 50% effectiveness
			// for item and base stats

			int unbuffedBonus = baseBonus + itemBonus;
			buffBonus -= Math.Abs(debuff);

			if (buffBonus < 0)
			{
				unbuffedBonus += buffBonus / 2;
				buffBonus = 0;
				unbuffedBonus = Math.Max(0, unbuffedBonus);
			}

			int stat = unbuffedBonus + buffBonus + abilityBonus;
			stat = (int)(stat * boni.RawValueOf(Bonus.Multiplier.ComponentOf(property)) / 1000.0);
			
			if (living is GamePlayer && property == eProperty.Constitution)
			{
				GamePlayer player = living as GamePlayer;
				stat -= player.TotalConstitutionLostAtDeath;
			}

			return Math.Max(1, stat);
        }

        public override int CalcValueFromBuffs(GameLiving living, eProperty property)
        {
			var boni = living.Boni;

			int baseBuffBonus = boni.RawValueOf(Bonus.BaseBuff.ComponentOf(property));
			int specBuffBonus = boni.RawValueOf(Bonus.SpecBuff.ComponentOf(property));

			if (living is GamePlayer)
            {
                GamePlayer player = living as GamePlayer;
				var manaStat = (eProperty)(player.CharacterClass.ManaStat);
				bool isListCaster = player.CharacterClass.ClassType == eClassType.ListCaster;

				if (property == manaStat && isListCaster)
				{
					specBuffBonus += player.Boni.RawValueOf(Bonus.BaseBuff.ComponentOf(eProperty.Acuity));
				}

				int baseBuffBonusCap = (int)(living.Level * 1.25);
				int specBuffBonusCap = (int)(living.Level * 1.5 * 1.25);

				baseBuffBonus = Math.Min(baseBuffBonus, baseBuffBonusCap);
				specBuffBonus = Math.Min(specBuffBonus, specBuffBonusCap);
			}

            return baseBuffBonus + specBuffBonus;
        }

        public override int CalcValueFromItems(GameLiving living, eProperty property)
        {
			var boni = living.Boni;

			int itemBonus = GetCombinedBonusValue(living, Bonus.Item.ComponentOf(property));
			int overcap = GetCombinedBonusValue(living, Bonus.ItemOvercap.ComponentOf(property));
			int mythicalCapIncrease = GetCombinedBonusValue(living, Bonus.Mythical.ComponentOf(property));

			int baseCap = (int)(living.Level * 1.5);
			int overcapCap = living.Level / 2 + 1;
			int capIncreaseHardCap = 52;

			int effectiveOvercap = Math.Min(overcap, overcapCap);
			int cap = baseCap + Math.Min(effectiveOvercap + mythicalCapIncrease, capIncreaseHardCap);
            return Math.Min(itemBonus, cap);
        }

		private int GetCombinedBonusValue(GameLiving living, BonusComponent component)
		{
			var boni = living.Boni;

			int rawPropertyValue = boni.RawValueOf(component);

			if (living is GamePlayer)
			{
				GamePlayer player = living as GamePlayer;
				var property = component.Type.ID;

				if (property == (eProperty)player.CharacterClass.ManaStat)
				{
					if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
					{
						rawPropertyValue += boni.RawValueOf(component.Category.ComponentOf(eProperty.Acuity));
					}
				}
			}

			return rawPropertyValue;
		}
	}
}
