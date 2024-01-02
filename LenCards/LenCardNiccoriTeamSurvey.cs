using LenMod.LenActions;
using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardNiccoriTeamSurvey : Card
    {
        public override string Name() => "Niccori Team Survey";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.description = Loc.GetLocString(Manifest.LenCardNiccoriTeamSurvey?.DescLocKey ?? throw new Exception("Missing card"));
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 3;
                    result.exhaust = true;
                    break;
                case Upgrade.A:
                    result.cost = 2;
                    result.exhaust = true;
                    break;
                case Upgrade.B:
                    result.cost = 3;
                    result.exhaust = false;
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
                internalCounter = artifactBananaStash.counter;
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
                AGainBanana aGainBanana1 = new AGainBanana();
                aGainBanana1.amount = -1;
                aGainBanana1.disabled = flagNoBananas;
                cardActionList1.Add(aGainBanana1);
                internalCounter -= 1;
            }
            while (internalCounter > 0);
            result = cardActionList1;
            return result;
        }
    }
}