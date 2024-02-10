using Nickel;
using System.Linq;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactBrioche : Artifact, IModdedArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Brioche", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = ModEntry.Instance.Sprites["Brioche"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Brioche", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Brioche", "description"]).Localize
        });
    }
    public override string Name() => "BRIOCHE";
    public override void OnReceiveArtifact(State state)
    {
        var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash == null)
        {
            state.artifacts.Add(new LenArtifactBananaStash());
            artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        }
        if (artifactBananaStash != null)
            artifactBananaStash.enemyDamage += 1;
    }
    public override void OnRemoveArtifact(State state)
    {
        var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash != null)
            artifactBananaStash.enemyDamage -= 1;
    }
}
