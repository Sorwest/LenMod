using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardPlusBoy : Card
    {
        public override string Name() => "Plus Boy";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.exhaust = true;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 2;
                    result.singleUse = true;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardPlusBoy?.DescLocKey ?? throw new Exception("Missing card")), 5);
                    break;
                case Upgrade.A:
                    result.cost = 2;
                    result.singleUse = true;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardPlusBoy?.DescLocKey ?? throw new Exception("Missing card")), 8);
                    break;
                case Upgrade.B:
                    result.cost = 2;
                    result.singleUse = false;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardPlusBoy?.DescLocKey ?? throw new Exception("Missing card")), 4);
                    break;
            }
            return result;
        }
        public override void AfterWasPlayed(State state, Combat combat)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
                switch (upgrade)
                {
                    case Upgrade.None:
                        artifactBananaStash.counter += 5;
                        break;
                    case Upgrade.A:
                        artifactBananaStash.counter += 8;
                        break;
                    case Upgrade.B:
                        artifactBananaStash.counter += 4;
                        break;
                }
        }
    }
}