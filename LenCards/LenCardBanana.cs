using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardBanana : Card
    {
        public override string Name() => "Banana﻿";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 1;
                    result.exhaust = true;
                    result.description = Loc.GetLocString(Manifest.LenCardBanana?.DescLocKey ?? throw new Exception("Missing card"));
                    break;
                case Upgrade.A:
                    result.cost = 0;
                    result.exhaust = true;
                    result.description = Loc.GetLocString(Manifest.LenCardBanana?.DescALocKey ?? throw new Exception("Missing card"));
                    break;
                case Upgrade.B:
                    result.cost = 1;
                    result.exhaust = false;
                    result.description = Loc.GetLocString(Manifest.LenCardBanana?.DescLocKey ?? throw new Exception("Missing card"));
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            List<CardAction> cardActionList1 = new List<CardAction>();
            ADrawCard aDrawCard1 = new ADrawCard();
            aDrawCard1.count = 1;
            cardActionList1.Add(aDrawCard1);
            result = cardActionList1;
            return result;
        }
        public override void AfterWasPlayed(State state, Combat combat)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
                switch (upgrade)
                {
                    case Upgrade.None:
                        artifactBananaStash.counter += 1;
                        break;
                    case Upgrade.A:
                        artifactBananaStash.counter += 2;
                        break;
                    case Upgrade.B:
                        artifactBananaStash.counter += 1;
                        break;
                }
        }
    }
}