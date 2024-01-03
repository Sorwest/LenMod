namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardServantOfEvil : Card
    {
        public override string Name() => "Servant of Evil";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 3;
                    result.exhaust = true;
                    break;
                case Upgrade.A:
                    result.cost = 3;
                    result.exhaust = true;
                    result.retain = true;
                    break;
                case Upgrade.B:
                    result.cost = 4;
                    result.exhaust = false;
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            List<CardAction> cardActionList1 = new List<CardAction>();
            AStatus aStatus1 = new AStatus();
            aStatus1.status = Status.serenity;
            aStatus1.statusAmount = 1;
            aStatus1.targetPlayer = true;
            cardActionList1.Add(aStatus1);
            AStatus aStatus2 = new AStatus();
            aStatus2.status = Status.lockdown;
            aStatus2.statusAmount = 2;
            aStatus2.targetPlayer = true;
            cardActionList1.Add(aStatus2);
            AStatus aStatus3 = new AStatus();
            aStatus3.status = Status.shield;
            aStatus3.statusAmount = 4;
            aStatus3.targetPlayer = true;
            cardActionList1.Add(aStatus3);
            AStatus aStatus4 = new AStatus();
            aStatus4.status = Status.tempShield;
            aStatus4.statusAmount = 2;
            aStatus4.targetPlayer = true;
            cardActionList1.Add(aStatus4);
            AStatus aStatus5 = new AStatus();
            aStatus5.status = Status.strafe;
            aStatus5.statusAmount = 1;
            aStatus5.targetPlayer = true;
            cardActionList1.Add(aStatus5);
            result = cardActionList1;
            return result;
        }
    }
}