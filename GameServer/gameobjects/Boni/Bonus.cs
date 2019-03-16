using System.Collections.Generic;

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

		public int Value { get;}
		public eProperty Type { get; }
		public ePropertyCategory Category { get; }

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
	}

	public class BonusComponent
	{
		public ePropertyCategory Category { get; }
		public eProperty Property { get; }

		public BonusComponent(ePropertyCategory category, eProperty property)
		{
			bool isItem = category == ePropertyCategory.Item;
			bool isItemStatOvercap = isItem && property >= eProperty.StrCapBonus && property <= eProperty.AcuCapBonus;
			bool isMythical = isItem && property >= eProperty.MythicalStatCapBonus_First && property <= eProperty.MythicalStatCapBonus_Last;

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

		public override bool Equals(object obj)
		{
			var category2 = obj as BonusCategory;
			if (category2 is null) { return false; }
			return this.Name == category2.Name;
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
