﻿using DOL.GS.PropertyCalc;
using System;

namespace DOL.GS
{
	public class Boni
	{
		private GameLiving owner;
		private BonusProperty[] properties = new BonusProperty[(int)eProperty.MaxProperty + 1];

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
			for (int i=0;i<properties.Length;i++)
			{
				properties[i] = new BonusProperty(owner, (eProperty)i);
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
			if (IsStat(bonus.Type))
			{
				properties[(int)bonus.Type].Set(bonus.Value, bonus.Category);
			}
			else
			{
				var indexer = GetIndexer(bonus.Category);
				indexer[bonus.Type] = bonus.Value;
			}
		}

		public int GetValueOf(BonusComponent component)
		{
			if(IsStat(component.Property))
			{
				return properties[(int)component.Property].Get(component.Category);
			}
			var indexer = GetIndexer(component.Category);
			return indexer[component.Property];
		}

		private bool IsStat(eProperty property)
		{
			return (property >= eProperty.Stat_First && property <= eProperty.Stat_Last) || property == eProperty.Acuity;
		}

		private bool IsStatOvercap(eProperty property)
		{
			return property >= eProperty.StrCapBonus && property <= eProperty.AcuCapBonus;
		}

		private bool IsMythicalStat(eProperty property)
		{
			return property >= eProperty.MythicalStatCapBonus_First && property <= eProperty.MythicalStatCapBonus_Last;
		}

		public void Clear(BonusCategory category)
		{
			for (int i=0;i<=(int)eProperty.MaxProperty;i++)
			{

				SetTo(new Bonus(0, category.Value , (eProperty)i));
			}
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
