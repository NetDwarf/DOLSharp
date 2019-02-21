using DOL.GS.PropertyCalc;

namespace DOL.GS
{
	public class Boni
	{
		private GameLiving owner;
		private BonusProperty[] properties = new BonusProperty[(int)eProperty.MaxProperty + 1];

		public IMultiplicativeProperties MultiplicativeBuff { get; } = new MultiplicativePropertiesHybrid();

		public Boni(GameLiving owner)
		{
			this.owner = owner;
			for (int i = 0; i < properties.Length; i++)
			{
				properties[i] = new BonusProperty(owner,(eProperty)i);
			}
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
			Get(bonus.Type).Set(bonus.Value, bonus.Category);
		}

		public int GetValueOf(BonusComponent component)
		{
			return Get(component.Property).Get(component.Category);
		}

		private BonusProperty Get(eProperty property)
		{
			return properties[(int)property];
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

		public static ePropertyCategory[] IndexerCategories {
			get
			{
				return new ePropertyCategory[] { ePropertyCategory.Base,
					ePropertyCategory.Ability,
					ePropertyCategory.Item,
					ePropertyCategory.BaseBuff,
					ePropertyCategory.SpecBuff,
					ePropertyCategory.ExtraBuff,
					ePropertyCategory.Debuff,
					ePropertyCategory.SpecDebuff};
			}
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

	public enum ePropertyCategory
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
