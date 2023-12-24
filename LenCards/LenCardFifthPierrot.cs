namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardFifthPierrot : Card
    {
        public override string Name() => "Fifth Pierrot";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.cost = 1;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.exhaust = true;
                    break;
                case Upgrade.A:
                    result.exhaust = true;
                    break;
                case Upgrade.B:
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
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = Status.powerdrive;
                    aStatus1.statusAmount = 1;
                    aStatus1.targetPlayer = true;
                    cardActionList1.Add(aStatus1);
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.shield;
                    aStatus2.statusAmount = 0;
                    aStatus2.mode = AStatusMode.Set;
                    aStatus2.targetPlayer = true;
                    cardActionList1.Add(aStatus2);
                    AEndTurn aEndTurn1 = new AEndTurn();
                    cardActionList1.Add(aEndTurn1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AAttack aAttack1 = new AAttack();
                    aAttack1.damage = GetDmg(s, 0);
                    aAttack1.stunEnemy = true;
                    cardActionList2.Add(aAttack1);
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.powerdrive;
                    aStatus3.statusAmount = 1;
                    aStatus3.targetPlayer = true;
                    cardActionList2.Add(aStatus3);
                    AStatus aStatus4 = new AStatus();
                    aStatus4.status = Status.shield;
                    aStatus4.statusAmount = 0;
                    aStatus4.mode = AStatusMode.Set;
                    aStatus4.targetPlayer = true;
                    cardActionList2.Add(aStatus4);
                    AEndTurn aEndTurn2 = new AEndTurn();
                    cardActionList2.Add(aEndTurn2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AStatus aStatus5 = new AStatus();
                    aStatus5.status = Status.powerdrive;
                    aStatus5.statusAmount = 1;
                    aStatus5.targetPlayer = true;
                    cardActionList3.Add(aStatus5);
                    AStatus aStatus6 = new AStatus();
                    aStatus6.status = Status.shield;
                    aStatus6.statusAmount = 0;
                    aStatus6.mode = AStatusMode.Set;
                    aStatus6.targetPlayer = true;
                    cardActionList3.Add(aStatus6);
                    AEndTurn aEndTurn3 = new AEndTurn();
                    cardActionList3.Add(aEndTurn3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}