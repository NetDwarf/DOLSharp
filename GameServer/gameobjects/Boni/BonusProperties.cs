using DOL.GS.PropertyCalc;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace DOL.GS
{
	public class BonusProperties
	{
		private IPropertyCalculator[] propertyCalculators;
		private GameLiving owner;
		private PropertyCalculatorFactory propCalcFactory;
		
		public BonusProperties(GameLiving owner, IPropertyCalculator[] propertyCalculators)
		{
			this.propertyCalculators = propertyCalculators;
			this.owner = owner;
			this.propCalcFactory = new PropertyCalculatorFactory(owner);
		}

		public int ValueOf(BonusType type)
		{
			IPropertyCalculator propCalculator = propCalcFactory.Create(type);
			int result = propCalculator.CalcValue(owner, type.ID);
			if(owner is GamePlayer && type.Equals(Bonus.Stat.Constitution))
			{
				var player = owner as GamePlayer;
				result -= player.TotalConstitutionLostAtDeath;
			}
			return result;
		}
	}

	public class PropertyCalculatorFactory
	{
		private static readonly IPropertyCalculator[] m_propertyCalc = new IPropertyCalculator[(int)eProperty.MaxProperty + 1];
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
		private static readonly bool loadedAllCalculators;
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
									m_propertyCalc[i] = calc;
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
				loadedAllCalculators = true;
			}
			catch (Exception e)
			{
				if (log.IsErrorEnabled)
				{
					log.Error("GameLiving.LoadCalculators()", e);
				}
				loadedAllCalculators = false;
			}
		}

		public PropertyCalculatorFactory(GameLiving owner)
		{
			this.owner = owner;
		}

		public IPropertyCalculator Create(BonusType type)
		{
			var typeID = type.ID;
			return m_propertyCalc[(int)type.ID];
		}

		private bool OwnerIsArcher
		{
			get
			{
				if (owner is GamePlayer)
				{
					var player = owner as GamePlayer;
					var isScout = player.CharacterClass.ID == (int)eCharacterClass.Scout;
					var isHunter = player.CharacterClass.ID == (int)eCharacterClass.Hunter;
					var isRanger = player.CharacterClass.ID == (int)eCharacterClass.Ranger;
					return isScout || isHunter || isRanger;
				}
				return false;
			}
		}

		public bool AllCalculatorsLoaded { get { return loadedAllCalculators; } }
	}

	public enum eBonusProperty
	{
		Strength,
		Constitution,
		Dexterity,
		Quickness,
		Acuity,
	}
}
