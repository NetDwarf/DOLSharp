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
using DOL.AI.Brain;
using DOL.Language;
using System.Collections.Generic;
using System.Text;

namespace DOL.GS.Spells
{
	public class SpellDelve
	{
		private ISpellHandler spellHandler;
		private Spell Spell => spellHandler.Spell;

		public int Index => Spell.InternalID;
		public string Name => Spell.Name;
		public string Target => Spell.Target;
		public int CastTime => Spell.CastTime;
		public eDamageType MagicType => Spell.DamageType;
		public int Level => Spell.Level;
		public int Cost => Spell.Power;
		public string CostType => spellHandler.CostType;
		public int Range => Spell.Range;
		public int Duration => Spell.Duration;
		public int RecastDelay => Spell.RecastDelay;
		public int Radius => Spell.Radius;
		public int ConcentrationCost => Spell.Concentration;
		public int Frequency => Spell.Frequency;
		public int SubSpellID => Spell.SubSpellID;
		public string ShortDescription => spellHandler.ShortDescription;

		public SpellDelve(ISpellHandler spellHandler)
		{
			this.spellHandler = spellHandler;
		}

		public string GetClientMessage()
		{
			var clientDelve = new ClientDelve(this);
			return clientDelve.ClientMessage;
		}
	}

	public class ClientDelve
	{
		private SpellDelve spellDelve;

		private static Dictionary<eDamageType, int> damageTypeToIdLookup = new Dictionary<eDamageType, int>()
		{
			{eDamageType.Crush, 1 },
			{eDamageType.Slash, 2 },
			{eDamageType.Thrust, 3 },
			{eDamageType.Heat, 10 },
			{eDamageType.Spirit, 11 },
			{eDamageType.Cold, 12 },
			{eDamageType.Matter, 13 },
			{eDamageType.Body, 16 },
			{eDamageType.Energy, 20 },
		};

		public ClientDelve(SpellDelve spellDelve)
		{
			this.spellDelve = spellDelve;
		}

		public string ClientMessage
		{
			get
			{
				StringBuilder res = new StringBuilder("(Spell ");
				AddLine(res, "Function", "light");
				AddLine(res, "Index", unchecked((ushort)spellDelve.Index));
				AddLine(res, "Name", spellDelve.Name);
				if (spellDelve.CastTime >= 2000)
					AddLine(res, "cast_timer", spellDelve.CastTime - 2000);
				else if (spellDelve.CastTime > 0)
					AddLine(res, "cast_timer", 0);
				if (spellDelve.CastTime == 0)
					AddLine(res, "instant", "1");
				AddLine(res, "damage_type", GetMagicTypeID());
				AddLine(res, "level", spellDelve.Level);
				AddLine(res, "power_cost", spellDelve.Cost);
				AddLine(res, "cost_type", GetCostTypeID());
				AddLine(res, "range", spellDelve.Range);
				AddLine(res, "duration", spellDelve.Duration / 1000);
				AddLine(res, "dur_type", GetDurationType());
				AddLine(res, "timer_value", spellDelve.RecastDelay / 1000);
				AddLine(res, "target", GetSpellTargetType());
				AddLine(res, "radius", spellDelve.Radius);
				AddLine(res, "concentration_points", spellDelve.ConcentrationCost);
				AddLine(res, "frequency", spellDelve.Frequency);

				AddLine(res, "delve_string", spellDelve.ShortDescription);

				res.Append(")");
				return res.ToString();
			}
		}

		private void AddLine(StringBuilder stringBuilder, string key, object value)
		{
			ushort MAX_DELVE_STR_LENGTH = 2048;
			if ((stringBuilder.Length + key.Length + value.ToString().Length + 7) > MAX_DELVE_STR_LENGTH)
			{
				return;
			}
			if ((value.ToString() != "0" && value.ToString() != string.Empty) || key == "cast_timer")
			{
				stringBuilder.Append($"({key} \"{value}\")");
			}
		}

		private int GetSpellTargetType()
		{
			switch (spellDelve.Target)
			{
				case "Realm":
					return 7;
				case "Self":
					return 0;
				case "Enemy":
					return 1;
				case "Pet":
					return 6;
				case "Group":
					return 3;
				case "Area":
					return 9;
				case "Corpse":
					return 8;
				default:
					return 0;
			}
		}

		private int GetDurationType()
		{
			if (spellDelve.Duration > 0)
			{
				return 2;
			}
			if (spellDelve.ConcentrationCost > 0)
			{
				return 4;
			}

			return 0;
		}

		private int GetCostTypeID()
		{
			if (spellDelve.CostType.ToLower().Equals("health")) return 2;
			if (spellDelve.CostType.ToLower().Equals("endurance")) return 3;
			return 0;
		}

		private int GetMagicTypeID()
		{
			if (damageTypeToIdLookup.TryGetValue(spellDelve.MagicType, out int damageTypeID))
			{
				return damageTypeID;
			}
			return 0;
		}
	}
}
