namespace DOL.GS.Trait
{
    public class StatBonus
    {
        protected GameLiving living;
        protected eProperty propertyID;

        public StatBonus(GameLiving living, eProperty propertyID)
        {
            this.living = living;
            this.propertyID = propertyID;
        }

        public virtual int FromBase
        {
            get
            {
                return living.GetBaseStat((eStat)propertyID);
            }
        }
        public virtual int FromAbility
        {
            get
            {
                return living.AbilityBonus[propertyID];
            }
        }
        public virtual int FromBaseBuff
        {
            get
            {
                return living.BaseBuffBonusCategory[propertyID];
            }
        }
        public virtual int FromSpecBuff
        {
            get
            {
                return living.SpecBuffBonusCategory[propertyID];
            }
        }
        public virtual int FromItems
        {
            get
            {
                return living.ItemBonus[propertyID];
            }
        }
        public virtual int ItemCapIncrease
        {
            get
            {
                return living.ItemBonus[(eProperty.StatCapBonus_First - eProperty.Stat_First + propertyID)];
            }
        }
        public virtual int ItemMythicalBonus
        {
            get
            {
                return living.ItemBonus[(int)(eProperty.MythicalStatCapBonus_First - eProperty.Stat_First + propertyID)];
            }
        }
        public virtual int FromDebuff
        {
            get
            {
                return living.DebuffCategory[propertyID];
            }
        }
    }

    public class CasterAcuityBonus : StatBonus
    {
        public CasterAcuityBonus(GameLiving living, eProperty propertyID) : base(living, propertyID) { }

        public override int FromAbility
        {
            get
            {
                return living.AbilityBonus[propertyID] + living.AbilityBonus[eProperty.Acuity];
            }
        }
        public override int FromItems
        {
            get
            {
                return living.ItemBonus[propertyID] + living.ItemBonus[eProperty.Acuity];
            }
        }
        public override int ItemCapIncrease
        {
            get
            {
                return living.ItemBonus[(eProperty.StatCapBonus_First - eProperty.Stat_First + propertyID)] + living.ItemBonus[eProperty.AcuCapBonus];
            }
        }
        public override int ItemMythicalBonus
        {
            get
            {
                return living.ItemBonus[(int)(eProperty.MythicalStatCapBonus_First - eProperty.Stat_First + propertyID)] + living.ItemBonus[eProperty.MythicalAcuCapBonus];
            }
        }
        //acuity was not used for debuffs in StatCalculator
    }

    public class ListCasterAcuityBonus : CasterAcuityBonus
    {
        public ListCasterAcuityBonus(GameLiving living, eProperty propertyID) : base(living, propertyID) { }

        public override int FromSpecBuff
        {
            get
            {
                //In StatCalculator Acuity was handled as BaseBuffBonus but with SpecBuffCap
                return living.SpecBuffBonusCategory[propertyID] + living.BaseBuffBonusCategory[eProperty.Acuity];
            }
        }
    }
}
