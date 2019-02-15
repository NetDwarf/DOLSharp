using DOL.GS.PropertyCalc;
using System;

namespace DOL.GS
{
	public class Boni
	{
		private IPropertyIndexer GetIndexer(ePropertyCategory category)
		{
			switch (category)
			{
				case ePropertyCategory.Base:
					return BaseBoni;
				case ePropertyCategory.Ability:
					return AbilityBoni;
				case ePropertyCategory.Item:
					return ItemBoni;
				case ePropertyCategory.BaseBuff:
					return BaseBuffBoni;
				case ePropertyCategory.SpecBuff:
					return SpecBuffBoni;
				case ePropertyCategory.ExtraBuff:
					return ExtraBuffBoni;
				case ePropertyCategory.Debuff:
					return DebuffBoni;
				case ePropertyCategory.SpecDebuff:
					return SpecBuffBoni;
				default:
					throw new ArgumentException();
			}
		}
		public BasePropertyIndexer BaseBoni { get; } = new BasePropertyIndexer();
		public IPropertyIndexer AbilityBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer ItemBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer BaseBuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer SpecBuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer ExtraBuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer DebuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer SpecDebuffBoni { get; } = new PropertyIndexer();

		public void Add(Bonus bonus)
		{
			var indexer = GetIndexer(bonus.Category);
			indexer[bonus.Property] += bonus.Value;
		}

		public void Remove(Bonus bonus)
		{
			var malus = new Bonus(-1 * bonus.Value, bonus.Category, bonus.Property);
			Add(malus);
		}

		public void SetTo(Bonus bonus)
		{
			var indexer = GetIndexer(bonus.Category);
			indexer[bonus.Property] = bonus.Value;
		}

		public int GetValueOf(BonusKind component)
		{
			var indexer = GetIndexer(component.Category);
			return indexer[component.Property];
		}

		public void Clear(BonusCategory category)
		{
			var indexer = GetIndexer(category.Value);
			indexer.Clear();
		}
	}

	public class Bonus
	{
		public Bonus(int value, ePropertyCategory category, eProperty prop)
		{
			this.Value = value;
			this.Property = prop;
			this.Category = category;
		}

		public int Value { get; }
		public eProperty Property { get; }
		public ePropertyCategory Category { get; }

		public static BonusCategory Base { get { return new BonusCategory(ePropertyCategory.Base); } }
		public static BonusCategory Ability { get { return new BonusCategory(ePropertyCategory.Ability); } }
		public static BonusCategory Item { get { return new BonusCategory(ePropertyCategory.Item); } }
		public static BonusCategory BaseBuff { get { return new BonusCategory(ePropertyCategory.BaseBuff); } }
		public static BonusCategory SpecBuff { get { return new BonusCategory(ePropertyCategory.SpecBuff); } }
		public static BonusCategory Extrabuff { get { return new BonusCategory(ePropertyCategory.ExtraBuff); } }
		public static BonusCategory Debuff { get { return new BonusCategory(ePropertyCategory.Debuff); } }
		public static BonusCategory SpecDebuff { get { return new BonusCategory(ePropertyCategory.SpecDebuff); } }
	}

	public class BonusKind
	{
		public ePropertyCategory Category { get; }
		public eProperty Property { get; }

		public BonusKind(ePropertyCategory category, eProperty property)
		{
			Category = category;
			Property = property;
		}

		public Bonus Create(int value)
		{
			return new Bonus(value, Category, Property);
		}
	}

	public class BonusCategory
	{
		public BonusCategory(ePropertyCategory category)
		{
			this.Value = category;
		}

		public ePropertyCategory Value { get; }

		public virtual BonusKind Strength { get { return new BonusKind(Value, eProperty.Strength); } }
		public virtual BonusKind Constitution { get { return new BonusKind(Value, eProperty.Constitution); } }
		public virtual BonusKind Dexterity { get { return new BonusKind(Value, eProperty.Dexterity); } }
		public virtual BonusKind Quickness { get { return new BonusKind(Value, eProperty.Quickness); } }
		public virtual BonusKind Empathy { get { return new BonusKind(Value, eProperty.Empathy); } }
		public virtual BonusKind Intelligence { get { return new BonusKind(Value, eProperty.Intelligence); } }
		public virtual BonusKind Piety { get { return new BonusKind(Value, eProperty.Piety); } }
		public virtual BonusKind Charisma { get { return new BonusKind(Value, eProperty.Charisma); } }

		public Bonus Create(int value, eProperty property)
		{
			return new Bonus(value, this.Value, property);
		}

		public BonusKind ComponentOf(eProperty property)
		{
			return new BonusKind(this.Value, property);
		}
	}

	public enum ePropertyCategory
	{
		Base,
		Ability,
		Item,
		BaseBuff,
		SpecBuff,
		ExtraBuff,
		SpecDebuff,
		Debuff,
	}
}
