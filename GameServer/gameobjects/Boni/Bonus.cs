namespace DOL.GS
{
	public class Bonus
	{
		public Bonus(int value, BonusCategory category, BonusType type)
		{
			this.TypeID = type.ID;
			this.CategoryID = category.ID;
			ConvertOldBonus();
			this.Value = value;
		}

		public int Value { get; }
		private eProperty TypeID { get; set; }
		private ePropertyCategory CategoryID { get; set; }
		public BonusType Type { get { return new BonusType(TypeID); } }
		public BonusCategory Category { get { return new BonusCategory(CategoryID); } }

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

		public static BonusType Strength { get { return new BonusType(eProperty.Strength); } }
		public static BonusType Constitution { get { return new BonusType(eProperty.Constitution); } }
		public static BonusType Dexterity { get { return new BonusType(eProperty.Dexterity); } }
		public static BonusType Quickness { get { return new BonusType(eProperty.Quickness); } }
		public static BonusType Intelligence { get { return new BonusType(eProperty.Intelligence); } }
		public static BonusType Piety { get { return new BonusType(eProperty.Piety); } }
		public static BonusType Empathy { get { return new BonusType(eProperty.Empathy); } }
		public static BonusType Charisma { get { return new BonusType(eProperty.Charisma); } }
		public static BonusType Acuity { get { return new BonusType(eProperty.Acuity); } }

		private void ConvertOldBonus()
		{
			bool isItem = Category.ID == ePropertyCategory.Item;
			bool isItemStatOvercap = isItem && Type.ID >= eProperty.StrCapBonus && Type.ID <= eProperty.AcuCapBonus;
			bool isMythical = isItem && Type.ID >= eProperty.MythicalStatCapBonus_First && Type.ID <= eProperty.MythicalStatCapBonus_Last;
			bool isResistOvercap = isItem && Type.ID >= eProperty.ResCapBonus_First && Type.ID <= eProperty.ResCapBonus_Last;
			bool isRegenDebuff = Category.Equals(Bonus.SpecBuff) && Type.IsRegen;

			if (Type.ID == eProperty.AcuCapBonus)
			{
				TypeID = eProperty.Acuity;
				CategoryID = ePropertyCategory.ItemOvercap;
			}
			else if (Type.ID == eProperty.MythicalAcuCapBonus)
			{
				TypeID = eProperty.Acuity;
				CategoryID = ePropertyCategory.Mythical;
			}
			else if (isItemStatOvercap)
			{
				TypeID = Type.ID - eProperty.StrCapBonus + eProperty.Stat_First;
				CategoryID = ePropertyCategory.ItemOvercap;
			}
			else if (isMythical)
			{
				TypeID = Type.ID - eProperty.MythicalStatCapBonus_First + eProperty.Stat_First;
				CategoryID = ePropertyCategory.Mythical;
			}
			else if (isResistOvercap)
			{
				TypeID = Type.ID - eProperty.ResCapBonus_First + eProperty.Resist_First;
				CategoryID = ePropertyCategory.Mythical;
			}
			else if (isRegenDebuff)
			{
				TypeID = Type.ID;
				CategoryID = ePropertyCategory.Debuff;
			}
			else
			{
				CategoryID = Category.ID;
				TypeID = Type.ID;
			}
			this.TypeID = TypeID;
			this.CategoryID = CategoryID;
		}
	}

	public class BonusComponent
	{
		public BonusType Type { get; }
		public BonusCategory Category { get; }

		public BonusComponent(BonusCategory category, BonusType type)
		{
			this.Category = category;
			this.Type = type;
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
		
		public BonusCategory(ePropertyCategory category)
		{
			this.ID = category;
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

		public BonusComponent From(BonusCategory category) { return new BonusComponent(category, this); }

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
