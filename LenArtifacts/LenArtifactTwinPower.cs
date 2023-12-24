namespace LenMod.LenArtifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.Boss }, unremovable = true)]
    public class LenArtifactTwinPower : Artifact
    {
        public override string Name() => "BRIOCHE";
        public override void OnReceiveArtifact(State state)
        {
            state.ship.baseEnergy += 1;
        }
        public override void OnRemoveArtifact(State state)
        {
            state.ship.baseEnergy -= 1;
        }
        public override void OnTurnStart(State state, Combat combat)
        {
            if (combat.turn % 2 == 0)
            {
                AStatus aStatus1 = new AStatus();
                aStatus1.status = Status.overdrive;
                aStatus1.statusAmount = 2;
                aStatus1.targetPlayer = false;
                combat.QueueImmediate(aStatus1);
            }
        }
    }
}
