using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardVampiresPathos : Card
    {
        public override string Name() => "Vampire's PathoS";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.exhaust = true;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 2;
                    result.description = Loc.GetLocString(Manifest.LenCardVampiresPathos?.DescLocKey ?? throw new Exception("Missing card"));
                    break;
                case Upgrade.A:
                    result.cost = 1;
                    result.description = Loc.GetLocString(Manifest.LenCardVampiresPathos?.DescLocKey ?? throw new Exception("Missing card"));
                    break;
                case Upgrade.B:
                    result.cost = 2;
                    result.description = Loc.GetLocString(Manifest.LenCardVampiresPathos?.DescBLocKey ?? throw new Exception("Missing card"));
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            var enemyDamage = 0;
            var shieldNumber = 0;
            var flagNoBananas = false;
            var internalCounter = 0;
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                enemyDamage = artifactBananaStash.enemyDamage;
                shieldNumber = artifactBananaStash.shieldNumber;
                switch (upgrade)
                {
                    case Upgrade.None:
                        internalCounter = 1;
                        break;
                    case Upgrade.A:
                        internalCounter = 1;
                        break;
                    case Upgrade.B:
                        internalCounter = 2;
                        if (artifactBananaStash.counter < 2)
                            internalCounter = 1;
                        break;
                }
                if (artifactBananaStash.counter <= 0)
                    flagNoBananas = true;
            }
            List<CardAction> cardActionList1 = new List<CardAction>();
            do
            {
                if (internalCounter <= 0)
                    break;
                if (shieldNumber > 0)
                {
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = Status.shield;
                    aStatus1.statusAmount = shieldNumber;
                    aStatus1.targetPlayer = true;
                    aStatus1.disabled = flagNoBananas;
                    cardActionList1.Add(aStatus1);
                }
                AAttack aAttack1 = new AAttack();
                aAttack1.damage = GetDmg(s, enemyDamage);
                aAttack1.piercing = true;
                aAttack1.targetPlayer = false;
                aAttack1.fast = true;
                aAttack1.disabled = flagNoBananas;
                cardActionList1.Add(aAttack1);
                internalCounter -= 1;
            }
            while (internalCounter > 0);
            AHeal aHeal1 = new AHeal();
            aHeal1.healAmount = 1;
            aHeal1.targetPlayer = true;
            cardActionList1.Add(aHeal1);
            result = cardActionList1;
            return result;
        }
        public override void AfterWasPlayed(State state, Combat combat)
        {
            var artifactBananaStash = state.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null && artifactBananaStash.counter > 0)
                switch (upgrade)
                {
                    case Upgrade.None:
                        artifactBananaStash.counter -= 1;
                        break;
                    case Upgrade.A:
                        artifactBananaStash.counter -= 1;
                        break;
                    case Upgrade.B:
                        artifactBananaStash.counter -= 2;
                        break;
                }
        }
    }
}