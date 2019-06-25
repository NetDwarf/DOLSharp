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
using DOL.AI.Brain;

namespace DOL.GS.PropertyCalc
{
	[PropertyCalculator(eProperty.Resist_First, eProperty.Resist_Last)]
	public class ResistCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property)
		{
			var bonusType = new BonusType((eBonusType)property);
			var bonusProperties = new Boni(living);
			return bonusProperties.ValueOf(bonusType);
		}

		[Obsolete("Use Boni instead!")]
		public override int CalcValueBase(GameLiving living, eProperty property)
		{
			int propertyIndex = (int)property;
			int debuff = living.DebuffCategory[propertyIndex];
			int racialBonus = (living is GamePlayer) ? living.LivingRace.GetResist((eResist)property) : 0;
			int itemBonus = CalcValueFromItems(living, property);
			int buffBonus = CalcValueFromBuffs(living, property);
			buffBonus -= Math.Abs(debuff);
			if (buffBonus < 0)
			{
				itemBonus += buffBonus / 2;
				buffBonus = 0;
				if (itemBonus < 0) itemBonus = 0;
			}

			int hardCap = 70;
			return Math.Min(itemBonus + buffBonus + racialBonus, hardCap);
		}

		public override int CalcValueFromBuffs(GameLiving living, eProperty property)
		{
			var bonusType = new BonusType((eBonusType)property);
			var bonusProperties = new Boni(living);
			return bonusProperties.BuffValueOf(bonusType);
		}

		public override int CalcValueFromItems(GameLiving living, eProperty property)
		{
			var bonusType = new BonusType((eBonusType)property);
			var bonusProperties = new Boni(living);
			return bonusProperties.ItemValueOf(bonusType);
		}
	}

	[PropertyCalculator(eProperty.Resist_Natural)]
	public class ResistNaturalCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property)
        {
				var bonusType = new BonusType((eBonusType)property);
				var bonusProperties = new Boni(living);
				return bonusProperties.ValueOf(bonusType);
        }

        public override int CalcValueFromBuffs(GameLiving living, eProperty property)
        {
			var bonusType = new BonusType((eBonusType)property);
			var bonusProperties = new Boni(living);
			return bonusProperties.BuffValueOf(bonusType);
		}

        public override int CalcValueFromItems(GameLiving living, eProperty property)
        {
			var bonusType = new BonusType((eBonusType)property);
			var bonusProperties = new Boni(living);
			return bonusProperties.ItemValueOf(bonusType);
		}
	}
}
