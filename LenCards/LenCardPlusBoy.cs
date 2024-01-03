using LenMod.LenActions;
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
                    break;
                case Upgrade.A:
                    result.cost = 2;
                    result.singleUse = true;
                    break;
                case Upgrade.B:
                    result.cost = 2;
                    result.singleUse = false;
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
                    aGainBanana1.amount = 5;
                    cardActionList1.Add(aGainBanana1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AGainBanana aGainBanana2 = new AGainBanana();
                    aGainBanana2.amount = 8;
                    cardActionList2.Add(aGainBanana2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AGainBanana aGainBanana3 = new AGainBanana();
                    aGainBanana3.amount = 4;
                    cardActionList3.Add(aGainBanana3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}