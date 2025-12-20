using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Cards;
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
        => [new TTCard { card = new LenCardWishBottle() }];
    public override void OnCombatStart(State state, Combat combat)
    {
        combat.Queue(new AAddCard()
        {
            card = new LenCardWishBottle() { temporaryOverride = true },
            destination = CardDestination.Hand,
            amount = 1
        });
        Pulse();
    }
}
