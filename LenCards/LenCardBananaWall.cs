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
        public override void AfterWasPlayed(State state, Combat combat)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null && artifactBananaStash.counter > 0)
                artifactBananaStash.counter -= 1;
        }
    }
}