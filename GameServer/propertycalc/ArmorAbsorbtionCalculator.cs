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
namespace DOL.GS.PropertyCalc
{
	[PropertyCalculator(eProperty.ArmorAbsorption)]
	public class ArmorAbsorptionCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property)
		{
			var bonusProperties = new Boni(living);
			int bonusValue = bonusProperties.ValueOf(new BonusType(eBonusType.ArmorAbsorption));
			int abs = 0;
			if (living is GameNPC)
			{
				if (living.Level >= 30) abs = 27;
				else if (living.Level >= 20) abs = 19;
				else if (living.Level >= 10) abs = 10;

				abs += (living.GetModified(eProperty.Constitution)
					+ living.GetModified(eProperty.Dexterity) - 120) / 12;
				bonusValue += abs;
			}
			return bonusValue;
		}
	}
}
