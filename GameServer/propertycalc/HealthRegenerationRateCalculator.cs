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

using DOL.GS.Keeps;

namespace DOL.GS.PropertyCalc
{
	[PropertyCalculator(eProperty.HealthRegenerationRate)]
	public class HealthRegenerationRateCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property)
		{
			if (living.IsDiseased)
				return 0; // no HP regen if diseased
			if (living is GameKeepDoor)
				return (int)(living.MaxHealth * 0.05); //5% each time for keep door

			double regen = 1;

			/* PATCH 1.87 COMBAT AND REGENERATION
			  - While in combat, health and power regeneration ticks will happen twice as often.
    		  - Each tick of health and power is now twice as effective.
              - All health and power regeneration aids are now twice as effective.
             */

			if (living.Level < 26)
			{
				regen = 10 + (living.Level * 0.2);
			}
			else
			{
				regen = living.Level * 0.6;
			}

			// assumes NPC regen is now half as effective as GamePlayer (as noted above) - tolakram
			// http://www.dolserver.net/viewtopic.php?f=16&t=13197

			if (living is GameNPC && living.InCombat)
			{
				regen /= 2.0;
			}
            
			regen *= ServerProperties.Properties.HEALTH_REGEN_RATE;

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
