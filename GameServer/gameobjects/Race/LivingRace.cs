namespace DOL.GS
{
	public class LivingRace
	{
		public short ID { get; set; } = 0;

		public virtual int GetResist(eResist resistID)
		{
			return SkillBase.GetRaceResist(ID, resistID);
		}
	}
}
