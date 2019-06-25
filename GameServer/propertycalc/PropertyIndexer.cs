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
	public sealed class IndexerBoniAdapter : IPropertyIndexer
	{
		private Boni boni;
		private BonusPart category;
		private BonusFactory bonusFactory = new BonusFactory();

		public IndexerBoniAdapter(Boni boni, BonusPart category)
		{
			this.boni = boni;
			this.category = category;
		}

		public int this[int index]
		{
			get
			{
				var bonusComponent = bonusFactory.CreateComponent((eProperty)index, category.ID);
				return boni.RawValueOf(bonusComponent);
			}
			set
			{
				var bonusComponent = bonusFactory.CreateComponent((eProperty)index, category.ID);
				boni.SetTo(bonusComponent.Create(value));
			}
		}

		public int this[eProperty property]
		{
			get
			{
				return this[(int)property];
			}
			set
			{
				this[(int)property] = value;
			}
		}
	}
}
