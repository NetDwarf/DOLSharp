﻿using System;

namespace DOL.GS
{
	public class Bonus
	{
		public Bonus(int value, BonusPart part, BonusType type)
		{
			this.typeID = type.ID;
			this.partID = part.ID;
			this.Value = value;
			ConvertOldBonus();
		}

		public int Value { get; }
		private eProperty typeID;
		private eBonusPart partID;
		public BonusType Type { get { return new BonusType(typeID); } private set { typeID = value.ID; } }
		public BonusPart Part { get { return new BonusPart(partID); } private set { partID = value.ID; } }

		public static BonusPart Base { get { return new BonusPart(eBonusPart.Base); } }
		public static BonusPart Ability { get { return new BonusPart(eBonusPart.Ability); } }
		public static BonusPart Item { get { return new BonusPart(eBonusPart.Item); } }
		public static BonusPart ItemOvercap { get { return new BonusPart(eBonusPart.ItemOvercap); } }
		public static BonusPart Mythical { get { return new BonusPart(eBonusPart.Mythical); } }
		public static BonusPart BaseBuff { get { return new BonusPart(eBonusPart.BaseBuff); } }
		public static BonusPart SpecBuff { get { return new BonusPart(eBonusPart.SpecBuff); } }
		public static BonusPart ExtraBuff { get { return new BonusPart(eBonusPart.ExtraBuff); } }
		public static BonusPart Debuff { get { return new BonusPart(eBonusPart.Debuff); } }
		public static BonusPart SpecDebuff { get { return new BonusPart(eBonusPart.SpecDebuff); } }
		public static BonusPart Multiplier { get { return new BonusPart(eBonusPart.Multiplier); } }

		public static BonusType Strength { get { return new BonusType(eBonusType.Strength); } }
		public static BonusType Constitution { get { return new BonusType(eBonusType.Constitution); } }
		public static BonusType Dexterity { get { return new BonusType(eBonusType.Dexterity); } }
		public static BonusType Quickness { get { return new BonusType(eBonusType.Quickness); } }
		public static BonusType Intelligence { get { return new BonusType(eBonusType.Intelligence); } }
		public static BonusType Piety { get { return new BonusType(eBonusType.Piety); } }
		public static BonusType Empathy { get { return new BonusType(eBonusType.Empathy); } }
		public static BonusType Charisma { get { return new BonusType(eBonusType.Charisma); } }
		public static BonusType Acuity { get { return new BonusType(eBonusType.Acuity); } }

		private void ConvertOldBonus()
		{
			bool isItem = Part.ID == eBonusPart.Item;
			bool isItemStatOvercap = isItem && Type.ID >= eProperty.StrCapBonus && Type.ID <= eProperty.AcuCapBonus;
			bool isMythical = isItem && Type.ID >= eProperty.MythicalStatCapBonus_First && Type.ID <= eProperty.MythicalStatCapBonus_Last;
			bool isResistOvercap = isItem && Type.ID >= eProperty.ResCapBonus_First && Type.ID <= eProperty.ResCapBonus_Last;
			bool isRegenDebuff = Part.Equals(Bonus.SpecBuff) && Type.IsRegen;

			if (Type.ID == eProperty.AcuCapBonus)
			{
				Type = Acuity;
				Part = ItemOvercap;
			}
			else if (Type.ID == eProperty.MythicalAcuCapBonus)
			{
				Type = Acuity;
				Part = Mythical;
			}
			else if (isItemStatOvercap)
			{
				Type = new BonusType(Type.ID - eProperty.StrCapBonus + eProperty.Stat_First);
				Part = ItemOvercap;
			}
			else if (isMythical)
			{
				Type = new BonusType(Type.ID - eProperty.MythicalStatCapBonus_First + eProperty.Stat_First);
				Part = Mythical;
			}
			else if (isResistOvercap)
			{
				Type = new BonusType(Type.ID - eProperty.ResCapBonus_First + eProperty.Resist_First);
				Part = Mythical;
			}
			else if (isRegenDebuff)
			{
				Part = Debuff;
			}
		}
	}

	public class BonusComponent
	{
		public BonusType Type { get; }
		public BonusPart Part { get; }

		public BonusComponent(BonusPart part, BonusType type)
		{
			this.Part = part;
			this.Type = type;
		}

		public Bonus Create(int value)
		{
			return new Bonus(value, Part, Type);
		}

		public override bool Equals(object obj)
		{
			var comp2 = obj as BonusComponent;
			if (comp2 is null) { return false; }
			return this.Part.Equals(comp2.Part) && this.Type.Equals(comp2.Type);
		}
	}

	public class BonusPart
	{
		public eBonusPart ID { get; }
		
		public BonusPart(eBonusPart part)
		{
			this.ID = part;
		}

		public override bool Equals(object obj)
		{
			var part2 = obj as BonusPart;
			if (part2 is null) { return false; }
			return this.ID == part2.ID;
		}
	}

	public class BonusType
	{
		public eProperty ID { get; }

		public BonusType(eProperty id)
		{
			this.ID = id;
		}

		public BonusType(eBonusType id)
		{
			this.ID = ConvertToPropertyID(id);
		}

		private eProperty ConvertToPropertyID(eBonusType bonusTypeID)
		{
			return (eProperty)bonusTypeID;
		}

		public BonusComponent Base { get { return From(Bonus.Base); } }
		public BonusComponent Ability { get { return From(Bonus.Ability); } }
		public BonusComponent Item { get { return From(Bonus.Item); } }
		public BonusComponent ItemOvercap { get { return From(Bonus.ItemOvercap); } }
		public BonusComponent Mythical { get { return From(Bonus.Mythical); } }
		public BonusComponent BaseBuff { get { return From(Bonus.BaseBuff); } }
		public BonusComponent SpecBuff { get { return From(Bonus.SpecBuff); } }
		public BonusComponent ExtraBuff { get { return From(Bonus.ExtraBuff); } }
		public BonusComponent Debuff { get { return From(Bonus.Debuff); } }
		public BonusComponent SpecDebuff { get { return From(Bonus.SpecDebuff); } }
		public BonusComponent Multiplier { get { return From(Bonus.Multiplier); } }

		public BonusComponent From(BonusPart part) { return new BonusComponent(part, this); }

		public override bool Equals(object obj)
		{
			var type2 = obj as BonusType;
			if (type2 is null) { return false; }
			return this.ID == type2.ID;
		}

		public bool IsStat
		{
			get
			{
				return ID >= eProperty.Stat_First && ID <= eProperty.Stat_Last;
			}
		}

		public bool IsBaseStat { get { return ID >= eProperty.Stat_First && ID <= eProperty.Quickness; } }

		public bool IsAcuityStat => (ID >= eProperty.Intelligence && ID <= eProperty.Stat_Last) || ID == eProperty.Acuity;

		public bool IsResist => ID >= eProperty.Resist_First && ID <= eProperty.Resist_Last || ID == eProperty.Resist_Natural;

		public bool IsRegen => ID == eProperty.HealthRegenerationRate || ID == eProperty.PowerRegenerationRate || ID == eProperty.EnduranceRegenerationRate;
	}

	public enum eBonusPart : byte
	{
		Base,
		Ability,
		Item,
		ItemOvercap,
		Mythical,
		BaseBuff,
		SpecBuff,
		ExtraBuff,
		Debuff,
		SpecDebuff,
		Multiplier,
		__Last = Multiplier,
	}

	public enum eBonusType : byte
	{
		Undefined = 0,
		// Note, these are set in the ItemDB now.  Changing
		//any order will screw things up.
		// char stats
		#region Stats
		Stat_First = 1,
		Strength = 1,
		Dexterity = 2,
		Constitution = 3,
		Quickness = 4,
		Intelligence = 5,
		Piety = 6,
		Empathy = 7,
		Charisma = 8,
		Stat_Last = 8,

		MaxMana = 9,
		MaxHealth = 10,
		#endregion

		#region Resists
		// resists
		Resist_First = 11,
		Resist_Body = 11,
		Resist_Cold = 12,
		Resist_Crush = 13,
		Resist_Energy = 14,
		Resist_Heat = 15,
		Resist_Matter = 16,
		Resist_Slash = 17,
		Resist_Spirit = 18,
		Resist_Thrust = 19,
		Resist_Last = 19,
		#endregion

		#region Skills
		// skills
		Skill_First = 20,
		Skill_Two_Handed = 20,
		Skill_Body = 21,
		Skill_Chants = 22,
		Skill_Critical_Strike = 23,
		Skill_Cross_Bows = 24,
		Skill_Crushing = 25,
		Skill_Death_Servant = 26,
		Skill_DeathSight = 27,
		Skill_Dual_Wield = 28,
		Skill_Earth = 29,
		Skill_Enhancement = 30,
		Skill_Envenom = 31,
		Skill_Fire = 32,
		Skill_Flexible_Weapon = 33,
		Skill_Cold = 34,
		Skill_Instruments = 35,
		Skill_Long_bows = 36,
		Skill_Matter = 37,
		Skill_Mind = 38,
		Skill_Pain_working = 39,
		Skill_Parry = 40,
		Skill_Polearms = 41,
		Skill_Rejuvenation = 42,
		Skill_Shields = 43,
		Skill_Slashing = 44,
		Skill_Smiting = 45,
		Skill_SoulRending = 46,
		Skill_Spirit = 47,
		Skill_Staff = 48,
		Skill_Stealth = 49,
		Skill_Thrusting = 50,
		Skill_Wind = 51,
		Skill_Sword = 52,
		Skill_Hammer = 53,
		Skill_Axe = 54,
		Skill_Left_Axe = 55,
		Skill_Spear = 56,
		Skill_Mending = 57,
		Skill_Augmentation = 58,
		//Skill_Cave_Magic = 59,
		Skill_Darkness = 60,
		Skill_Suppression = 61,
		Skill_Runecarving = 62,
		Skill_Stormcalling = 63,
		Skill_BeastCraft = 64,
		Skill_Light = 65,
		Skill_Void = 66,
		Skill_Mana = 67,
		Skill_Composite = 68,
		Skill_Battlesongs = 69,
		Skill_Enchantments = 70,
		// 71 Available for a Skill
		Skill_Blades = 72,
		Skill_Blunt = 73,
		Skill_Piercing = 74,
		Skill_Large_Weapon = 75,
		Skill_Mentalism = 76,
		Skill_Regrowth = 77,
		Skill_Nurture = 78,
		Skill_Nature = 79,
		Skill_Music = 80,
		Skill_Celtic_Dual = 81,
		Skill_Celtic_Spear = 82,
		Skill_RecurvedBow = 83,
		Skill_Valor = 84,
		Skill_Subterranean = 85,
		Skill_BoneArmy = 86,
		Skill_Verdant = 87,
		Skill_Creeping = 88,
		Skill_Arboreal = 89,
		Skill_Scythe = 90,
		Skill_Thrown_Weapons = 91,
		Skill_HandToHand = 92,
		Skill_ShortBow = 93,
		Skill_Pacification = 94,
		Skill_Savagery = 95,
		Skill_Nightshade = 96,
		Skill_Pathfinding = 97,
		Skill_Summoning = 98,
		Skill_Dementia = 99,
		Skill_ShadowMastery = 100,
		Skill_VampiiricEmbrace = 101,
		Skill_EtherealShriek = 102,
		Skill_PhantasmalWail = 103,
		Skill_SpectralForce = 104,
		Skill_OdinsWill = 105,
		Skill_Cursing = 106,
		Skill_Hexing = 107,
		Skill_Witchcraft = 108,
		Skill_MaulerStaff = 109,
		Skill_FistWraps = 110,
		Skill_Power_Strikes = 111,
		Skill_Magnetism = 112,
		Skill_Aura_Manipulation = 113,
		Skill_SpectralGuard = 114,
		Skill_Archery = 115,
		Skill_Last = 115,
		#endregion

		// 116 - 119 Available

		Focus_Darkness = 120,
		Focus_Suppression = 121,
		Focus_Runecarving = 122,
		Focus_Spirit = 123,
		Focus_Fire = 124,
		Focus_Air = 125,
		Focus_Cold = 126,
		Focus_Earth = 127,
		Focus_Light = 128,
		Focus_Body = 129,
		Focus_Matter = 130,
		// 131 Available
		Focus_Mind = 132,
		Focus_Void = 133,
		Focus_Mana = 134,
		Focus_Enchantments = 135,
		Focus_Mentalism = 136,
		Focus_Summoning = 137,
		Focus_BoneArmy = 138,
		Focus_PainWorking = 139,
		Focus_DeathSight = 140,
		Focus_DeathServant = 141,
		Focus_Verdant = 142,
		Focus_CreepingPath = 143,
		Focus_Arboreal = 144,
		MaxSpeed = 145,
		// 146 Available
		MaxConcentration = 147,
		ArmorFactor = 148,
		ArmorAbsorption = 149,
		HealthRegenerationRate = 150,
		PowerRegenerationRate = 151,
		EnduranceRegenerationRate = 152,
		SpellRange = 153,
		ArcheryRange = 154,
		MeleeSpeed = 155,
		Acuity = 156,
		Focus_EtherealShriek = 157,
		Focus_PhantasmalWail = 158,
		Focus_SpectralForce = 159,
		Focus_Cursing = 160,
		Focus_Hexing = 161,
		Focus_Witchcraft = 162,
		AllMagicSkills = 163,
		AllMeleeWeaponSkills = 164,
		AllFocusLevels = 165,
		LivingEffectiveLevel = 166,
		AllDualWieldingSkills = 167,
		AllArcherySkills = 168,
		EvadeChance = 169,
		BlockChance = 170,
		ParryChance = 171,
		FatigueConsumption = 172,
		MeleeDamage = 173,
		RangedDamage = 174,
		FumbleChance = 175,
		MesmerizeDurationReduction = 176,
		StunDurationReduction = 177,
		SpeedDecreaseDurationReduction = 178,
		BladeturnReinforcement = 179,
		DefensiveBonus = 180,
		SpellFumbleChance = 181,
		NegativeReduction = 182,
		PieceAblative = 183,
		ReactionaryStyleDamage = 184,
		SpellPowerCost = 185,
		StyleCostReduction = 186,
		ToHitBonus = 187,

		#region TOA
		//TOA
		ToABonus_First = 188,
		ArcherySpeed = 188,
		ArrowRecovery = 189,
		BuffEffectiveness = 190,
		CastingSpeed = 191,
		DeathExpLoss = 192,
		DebuffEffectivness = 193,
		Fatigue = 194,
		HealingEffectiveness = 195,
		PowerPool = 196,
		ResistPierce = 197,
		SpellDamage = 198,
		SpellDuration = 199,
		StyleDamage = 200,
		ToABonus_Last = 200,
		#endregion

		#region Cap Bonuses
		//Caps bonuses
		MaxHealthCapBonus = 210,
		PowerPoolCapBonus = 211,
		#endregion

		WeaponSkill = 212,
		AllSkills = 213,
		CriticalMeleeHitChance = 214,
		CriticalArcheryHitChance = 215,
		CriticalSpellHitChance = 216,
		WaterSpeed = 217,
		SpellLevel = 218,
		MissHit = 219,
		KeepDamage = 220,

		DPS = 230,
		MagicAbsorption = 231,
		CriticalHealHitChance = 232,

		MythicalSafeFall = 233,
		MythicalDiscumbering = 234,
		MythicalCoin = 235,

		BountyPoints = 247,
		XpPoints = 248,
		Resist_Natural = 249,
		ExtraHP = 250,
		Conversion = 251,
		StyleAbsorb = 252,
		RealmPoints = 253,
		ArcaneSyphon = 254,
		LivingEffectiveness = 255,
		MaxProperty = 255
	}
}
