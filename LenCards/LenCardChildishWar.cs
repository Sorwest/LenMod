namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardChildishWar : Card
    {
        public override string Name() => "Childish War﻿";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.exhaust = true;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 3;
                    break;
                case Upgrade.A:
                    result.cost = 2;
                    break;
                case Upgrade.B:
                    result.cost = 0;
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
                    aStatus1.status = Status.payback;
                    aStatus1.statusAmount = 1;
                    aStatus1.targetPlayer = true;
                    cardActionList1.Add(aStatus1);
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = Status.tempPayback;
                    aStatus2.statusAmount = 1;
                    aStatus2.targetPlayer = false;
                    cardActionList1.Add(aStatus2);
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = Status.tempShield;
                    aStatus3.statusAmount = 10;
                    aStatus3.targetPlayer = true;
                    cardActionList1.Add(aStatus3);
                    AAttack aAttack1 = new AAttack();
                    aAttack1.damage = GetDmg(s, 2);
                    cardActionList1.Add(aAttack1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AStatus aStatus4 = new AStatus();
                    aStatus4.status = Status.payback;
                    aStatus4.statusAmount = 1;
                    aStatus4.targetPlayer = true;
                    cardActionList2.Add(aStatus4);
                    AStatus aStatus5 = new AStatus();
                    aStatus5.status = Status.tempPayback;
                    aStatus5.statusAmount = 1;
                    aStatus5.targetPlayer = false;
                    cardActionList2.Add(aStatus5);
                    AStatus aStatus6 = new AStatus();
                    aStatus6.status = Status.tempShield;
                    aStatus6.statusAmount = 10;
                    aStatus6.targetPlayer = true;
                    cardActionList2.Add(aStatus6);
                    AAttack aAttack2 = new AAttack();
                    aAttack2.damage = GetDmg(s, 3);
                    cardActionList2.Add(aAttack2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AStatus aStatus7 = new AStatus();
                    aStatus7.status = Status.payback;
                    aStatus7.statusAmount = 1;
                    aStatus7.targetPlayer = true;
                    cardActionList3.Add(aStatus7);
                    AStatus aStatus8 = new AStatus();
                    aStatus8.status = Status.tempPayback;
                    aStatus8.statusAmount = 1;
                    aStatus8.targetPlayer = false;
                    cardActionList3.Add(aStatus8);
                    AStatus aStatus9 = new AStatus();
                    aStatus9.status = Status.tempShield;
                    aStatus9.statusAmount = 4;
                    aStatus9.targetPlayer = true;
                    cardActionList3.Add(aStatus9);
                    AAttack aAttack3 = new AAttack();
                    aAttack3.damage = GetDmg(s, 3);
                    cardActionList3.Add(aAttack3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}