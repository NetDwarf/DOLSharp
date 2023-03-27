using System;
using DOL.Database;
using DOL.Events;

namespace DOL.GS.ServerRules;

[ServerRules(eGameServerType.GST_Casual)]
public class CoopWithRvr : AbstractServerRules
{
    private static AbstractServerRules pveServerRules = new PvEServerRules();
    private static AbstractServerRules normalServerRules = new NormalServerRules();

    public override string RulesDescription()
        => "PvE/Coop rules in PvE zones and realm vs realm in RvR zones.";

    public override bool IsAllowedCharsInAllRealms(GameClient client)
        => true;

    public override bool IsAllowedToGroup(GamePlayer source, GamePlayer target, bool quiet)
        => SelectRuleSet(source).IsAllowedToGroup(source, target, quiet);

    public override bool IsAllowedToJoinGuild(GamePlayer source, Guild guild)
        => SelectRuleSet(source).IsAllowedToJoinGuild(source, guild);

    public override bool IsAllowedToTrade(GameLiving source, GameLiving target, bool quiet)
        => SelectRuleSet(source).IsAllowedToTrade(source, target, quiet);

    public override bool IsAllowedToUnderstand(GameLiving source, GamePlayer target)
        => SelectRuleSet(source).IsAllowedToUnderstand(source, target);

    public override bool IsSameRealm(GameLiving source, GameLiving target, bool quiet)
        => SelectRuleSet(source).IsSameRealm(source, target, quiet);

    public override byte GetColorHandling(GameClient client)
        => SelectRuleSet(client.Player).GetColorHandling(client);

    public override bool IsAllowedToAttack(GameLiving attacker, GameLiving defender, bool quiet)
        => SelectRuleSet(attacker).IsAllowedToAttack(attacker, defender, quiet);

    public override bool IsAllowedToCastSpell(GameLiving caster, GameLiving target, Spell spell, SpellLine spellLine)
        => SelectRuleSet(caster).IsAllowedToCastSpell(caster, target, spell, spellLine);

    public override bool IsAllowedToSpeak(GamePlayer source, string communicationType)
        => SelectRuleSet(source).IsAllowedToSpeak(source, communicationType);

    public override bool IsAllowedToBind(GamePlayer player, BindPoint point)
        => SelectRuleSet(player).IsAllowedToBind(player, point);

    public override bool IsAllowedToCraft(GamePlayer player, ItemTemplate item)
        => SelectRuleSet(player).IsAllowedToCraft(player, item);

    public override bool IsAllowedToClaim(GamePlayer player, Region region)
        => SelectRuleSet(player).IsAllowedToClaim(player, region);

    public override bool IsAllowedToZone(GamePlayer player, Region region)
        => SelectRuleSet(player).IsAllowedToZone(player, region);

    public override string ReasonForDisallowMounting(GamePlayer player)
        => SelectRuleSet(player).ReasonForDisallowMounting(player);

    public override bool CanTakeFallDamage(GamePlayer player)
        => SelectRuleSet(player).CanTakeFallDamage(player);

    public override bool CheckAbilityToUseItem(GameLiving living, ItemTemplate item)
        => SelectRuleSet(living).CheckAbilityToUseItem(living, item);

    public override int GetObjectSpecLevel(GamePlayer player, eObjectType objectType)
        => SelectRuleSet(player).GetObjectSpecLevel(player, objectType);

    public override int GetBaseObjectSpecLevel(GamePlayer player, eObjectType objectType)
        => SelectRuleSet(player).GetBaseObjectSpecLevel(player, objectType);

    public override void OnNPCKilled(GameNPC killedNPC, GameObject killer)
        => SelectRuleSet(killedNPC).OnNPCKilled(killedNPC, killer);

    public override void OnLivingKilled(GameLiving killedLiving, GameObject killer)
        => SelectRuleSet(killer).OnLivingKilled(killedLiving, killer);

    public override void OnPlayerKilled(GamePlayer killedPlayer, GameObject killer)
        => SelectRuleSet(killer).OnPlayerKilled(killedPlayer, killer);

    public override byte GetLivingRealm(GamePlayer player, GameLiving target)
        => SelectRuleSet(player).GetLivingRealm(player, target);

    public override string GetPlayerName(GamePlayer source, GamePlayer target)
        => SelectRuleSet(source).GetPlayerName(source, target);

    public override string GetPlayerPrefixName(GamePlayer source, GamePlayer target)
        => SelectRuleSet(source).GetPlayerPrefixName(source, target);

    public override string GetPlayerLastName(GamePlayer source, GamePlayer target)
        => SelectRuleSet(source).GetPlayerLastName(source, target);

    public override string GetPlayerGuildName(GamePlayer source, GamePlayer target)
        => SelectRuleSet(source).GetPlayerGuildName(source, target);

    public override string GetPlayerTitle(GamePlayer source, GamePlayer target)
        => SelectRuleSet(source).GetPlayerTitle(source, target);

    public override int GetPlayerRealmPointsTotal(GamePlayer source)
        => SelectRuleSet(source).GetPlayerRealmPointsTotal(source);

    public override void OnGameEntered(DOLEvent e, object sender, EventArgs args)
    {
        base.OnGameEntered(e, sender, args);
        SetNameColorScheme(sender as GamePlayer);
    }

    public override void OnRegionChanged(DOLEvent e, object sender, EventArgs args)
    {
        base.OnRegionChanged(e, sender, args);
        SetNameColorScheme(sender as GamePlayer);
    }

    private AbstractServerRules SelectRuleSet(GameObject obj)
    {
        if (obj.CurrentRegion.IsRvR)
        {
            return normalServerRules;
        }
        return pveServerRules;
    }

    private static void SetNameColorScheme(GamePlayer player)
    {
        if (!player.CurrentRegion.IsRvR)
        {
            player.Out.SendRegionColorScheme((int)eGameServerType.GST_PvE);
        }
        else
        {
            player.Out.SendRegionColorScheme();
        }
    }
}
