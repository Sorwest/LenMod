namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardBringItOn : Card
    {
        public override string Name() => "Bring It On";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.cost = 2;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.buoyant = false;
                    break;
                case Upgrade.A:
                    result.buoyant = true;
                    break;
                case Upgrade.B:
                    result.buoyant = false;
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
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = Status.shield;
                    aStatus1.statusAmount = 2;
                    aStatus1.targetPlayer = true;
                    cardActionList1.Add(aStatus1);
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.tempPayback;
                    aStatus2.statusAmount = 1;
                    aStatus2.targetPlayer = true;
                    cardActionList1.Add(aStatus2);
                    AEndTurn aEndTurn1 = new AEndTurn();
                    cardActionList1.Add(aEndTurn1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.shield;
                    aStatus3.statusAmount = 3;
                    aStatus3.targetPlayer = true;
                    cardActionList2.Add(aStatus3);
                    AStatus aStatus4 = new AStatus();
                    aStatus4.status = Status.tempPayback;
                    aStatus4.statusAmount = 1;
                    aStatus4.targetPlayer = true;
                    cardActionList2.Add(aStatus4);
                    AEndTurn aEndTurn2 = new AEndTurn();
                    cardActionList2.Add(aEndTurn2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AStatus aStatus5 = new AStatus();
                    aStatus5.status = Status.tempShield;
                    aStatus5.statusAmount = 2;
                    aStatus5.targetPlayer = true;
                    cardActionList3.Add(aStatus5);
                    AStatus aStatus6 = new AStatus();
                    aStatus6.status = Status.tempPayback;
                    aStatus6.statusAmount = 1;
                    aStatus6.targetPlayer = true;
                    cardActionList3.Add(aStatus6);
                    AStatus aStatus7 = new AStatus();
                    aStatus7.status = Status.payback;
                    aStatus7.statusAmount = 1;
                    aStatus7.targetPlayer = true;
                    cardActionList3.Add(aStatus7);
                    AEndTurn aEndTurn3 = new AEndTurn();
                    cardActionList3.Add(aEndTurn3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}