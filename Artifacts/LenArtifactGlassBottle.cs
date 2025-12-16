using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactGlassBottle : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("GlassBottle", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.Common]
            },
            Sprite = ModEntry.Instance.Sprites["GlassBottle"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GlassBottle", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "GlassBottle", "description"]).Localize
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
        => [
            ..StatusMeta.GetTooltips(BananaManager.BananaStatus.Status, 1)
        ];
    public override string Name() => "GLASS BOTTLE";
    public override void OnCombatStart(State s, Combat c)
    {
        c.Queue(new AStatus()
        {
            status = BananaManager.BananaStatus.Status,
            statusAmount = 3,
            targetPlayer = true,
            timer = 0
        });
        Pulse();
    }
}
