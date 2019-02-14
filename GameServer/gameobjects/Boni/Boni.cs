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
			var malus = new Bonus(-1 * bonus.Value, bonus.Property, bonus.Category);
			Add(malus);
		}

		public int GetValueOf(PropertyComponent component)
		{
			var indexer = GetIndexer(component.Category);
			return indexer[component.Property];
		}

		public void Clear(PropertyCategory category)
		{
			var indexer = GetIndexer(category.Value);
			indexer.Clear();
		}
	}

	public class Bonus
	{
		public Bonus(int value, eProperty prop,ePropertyCategory category)
		{
			this.Value = value;
			this.Property = prop;
			this.Category = category;
		}

		public int Value { get; }
		public eProperty Property { get; }
		public ePropertyCategory Category { get; }

		public static PropertyCategory Ability { get { return new PropertyCategory(ePropertyCategory.Ability); } }
		public static PropertyCategory Item { get { return new PropertyCategory(ePropertyCategory.Item); } }
		public static PropertyCategory BaseBuff { get { return new PropertyCategory(ePropertyCategory.BaseBuff); } }
		public static PropertyCategory SpecBuff { get { return new PropertyCategory(ePropertyCategory.SpecBuff); } }
		public static PropertyCategory Extrabuff { get { return new PropertyCategory(ePropertyCategory.ExtraBuff); } }
		public static PropertyCategory Debuff { get { return new PropertyCategory(ePropertyCategory.Debuff); } }
		public static PropertyCategory SpecDebuff { get { return new PropertyCategory(ePropertyCategory.SpecDebuff); } }
	}

	public class PropertyComponent
	{
		public ePropertyCategory Category { get; }
		public eProperty Property { get; }

		public PropertyComponent(ePropertyCategory category, eProperty property)
		{
			Category = category;
			Property = property;
		}

		public Bonus Create(int value)
		{
			return new Bonus(value, Property, Category);
		}
	}

	public class PropertyCategory
	{
		public PropertyCategory(ePropertyCategory category)
		{
			this.Value = category;
		}

		public ePropertyCategory Value { get; }
		public PropertyComponent Constitution { get { return new PropertyComponent(Value, eProperty.Constitution); } }
		
		public Bonus CreateBonus(int value, eProperty property)
		{
			return new Bonus(value, property, this.Value);
		}

		public PropertyComponent ComponentOf(eProperty property)
		{
			return new PropertyComponent(this.Value, property);
		}
	}

	public enum ePropertyCategory
	{
		Ability,
		Item,
		BaseBuff,
		SpecBuff,
		ExtraBuff,
		SpecDebuff,
		Debuff,
	}
}
