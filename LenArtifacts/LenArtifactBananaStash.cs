namespace LenMod.LenArtifacts
{
    [ArtifactMeta(pools = new ArtifactPool[] { ArtifactPool.EventOnly }, unremovable = true)]
    public class LenArtifactBananaStash : Artifact
    {
        public override string Name() => "BANANA STASH";
        public int counter;
        public int enemyDamage = 0;
        public int shieldNumber = 0;
        public override int? GetDisplayNumber(State s)
        {
            return counter;
        }
        public override Spr GetSprite()
        {
            Spr sprite = new Spr();
            if (counter > 0)
                sprite = (Spr)Manifest.LenArtifactBananaStashSprite!.Id!;
            if (counter <= 0)
                sprite = (Spr)Manifest.LenArtifactBananaStashOffSprite!.Id!;
            if (counter < 0)
                counter = 0;
            return sprite;
        }
        public override void OnReceiveArtifact(State state)
        {
            counter += 6;
            enemyDamage += 1;
        }
        public override void OnRemoveArtifact(State state)
        {
            counter = 0;
            enemyDamage -= 1;
        }
        public override void OnTurnStart(State state, Combat combat)
        {
            if (Manifest.LenStatusNotes?.Id == null)
                return;
            var status = (Status)Manifest.LenStatusNotes.Id;
            var amount = state.ship.Get(status);
            //don't trigger artifact passive if ship has Notes status
            if (amount > 0)
                return;
            if (!state.ship.isPlayerShip)
                return;
            //don't trigger if bananas are somehow negative
            if (counter <= 0)
                return;
            if (shieldNumber > 0)
            {
                AStatus aStatus1 = new AStatus();
                aStatus1.status = Status.shield;
                aStatus1.statusAmount = shieldNumber;
                aStatus1.targetPlayer = true;
                combat.QueueImmediate(aStatus1);
            }
            if (enemyDamage > 0)
            {
                AHurt aHurt1 = new AHurt();
                aHurt1.hurtAmount = enemyDamage;
                aHurt1.targetPlayer = false;
                combat.QueueImmediate(aHurt1);
                counter -= 1;
                this.Pulse();
            }
        }
    }
}
