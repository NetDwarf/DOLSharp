namespace DOL.GS
{
	public class Bonus
	{
		public Bonus(int value, ePropertyCategory category, eProperty prop)
		{
			var component = new BonusComponent(category, prop);
			this.Type = component.Property;
			this.Category = component.Category;
			this.Value = value;
		}

		public int Value { get; }
		public eProperty Type { get; }
		public ePropertyCategory Category { get; }

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
		public ePropertyCategory Category { get; }
		public eProperty Property { get; }
		public BonusType Type { get { return new BonusType(Property); } }
		public BonusCategory BonusCategory { get { return new BonusCategory(Category); } }

		public BonusComponent(ePropertyCategory category, eProperty property)
		{
			bool isItem = category == ePropertyCategory.Item;
			bool isItemStatOvercap = isItem && property >= eProperty.StrCapBonus && property <= eProperty.AcuCapBonus;
			bool isMythical = isItem && property >= eProperty.MythicalStatCapBonus_First && property <= eProperty.MythicalStatCapBonus_Last;
			bool isResistOvercap = isItem && property >= eProperty.ResCapBonus_First && property <= eProperty.ResCapBonus_Last;

			if (isItemStatOvercap)
			{
				if (property == eProperty.AcuCapBonus)
				{
					Property = eProperty.Acuity;
				}
				else
				{
					Property = property - eProperty.StrCapBonus + eProperty.Stat_First;
				}
				Category = ePropertyCategory.ItemOvercap;
			}
			else if (isMythical)
			{
				if (property == eProperty.MythicalAcuCapBonus)
				{
					Property = eProperty.Acuity;
				}
				else
				{
					Property = property - eProperty.MythicalStatCapBonus_First + eProperty.Stat_First;
				}
				Category = ePropertyCategory.Mythical;
			}
			else if(isResistOvercap)
			{
				Property = property - eProperty.ResCapBonus_First + eProperty.Resist_First;
				Category = ePropertyCategory.Mythical;
			}
			else
			{
				Category = category;
				Property = property;
			}
		}

		public Bonus Create(int value)
		{
			return new Bonus(value, Category, Property);
		}

		public override bool Equals(object obj)
		{
			var comp2 = obj as BonusComponent;
			if (comp2 is null) { return false; }
			return this.Category == comp2.Category && this.Property == comp2.Property;
		}
	}

	public class BonusCategory
	{
		public ePropertyCategory Name { get; }

		public BonusComponent Strength { get { return new BonusComponent(Name, eProperty.Strength); } }
		public BonusComponent Constitution { get { return new BonusComponent(Name, eProperty.Constitution); } }
		public BonusComponent Dexterity { get { return new BonusComponent(Name, eProperty.Dexterity); } }
		public BonusComponent Quickness { get { return new BonusComponent(Name, eProperty.Quickness); } }
		public BonusComponent Empathy { get { return new BonusComponent(Name, eProperty.Empathy); } }
		public BonusComponent Intelligence { get { return new BonusComponent(Name, eProperty.Intelligence); } }
		public BonusComponent Piety { get { return new BonusComponent(Name, eProperty.Piety); } }
		public BonusComponent Charisma { get { return new BonusComponent(Name, eProperty.Charisma); } }
		public BonusComponent Acuity { get { return new BonusComponent(Name, eProperty.Acuity); } }
		
		public BonusCategory(ePropertyCategory category)
		{
			this.Name = category;
		}
		
		public Bonus Create(int value, eProperty property)
		{
			return new Bonus(value, this.Name, property);
		}

		public BonusComponent ComponentOf(eProperty property)
		{
			return new BonusComponent(this.Name, property);
		}

		public BonusComponent ComponentOf(BonusType type)
		{
			return new BonusComponent(this.Name, type.ID);
		}

		public override bool Equals(object obj)
		{
			var category2 = obj as BonusCategory;
			if (category2 is null) { return false; }
			return this.Name == category2.Name;
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

		public BonusComponent From(BonusCategory category) { return new BonusComponent(category.Name, ID); }

		public override bool Equals(object obj)
		{
			var type2 = obj as BonusType;
			if (type2 is null) { return false; }
			return this.ID == type2.ID;
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
