using DOL.AI;
using DOL.AI.Brain;
using DOL.Database;
using DOL.GS;
using NSubstitute;

namespace DOL.UnitTests.GameServer
{
    public static class Create
    {
        public static Spell Spell()
        {
            return new Spell(new DBSpell(), 0);
        }

        public static Spell DamageSpell(double damage)
        {
            var spell = new Spell(new DBSpell(), 0);
            spell.Damage = 100;
            return spell;
        }

        public static FakePlayer FakePlayer()
        {
            return FakePlayer(new DefaultCharacterClass());
        }

        public static FakePlayer FakePlayer(ICharacterClass charClass)
        {
            var player = new FakePlayer();
			player.LivingRace = new FakeRace();
            player.characterClass = charClass;
            return player;
        }

        public static GameNPC FakeNPC()
        {
            var brain = Substitute.For<ABrain>();
            var npc = new GameNPC(brain);
			npc.LivingRace = new FakeRace();
            return npc;
        }

        public static GamePet Pet(GameLiving owner)
        {
            var brain = Substitute.For<ABrain, IControlledBrain>();
            var npcTemplate = Substitute.For<INpcTemplate>();
            (brain as IControlledBrain).Owner.Returns(owner);
            GamePet pet = new GamePet(brain);
            return pet;
        }
    }
}
