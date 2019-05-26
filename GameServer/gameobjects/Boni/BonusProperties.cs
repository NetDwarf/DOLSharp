using DOL.GS.PropertyCalc;
using System;
using System.Collections.Generic;
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
			if(owner is GamePlayer && type.IsStat)
			{
				var player = owner as GamePlayer;
				bool ownerIsArcher = player.CharacterClass.ID == (int)eCharacterClass.Scout || player.CharacterClass.ID == (int)eCharacterClass.Hunter || player.CharacterClass.ID == (int)eCharacterClass.Ranger;
				bool typeIsManaStat = type.ID == (eProperty)player.CharacterClass.ManaStat;
				if (typeIsManaStat && !ownerIsArcher)
				{
					return new StatProperty(player, type, Bonus.Stat.Acuity);
				}

				return new StatProperty(player, type);
			}
			else if(type.IsStat)
			{
				return new NPCStatProperty(owner, type);
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

	public class StatProperty : IBonusProperty
	{
		private GamePlayer owner;
		private BonusType[] affectingTypes;

		public StatProperty(GamePlayer owner, params BonusType[] affectingTypes)
		{
			this.owner = owner;
			this.affectingTypes = affectingTypes;
		}

		private BonusType PrimaryType { get { return affectingTypes[0]; } }

		public virtual int Value
		{
			get
			{
				int baseBonus = GetRawValueOf(Bonus.Base);
				int abilityBonus = GetRawValueOf(Bonus.Ability);
				int debuff = Math.Abs(GetRawValueOf(Bonus.Debuff));

				int itemBonus = ItemValue;
				int buffBonus = BuffValue;

				// Apply debuffs, 100% effectiveness for player buffs, 50% effectiveness
				// for item and base stats

				int unbuffedBonus = baseBonus + itemBonus;
				buffBonus -= debuff;

				if (buffBonus < 0)
				{
					unbuffedBonus += buffBonus / 2;
					buffBonus = 0;
					unbuffedBonus = Math.Max(0, unbuffedBonus);
				}

				int stat = unbuffedBonus + buffBonus + abilityBonus;
				stat = (int)(stat * GetMultiplier());

				if (PrimaryType.ID == eProperty.Constitution)
				{
					stat -= owner.TotalConstitutionLostAtDeath;
				}

				return Math.Max(1, stat);
			}
		}

		public int BuffValue
		{
			get
			{
				int baseBuffBonus = GetEffectiveValueOf(Bonus.BaseBuff);
				int specBuffBonus = GetEffectiveValueOf(Bonus.SpecBuff);

				return baseBuffBonus + specBuffBonus;
			}
		}

		public int ItemValue
		{
			get
			{
				int rawItemBonus = GetRawValueOf(Bonus.Item);
				int overcap = GetEffectiveValueOf(Bonus.ItemOvercap);
				int rawMythicalCapIncrease = GetRawValueOf(Bonus.Mythical);

				int capIncreaseHardcap = GetCapOf(Bonus.Mythical);
				int baseCap = GetCapOf(Bonus.Item);
				int capIncrease = Math.Min(capIncreaseHardcap, overcap + rawMythicalCapIncrease);
				
				int cap = baseCap + capIncrease;
				return Math.Min(rawItemBonus, cap);
			}
		}

		private int GetEffectiveValueOf(BonusCategory category)
		{
			int rawValue = GetRawValueOf(category);
			int cap = GetCapOf(category);
			return Math.Min(cap, rawValue);
		}

		private int GetCapOf(BonusCategory category)
		{
			var propertyCaps = new PropertyCaps(owner);
			int cap = propertyCaps.ValueOf(PrimaryType.From(category));
			return cap;
		}

		protected virtual int GetRawValueOf(BonusCategory category)
		{
			var boni = owner.Boni;

			int rawValue = 0;

			foreach (var affectingType in affectingTypes)
			{
				rawValue += boni.RawValueOf(affectingType.From(category));
			}

			return rawValue;
		}

		protected virtual double GetMultiplier()
		{
			double rawValue = 1.0;

			foreach (var affectingType in affectingTypes)
			{
				rawValue *= owner.Boni.RawValueOf(affectingType.Multiplier) / (double)1000;
			}

			return rawValue;
		}
	}

	public class NPCStatProperty : IBonusProperty
	{
		private GameLiving owner;
		private BonusType type;

		public NPCStatProperty(GameLiving owner, BonusType type)
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

				int stat = unbuffedBonus + buffBonus + abilityBonus;
				stat = (int)(stat * boni.RawValueOf(Bonus.Multiplier.ComponentOf(type)) / 1000.0);

				return Math.Max(1, stat);
			}
		}

		public int BuffValue
		{
			get
			{
				var boni = owner.Boni;

				int baseBuffBonus = boni.RawValueOf(Bonus.BaseBuff.ComponentOf(type));
				int specBuffBonus = boni.RawValueOf(Bonus.SpecBuff.ComponentOf(type));

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
			if(type.IsStat)
			{
				throw new ArgumentException("StatCalculators are superseded by StatProperty and NPCStatProperty.");
			}
			return legacyCalculators[(int)type.ID];
		}

		public bool AllCalculatorsLoaded { get { return loadedAllLegacyCalculators; } }
	}
}
