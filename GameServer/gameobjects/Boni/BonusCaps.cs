namespace DOL.GS
{
	public class PropertyCaps
	{
		private GamePlayer owner;
		private IPropertyCap[] caps = new IPropertyCap[(int)eCapCategory.__Last];

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
			caps[(int)eCapCategory.EssenceResist] = new EssenceResistCap(owner);
			caps[(int)eCapCategory.MeleeDamage] = new MeleeDamageCap(owner);
			caps[(int)eCapCategory.MeleeSpeed] = new MeleeSpeedCap(owner);
		}

		public IPropertyCap Of(BonusType bonusType)
		{
			var capCategory = CapCategoryOf(bonusType);
			return caps[(int)capCategory];
		}

		public int ValueOf(BonusComponent component)
		{
			return Of(component.Type).For(component.Category);
		}

		private eCapCategory CapCategoryOf(BonusType type)
		{
			if (type.IsBaseStat)
			{
				return eCapCategory.Stat;
			}
			else if(type.IsAcuityStat)
			{
				return eCapCategory.Acuity;
			}
			else if(type.ID == eProperty.Resist_Natural)
			{
				return eCapCategory.EssenceResist;
			}
			else if(type.IsResist)
			{
				return eCapCategory.Resist;
			}
			else if(type.ID == eProperty.MeleeDamage)
			{
				return eCapCategory.MeleeDamage;
			}
			else if (type.ID == eProperty.MeleeSpeed)
			{
				return eCapCategory.MeleeSpeed;
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
		private static int uncapped = int.MaxValue;

		protected int Uncapped { get { return uncapped; } }

		public DefaultBonusCap(GameLiving owner)
		{
			this.owner = owner;
		}

		public virtual int Base { get; } = uncapped;
		public virtual int Ability { get; } = uncapped;
		public virtual int Item { get; } = uncapped;
		public virtual int ItemOvercap { get; } = 0;
		public virtual int Mythical { get; } = 0;
		public virtual int Buff { get; } = uncapped;
		public virtual int BaseBuff { get; } = 0;
		public virtual int SpecBuff { get; } = 0;
		public virtual int ExtraBuff { get; } = uncapped;
		public virtual int Debuff { get; } = uncapped;
		public virtual int HardCap { get; } = uncapped;

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
				case ePropertyCategory.Debuff:
					return Debuff;
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

	public class EssenceResistCap : DefaultBonusCap
	{
		public EssenceResistCap(GameLiving owner) : base(owner) { }

		public override int Item { get { return (int)(0.5 * owner.Level + 1); } }
		public override int BaseBuff { get { return 25; } }
		public override int SpecBuff { get { return 0; } }
		public override int ExtraBuff { get { return 0; } }
	}

	public class MeleeDamageCap : DefaultBonusCap
	{
		public MeleeDamageCap(GameLiving owner) : base(owner) { }
		
		public override int Item { get { return 10; } }
		public override int BaseBuff { get { return Uncapped; } }
		public override int SpecBuff { get { return Uncapped; } }
		public override int ExtraBuff { get { return 0; } }
		public override int Debuff { get { return 10; } }
	}

	public class MeleeSpeedCap : DefaultBonusCap
	{
		public MeleeSpeedCap(GameLiving owner) : base(owner) { }

		public override int Item { get { return 10; } }
		public override int BaseBuff { get { return Uncapped; } }
		public override int SpecBuff { get { return 0; } }
		public override int ExtraBuff { get { return 0; } }
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
		EssenceResist,
		MeleeDamage,
		MeleeSpeed,
		__Last,
	}
}
