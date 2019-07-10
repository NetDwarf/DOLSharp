using DOL.GS.PropertyCalc;

namespace DOL.GS
{
	public class BonusAggregator
	{
		public IPropertyIndexer AbilityBonus { get; } = new PropertyIndexer();
		public IPropertyIndexer ItemBonus { get; private set; } = new PropertyIndexer();
		public IPropertyIndexer BaseBuffBonusCategory { get; } = new PropertyIndexer();
		public IPropertyIndexer SpecBuffBonusCategory { get; } = new PropertyIndexer();
		public IPropertyIndexer BuffBonusCategory4 { get; } = new PropertyIndexer();
		public IPropertyIndexer DebuffCategory { get; } = new PropertyIndexer();
		public IPropertyIndexer SpecDebuffCategory { get; } = new PropertyIndexer();

		public void ClearItemBonuses()
		{
			ItemBonus = new PropertyIndexer();
		}
	}
}
