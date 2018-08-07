using System;

namespace DOL.GS.Trait
{
    public class StatProperty
    {
        private GameLiving living;
        private eProperty propertyID;
        private StatBonus bonusData;

        public StatProperty(GameLiving living, eProperty propertyID)
        {
            this.living = living;
            this.propertyID = propertyID;
            if (living is GamePlayer)
            {
                var player = living as GamePlayer;
                if (propertyID == (eProperty)(player.CharacterClass.ManaStat))
                {
                    if (player.CharacterClass.ClassType == eClassType.ListCaster)
                    {
                        this.bonusData = new ListCasterAcuityBonus(living, propertyID);
                        return;
                    }
                    else if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
                    {
                        this.bonusData = new CasterAcuityBonus(living, propertyID);
                        return;
                    }
                }
            }
            this.bonusData = new StatBonus(living, propertyID);
        }
        private int BaseBuff
        {
            get
            {
                int baseBonusValue = bonusData.FromBaseBuff;
                int baseBuffBonusCap = (living is GamePlayer) ? (int)(living.Level * 1.25) : Int16.MaxValue;
                baseBonusValue = Math.Min(baseBonusValue, baseBuffBonusCap);
                return baseBonusValue;
            }
        }
        private int SpecBuff
        {
            get
            {
                int specBonusValue = bonusData.FromSpecBuff;
                int specBuffBonusCap = (living is GamePlayer) ? (int)(living.Level * 1.5 * 1.25) : Int16.MaxValue;
                specBonusValue = Math.Min(specBonusValue, specBuffBonusCap);
                return specBonusValue;
            }
        }
        private int ItemBase
        {
            get
            {
                int itemBonus = bonusData.FromItems;
                return itemBonus;
            }
        }
        private int ItemCapIncrease
        {
            get
            {
                int itemBonusCapIncrease = bonusData.ItemCapIncrease;
                int itemBonusCapIncreaseCap = living.Level / 2 + 1;
                if (living is GamePlayer)
                {
                    GamePlayer player = living as GamePlayer;

                    if (propertyID == (eProperty)player.CharacterClass.ManaStat)
                    {
                        if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
                        {
                            itemBonusCapIncrease += living.ItemBonus[(int)eProperty.AcuCapBonus];
                        }
                    }
                }

                return Math.Min(itemBonusCapIncrease, itemBonusCapIncreaseCap);
            }
        }
        private int ItemMythicalCapIncrease
        {
            get
            {
                int mythicalBonus = bonusData.ItemMythicalBonus;
                return mythicalBonus;
            }
        }

        public int Base
        {
            get
            {
                int baseStat = bonusData.FromBase;
                return baseStat;
            }
        }
        public int Ability
        {
            get
            {
                int abilityBonus = bonusData.FromAbility;
                return abilityBonus;
            }
        }
        public int Item
        {
            get
            {
                if (living == null)
                    return 0;

                int itemBonusCap = (int)(living.Level * 1.5);

                int capIncreaseCap = 52;
                int allCapIncrease = Math.Min(capIncreaseCap, ItemMythicalCapIncrease + ItemCapIncrease);
                return Math.Min(ItemBase, itemBonusCap + allCapIncrease);
            }
        }
        public int Buff
        {
            get
            {
                if (living == null)
                    return 0;

                return BaseBuff + SpecBuff;
            }
        }
        public int Debuff
        {
            get
            {
                int debuffValue = living.DebuffCategory[propertyID];
                int unbuffedBonus = Base + Item;
                int buffBonus = Buff - Math.Abs(debuffValue);

                if (buffBonus < 0)
                {
                    unbuffedBonus += buffBonus / 2;
                    buffBonus = 0;
                    if (unbuffedBonus < 0)
                        unbuffedBonus = 0;
                }
                return Base + Item + Buff - (unbuffedBonus + buffBonus);
            }
        }

        public int Value
        {
            get
            {
                int statBonus = Base + Ability + Item + Buff - Debuff;

                if (living is GamePlayer && propertyID == eProperty.Constitution)
                {
                    GamePlayer player = living as GamePlayer;
                    statBonus -= player.TotalConstitutionLostAtDeath;
                }
                statBonus = (int)(statBonus * living.BuffBonusMultCategory1.Get((int)propertyID));
                return Math.Max(1, statBonus);
            }
        }
    }
}
