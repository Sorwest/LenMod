using LenMod.LenActions;

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
                    break;
                case Upgrade.A:
                    result.cost = 0;
                    result.exhaust = true;
                    break;
                case Upgrade.B:
                    result.cost = 1;
                    result.exhaust = false;
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            switch (upgrade)
            {
                case Upgrade.None:
                    List<CardAction> cardActionList1 = new List<CardAction>();
                    AGainBanana aGainBanana1 = new AGainBanana();
                    aGainBanana1.amount = 1;
                    cardActionList1.Add(aGainBanana1);
                    ADrawCard aDrawCard1 = new ADrawCard();
                    aDrawCard1.count = 1;
                    cardActionList1.Add(aDrawCard1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AGainBanana aGainBanana2 = new AGainBanana();
                    aGainBanana2.amount = 1;
                    cardActionList2.Add(aGainBanana2);
                    ADrawCard aDrawCard2 = new ADrawCard();
                    aDrawCard2.count = 1;
                    cardActionList2.Add(aDrawCard2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AGainBanana aGainBanana3 = new AGainBanana();
                    aGainBanana3.amount = 1;
                    cardActionList3.Add(aGainBanana3);
                    ADrawCard aDrawCard3 = new ADrawCard();
                    aDrawCard3.count = 1;
                    cardActionList3.Add(aDrawCard3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}