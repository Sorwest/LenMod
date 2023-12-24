namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardRemoteControl : Card
    {
        public override string Name() => "Remote Control﻿";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 1;
                    break;
                case Upgrade.A:
                    result.cost = 0;
                    break;
                case Upgrade.B:
                    result.cost = 1;
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
                    AAttack aAttack1 = new AAttack();
                    aAttack1.damage = GetDmg(s, 0);
                    aAttack1.moveEnemy = 1;
                    cardActionList1.Add(aAttack1);
                    AAttack aAttack2 = new AAttack();
                    aAttack2.damage = GetDmg(s, 2);
                    aAttack2.moveEnemy = -1;
                    cardActionList1.Add(aAttack2);
                    AAttack aAttack3 = new AAttack();
                    aAttack3.damage = GetDmg(s, 0);
                    aAttack3.moveEnemy = 1;
                    cardActionList1.Add(aAttack3);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    AAttack aAttack4 = new AAttack();
                    aAttack4.damage = GetDmg(s, 0);
                    aAttack4.moveEnemy = 1;
                    cardActionList2.Add(aAttack4);
                    AAttack aAttack5 = new AAttack();
                    aAttack5.damage = GetDmg(s, 3);
                    aAttack5.moveEnemy = -1;
                    cardActionList2.Add(aAttack5);
                    AAttack aAttack6 = new AAttack();
                    aAttack6.damage = GetDmg(s, 0);
                    aAttack6.moveEnemy = 1;
                    cardActionList2.Add(aAttack6);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    AAttack aAttack7 = new AAttack();
                    aAttack7.damage = GetDmg(s, 0);
                    aAttack7.moveEnemy = 1;
                    cardActionList3.Add(aAttack7);
                    AAttack aAttack8 = new AAttack();
                    aAttack8.damage = GetDmg(s, 3);
                    aAttack8.moveEnemy = -1;
                    cardActionList3.Add(aAttack8);
                    AAttack aAttack9 = new AAttack();
                    aAttack9.damage = GetDmg(s, 1);
                    aAttack9.moveEnemy = 1;
                    cardActionList3.Add(aAttack9);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}