using System;
using System.Collections.Generic;

namespace DOL.GS
{
	public class PropertyCaps
	{
		private GamePlayer owner;
		private IPropertyCap[] caps = new IPropertyCap[(int)eCapCategory.__Last + 1];
		private static List<eProperty> stats = new List<eProperty>()
		{
			eProperty.Constitution,
			eProperty.Strength,
			eProperty.Dexterity,
			eProperty.Quickness,
		};
		private static List<eProperty> acuityStats = new List<eProperty>()
		{
			eProperty.Empathy,
			eProperty.Charisma,
			eProperty.Piety,
			eProperty.Intelligence,
			eProperty.Acuity
		};
		private static List<eProperty> resists = new List<eProperty>()
		{
			eProperty.Resist_Body,
			eProperty.Resist_Cold,
			eProperty.Resist_Energy,
			eProperty.Resist_Heat,
			eProperty.Resist_Matter,
			eProperty.Resist_Spirit,
			eProperty.Resist_Slash,
			eProperty.Resist_Crush,
			eProperty.Resist_Thrust,
		};

		public PropertyCaps(GamePlayer owner)
		{
			this.owner = owner;
			bool isListCaster = owner.CharacterClass.ClassType == eClassType.ListCaster;

			caps[(int)eCapCategory.Unknown] = new NullCap();
			caps[(int)eCapCategory.Stat] = new StatCap(owner);
			if (isListCaster)
			{
				caps[(int)eCapCategory.Acuity] = new ListCasterAcuityCap(owner);
			}
			else
			{
				caps[(int)eCapCategory.Acuity] = new AcuityCap(owner);
			}
			caps[(int)eCapCategory.Resist] = new ResistCap(owner);
		}

		public IPropertyCap Of(BonusType bonusType)
		{
			var capCategory = CapCategoryOf(bonusType.ID);
			return caps[(int)capCategory];
		}

		public int ValueOf(BonusComponent component)
		{
			return Of(component.Type).For(component.Category);
		}

		private eCapCategory CapCategoryOf(eProperty property)
		{
			var manaStat = (eProperty)owner.CharacterClass.ManaStat;
			bool isAcuityManastat = (manaStat == property || property == eProperty.Acuity) && acuityStats.Contains(manaStat);
			if (stats.Contains(property))
			{
				return eCapCategory.Stat;
			}
			else if(isAcuityManastat)
			{
				return eCapCategory.Acuity;
			}
			else if(resists.Contains(property))
			{
				return eCapCategory.Resist;
			}
			else
			{
				return eCapCategory.Unknown;
			}
		}
	}

	public interface IPropertyCap
	{
		int Base { get; }
		int Ability { get; }
		int Item { get; }
		int ItemOvercap { get; }
		int Mythical { get; }
		int Buff { get; }
		int BaseBuff { get; }
		int SpecBuff { get; }
		int ExtraBuff { get; }
		int HardCap { get; }

		int For(BonusCategory category);
	}

	public class DefaultBonusCap : IPropertyCap
	{
		protected GameLiving owner;
		private static int defaultCap = int.MaxValue;

		public DefaultBonusCap(GameLiving owner)
		{
			this.owner = owner;
		}

		public virtual int Base { get; } = defaultCap;
		public virtual int Ability { get; } = defaultCap;
		public virtual int Item { get; } = defaultCap;
		public virtual int ItemOvercap { get; } = 0;
		public virtual int Mythical { get; } = 0;
		public virtual int Buff { get; } = defaultCap;
		public virtual int BaseBuff { get; } = 0;
		public virtual int SpecBuff { get; } = 0;
		public virtual int ExtraBuff { get; } = defaultCap;
		public virtual int HardCap { get; } = defaultCap;

		public int For(BonusCategory category)
		{
			switch (category.ID)
			{
				case ePropertyCategory.Base:
					return Base;
				case ePropertyCategory.Ability:
					return Ability;
				case ePropertyCategory.Item:
					return Item;
				case ePropertyCategory.ItemOvercap:
					return ItemOvercap;
				case ePropertyCategory.Mythical:
					return Mythical;
				case ePropertyCategory.BaseBuff:
					return BaseBuff;
				case ePropertyCategory.SpecBuff:
					return SpecBuff;
				case ePropertyCategory.ExtraBuff:
					return ExtraBuff;
				default:
					return int.MaxValue;
			}
		}
	}

	public class StatCap : DefaultBonusCap
	{
		public StatCap(GameLiving owner) : base(owner) { }

		public override int Item { get { return (int)(1.5 * owner.Level); } }
		public override int ItemOvercap { get { return (int)(0.5 * owner.Level + 1); } }
		public override int Mythical { get { return 52; } }
		public override int BaseBuff { get { return (int)(1.25 * owner.Level); } }
		public override int SpecBuff { get { return (int)(1.25 * 1.5 * owner.Level); } }
	}

	public class AcuityCap : StatCap
	{
		public AcuityCap(GameLiving owner) : base(owner) { }
		
		public override int BaseBuff { get { return 0; } }
		public override int SpecBuff { get { return 0; } }
	}

	public class ListCasterAcuityCap : AcuityCap
	{
		public ListCasterAcuityCap(GameLiving owner) : base(owner) { }

		public override int BaseBuff { get { return (int)(1.25 * 1.5 * owner.Level); } }
		public override int SpecBuff { get { return (int)(1.25 * 1.5 * owner.Level); } }
	}

	public class ResistCap : DefaultBonusCap
	{
		public ResistCap(GameLiving owner) : base(owner) { }

		public override int Item { get { return (int)(0.5 * owner.Level + 1); } }
		public override int Mythical { get { return 5; } }
		public override int Buff { get { return 24; } }
		public override int BaseBuff { get { return int.MaxValue; } }
		public override int HardCap { get { return 70; } }
	}

	public class NullCap : IPropertyCap
	{
		public virtual int Base { get; } = 0;
		public virtual int Ability { get; } = 0;
		public virtual int Item { get; } = 0;
		public virtual int ItemOvercap { get; } = 0;
		public virtual int Mythical { get; } = 0;
		public virtual int Buff { get; } = 0;
		public virtual int BaseBuff { get; } = 0;
		public virtual int SpecBuff { get; } = 0;
		public virtual int ExtraBuff { get; } = 0;
		public virtual int HardCap { get; } = 0;

		public int For(BonusCategory category)
		{
			return 0;
		}
	}

	public enum eCapCategory
	{
		Unknown,
		Stat,
		Acuity,
		Resist,
		__Last = Resist
	}
}
