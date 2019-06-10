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
	[PropertyCalculator(eProperty.PowerRegenerationRate)]
	public class PowerRegenerationRateCalculator : PropertyCalculator
	{
		public PowerRegenerationRateCalculator() {}

		public override int CalcValue(GameLiving living, eProperty property) 
		{
			/* PATCH 1.87 COMBAT AND REGENERATION
			  - While in combat, health and power regeneration ticks will happen twice as often.
    		  - Each tick of health and power is now twice as effective.
              - All health and power regeneration aids are now twice as effective.
             */

			double regen = 5 + (living.Level / 2.75);

			if (living is GameNPC && living.InCombat)
				regen /= 2.0;

			// tolakram - there is no difference per tic between combat and non combat
			
			regen *= ServerProperties.Properties.MANA_REGEN_RATE;

			double decimals = regen - (int)regen;
			if (RandomRoudingUpEnabled && Util.ChanceDouble(decimals)) 
			{
				regen += 1;	// compensate int rounding error
			}

			var bonusProperties = new BonusProperties(living);
			regen += bonusProperties.ValueOf(new BonusType(property));

			regen = Math.Max(1, regen);

			return (int)regen;
		}

		public bool RandomRoudingUpEnabled { get; set; } = true;
	}
}
