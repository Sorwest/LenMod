namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardFunkyNightTown : Card
    {
        public override string Name() => "Funky Night Town";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
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
                    aStatus1.status = Status.tempShield;
                    aStatus1.statusAmount = 1;
                    aStatus1.targetPlayer = true;
                    cardActionList1.Add(aStatus1);
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.shield;
                    aStatus2.statusAmount = 1;
                    aStatus2.targetPlayer = true;
                    cardActionList1.Add(aStatus2);
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.evade;
                    aStatus3.statusAmount = 1;
                    aStatus3.targetPlayer = true;
                    cardActionList1.Add(aStatus3);
                    AAttack aAttack1 = new AAttack();
                    aAttack1.damage = GetDmg(s, 1);
                    cardActionList1.Add(aAttack1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AStatus aStatus4 = new AStatus();
                    aStatus4.status = Status.tempShield;
                    aStatus4.statusAmount = 1;
                    aStatus4.targetPlayer = true;
                    cardActionList2.Add(aStatus4);
                    AStatus aStatus5 = new AStatus();
                    aStatus5.status = Status.tempShield;
                    aStatus5.statusAmount = 1;
                    aStatus5.targetPlayer = true;
                    cardActionList2.Add(aStatus5);
                    AStatus aStatus6 = new AStatus();
                    aStatus6.status = Status.evade;
                    aStatus6.statusAmount = 1;
                    aStatus6.targetPlayer = true;
                    cardActionList2.Add(aStatus6);
                    AAttack aAttack2 = new AAttack();
                    aAttack2.damage = GetDmg(s, 1);
                    cardActionList2.Add(aAttack2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AStatus aStatus7 = new AStatus();
                    aStatus7.status = Status.tempShield;
                    aStatus7.statusAmount = 1;
                    aStatus7.targetPlayer = true;
                    cardActionList3.Add(aStatus7);
                    AStatus aStatus8 = new AStatus();
                    aStatus8.status = Status.shield;
                    aStatus8.statusAmount = 1;
                    aStatus8.targetPlayer = true;
                    cardActionList3.Add(aStatus8);
                    AStatus aStatus9 = new AStatus();
                    aStatus9.status = Status.evade;
                    aStatus9.statusAmount = 1;
                    aStatus9.targetPlayer = true;
                    cardActionList3.Add(aStatus9);
                    AAttack aAttack3 = new AAttack();
                    aAttack3.damage = GetDmg(s, 2);
                    aAttack3.piercing = true;
                    cardActionList3.Add(aAttack3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}