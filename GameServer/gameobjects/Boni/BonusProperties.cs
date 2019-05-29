using DOL.GS.PropertyCalc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DOL.GS
{
	public class BonusProperties
	{
		private GameLiving owner;
		
		public BonusProperties(GameLiving owner)
		{
			this.owner = owner;
		}

		public int ValueOf(BonusType type)
		{
			var bonusProperty = CreatePropertyOf(type);
			return bonusProperty.Value;
		}

		public int BuffValueOf(BonusType type)
		{
			var bonusProperty = CreatePropertyOf(type);
			return bonusProperty.BuffValue;
		}

		public int ItemValueOf(BonusType type)
		{
			var bonusProperty = CreatePropertyOf(type);
			return bonusProperty.ItemValue;
		}

		private IBonusProperty CreatePropertyOf(BonusType type)
		{
			bool typeIsMeleeDamage = type.ID == eProperty.MeleeDamage;
			bool typeIsGeneralized = type.IsStat || type.IsResist || typeIsMeleeDamage;

			if (type.ID == eProperty.MeleeSpeed)
			{
				return new InvertedPercentProperty(owner, type);
			}
			if (typeIsGeneralized)
			{
				if (owner is GamePlayer)
				{
					var player = owner as GamePlayer;
					bool ownerIsArcher = player.CharacterClass.ID == (int)eCharacterClass.Scout || player.CharacterClass.ID == (int)eCharacterClass.Hunter || player.CharacterClass.ID == (int)eCharacterClass.Ranger;
					bool typeIsManaStat = type.ID == (eProperty)player.CharacterClass.ManaStat;
					if (typeIsManaStat && !ownerIsArcher)
					{
						return new BonusProperty(player, type, Bonus.Stat.Acuity);
					}

					return new BonusProperty(player, type);
				}
				else
				{
					return new NPCBonusProperty(owner, type);
				}
			}
			return new LegacyBonusProperty(owner, type);
		}
	}

	public interface IBonusProperty
	{
		int Value { get; }
		int BuffValue { get; }
		int ItemValue { get; }
	}

	public class BonusProperty : IBonusProperty
	{
		private GamePlayer owner;
		private BonusType[] affectingTypes;

		public BonusProperty(GamePlayer owner, params BonusType[] affectingTypes)
		{
			this.owner = owner;
			this.affectingTypes = affectingTypes;
		}

		private BonusType PrimaryType { get { return affectingTypes[0]; } }

		public virtual int Value
		{
			get
			{
				int debuff = Math.Abs(GetEffectiveValueOf(Bonus.Debuff));
				int abilityBonus = GetRawValueOf(Bonus.Ability);
				int baseBonus = GetRawValueOf(Bonus.Base);
				if (PrimaryType.IsResist)
				{
					baseBonus = owner.LivingRace.GetResist((eResist)PrimaryType.ID);
				}

				int itemBonus = ItemValue;
				int buffBonus = BuffValue;

				// Stats and resist debuffs have 100% Effectiveness for player buffs, but only 50%
				// effectiveness for item bonuses and baseBonus(?).

				int unbuffedBonus = baseBonus + itemBonus;
				buffBonus -= debuff;
				if (buffBonus < 0 && (PrimaryType.IsStat || PrimaryType.IsResist))
				{
					unbuffedBonus += buffBonus / 2;
					buffBonus = 0;
					unbuffedBonus = Math.Max(0, unbuffedBonus);
				}

				int effectiveBonus = unbuffedBonus + buffBonus + abilityBonus;
				effectiveBonus = (int)(effectiveBonus * GetMultiplier());

				if (PrimaryType.ID == eProperty.Constitution)
				{
					effectiveBonus -= owner.TotalConstitutionLostAtDeath;
				}

				if (PrimaryType.IsStat)
				{
					effectiveBonus = Math.Max(1, effectiveBonus);
				}

				return Math.Min(effectiveBonus, Cap.HardCap);
			}
		}

		public int BuffValue
		{
			get
			{
				int baseBuffBonus = GetEffectiveValueOf(Bonus.BaseBuff);
				int specBuffBonus = GetEffectiveValueOf(Bonus.SpecBuff);
				int extraBuffBonus = GetEffectiveValueOf(Bonus.ExtraBuff);
				int buffBonus = baseBuffBonus + specBuffBonus + extraBuffBonus;
				return Math.Min(buffBonus, Cap.Buff);
			}
		}

		public int ItemValue
		{
			get
			{
				int rawItemBonus = GetRawValueOf(Bonus.Item);
				int overcap = GetEffectiveValueOf(Bonus.ItemOvercap);
				int rawMythicalCapIncrease = GetRawValueOf(Bonus.Mythical);

				int mythicalCapIncreaseCap = Cap.Mythical;
				int baseCap = Cap.Item;
				int capIncrease = Math.Min(mythicalCapIncreaseCap, overcap + rawMythicalCapIncrease);
				
				int cap = baseCap + capIncrease;
				return Math.Min(rawItemBonus, cap);
			}
		}

		private int GetEffectiveValueOf(BonusCategory category)
		{
			int rawValue = GetRawValueOf(category);
			int cap = Cap.For(category);
			return Math.Min(cap, rawValue);
		}

		private IPropertyCap Cap
		{
			get
			{
				var propertyCaps = new PropertyCaps(owner);
				var cap = propertyCaps.Of(PrimaryType);
				return cap;
			}
		}

		private int GetRawValueOf(BonusCategory category)
		{
			var boni = owner.Boni;

			int rawValue = 0;

			foreach (var affectingType in affectingTypes)
			{
				rawValue += boni.RawValueOf(affectingType.From(category));
			}

			return rawValue;
		}

		private double GetMultiplier()
		{
			double rawValue = 1.0;

			foreach (var affectingType in affectingTypes)
			{
				rawValue *= owner.Boni.RawValueOf(affectingType.Multiplier) / (double)1000;
			}

			return rawValue;
		}
	}

	public class InvertedPercentProperty : IBonusProperty
	{
		private GameLiving owner;
		private BonusType type;

		public InvertedPercentProperty(GameLiving owner, BonusType type)
		{
			this.owner = owner;
			this.type = type;
		}

		public virtual int Value
		{
			get
			{
				var baseBonus = 100;
				var debuff = owner.Boni.RawValueOf(type.From(Bonus.Debuff));
				return Math.Max(1, 100
				- BuffValue // less is faster = buff
				+ debuff // more is slower = debuff
				- ItemValue);
			}
		}

		public int BuffValue
		{
			get
			{
				return owner.Boni.RawValueOf(type.From(Bonus.BaseBuff));
			}
		}

		public int ItemValue
		{
			get
			{
				var rawItemBonus = owner.Boni.RawValueOf(type.From(Bonus.Item));
				var itemCap = 10;
				return Math.Min(itemCap, rawItemBonus);
			}
		}
	}

	public class NPCBonusProperty : IBonusProperty
	{
		private GameLiving owner;
		private BonusType type;

		public NPCBonusProperty(GameLiving owner, BonusType type)
		{
			this.owner = owner;
			this.type = type;
		}

		public virtual int Value
		{
			get
			{
				var boni = owner.Boni;

				int baseBonus = boni.RawValueOf(Bonus.Base.ComponentOf(type));
				int abilityBonus = boni.RawValueOf(Bonus.Ability.ComponentOf(type));
				int debuff = boni.RawValueOf(Bonus.Debuff.ComponentOf(type));

				int itemBonus = ItemValue;
				int buffBonus = BuffValue;

				// Apply debuffs, 100% effectiveness for player buffs, 50% effectiveness
				// for item and base stats

				int unbuffedBonus = baseBonus + itemBonus;
				buffBonus -= Math.Abs(debuff);

				if (buffBonus < 0)
				{
					unbuffedBonus += buffBonus / 2;
					buffBonus = 0;
					unbuffedBonus = Math.Max(0, unbuffedBonus);
				}

				int effectiveBonus = unbuffedBonus + buffBonus + abilityBonus;
				effectiveBonus = (int)(effectiveBonus * boni.RawValueOf(Bonus.Multiplier.ComponentOf(type)) / 1000.0);

				if(type.IsStat)
				{
					effectiveBonus = Math.Max(1, effectiveBonus);
				}
				return effectiveBonus;
			}
		}

		public int BuffValue
		{
			get
			{
				var boni = owner.Boni;

				int baseBuffBonus = boni.RawValueOf(Bonus.BaseBuff.ComponentOf(type));
				int specBuffBonus = boni.RawValueOf(Bonus.SpecBuff.ComponentOf(type));
				int extraBuffBonus = boni.RawValueOf(type.From(Bonus.ExtraBuff));

				return baseBuffBonus + specBuffBonus;
			}
		}

		public int ItemValue
		{
			get
			{
				return 0;
			}
		}
	}

	public class LegacyBonusProperty : IBonusProperty
	{
		private GameLiving owner;
		private IPropertyCalculator propertyCalculator;
		private BonusType type;

		public LegacyBonusProperty(GameLiving owner, BonusType type)
		{
			this.owner = owner;
			this.type = type;
			var propCalcFactory = new PropertyCalculatorFactory(owner);
			this.propertyCalculator = propCalcFactory.Create(type);
		}

		public virtual int Value
		{
			get
			{
				return propertyCalculator.CalcValue(owner, type.ID);
			}
		}

		public int BuffValue
		{
			get
			{
				return propertyCalculator.CalcValueFromBuffs(owner, type.ID);
			}
		}

		public int ItemValue
		{
			get
			{
				return propertyCalculator.CalcValueFromItems(owner, type.ID);
			}
		}
	}

	public class PropertyCalculatorFactory
	{
		private static readonly IPropertyCalculator[] legacyCalculators = new IPropertyCalculator[(int)eProperty.MaxProperty + 1];
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly bool loadedAllLegacyCalculators;
		private GameLiving owner;

		static PropertyCalculatorFactory()
		{
			try
			{
				foreach (Assembly asm in ScriptMgr.GameServerScripts)
				{
					foreach (Type t in asm.GetTypes())
					{
						try
						{
							if (!t.IsClass || t.IsAbstract) continue;
							if (!typeof(IPropertyCalculator).IsAssignableFrom(t)) continue;
							IPropertyCalculator calc = (IPropertyCalculator)Activator.CreateInstance(t);
							foreach (PropertyCalculatorAttribute attr in t.GetCustomAttributes(typeof(PropertyCalculatorAttribute), false))
							{
								for (int i = (int)attr.Min; i <= (int)attr.Max; i++)
								{
									legacyCalculators[i] = calc;
								}
							}
						}
						catch (Exception e)
						{
							if (log.IsErrorEnabled)
							{
								log.Error("Error while working with type " + t.FullName, e);
							}
						}
					}
				}
				loadedAllLegacyCalculators = true;
			}
			catch (Exception e)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("GameLiving.LoadCalculators()", e);
				}
				loadedAllLegacyCalculators = false;
			}
		}

		public PropertyCalculatorFactory(GameLiving owner)
		{
			this.owner = owner;
		}

		public IPropertyCalculator Create(BonusType type)
		{
			return legacyCalculators[(int)type.ID];
		}

		public bool AllCalculatorsLoaded { get { return loadedAllLegacyCalculators; } }
	}
}
