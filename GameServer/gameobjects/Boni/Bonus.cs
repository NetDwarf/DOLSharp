namespace DOL.GS
{
	public class Bonus
	{
		public Bonus(int value, BonusCategory category, BonusType type)
		{
			var component = new BonusComponent(category, type);
			this.TypeID = component.Type.ID;
			this.CategoryID = component.Category.ID;
			this.Value = value;
		}

		public int Value { get; }
		private eProperty TypeID { get; }
		private ePropertyCategory CategoryID { get; }
		public BonusType Type { get { return new BonusType(TypeID); } }
		public BonusCategory Category { get { return new BonusCategory(CategoryID); } }

		public static BonusCategory Create(ePropertyCategory categoryID) { return new BonusCategory(categoryID); }
		public static BonusType Create(eProperty typeID) { return new BonusType(typeID); }

		public static BonusCategory Base { get { return new BonusCategory(ePropertyCategory.Base); } }
		public static BonusCategory Ability { get { return new BonusCategory(ePropertyCategory.Ability); } }
		public static BonusCategory Item { get { return new BonusCategory(ePropertyCategory.Item); } }
		public static BonusCategory ItemOvercap { get { return new BonusCategory(ePropertyCategory.ItemOvercap); } }
		public static BonusCategory Mythical { get { return new BonusCategory(ePropertyCategory.Mythical); } }
		public static BonusCategory BaseBuff { get { return new BonusCategory(ePropertyCategory.BaseBuff); } }
		public static BonusCategory SpecBuff { get { return new BonusCategory(ePropertyCategory.SpecBuff); } }
		public static BonusCategory ExtraBuff { get { return new BonusCategory(ePropertyCategory.ExtraBuff); } }
		public static BonusCategory Debuff { get { return new BonusCategory(ePropertyCategory.Debuff); } }
		public static BonusCategory SpecDebuff { get { return new BonusCategory(ePropertyCategory.SpecDebuff); } }
		public static BonusCategory Multiplier { get { return new BonusCategory(ePropertyCategory.Multiplier); } }

		public static BonusType UndefinedType { get { return new BonusType(eProperty.Undefined); } }
		public static StatBonus Stat { get { return new StatBonus(); } }
		public static ResistBonus Resist { get { return new ResistBonus(); } }
	}

	public class BonusComponent
	{
		private eProperty typeID;
		private ePropertyCategory categoryID;

		public BonusType Type { get { return new BonusType(typeID); } }
		public BonusCategory Category { get { return new BonusCategory(categoryID); } }

		public BonusComponent(ePropertyCategory category, eProperty property)
			: this(new BonusCategory(category), new BonusType(property)) { }

		public BonusComponent(BonusCategory category, BonusType type)
		{
			bool isItem = category.ID == ePropertyCategory.Item;
			bool isItemStatOvercap = isItem && type.ID >= eProperty.StrCapBonus && type.ID <= eProperty.AcuCapBonus;
			bool isMythical = isItem && type.ID >= eProperty.MythicalStatCapBonus_First && type.ID <= eProperty.MythicalStatCapBonus_Last;
			bool isResistOvercap = isItem && type.ID >= eProperty.ResCapBonus_First && type.ID <= eProperty.ResCapBonus_Last;
			bool isRegenDebuff = category.Equals(Bonus.SpecBuff) && type.IsRegen;

			if (type.ID == eProperty.AcuCapBonus)
			{
				typeID = eProperty.Acuity;
				categoryID = ePropertyCategory.ItemOvercap;
			}
			else if (type.ID == eProperty.MythicalAcuCapBonus)
			{
				typeID = eProperty.Acuity;
				categoryID = ePropertyCategory.Mythical;
			}
			else if(isItemStatOvercap)
			{
				typeID = type.ID - eProperty.StrCapBonus + eProperty.Stat_First;
				categoryID = ePropertyCategory.ItemOvercap;
			}
			else if (isMythical)
			{
				typeID = type.ID - eProperty.MythicalStatCapBonus_First + eProperty.Stat_First;
				categoryID = ePropertyCategory.Mythical;
			}
			else if (isResistOvercap)
			{
				typeID = type.ID - eProperty.ResCapBonus_First + eProperty.Resist_First;
				categoryID = ePropertyCategory.Mythical;
			}
			else if(isRegenDebuff)
			{
				typeID = type.ID;
				categoryID = ePropertyCategory.Debuff;
			}
			else
			{
				categoryID = category.ID;
				typeID = type.ID;
			}
		}

		public Bonus Create(int value)
		{
			return new Bonus(value, Category, Type);
		}

		public override bool Equals(object obj)
		{
			var comp2 = obj as BonusComponent;
			if (comp2 is null) { return false; }
			return this.Category.Equals(comp2.Category) && this.Type.Equals(comp2.Type);
		}
	}

	public class BonusCategory
	{
		public ePropertyCategory ID { get; }

		public BonusComponent Strength { get { return new BonusComponent(ID, eProperty.Strength); } }
		public BonusComponent Constitution { get { return new BonusComponent(ID, eProperty.Constitution); } }
		public BonusComponent Dexterity { get { return new BonusComponent(ID, eProperty.Dexterity); } }
		public BonusComponent Quickness { get { return new BonusComponent(ID, eProperty.Quickness); } }
		public BonusComponent Empathy { get { return new BonusComponent(ID, eProperty.Empathy); } }
		public BonusComponent Intelligence { get { return new BonusComponent(ID, eProperty.Intelligence); } }
		public BonusComponent Piety { get { return new BonusComponent(ID, eProperty.Piety); } }
		public BonusComponent Charisma { get { return new BonusComponent(ID, eProperty.Charisma); } }
		public BonusComponent Acuity { get { return new BonusComponent(ID, eProperty.Acuity); } }
		
		public BonusCategory(ePropertyCategory category)
		{
			this.ID = category;
		}
		
		public Bonus Create(int value, eProperty property)
		{
			var type = new BonusType(property);
			return Create(value, type);
		}

		public Bonus Create(int value, BonusType type)
		{
			return new Bonus(value, this, type);
		}

		public BonusComponent ComponentOf(eProperty property)
		{
			return new BonusComponent(this.ID, property);
		}

		public BonusComponent ComponentOf(BonusType type)
		{
			return new BonusComponent(this.ID, type.ID);
		}

		public override bool Equals(object obj)
		{
			var category2 = obj as BonusCategory;
			if (category2 is null) { return false; }
			return this.ID == category2.ID;
		}
	}

	public class BonusType
	{
		public eProperty ID { get; }

		public BonusType(eProperty id)
		{
			this.ID = id;
		}

		public BonusComponent Base { get { return new BonusComponent(ePropertyCategory.Base, ID); } }
		public BonusComponent Ability { get { return new BonusComponent(ePropertyCategory.Ability, ID); } }
		public BonusComponent Item { get { return new BonusComponent(ePropertyCategory.Item, ID); } }
		public BonusComponent ItemOvercap { get { return new BonusComponent(ePropertyCategory.ItemOvercap, ID); } }
		public BonusComponent Mythical { get { return new BonusComponent(ePropertyCategory.Mythical, ID); } }
		public BonusComponent BaseBuff { get { return new BonusComponent(ePropertyCategory.BaseBuff, ID); } }
		public BonusComponent SpecBuff { get { return new BonusComponent(ePropertyCategory.SpecBuff, ID); } }
		public BonusComponent ExtraBuff { get { return new BonusComponent(ePropertyCategory.ExtraBuff, ID); } }
		public BonusComponent Debuff { get { return new BonusComponent(ePropertyCategory.Debuff, ID); } }
		public BonusComponent SpecDebuff { get { return new BonusComponent(ePropertyCategory.SpecDebuff, ID); } }
		public BonusComponent Multiplier { get { return new BonusComponent(ePropertyCategory.Multiplier, ID); } }

		public BonusComponent From(BonusCategory category) { return new BonusComponent(category.ID, ID); }

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

		public bool IsAcuityStat { get { return (ID >= eProperty.Intelligence && ID <= eProperty.Stat_Last) || ID == eProperty.Acuity; } }

		public bool IsResist {
			get
			{
				return ID >= eProperty.Resist_First && ID <= eProperty.Resist_Last || ID == eProperty.Resist_Natural;
			}
		}

		public bool IsRegen
		{
			get
			{
				return ID == eProperty.HealthRegenerationRate || ID == eProperty.PowerRegenerationRate || ID == eProperty.EnduranceRegenerationRate;
			}
		}

		public bool IsTOAPercentBonus
		{
			get
			{
				bool isMeleeDamage = ID == eProperty.MeleeDamage;
				bool isArcherSpeed = ID == eProperty.ArcherySpeed;
				bool isMeleeSpeed = ID == eProperty.MeleeSpeed;
				bool isSpellRange = ID == eProperty.SpellRange;
				return isMeleeDamage || isArcherSpeed || isMeleeSpeed || isSpellRange;
			}
		}
	}

	public class StatBonus
	{
		public BonusType Strength { get { return new BonusType(eProperty.Strength); } }
		public BonusType Constitution { get { return new BonusType(eProperty.Constitution); } }
		public BonusType Dexterity { get { return new BonusType(eProperty.Dexterity); } }
		public BonusType Quickness { get { return new BonusType(eProperty.Quickness); } }
		public BonusType Intelligence { get { return new BonusType(eProperty.Intelligence); } }
		public BonusType Piety { get { return new BonusType(eProperty.Piety); } }
		public BonusType Empathy { get { return new BonusType(eProperty.Empathy); } }
		public BonusType Charisma { get { return new BonusType(eProperty.Charisma); } }
		public BonusType Acuity { get { return new BonusType(eProperty.Acuity); } }
	}

	public class ResistBonus
	{
		public BonusType Slash { get { return new BonusType(eProperty.Resist_Slash); } }
		public BonusType Crush { get { return new BonusType(eProperty.Resist_Crush); } }
		public BonusType Thrust { get { return new BonusType(eProperty.Resist_Thrust); } }
		public BonusType Body { get { return new BonusType(eProperty.Resist_Body); } }
		public BonusType Cold { get { return new BonusType(eProperty.Resist_Cold); } }
		public BonusType Energy { get { return new BonusType(eProperty.Resist_Energy); } }
		public BonusType Heat { get { return new BonusType(eProperty.Resist_Heat); } }
		public BonusType Matter { get { return new BonusType(eProperty.Resist_Matter); } }
		public BonusType Spirit { get { return new BonusType(eProperty.Resist_Spirit); } }
		public BonusType Essence { get { return new BonusType(eProperty.Resist_Natural); } }
	}

	public enum ePropertyCategory : byte
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
}
