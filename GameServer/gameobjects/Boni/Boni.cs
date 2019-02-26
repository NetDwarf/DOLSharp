using DOL.GS.PropertyCalc;
using System.Collections.Generic;

namespace DOL.GS
{
	public class Boni
	{
		private GameLiving owner;
		private List<IBonusProperty> properties = new List<IBonusProperty>();

		public IMultiplicativeProperties MultiplicativeBuff { get; } = new MultiplicativePropertiesHybrid();

		public Boni(GameLiving owner)
		{
			this.owner = owner;
		}

		public void Add(Bonus bonus)
		{
			int oldValue = GetValueOf(new BonusComponent(bonus.Category, bonus.Type));
			SetTo(new Bonus(oldValue + bonus.Value, bonus.Category, bonus.Type));
		}

		public void Remove(Bonus bonus)
		{
			var malus = new Bonus(-1 * bonus.Value, bonus.Category, bonus.Type);
			Add(malus);
		}

		public void SetTo(Bonus bonus)
		{
			var propIndex = properties.FindIndex(s => s.Type == bonus.Type);
			if(propIndex < 0)
			{
				var bonusProperty = new BonusProperty(owner, bonus.Type);
				bonusProperty.Set(bonus.Value, bonus.Category);
				properties.Add(bonusProperty);
			}
			else
			{
				properties[propIndex].Set(bonus.Value, bonus.Category);
			}
			Get(bonus.Type).Set(bonus.Value, bonus.Category);
		}

		public int GetValueOf(BonusComponent component)
		{
			return Get(component.Property).Get(new BonusCategory(component.Category));
		}

		private IBonusProperty Get(eProperty property)
		{
			var propIndex = properties.FindIndex(s => s.Type == property);
			if(propIndex < 0) { return BonusProperty.Dummy(); }
			return properties[propIndex];
		}

		public void Clear(BonusCategory category)
		{
			for (int i = 0; i <= (int)eProperty.MaxProperty; i++)
			{
				SetTo(new Bonus(0, category.Name, (eProperty)i));
			}
		}
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
			if(comp2 is null) { return false; }
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
		__Last = SpecDebuff,
	}
}
