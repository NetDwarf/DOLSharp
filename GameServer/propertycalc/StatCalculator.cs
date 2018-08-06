/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System;

namespace DOL.GS.PropertyCalc
{
    [PropertyCalculator(eProperty.Stat_First, eProperty.Stat_Last)]
	public class StatCalculator : PropertyCalculator
    {
        public class StatBonus
        {
            private GameLiving living;
            private eProperty propertyID;

            public StatBonus(GameLiving living, eProperty propertyID)
            {
                this.living = living;
                this.propertyID = propertyID;
            }
            private int BaseBuff
            {
                get
                {
                    int baseBonusValue = living.BaseBuffBonusCategory[propertyID];
                    int baseBuffBonusCap = (living is GamePlayer) ? (int)(living.Level * 1.25) : Int16.MaxValue;
                    baseBonusValue = Math.Min(baseBonusValue, baseBuffBonusCap);
                    return baseBonusValue;
                }
            }
            private int SpecBuff
            {
                get
                {
                    int specBonusValue = living.SpecBuffBonusCategory[propertyID];
                    if (living is GamePlayer)
                    {
                        GamePlayer player = living as GamePlayer;
                        if (propertyID == (eProperty)(player.CharacterClass.ManaStat))
                            if (player.CharacterClass.ClassType == eClassType.ListCaster)
                                specBonusValue += player.BaseBuffBonusCategory[(int)eProperty.Acuity];
                    }
                    int specBuffBonusCap = (living is GamePlayer) ? (int)(living.Level * 1.5 * 1.25) : Int16.MaxValue;
                    specBonusValue = Math.Min(specBonusValue, specBuffBonusCap);
                    return specBonusValue;
                }
            }
            private int ItemBase
            {
                get
                {
                    int itemBonus = living.ItemBonus[propertyID];
                    if (living is GamePlayer)
                    {
                        GamePlayer player = living as GamePlayer;

                        if (propertyID == (eProperty)player.CharacterClass.ManaStat)
                        {
                            if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
                            {
                                itemBonus += living.ItemBonus[(int)eProperty.Acuity];
                            }
                        }
                    }
                    return itemBonus;
                }
            }
            private int ItemCapIncrease
            {
                get
                {
                    int itemBonusCapIncrease = living.ItemBonus[(int)(eProperty.StatCapBonus_First - eProperty.Stat_First + propertyID)];
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
                    int mythicalBonus = living.ItemBonus[(int)(eProperty.MythicalStatCapBonus_First - eProperty.Stat_First + propertyID)];
                    if (living is GamePlayer)
                    {
                        GamePlayer player = living as GamePlayer;

                        if (propertyID == (eProperty)player.CharacterClass.ManaStat)
                        {
                            if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
                            {
                                mythicalBonus += living.ItemBonus[(int)eProperty.MythicalAcuCapBonus];
                            }
                        }
                    }

                    return mythicalBonus;
                }
            }
            
            public int Base
            {
                get
                {
                    int baseStat = living.GetBaseStat((eStat)propertyID);
                    if (living is GamePlayer)
                    {
                        GamePlayer player = living as GamePlayer;

                        baseStat -= player.TotalConstitutionLostAtDeath;
                    }
                    return baseStat;
                }
            }
            public int Ability
            {
                get
                {
                    int abilityBonus = living.AbilityBonus[propertyID];
                    if (living is GamePlayer)
                    {
                        GamePlayer player = living as GamePlayer;
                        if (propertyID == (eProperty)(player.CharacterClass.ManaStat))
                        {
                            if (player.CharacterClass.ID != (int)eCharacterClass.Scout && player.CharacterClass.ID != (int)eCharacterClass.Hunter && player.CharacterClass.ID != (int)eCharacterClass.Ranger)
                            {
                                abilityBonus += player.AbilityBonus[(int)eProperty.Acuity];
                            }
                        }
                    }
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

                    statBonus = (int)(statBonus * living.BuffBonusMultCategory1.Get((int)propertyID));
                    return Math.Max(1, statBonus);
                }
            }
        }

        public StatCalculator() { }

        public override int CalcValue(GameLiving living, eProperty property)
        {
            var statBonus = new StatBonus(living, property);
            return statBonus.Value;
        }

        /// <summary>
        /// Calculate modified bonuses from buffs only.
        /// </summary>
        public override int CalcValueFromBuffs(GameLiving living, eProperty property)
        {
            var statBonus = new StatBonus(living, property);

            return statBonus.Buff;
        }

        /// <summary>
        /// Calculate modified bonuses from items only.
        /// </summary>
        public override int CalcValueFromItems(GameLiving living, eProperty property)
        {
            var statBonus = new StatBonus(living, property);

            return statBonus.Item;
        }
	}
}
