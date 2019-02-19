using DOL.GS.PropertyCalc;
using System;

namespace DOL.GS
{
	public class Boni
	{
		private GameLiving owner;

		public IPropertyIndexer BaseBoni { get; } = new BasePropertyIndexer();
		public IPropertyIndexer AbilityBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer ItemBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer BaseBuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer SpecBuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer ExtraBuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer DebuffBoni { get; } = new PropertyIndexer();
		public IPropertyIndexer SpecDebuffBoni { get; } = new PropertyIndexer();
		public IMultiplicativeProperties MultiplicativeBuff { get; } = new MultiplicativePropertiesHybrid();

		public Boni(GameLiving owner)
		{
			this.owner = owner;
		}

		public void Add(Bonus bonus)
		{
			var indexer = GetIndexer(bonus.Category);
			indexer[bonus.Type] += bonus.Value;
		}

		public void Remove(Bonus bonus)
		{
			var malus = new Bonus(-1 * bonus.Value, bonus.Category, bonus.Type);
			Add(malus);
		}

		public void SetTo(Bonus bonus)
		{
			var indexer = GetIndexer(bonus.Category);
			indexer[bonus.Type] = bonus.Value;
		}

		public BonusProperty GetProperty(eProperty property)
		{
			if (false)
			{

			}
			else
			{
				var bonusProperty = new BonusProperty(owner, property);
				foreach (var category in BonusCategory.IndexerCategories)
				{
					int componentValue = GetValueOf(new BonusComponent(category, property));
					bonusProperty.Add(componentValue, new BonusCategory(category));
				}
				bonusProperty.SetMultiplier(MultiplicativeBuff.Get((int)property));

				return bonusProperty;
			}
		}

		public int GetValueOf(BonusComponent component)
		{
			var indexer = GetIndexer(component.Category);
			return indexer[component.Property];
		}

		public void Clear(BonusCategory category)
		{
			var indexer = GetIndexer(category.Value);
			indexer.Clear();
		}

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
					return SpecDebuffBoni;
				default:
					throw new ArgumentException();
			}
		}
	}

	public class BonusComponent
	{
		public ePropertyCategory Category { get; }
		public eProperty Property { get; }

		public BonusComponent(ePropertyCategory category, eProperty property)
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
		public ePropertyCategory Value { get; }

		public BonusComponent Strength { get { return new BonusComponent(Value, eProperty.Strength); } }
		public BonusComponent Constitution { get { return new BonusComponent(Value, eProperty.Constitution); } }
		public BonusComponent Dexterity { get { return new BonusComponent(Value, eProperty.Dexterity); } }
		public BonusComponent Quickness { get { return new BonusComponent(Value, eProperty.Quickness); } }
		public BonusComponent Empathy { get { return new BonusComponent(Value, eProperty.Empathy); } }
		public BonusComponent Intelligence { get { return new BonusComponent(Value, eProperty.Intelligence); } }
		public BonusComponent Piety { get { return new BonusComponent(Value, eProperty.Piety); } }
		public BonusComponent Charisma { get { return new BonusComponent(Value, eProperty.Charisma); } }


		public BonusCategory(ePropertyCategory category)
		{
			this.Value = category;
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
			return new Bonus(value, this.Value, property);
		}

		public BonusComponent ComponentOf(eProperty property)
		{
			return new BonusComponent(this.Value, property);
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
		Debuff,
		SpecDebuff,
		__Last = SpecDebuff,
	}
}
