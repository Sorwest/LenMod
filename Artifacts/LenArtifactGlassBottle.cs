using Nickel;
using System.Linq;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactGlassBottle : Artifact, IModdedArtifact
{
    public static void Register(IModHelper helper)
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
    public override string Name() => "GLASS BOTTLE";
    public override void OnCombatStart(State state, Combat combat)
    {
        var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash == null)
        {
            state.artifacts.Add(new LenArtifactBananaStash());
            artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        }
        if (artifactBananaStash != null)
        {
            artifactBananaStash.counter += 3;
            Pulse();
        }
    }
}
