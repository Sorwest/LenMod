using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactBananaStash : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
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
    public int counter = 3;
    public bool stillHasBananas = true;
    public override List<Tooltip>? GetExtraTooltips()
    {
        return [
            ..StatusMeta.GetTooltips(BananaManager.BananaStatus.Status, 1)
        ];
    }
    public override int? GetDisplayNumber(State s)
    {
        return counter > 0 ? counter : null;
    }
    public override Spr GetSprite()
    {
        if (stillHasBananas && counter > 0)
            return ModEntry.Instance.Sprites["BananaStash"].Sprite;
        else if (stillHasBananas)
            return ModEntry.Instance.Sprites["BananaStashOn"].Sprite;
        else
            return ModEntry.Instance.Sprites["BananaStashOff"].Sprite;
    }
    public override void OnReceiveArtifact(State s)
    {
        counter = 3;
    }
    public override void OnRemoveArtifact(State state)
    {
        ModEntry.Instance.Helper.ModData.RemoveModData(state, "BananaStored");
    }
    public override void OnCombatStart(State s, Combat c)
    {
        if (counter > 0)
        {
            ModEntry.Instance.Helper.ModData.SetModData(s, "BananaStored", counter);
            s.ship.Set(BananaManager.BananaStatus.Status, counter);
            counter = 0;
        }
    }
    public override void OnCombatEnd(State s)
    {
        int amount = s.ship.Get(BananaManager.BananaStatus.Status);
        counter = amount;
        stillHasBananas = amount > 0;
        if (amount > 0)
            ModEntry.Instance.Helper.ModData.SetModData(s, "BananaStored", amount);
        else
            ModEntry.Instance.Helper.ModData.RemoveModData(s, "BananaStored");
    }
}