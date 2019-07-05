namespace DOL.GS.PropertyCalc
{
	[PropertyCalculator(eProperty.Fatigue)]
	public class FatigueCalculator : PropertyCalculator
	{
		public override int CalcValue(GameLiving living, eProperty property)
		{
			var bonusProperty = EndurancePoolFactory.Create(living);
			return bonusProperty.Value;
		}
	}
}
