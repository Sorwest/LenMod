namespace LenMod.LenArtifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Common })]
    public class LenArtifactMaidDress : Artifact
    {
        public override string Name() => "MAID DRESS";
        public override void OnReceiveArtifact(State state)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash == null)
            {
                state.artifacts.Add(new LenArtifactBananaStash());
                artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
                //artifactBananaStash!.OnReceiveArtifact(state);
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
}
