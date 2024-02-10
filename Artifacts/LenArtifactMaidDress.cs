using Nickel;
using System.Linq;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactMaidDress : Artifact, IModdedArtifact
{
    public static void Register(IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("MaidDress", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.Common]
            },
            Sprite = ModEntry.Instance.Sprites["MaidDress"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MaidDress", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "MaidDress", "description"]).Localize
        });
    }
    public override string Name() => "MAID DRESS";
    public override void OnReceiveArtifact(State state)
    {
        var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash == null)
        {
            state.artifacts.Add(new LenArtifactBananaStash());
            artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        }
        artifactBananaStash!.shieldNumber += 2;
    }
    public override void OnRemoveArtifact(State state)
    {
        var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash != null)
            artifactBananaStash.shieldNumber -= 2;
    }
}
