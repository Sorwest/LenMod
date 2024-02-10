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
    public int enemyDamage;
    public int shieldNumber;
    public override int? GetDisplayNumber(State s)
    {
        return counter;
    }
    public override Spr GetSprite()
    {
        Spr sprite = new Spr();
        if (counter > 0)
            sprite = ModEntry.Instance.Sprites["LenArtifactBananaStashSprite"].Sprite;
        if (counter <= 0)
            sprite = ModEntry.Instance.Sprites["LenArtifactBananaStashOffSprite"].Sprite;
        if (counter < 0)
            counter = 0;
        return sprite;
    }
    public override void OnReceiveArtifact(State state)
    {
        counter += 6;
        enemyDamage += 1;
    }
    public override void OnRemoveArtifact(State state)
    {
        counter = 0;
        enemyDamage -= 1;
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        Status status = ModEntry.Instance.MusicNoteStatus.Status;
        var amount = state.ship.Get(status);
        if (amount > 0)
            return;
        if (!state.ship.isPlayerShip)
            return;
        if (counter <= 0)
            return;
        if (shieldNumber > 0)
        {
            combat.Queue(new AStatus()
            {
                status = Status.shield,
                statusAmount = shieldNumber,
                targetPlayer = true
            });
        }
        if (enemyDamage > 0)
        {
            combat.Queue(new AHurt()
            {
                hurtAmount = enemyDamage,
                targetPlayer = false
            });
            counter--;
            Pulse();
        }
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        List<Tooltip> tooltips = new List<Tooltip>();
        var str = "";
        if (shieldNumber > 0)
        {
            str = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "flavor"], new { Amount = shieldNumber });
        }
        tooltips.Add(new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.action,
            () => ModEntry.Instance.Sprites["EatBanana"].Sprite,
            () => ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "name"]),
            () => ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "description"], new { Damage = enemyDamage, MaidDress = str }),
            key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::EatBanana"
        ));
        return tooltips;
    }
}
