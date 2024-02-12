using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactBananaStash : Artifact, IModdedArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("BananaStash", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = ModEntry.Instance.Sprites["BananaStash"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BananaStash", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BananaStash", "description"]).Localize
        });
    }
    public override string Name() => "BANANA STASH";
    public int counter;
    public bool stillHasBananas = true;
    public int enemyDamage;
    public int shieldNumber;
    public override int? GetDisplayNumber(State s)
    {
        return counter > 0 ? counter : null;
    }
    public override Spr GetSprite()
    {
        if (counter > 0 || stillHasBananas)
            return ModEntry.Instance.Sprites["BananaStash"].Sprite;
        else
        {
            return ModEntry.Instance.Sprites["BananaStashOff"].Sprite;
        }
    }
    public override void OnReceiveArtifact(State state)
    {
        if (counter == 0)
            counter += 6;
        if (enemyDamage == 0)
            enemyDamage += 1;
    }
    public override void OnRemoveArtifact(State state)
    {
        counter = 0;
        enemyDamage -= 1;
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        if (counter > 0)
        {
            combat.QueueImmediate(new AStatus()
            {
                status = ModEntry.Instance.BananaStatus.Status,
                statusAmount = counter,
                targetPlayer = true,
                timer = 0
            });
            counter = 0;
            stillHasBananas = true;
        }
    }
    public override void OnCombatEnd(State state)
    {
        if (state.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0)
            counter = state.ship.Get(ModEntry.Instance.BananaStatus.Status);
        else
            stillHasBananas = false;
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tooltips = new List<Tooltip>();
        var str = "";
        if (shieldNumber > 0)
        {
            str = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "maidDress"], new { Amount = shieldNumber });
        }
        tooltips.Add(new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.action,
            () => ModEntry.Instance.Sprites["EatBanana"].Sprite,
            () => ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "name"]),
            () => ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "description"], new { Damage = enemyDamage > 0 ? enemyDamage : 1, MaidDress = str }),
            key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::EatBanana"
        ));
        return tooltips;
    }
}
