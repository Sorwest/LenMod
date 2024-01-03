using LenMod.LenActions;
using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardBreaktime : Card
    {
        public override string Name() => "Breaktime﻿";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 1;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardBreaktime?.DescLocKey ?? throw new Exception("Missing card")), 2);
                    break;
                case Upgrade.A:
                    result.cost = 1;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardBreaktime?.DescLocKey ?? throw new Exception("Missing card")), 3);
                    break;
                case Upgrade.B:
                    result.cost = 1;
                    result.infinite = true;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardBreaktime?.DescLocKey ?? throw new Exception("Missing card")), 2);
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
                if (artifactBananaStash.counter <= 0)
                    flagNoBananas = true;
            }
            switch (upgrade)
            {
                case Upgrade.None:
                    internalCounter = 2;
                    break;
                case Upgrade.A:
                    internalCounter = 3;
                    break;
                case Upgrade.B:
                    internalCounter = 2;
                    break;
            }
            List<CardAction> cardActionList1 = new List<CardAction>();
            AThrowBanana aThrowBanana1 = new AThrowBanana();
            aThrowBanana1.disabled = flagNoBananas;
            cardActionList1.Add(aThrowBanana1);
            AGainBanana aGainBanana1 = new AGainBanana();
            aGainBanana1.amount = -1;
            aGainBanana1.disabled = flagNoBananas;
            cardActionList1.Add(aGainBanana1);
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
                aAttack1.disabled = flagNoBananas;
                cardActionList1.Add(aAttack1);
                internalCounter -= 1;
            }
            while (internalCounter > 0);
            result = cardActionList1;
            return result;
        }
    }
}