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
using System.Collections.Generic;

namespace DOL.GS.PropertyCalc
{
	public class MultiplicativePropertiesBoniAdapter : IMultiplicativeProperties
	{
		private Boni boni;
		private Dictionary<object, double> multiplicatorValues = new Dictionary<object, double>();

		public MultiplicativePropertiesBoniAdapter(Boni boni)
		{
			this.boni = boni;
		}

		public double Get(int index)
		{
			var bonusType = new BonusType((eProperty)index);
			return boni.RawValueOf(bonusType.Multiplier) / 1000.0;
		}

		public void Remove(int index, object key)
		{
			var bonusType = new BonusType((eProperty)index);
			bool keyExists = multiplicatorValues.TryGetValue(key, out double value);
			if(!keyExists)
			{
				return;
			}
			int perMilleValue = (int)(value*1000);
			multiplicatorValues.Remove(key);
			boni.Remove(bonusType.Multiplier.Create(perMilleValue));
		}

		public void Set(int index, object key, double value)
		{
			var bonusType = new BonusType((eProperty)index);
			int perMilleValue = (int)(value * 1000);
			multiplicatorValues.Add(key, value);
			boni.Add(bonusType.Multiplier.Create(perMilleValue));
		}
	}
}
