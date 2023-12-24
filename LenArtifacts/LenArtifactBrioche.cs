namespace LenMod.LenArtifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    public class LenArtifactBrioche : Artifact
    {
        public override string Name() => "BRIOCHE";
        public override void OnReceiveArtifact(State state)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
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
}
