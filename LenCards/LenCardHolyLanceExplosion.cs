using LenMod.LenActions;
using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardHolyLanceExplosion : Card
    {
        public override string Name() => "Holy Lance Explosion";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.description = Loc.GetLocString(Manifest.LenCardHolyLanceExplosion?.DescLocKey ?? throw new Exception("Missing card"));
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 2;
                    result.exhaust = true;
                    break;
                case Upgrade.A:
                    result.cost = 1;
                    result.exhaust = true;
                    break;
                case Upgrade.B:
                    result.cost = 2;
                    result.exhaust = false;
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            var flagNoBananas = false;
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                if (artifactBananaStash.counter <= 0)
                    flagNoBananas = true;
            }
            switch (upgrade)
            {
                case Upgrade.None:
                    List<CardAction> cardActionList1 = new List<CardAction>();
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = Status.droneShift;
                    aStatus1.statusAmount = 1;
                    aStatus1.targetPlayer = true;
                    cardActionList1.Add(aStatus1);
                    ASpawn aSpawn1 = new ASpawn();
                    Missile missile1 = new Missile();
                    missile1.missileType = MissileType.heavy;
                    missile1.skin = "sword";
                    aSpawn1.thing = missile1;
                    aSpawn1.disabled = flagNoBananas;
                    cardActionList1.Add(aSpawn1);
                    AGainBanana aGainBanana1 = new AGainBanana();
                    aGainBanana1.amount = -1;
                    aGainBanana1.disabled = flagNoBananas;
                    cardActionList1.Add(aGainBanana1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.droneShift;
                    aStatus2.statusAmount = 1;
                    aStatus2.targetPlayer = true;
                    cardActionList2.Add(aStatus2);
                    ASpawn aSpawn2 = new ASpawn();
                    Missile missile2 = new Missile();
                    missile2.missileType = MissileType.heavy;
                    missile2.skin = "sword";
                    aSpawn2.thing = missile2;
                    aSpawn2.disabled = flagNoBananas;
                    cardActionList2.Add(aSpawn2);
                    AGainBanana aGainBanana2 = new AGainBanana();
                    aGainBanana2.amount = -1;
                    aGainBanana2.disabled = flagNoBananas;
                    cardActionList2.Add(aGainBanana2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.droneShift;
                    aStatus3.statusAmount = 1;
                    aStatus3.targetPlayer = true;
                    cardActionList3.Add(aStatus3);
                    ASpawn aSpawn3 = new ASpawn();
                    Missile missile3 = new Missile();
                    missile3.missileType = MissileType.heavy;
                    missile3.skin = "sword";
                    aSpawn3.thing = missile3;
                    aSpawn3.disabled = flagNoBananas;
                    cardActionList3.Add(aSpawn3);
                    AGainBanana aGainBanana3 = new AGainBanana();
                    aGainBanana3.amount = -1;
                    aGainBanana3.disabled = flagNoBananas;
                    cardActionList3.Add(aGainBanana3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}