using LenMod.LenActions;
using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardBananaWall : Card
    {
        public override string Name() => "Banana Wall﻿";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 1;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardBananaWall?.DescLocKey ?? throw new Exception("Missing card")), 2);
                    break;
                case Upgrade.A:
                    result.cost = 0;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardBananaWall?.DescLocKey ?? throw new Exception("Missing card")), 2);
                    break;
                case Upgrade.B:
                    result.cost = 1;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardBananaWall?.DescLocKey ?? throw new Exception("Missing card")), 3);
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
                    ASmashBanana aSmashBanana1 = new ASmashBanana();
                    aSmashBanana1.disabled = flagNoBananas;
                    cardActionList1.Add(aSmashBanana1);
                    AGainBanana aGainBanana1 = new AGainBanana();
                    aGainBanana1.amount = -1;
                    aGainBanana1.disabled = flagNoBananas;
                    cardActionList1.Add(aGainBanana1);
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = Status.shield;
                    aStatus1.statusAmount = 2;
                    aStatus1.targetPlayer = true;
                    aStatus1.disabled = flagNoBananas;
                    cardActionList1.Add(aStatus1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    ASmashBanana aSmashBanana2 = new ASmashBanana();
                    aSmashBanana2.disabled = flagNoBananas;
                    cardActionList2.Add(aSmashBanana2);
                    AGainBanana aGainBanana2 = new AGainBanana();
                    aGainBanana2.amount = -1;
                    aGainBanana2.disabled = flagNoBananas;
                    cardActionList2.Add(aGainBanana2);
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.shield;
                    aStatus2.statusAmount = 2;
                    aStatus2.targetPlayer = true;
                    aStatus2.disabled = flagNoBananas;
                    cardActionList2.Add(aStatus2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    ASmashBanana aSmashBanana3 = new ASmashBanana();
                    aSmashBanana3.disabled = flagNoBananas;
                    cardActionList3.Add(aSmashBanana3);
                    AGainBanana aGainBanana3 = new AGainBanana();
                    aGainBanana3.amount = -1;
                    aGainBanana3.disabled = flagNoBananas;
                    cardActionList3.Add(aGainBanana3);
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.shield;
                    aStatus3.statusAmount = 3;
                    aStatus3.targetPlayer = true;
                    aStatus3.disabled = flagNoBananas;
                    cardActionList3.Add(aStatus3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}