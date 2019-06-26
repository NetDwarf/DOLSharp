using System;

using DOL.GS.Keeps;

namespace DOL.GS.PropertyCalc
{
	/// <summary>
	/// The Max HP calculator
	///
	/// BuffBonusCategory1 is used for absolute HP buffs
	/// BuffBonusCategory2 unused
	/// BuffBonusCategory3 unused
	/// BuffBonusCategory4 unused
	/// BuffBonusMultCategory1 unused
	/// </summary>
	[PropertyCalculator(eProperty.MaxHealth)]
	public class MaxHealthCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property)
		{
			if (living is GamePlayer)
			{
				var maxHealth = new PlayerHealthPool(living as GamePlayer);
				return maxHealth.Value;
			}
			else if ( living is GameKeepComponent )
			{
				var maxHealth = new KeepComponentHealthPool(living as GameKeepComponent);
				return maxHealth.Value;
			}
			else if ( living is GameKeepDoor )
			{
				var maxHealth = new KeepDoorHealthPool(living as GameKeepDoor);
				return maxHealth.Value;
			}
			else if (living is GameNPC)
			{
				var maxHealth = new NPCHealthPool(living as GameNPC);
				return maxHealth.Value;
			}
            else
            {
				var maxHealth = new GenericLivingHealthPool(living);
				return maxHealth.Value;
			}
		}
	}
}
