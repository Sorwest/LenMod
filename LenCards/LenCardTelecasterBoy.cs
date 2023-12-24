namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardTelecasterBoy : Card
    {
        public override string Name() => "Telecaster B-Boy";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.exhaust = true;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 2;
                    break;
                case Upgrade.A:
                    result.cost = 1;
                    break;
                case Upgrade.B:
                    result.cost = 2;
                    result.retain = true;
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
                    AMove aMove1 = new AMove();
                    aMove1.dir = 2;
                    aMove1.isRandom = true;
                    aMove1.targetPlayer = true;
                    cardActionList1.Add(aMove1);
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = Status.evade;
                    aStatus1.statusAmount = 3;
                    aStatus1.targetPlayer = true;
                    cardActionList1.Add(aStatus1);
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.loseEvadeNextTurn;
                    aStatus2.statusAmount = 1;
                    aStatus2.targetPlayer = true;
                    cardActionList1.Add(aStatus2);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AMove aMove2 = new AMove();
                    aMove2.dir = 2;
                    aMove2.isRandom = true;
                    aMove2.targetPlayer = true;
                    cardActionList2.Add(aMove2);
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.evade;
                    aStatus3.statusAmount = 3;
                    aStatus3.targetPlayer = true;
                    cardActionList2.Add(aStatus3);
                    AStatus aStatus4 = new AStatus();
                    aStatus4.status = Status.loseEvadeNextTurn;
                    aStatus4.statusAmount = 1;
                    aStatus4.targetPlayer = true;
                    cardActionList2.Add(aStatus4);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AMove aMove3 = new AMove();
                    aMove3.dir = 2;
                    aMove3.isRandom = true;
                    aMove3.targetPlayer = true;
                    cardActionList3.Add(aMove3);
                    AStatus aStatus5 = new AStatus();
                    aStatus5.status = Status.evade;
                    aStatus5.statusAmount = 3;
                    aStatus5.targetPlayer = true;
                    cardActionList3.Add(aStatus5);
                    AStatus aStatus6 = new AStatus();
                    aStatus6.status = Status.loseEvadeNextTurn;
                    aStatus6.statusAmount = 1;
                    aStatus6.targetPlayer = true;
                    cardActionList3.Add(aStatus6);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}