using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardParadichlorobenzene : Card
    {
        public override string Name() => "Paradichlorobenzene";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.exhaust = true;
            result.description = Loc.GetLocString(Manifest.LenCardParadichlorobenzene?.DescLocKey ?? throw new Exception("Missing card"));
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 2;
                    result.buoyant = false;
                    break;
                case Upgrade.A:
                    result.cost = 1;
                    result.buoyant = false;
                    break;
                case Upgrade.B:
                    result.cost = 3;
                    result.buoyant = true;
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            var flagBananas = false;
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                if (artifactBananaStash.counter > 0)
                    flagBananas = true;
            }
            switch (upgrade)
            {
                case Upgrade.None:
                    List<CardAction> cardActionList1 = new List<CardAction>();
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = Status.shield;
                    aStatus1.statusAmount = 0;
                    aStatus1.mode = AStatusMode.Set;
                    aStatus1.targetPlayer = false;
                    aStatus1.disabled = flagBananas;
                    cardActionList1.Add(aStatus1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.shield;
                    aStatus2.statusAmount = 0;
                    aStatus2.mode = AStatusMode.Set;
                    aStatus2.targetPlayer = false;
                    aStatus2.disabled = flagBananas;
                    cardActionList2.Add(aStatus2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.shield;
                    aStatus3.statusAmount = 0;
                    aStatus3.mode = AStatusMode.Set;
                    aStatus3.targetPlayer = false;
                    aStatus3.disabled = flagBananas;
                    cardActionList3.Add(aStatus3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
        public override void AfterWasPlayed(State state, Combat combat)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
                artifactBananaStash.counter += 1;
        }
    }
}