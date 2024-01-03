using LenMod.LenActions;
using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardNakakapagpabagabag : Card
    {
        public override string Name() => "Nakakapagpabagabag";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.description = Loc.GetLocString(Manifest.LenCardNakakapagpabagabag?.DescLocKey ?? throw new Exception("Missing card"));
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 1;
                    break;
                case Upgrade.A:
                    result.cost = 0;
                    break;
                case Upgrade.B:
                    result.cost = 1;
                    result.buoyant = true;
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            var flagNoBananas = false;
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null && artifactBananaStash.counter <= 0)
                flagNoBananas = true;
            List<CardAction> cardActionList1 = new List<CardAction>();
            ASmashBanana aSmashBanana1 = new ASmashBanana();
            aSmashBanana1.amount = -1;
            aSmashBanana1.disabled = flagNoBananas;
            cardActionList1.Add(aSmashBanana1);
            AStatus aStatus1 = new AStatus();
            aStatus1.status = Status.overdrive;
            aStatus1.statusAmount = 1;
            aStatus1.targetPlayer = true;
            aStatus1.disabled = flagNoBananas;
            cardActionList1.Add(aStatus1);
            result = cardActionList1;
            return result;
        }
    }
}