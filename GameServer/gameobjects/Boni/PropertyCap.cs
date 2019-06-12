using System;

namespace DOL.GS
{
	public class PropertyCapFactory
	{
		private GamePlayer owner;

		public PropertyCapFactory(GamePlayer owner)
		{
			this.owner = owner;
		}

		public IPropertyCap Create(BonusType type)
		{
			if (type.IsBaseStat) { return new StatCap(owner); }
			if(type.IsAcuityStat)
			{
				bool isListCaster = owner.CharacterClass.ClassType == eClassType.ListCaster;
				if(isListCaster)
				{
					return new ListCasterAcuityCap(owner);
				}
				else
				{
					return new AcuityCap(owner);
				}
			}
			if(type.ID == eProperty.Resist_Natural) { return new EssenceResistCap(owner); }
			if(type.IsResist) { return new ResistCap(owner); }
			if (type.ID == eProperty.MeleeDamage || type.ID == eProperty.ArcherySpeed)
			{
				return new MeleeDamageCap(owner);
			}
			if (type.ID == eProperty.MeleeSpeed) { return new MeleeSpeedCap(owner); }
			if (type.ID == eProperty.SpellRange) { return new SpellRangeCap(owner); }
			if (type.ID == eProperty.ArcheryRange) { return new ArcheryRangeCap(owner); }
			if (type.ID == eProperty.MissHit) { return new MissHit(owner); }
			if (type.ID == eProperty.ArcaneSyphon) { return new ArcaneSyphonCap(owner); }
			if (type.ID == eProperty.ArmorFactor) { return new ArmorFactorCap(owner); }
			if (type.ID == eProperty.ArmorAbsorption) { return new ArmorAbsorptionCap(owner); }
			if (type.IsRegen) { return new DefaultPropertyCap(owner); }

			throw new ArgumentException("There is no PropertyCap for " + type.ID);
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
		int Maximum { get; }
		int Minimum { get; }

		int For(BonusCategory category);
	}

	public class DefaultPropertyCap : IPropertyCap
	{
		protected GameLiving owner;

		protected int Uncapped => int.MaxValue;

		public DefaultPropertyCap(GameLiving owner)
		{
			this.owner = owner;
		}

		public virtual int Base => Uncapped;
		public virtual int Ability => Uncapped;
		public virtual int Item => Uncapped;
		public virtual int ItemOvercap => 0;
		public virtual int Mythical => 0;
		public virtual int Buff => Uncapped;
		public virtual int BaseBuff => Uncapped;
		public virtual int SpecBuff => Uncapped;
		public virtual int ExtraBuff => Uncapped;
		public virtual int Debuff => Uncapped;
		public virtual int Maximum => Uncapped;
		public virtual int Minimum => -Uncapped;

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

	public class StatCap : DefaultPropertyCap
	{
		public StatCap(GameLiving owner) : base(owner) { }

		public override int Item => (int)(1.5 * owner.Level);
		public override int ItemOvercap => (int)(0.5 * owner.Level + 1);
		public override int Mythical => 52;
		public override int BaseBuff => (int)(1.25 * owner.Level);
		public override int SpecBuff => (int)(1.25 * 1.5 * owner.Level);

		public override int Minimum => 1;
	}

	public class AcuityCap : StatCap
	{
		public AcuityCap(GameLiving owner) : base(owner) { }
		
		public override int BaseBuff => 0;
		public override int SpecBuff => 0;
	}

	public class ListCasterAcuityCap : AcuityCap
	{
		public ListCasterAcuityCap(GameLiving owner) : base(owner) { }

		public override int BaseBuff => (int)(1.25 * 1.5 * owner.Level);
		public override int SpecBuff => (int)(1.25 * 1.5 * owner.Level);
	}

	public class ResistCap : DefaultPropertyCap
	{
		public ResistCap(GameLiving owner) : base(owner) { }

		public override int Item => (int)(0.5 * owner.Level + 1);
		public override int Mythical => 5;
		public override int Buff => 24;
		public override int Maximum => 70; 
	}

	public class EssenceResistCap : DefaultPropertyCap
	{
		public EssenceResistCap(GameLiving owner) : base(owner) { }

		public override int Item => (int)(0.5 * owner.Level + 1);
		public override int BaseBuff => 25;
		public override int SpecBuff => 0;
		public override int ExtraBuff => 0;
	}

	public class MeleeDamageCap : DefaultPropertyCap
	{
		public MeleeDamageCap(GameLiving owner) : base(owner) { }
		
		public override int Item => 10;
		public override int BaseBuff => Uncapped;
		public override int SpecBuff => Uncapped;
		public override int ExtraBuff => 0;
		public override int Debuff => 10;
	}

	public class SpellRangeCap : DefaultPropertyCap
	{
		public SpellRangeCap(GameLiving owner) : base(owner) { }

		public override int Item => 10;
		public override int BaseBuff => 0;
		public override int SpecBuff => 5;

		public override int Minimum => -100;
	}

	public class ArcheryRangeCap : DefaultPropertyCap
	{
		public ArcheryRangeCap(GamePlayer owner) : base(owner) { }

		public override int Item => 10;
		public override int BaseBuff => 0;
		public override int SpecBuff => 0;

		public override int Minimum => -100;
	}

	public class MeleeSpeedCap : DefaultPropertyCap
	{
		public MeleeSpeedCap(GameLiving owner) : base(owner) { }

		public override int Item => 10;
		public override int BaseBuff => Uncapped;
		public override int SpecBuff => 0;
		public override int ExtraBuff => 0;

		public override int Maximum => 99;
	}

	public class ArcaneSyphonCap : DefaultPropertyCap
	{
		public ArcaneSyphonCap(GameLiving owner) : base(owner) { }

		public override int Item => 25;
	}

	public class ArmorFactorCap : DefaultPropertyCap
	{
		public ArmorFactorCap(GameLiving owner) : base(owner) { }

		public override int Item => owner.Level;
		public override int SpecBuff => (int)(owner.Level * 1.875);
	}

	public class ArmorAbsorptionCap :DefaultPropertyCap
	{
		public ArmorAbsorptionCap(GameLiving owner) : base(owner) { }

		public override int Maximum => 50;
	}

	public class MissHit : DefaultPropertyCap
	{
		public MissHit(GameLiving owner) : base(owner) { }
	}
}
