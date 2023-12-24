namespace LenMod.LenArtifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class LenArtifactGlassBottle : Artifact
    {
        public override string Name() => "GLASS BOTTLE";
        public override void OnCombatStart(State state, Combat combat)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                artifactBananaStash.counter += 3;
                this.Pulse();
            }
        }
    }
}
