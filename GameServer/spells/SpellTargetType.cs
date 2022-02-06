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

using DOL.Events;
using DOL.GS.Effects;
using DOL.GS.PacketHandler;

namespace DOL.GS.Spells
{
	internal class SpellTargetType
	{
		protected string targetType = "";

		protected SpellTargetType(string targetType)
		{
			this.targetType = targetType;
		}

		public static SpellTargetType Create(string targetType)
		{
			targetType = targetType.ToLower();
			if (targetType == "pet") return new PetTarget(targetType);
			if (targetType == "area") return new GroundTarget(targetType);
			if (targetType == "enemy") return new EnemyTarget(targetType);
			if (targetType == "realm") return new RealmTarget(targetType);
			if (targetType == "corpse") return new CorpseTarget(targetType);
			return new SpellTargetType(targetType);
		}

		protected bool IsTargetValid(SpellHandler spellHandler, GameLiving target, bool quiet)
		{
			if (target == null || target.ObjectState != GameLiving.eObjectState.Active)
			{
				if (!quiet) spellHandler.MessageToCaster("You must select a target for this spell!",
											eChatType.CT_SpellResisted);
				return false;
			}
			return true;
		}

		protected bool IsTargetInRange(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{
			if (!caster.IsWithinRadius(selectedTarget, spellHandler.CalculateSpellRange()))
			{
				if (caster is GamePlayer && !quiet)
				{
					spellHandler.MessageToCaster("That target is too far away!", eChatType.CT_SpellResisted);
				}
				caster.Notify(GameLivingEvent.CastFailed, new CastFailedEventArgs(spellHandler, CastFailedEventArgs.Reasons.TargetTooFarAway));
				return false;
			}
			return true;
		}

		protected bool IsTargetInView(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{
			if (selectedTarget == caster.TargetObject && !caster.TargetInView)
			{
				if (!quiet) spellHandler.MessageToCaster("Your target is not in visible!", eChatType.CT_SpellResisted);
				caster.Notify(GameLivingEvent.CastFailed, new CastFailedEventArgs(spellHandler, CastFailedEventArgs.Reasons.TargetNotInView));
				return false;
			}
			return true;
		}

		public virtual bool CheckBeginCast(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{
			if (targetType != "self" && targetType != "group" && targetType != "controlled"
				&& targetType != "cone" && spellHandler.Spell.Range > 0)
			{
				if (IsTargetValid(spellHandler, selectedTarget, quiet) == false) return false;
				if (IsTargetInRange(spellHandler, caster, selectedTarget, quiet) == false) return false;
				if (IsTargetInView(spellHandler, caster, selectedTarget, quiet) == false) return false;
			}

			return true;
		}

		public virtual bool CheckEndCast(SpellHandler spellHandler, GameLiving caster, GameLiving target)
		{
			var targetType = spellHandler.Spell.Target.ToLower();
			if (targetType != "self" && targetType != "group" && targetType != "cone" && spellHandler.Spell.Range > 0)
			{
				//all other spells that need a target
				if (target == null || target.ObjectState != GameObject.eObjectState.Active)
				{
					if (caster is GamePlayer)
						spellHandler.MessageToCaster("You must select a target for this spell!", eChatType.CT_SpellResisted);
					return false;
				}

				if (!caster.IsWithinRadius(target, spellHandler.CalculateSpellRange()))
				{
					if (caster is GamePlayer)
						spellHandler.MessageToCaster("That target is too far away!", eChatType.CT_SpellResisted);
					return false;
				}

				switch (targetType)
				{
					case "enemy":
						//enemys have to be in front and in view for targeted spells
						if (!caster.IsObjectInFront(target, 180))
						{
							spellHandler.MessageToCaster("Your target is not in view. The spell fails.", eChatType.CT_SpellResisted);
							return false;
						}

						if (!GameServer.ServerRules.IsAllowedToAttack((GameLiving)caster, target, false))
						{
							return false;
						}
						break;

					case "corpse":
						if (target.IsAlive || !GameServer.ServerRules.IsSameRealm((GameLiving)caster, target, true))
						{
							spellHandler.MessageToCaster("This spell only works on dead members of your realm!",
											eChatType.CT_SpellResisted);
							return false;
						}
						break;

					case "realm":
						if (GameServer.ServerRules.IsAllowedToAttack((GameLiving)caster, target, true))
						{
							return false;
						}
						break;
				}
			}
			return true;
		}

		public virtual GameLiving SelectSpellTarget(GameLiving caster, GameLiving currentTarget)
			=> currentTarget;
	}

    internal class PetTarget : SpellTargetType
    {
		public PetTarget(string targetType) : base(targetType) { }

		public override bool CheckBeginCast(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{
			bool casterHasPet = caster.ControlledBrain != null && caster.ControlledBrain.Body != null;
			if (casterHasPet == false)
			{
				if (!quiet) spellHandler.MessageToCaster("You must cast this spell on a creature you are controlling.",
											eChatType.CT_System);
				return false;
			}

			return true;
		}

		public override GameLiving SelectSpellTarget(GameLiving caster, GameLiving currentTarget)
        {
			bool currentTargetIsCastersPet = currentTarget == null || !caster.IsControlledNPC(currentTarget as GameNPC);
			bool casterHasPet = caster.ControlledBrain != null && caster.ControlledBrain.Body != null;

			if (currentTargetIsCastersPet && casterHasPet)
			{
				return caster.ControlledBrain.Body;
			}
			

			return currentTarget;
		}

		public override bool CheckEndCast(SpellHandler spellHandler, GameLiving caster, GameLiving target)
		{
			if (spellHandler.Spell.Range <= 0) return true;

			if (target == null || !caster.IsControlledNPC(target as GameNPC))
			{
				if (caster.ControlledBrain != null && caster.ControlledBrain.Body != null)
				{
					target = caster.ControlledBrain.Body;
				}
				else
				{
					spellHandler.MessageToCaster("You must cast this spell on a creature you are controlling.", eChatType.CT_System);
					return false;
				}
			}

			if (!caster.IsWithinRadius(target, spellHandler.CalculateSpellRange()))
			{
				spellHandler.MessageToCaster("That target is too far away!", eChatType.CT_SpellResisted);
				return false;
			}

			return true;
		}
	}

	internal class GroundTarget : SpellTargetType
    {
		public GroundTarget(string targetType) : base(targetType) { }

		public override bool CheckBeginCast(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{
			if (!caster.IsWithinRadius(caster.GroundTarget, spellHandler.CalculateSpellRange()))
			{
				if (!quiet) spellHandler.MessageToCaster("Your area target is out of range.  Select a closer target.", eChatType.CT_SpellResisted);
				return false;
			}
			if (!caster.GroundTargetInView)
			{
				spellHandler.MessageToCaster("Your ground target is not in view!", eChatType.CT_SpellResisted);
				return false;
			}

			return true;
		}

		public override bool CheckEndCast(SpellHandler spellHandler, GameLiving caster, GameLiving target)
		{
			if (!caster.IsWithinRadius(caster.GroundTarget, spellHandler.CalculateSpellRange()))
			{
				spellHandler.MessageToCaster("Your area target is out of range.  Select a closer target.", eChatType.CT_SpellResisted);
				return false;
			}

			return true;
		}
	}

    internal class EnemyTarget : SpellTargetType
    {
		public EnemyTarget(string targetType) : base(targetType) { }

        public override bool CheckBeginCast(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{

			if (spellHandler.Spell.Range > 0)
			{
				if (IsTargetValid(spellHandler, selectedTarget, quiet) == false) return false;
				if (IsTargetInRange(spellHandler, caster, selectedTarget, quiet) == false) return false;
				

				if (selectedTarget == caster)
				{
					if (!quiet) spellHandler.MessageToCaster("You can't attack yourself! ", eChatType.CT_System);
					return false;
				}

				if (SpellHandler.FindStaticEffectOnTarget(selectedTarget, typeof(NecromancerShadeEffect)) != null)
				{
					if (!quiet) spellHandler.MessageToCaster("Invalid target.", eChatType.CT_System);
					return false;
				}

				if (spellHandler.Spell.SpellType == "Charm" && spellHandler.Spell.CastTime == 0 && spellHandler.Spell.Pulse != 0)
					return true;

				if (caster.IsObjectInFront(selectedTarget, 180) == false)
				{
					if (!quiet) spellHandler.MessageToCaster("Your target is not in view!", eChatType.CT_SpellResisted);
					caster.Notify(GameLivingEvent.CastFailed, new CastFailedEventArgs(spellHandler, CastFailedEventArgs.Reasons.TargetNotInView));
					return false;
				}

				if (caster.TargetInView == false)
				{
					if (!quiet) spellHandler.MessageToCaster("Your target is not visible!", eChatType.CT_SpellResisted);
					caster.Notify(GameLivingEvent.CastFailed, new CastFailedEventArgs(spellHandler, CastFailedEventArgs.Reasons.TargetNotInView));
					return false;
				}

				if (!GameServer.ServerRules.IsAllowedToAttack(caster, selectedTarget, quiet))
				{
					return false;
				}

				if (IsTargetInView(spellHandler, caster, selectedTarget, quiet) == false) return false;


				if (!selectedTarget.IsAlive)
				{
					if (!quiet) spellHandler.MessageToCaster(selectedTarget.GetName(0, true) + " is dead!", eChatType.CT_SpellResisted);
					return false;
				}
			}

			return true;
		}

		public override bool CheckEndCast(SpellHandler spellHandler, GameLiving caster, GameLiving target)
		{
			if (spellHandler.Spell.Range <= 0)
			{
				return true;
			}

			IsTargetValid(spellHandler, target, false);

			if (!caster.IsWithinRadius(target, spellHandler.CalculateSpellRange()))
			{
				if (caster is GamePlayer)
					spellHandler.MessageToCaster("That target is too far away!", eChatType.CT_SpellResisted);
				return false;
			}

			if (!caster.IsObjectInFront(target, 180))
			{
				spellHandler.MessageToCaster("Your target is not in view. The spell fails.", eChatType.CT_SpellResisted);
				return false;
			}

			if (!GameServer.ServerRules.IsAllowedToAttack((GameLiving)caster, target, false))
			{
				return false;
			}

			return true;
		}
	}

	internal class RealmTarget : SpellTargetType
	{
		public RealmTarget(string targetType) : base(targetType) { }

		public override bool CheckBeginCast(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{
			if (spellHandler.Spell.Range <= 0) return true;

			if (IsTargetValid(spellHandler, selectedTarget, quiet) == false) return false;
			if (IsTargetInRange(spellHandler, caster, selectedTarget, quiet) == false) return false;

			if (GameServer.ServerRules.IsAllowedToAttack(caster, selectedTarget, true))
			{
				return false;
			}

			if (IsTargetInView(spellHandler, caster, selectedTarget, quiet) == false) return false;

			if (!selectedTarget.IsAlive)
			{
				if (!quiet) spellHandler.MessageToCaster(selectedTarget.GetName(0, true) + " is dead!", eChatType.CT_SpellResisted);
				return false;
			}

			return true;
		}

		public override bool CheckEndCast(SpellHandler spellHandler, GameLiving caster, GameLiving target)
		{
			if (spellHandler.Spell.Range <= 0)
			{
				return true;
			}

			IsTargetValid(spellHandler, target, false);

			if (!caster.IsWithinRadius(target, spellHandler.CalculateSpellRange()))
			{
				if (caster is GamePlayer)
					spellHandler.MessageToCaster("That target is too far away!", eChatType.CT_SpellResisted);
				return false;
			}

			if (GameServer.ServerRules.IsAllowedToAttack((GameLiving)caster, target, true))
			{
				return false;
			}

			return true;
		}
	}

	internal class CorpseTarget : SpellTargetType
	{
		public CorpseTarget(string targetType) : base(targetType) { }

		public override bool CheckBeginCast(SpellHandler spellHandler, GameLiving caster, GameLiving selectedTarget, bool quiet)
		{
			if (spellHandler.Spell.Range <= 0)
			{
				return true;
			}

			if (IsTargetValid(spellHandler, selectedTarget, quiet) == false) return false;
			if (IsTargetInRange(spellHandler, caster, selectedTarget, quiet) == false) return false;

			if (selectedTarget.IsAlive || !GameServer.ServerRules.IsSameRealm(caster, selectedTarget, true))
			{
				if (!quiet) spellHandler.MessageToCaster("This spell only works on dead members of your realm!", eChatType.CT_SpellResisted);
				return false;
			}

			if (IsTargetInView(spellHandler, caster, selectedTarget, quiet) == false) return false;

			return true;
		}

		public override bool CheckEndCast(SpellHandler spellHandler, GameLiving caster, GameLiving target)
		{
			if (spellHandler.Spell.Range <= 0)
			{
				return true;
			}

			if(IsTargetValid(spellHandler, target, false) == false) return false;

			if (!caster.IsWithinRadius(target, spellHandler.CalculateSpellRange()))
			{
				if (caster is GamePlayer)
					spellHandler.MessageToCaster("That target is too far away!", eChatType.CT_SpellResisted);
				return false;
			}
			if (target.IsAlive || !GameServer.ServerRules.IsSameRealm((GameLiving)caster, target, true))
			{
				spellHandler.MessageToCaster("This spell only works on dead members of your realm!",
								eChatType.CT_SpellResisted);
				return false;
			}
			return true;
		}
	}
}
