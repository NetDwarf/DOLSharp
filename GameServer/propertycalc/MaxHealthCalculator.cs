using System;

using DOL.GS.Keeps;

namespace DOL.GS.PropertyCalc
{
	[PropertyCalculator(eProperty.MaxHealth)]
	public class MaxHealthCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property)
		{
			var healthPoolProperty = HealthPoolFactory.Create(living);
			return healthPoolProperty.Value;
		}
	}
}
