using System;

namespace DOL.GS
{
	public class BonusCapFactory
	{
		private GamePlayer owner;

		public BonusCapFactory(GamePlayer owner)
		{
			this.owner = owner;
		}

		public IBonusCap Create(BonusType type)
		{
			if (type.IsBaseStat) { return new StatCap(owner); }
			if (type.IsAcuityStat)
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
			if (type.Equals(new BonusType(eBonusType.Resist_Natural))) { return new EssenceResistCap(owner); }
			if (type.IsResist) { return new ResistCap(owner); }
			if (type.Equals(new BonusType(eBonusType.MeleeDamage)) || type.Equals(new BonusType(eBonusType.ArcherySpeed)))
			{
				return new MeleeDamageCap(owner);
			}
			if (type.Equals(new BonusType(eBonusType.MeleeSpeed))) { return new MeleeSpeedCap(owner); }
			if (type.Equals(new BonusType(eBonusType.SpellRange))) { return new SpellRangeCap(owner); }
			if (type.Equals(new BonusType(eBonusType.ArcheryRange))) { return new ArcheryRangeCap(owner); }
			if (type.Equals(new BonusType(eBonusType.MissHit))) { return new MissHit(owner); }
			if (type.Equals(new BonusType(eBonusType.ArcaneSyphon))) { return new ArcaneSyphonCap(owner); }
			if (type.Equals(new BonusType(eBonusType.ArmorFactor))) { return new ArmorFactorCap(owner); }
			if (type.Equals(new BonusType(eBonusType.ArmorAbsorption))) { return new ArmorAbsorptionCap(owner); }
			if (type.IsRegen) { return new RegenCap(owner); }

			throw new ArgumentException("There is no PropertyCap for " + type.ID);
		}
	}

	public interface IBonusCap
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

		int For(BonusPart category);
	}

	public class DefaultBonusCap : IBonusCap
	{
		protected GameLiving owner;

		protected int Uncapped => int.MaxValue;

		public DefaultBonusCap(GameLiving owner)
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

		public int For(BonusPart category)
		{
			switch (category.ID)
			{
				case eBonusPart.Base:
					return Base;
				case eBonusPart.Ability:
					return Ability;
				case eBonusPart.Item:
					return Item;
				case eBonusPart.ItemOvercap:
					return ItemOvercap;
				case eBonusPart.Mythical:
					return Mythical;
				case eBonusPart.BaseBuff:
					return BaseBuff;
				case eBonusPart.SpecBuff:
					return SpecBuff;
				case eBonusPart.ExtraBuff:
					return ExtraBuff;
				case eBonusPart.Debuff:
					return Debuff;
				default:
					return int.MaxValue;
			}
		}
	}

	public class StatCap : DefaultBonusCap
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

	public class ResistCap : DefaultBonusCap
	{
		public ResistCap(GameLiving owner) : base(owner) { }

		public override int Item => (int)(0.5 * owner.Level + 1);
		public override int Mythical => 5;
		public override int Buff => 24;
		public override int Maximum => 70; 
	}

	public class EssenceResistCap : DefaultBonusCap
	{
		public EssenceResistCap(GameLiving owner) : base(owner) { }

		public override int Item => (int)(0.5 * owner.Level + 1);
		public override int BaseBuff => 25;
		public override int SpecBuff => 0;
		public override int ExtraBuff => 0;
	}

	public class MeleeDamageCap : DefaultBonusCap
	{
		public MeleeDamageCap(GameLiving owner) : base(owner) { }
		
		public override int Item => 10;
		public override int BaseBuff => Uncapped;
		public override int SpecBuff => Uncapped;
		public override int ExtraBuff => 0;
		public override int Debuff => 10;
	}

	public class SpellRangeCap : DefaultBonusCap
	{
		public SpellRangeCap(GameLiving owner) : base(owner) { }

		public override int Item => 10;
		public override int BaseBuff => 0;
		public override int SpecBuff => 5;

		public override int Minimum => -100;
	}

	public class ArcheryRangeCap : DefaultBonusCap
	{
		public ArcheryRangeCap(GamePlayer owner) : base(owner) { }

		public override int Item => 10;
		public override int BaseBuff => 0;
		public override int SpecBuff => 0;

		public override int Minimum => -100;
	}

	public class MeleeSpeedCap : DefaultBonusCap
	{
		public MeleeSpeedCap(GameLiving owner) : base(owner) { }

		public override int Item => 10;
		public override int BaseBuff => Uncapped;
		public override int SpecBuff => 0;
		public override int ExtraBuff => 0;

		public override int Maximum => 99;
	}

	public class ArcaneSyphonCap : DefaultBonusCap
	{
		public ArcaneSyphonCap(GameLiving owner) : base(owner) { }

		public override int Item => 25;
	}

	public class ArmorFactorCap : DefaultBonusCap
	{
		public ArmorFactorCap(GameLiving owner) : base(owner) { }

		public override int Item => owner.Level;
		public override int SpecBuff => (int)(owner.Level * 1.875);
	}

	public class ArmorAbsorptionCap :DefaultBonusCap
	{
		public ArmorAbsorptionCap(GameLiving owner) : base(owner) { }

		public override int Maximum => 50;
	}

	public class MissHit : DefaultBonusCap
	{
		public MissHit(GameLiving owner) : base(owner) { }
	}

	public class RegenCap : DefaultBonusCap
	{
		public RegenCap(GameLiving owner) : base(owner) { }

		public override int SpecBuff => 0;
	}
}
