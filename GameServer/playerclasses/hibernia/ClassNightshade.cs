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
using DOL.GS.Realm;

namespace DOL.GS.PlayerClass
{
	[CharacterClass((int)eCharacterClass.Nightshade, "Nightshade", "Stalker")]
	public class ClassNightshade : ClassStalker
	{
		private static readonly List<string> DefaultAutoTrainableSkills = new() { Specs.Stealth };
		private static readonly List<PlayerRace> DefaultEligibleRaces = new()
		{
			 PlayerRace.Celt, PlayerRace.Elf, PlayerRace.Lurikeen,
		};


		public ClassNightshade()
			: base()
		{
			m_profession = "PlayerClass.Profession.PathofEssence";
			m_specializationMultiplier = 22;
			m_primaryStat = eStat.DEX;
			m_secondaryStat = eStat.QUI;
			m_tertiaryStat = eStat.STR;
			m_manaStat = eStat.DEX;
			m_autotrainableSkills = DefaultAutoTrainableSkills;
			m_eligibleRaces = DefaultEligibleRaces;
		}

		public override bool CanUseLefthandedWeapon
		{
			get { return true; }
		}

		public override eClassType ClassType
		{
			get { return eClassType.Hybrid; }
		}

		public override bool HasAdvancedFromBaseClass()
		{
			return true;
		}
	}
}
