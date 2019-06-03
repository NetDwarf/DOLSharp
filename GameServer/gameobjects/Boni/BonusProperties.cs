using DOL.GS.Effects;
using DOL.GS.Keeps;
using DOL.GS.Spells;
using System;

namespace DOL.GS
{
	public class BonusProperties
	{
		private BonusPropertyFactory factory;
		
		public BonusProperties(GameLiving owner)
		{
			this.factory = new BonusPropertyFactory(owner);
		}

		public int ValueOf(BonusType type)
		{
			var bonusProperty = factory.CreatePropertyOf(type);
			return bonusProperty.Value;
		}

		public int BuffValueOf(BonusType type)
		{
			var bonusProperty = factory.CreatePropertyOf(type);
			return bonusProperty.BuffValue;
		}

		public int ItemValueOf(BonusType type)
		{
			var bonusProperty = factory.CreatePropertyOf(type);
			return bonusProperty.ItemValue;
		}
	}

	public class BonusPropertyFactory
	{
		private GameLiving owner;

		public BonusPropertyFactory(GameLiving owner)
		{
			this.owner = owner;
		}

		public IBonusProperty CreatePropertyOf(BonusType type)
		{
			if (owner is GamePlayer)
			{
				var player = owner as GamePlayer;

				//generalized Properties
				bool ownerIsArcher = player.CharacterClass.ID == (int)eCharacterClass.Scout || player.CharacterClass.ID == (int)eCharacterClass.Hunter || player.CharacterClass.ID == (int)eCharacterClass.Ranger;
				bool typeIsManaStat = type.ID == (eProperty)player.CharacterClass.ManaStat;
				if (typeIsManaStat && !ownerIsArcher)
				{
					return new PlayerBonusProperty(player, type, Bonus.Stat.Acuity);
				}

				return new PlayerBonusProperty(player, type);
			}
			else if(owner is GameKeepDoor || owner is GameKeepComponent)
			{
				if(type.ID != eProperty.ArmorFactor)
				{
					throw new ArgumentException("KeepComponent has only AF Property.");
				}
				return new KeepComponentArmorFactorProperty(owner);
			}
			else
			{
				return new NPCBonusProperty(owner, type);
			}
		}
	}

	public interface IBonusProperty
	{
		int Value { get; }
		int BuffValue { get; }
		int ItemValue { get; }
	}

	public class PlayerBonusProperty : IBonusProperty
	{
		private GamePlayer owner;
		private BonusType[] affectingTypes;

		public PlayerBonusProperty(GamePlayer owner, params BonusType[] affectingTypes)
		{
			this.owner = owner;
			this.affectingTypes = affectingTypes;
		}

		private BonusType PrimaryType { get { return affectingTypes[0]; } }

		public virtual int Value
		{
			get
			{
				int debuff = Math.Abs(CappedValueOf(Bonus.Debuff));
				int abilityBonus = GetRawValueOf(Bonus.Ability);
				int baseBonus = GetRawValueOf(Bonus.Base);
				if (PrimaryType.IsResist)
				{
					baseBonus = owner.LivingRace.GetResist((eResist)PrimaryType.ID);
				}

				int itemBonus = ItemValue;
				int buffBonus = BuffValue;

				bool isRangeBonusType = PrimaryType.ID == eProperty.ArcheryRange || PrimaryType.ID == eProperty.SpellRange;
				if (isRangeBonusType || debuff > 0)
				{
					GameSpellEffect nsreduction = SpellHandler.FindEffectOnTarget(owner, "NearsightReduction");
					if (nsreduction != null) debuff = (int)(debuff * (1.00 - nsreduction.Spell.Value * 0.01));
				}

				if (PrimaryType.ID == eProperty.ArcheryRange && owner.RangedAttackType == GameLiving.eRangedAttackType.Long)
				{
					abilityBonus += 50;
					IGameEffect effect = owner.EffectList.GetOfType<TrueshotEffect>();
					if (effect != null)
						effect.Cancel(false);
				}

				// Stats and resist debuffs have 100% Effectiveness for player buffs, but only 50%
				// effectiveness for item bonuses and baseBonus(?).

				int unbuffedBonus = baseBonus + itemBonus;
				buffBonus -= debuff;
				if ((PrimaryType.IsStat || PrimaryType.IsResist) && buffBonus < 0)
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

				effectiveBonus = Math.Max(Cap.Minimum, effectiveBonus);
				return Math.Min(effectiveBonus, Cap.Maximum);
			}
		}

		public int BuffValue
		{
			get
			{
				int baseBuffBonus = CappedValueOf(Bonus.BaseBuff);
				int specBuffBonus = CappedValueOf(Bonus.SpecBuff);
				int extraBuffBonus = CappedValueOf(Bonus.ExtraBuff);
				int buffBonus = baseBuffBonus + specBuffBonus + extraBuffBonus;
				return Math.Min(buffBonus, Cap.Buff);
			}
		}

		public int ItemValue
		{
			get
			{
				int rawItemBonus = GetRawValueOf(Bonus.Item);
				int overcap = CappedValueOf(Bonus.ItemOvercap);
				int rawMythicalCapIncrease = GetRawValueOf(Bonus.Mythical);

				int mythicalCapIncreaseCap = Cap.Mythical;
				int baseCap = Cap.Item;
				int capIncrease = Math.Min(mythicalCapIncreaseCap, overcap + rawMythicalCapIncrease);
				
				int cap = baseCap + capIncrease;
				return Math.Min(rawItemBonus, cap);
			}
		}

		private int CappedValueOf(BonusCategory category)
		{
			int rawValue = GetRawValueOf(category);
			int cap = Cap.For(category);
			return Math.Min(cap, rawValue);
		}

		private IPropertyCap Cap
		{
			get
			{
				var propertyCapFactory = new PropertyCapFactory(owner);
				var cap = propertyCapFactory.Create(PrimaryType);
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
				if(type.ID == eProperty.ArmorFactor)
				{
					baseBonus += (int)((1 + owner.Level / 170.0) * owner.Level * 9.34);
				}
				if(type.ID == eProperty.ArmorAbsorption)
				{
					var living = owner;
					int abs = 0;
					if (living.Level >= 30) abs = 27;
					else if (living.Level >= 20) abs = 19;
					else if (living.Level >= 10) abs = 10;

					abs += (living.GetModified(eProperty.Constitution)
						+ living.GetModified(eProperty.Dexterity) - 120) / 12;
					baseBonus += abs;
				}
				int abilityBonus = boni.RawValueOf(Bonus.Ability.ComponentOf(type));
				int debuff = boni.RawValueOf(Bonus.Debuff.ComponentOf(type));

				int itemBonus = ItemValue;
				int buffBonus = BuffValue;

				// Apply debuffs, 100% effectiveness for player buffs, 50% effectiveness
				// for item and base stats

				int unbuffedBonus = baseBonus + itemBonus;
				buffBonus -= Math.Abs(debuff);

				if ((type.IsStat || type.IsResist) && buffBonus < 0)
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
				if(type.ID == eProperty.ArmorFactor)
				{
					baseBuffBonus = 0;
				}
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

	public class KeepComponentArmorFactorProperty : IBonusProperty
	{
		private GameLiving owner;

		public KeepComponentArmorFactorProperty(GameLiving owner)
		{
			this.owner = owner;
		}
		public int Value
		{
			get
			{
				GameKeepComponent component = null;
				if (owner is GameKeepDoor)
					component = (owner as GameKeepDoor).Component;
				if (owner is GameKeepComponent)
					component = owner as GameKeepComponent;

				int amount = component.AbstractKeep.BaseLevel;
				if (component.Keep is GameKeep)
					return amount;
				else return amount / 2;
			}
		}

		public int BuffValue => 0;

		public int ItemValue => 0;
	}
}
