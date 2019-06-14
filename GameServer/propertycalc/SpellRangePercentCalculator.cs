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
	[PropertyCalculator(eProperty.SpellRange)]
	public class SpellRangePercentCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property) 
		{
			var bonusProperties = new Boni(living);
			var value = bonusProperties.ValueOf(new BonusType(property));
			return 100 + value;
		}

        public override int CalcValueFromBuffs(GameLiving living, eProperty property)
        {
			var bonusProperties = new Boni(living);
			return bonusProperties.BuffValueOf(new BonusType(property));
		}

        public override int CalcValueFromItems(GameLiving living, eProperty property)
        {
			var bonusProperties = new Boni(living);
			return bonusProperties.ItemValueOf(new BonusType(property));
		}
	}
}
