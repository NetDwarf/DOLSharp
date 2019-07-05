using System;

namespace DOL.GS
{
	public class PlayerEndurancePool : IBonusProperty
	{
		GamePlayer owner;

		public PlayerEndurancePool(GamePlayer owner)
		{
			this.owner = owner;
		}

		public int Value
		{
			get
			{
				var player = owner;
				int endurance = player.DBMaxEndurance;
				endurance += (int)(endurance * (Math.Min(15, owner.ItemBonus[eProperty.Fatigue]) * .01));
				return endurance;
			}
		}
	}

	public class DefaultEndurancePool : IBonusProperty
	{
		public int Value => 100;
	}

	public class EndurancePoolFactory
	{
		public static IBonusProperty Create(GameLiving owner)
		{
			if(owner is GamePlayer)
			{
				return new PlayerEndurancePool(owner as GamePlayer);
			}
			else
			{
				return new DefaultEndurancePool();
			}
		}
	}
}
