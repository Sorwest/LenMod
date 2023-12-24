namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.uncommon, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardBarisolChild : Card
    {
        public override string Name() => "Barisol's Child";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 2;
                    result.exhaust = true;
                    break;
                case Upgrade.A:
                    result.cost = 1;
                    result.exhaust = true;
                    break;
                case Upgrade.B:
                    result.cost = 2;
                    result.buoyant = true;
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
                    ASpawn aSpawn1 = new ASpawn();
                    ShieldDrone shieldDrone1 = new ShieldDrone();
                    shieldDrone1.targetPlayer = true;
                    aSpawn1.thing = shieldDrone1;
                    aSpawn1.offset = -1;
                    cardActionList1.Add(aSpawn1);
                    ASpawn aSpawn2 = new ASpawn();
                    AttackDrone attackDrone1 = new AttackDrone();
                    attackDrone1.targetPlayer = false;
                    aSpawn2.thing = attackDrone1;
                    aSpawn2.offset = 1;
                    cardActionList1.Add(aSpawn2);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    ASpawn aSpawn3 = new ASpawn();
                    ShieldDrone shieldDrone2 = new ShieldDrone();
                    shieldDrone2.targetPlayer = true;
                    aSpawn3.thing = shieldDrone2;
                    aSpawn3.offset = -1;
                    cardActionList2.Add(aSpawn3);
                    ASpawn aSpawn4 = new ASpawn();
                    AttackDrone attackDrone2 = new AttackDrone();
                    attackDrone2.targetPlayer = false;
                    attackDrone2.upgraded = true;
                    aSpawn4.thing = attackDrone2;
                    aSpawn4.offset = 1;
                    cardActionList2.Add(aSpawn4);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    ASpawn aSpawn5 = new ASpawn();
                    ShieldDrone shieldDrone3 = new ShieldDrone();
                    shieldDrone3.targetPlayer = true;
                    aSpawn5.thing = shieldDrone3;
                    aSpawn5.offset = -1;
                    cardActionList3.Add(aSpawn5);
                    ASpawn aSpawn6 = new ASpawn();
                    AttackDrone attackDrone3 = new AttackDrone();
                    attackDrone3.targetPlayer = false;
                    attackDrone3.upgraded = true;
                    aSpawn6.thing = attackDrone3;
                    aSpawn6.offset = 1;
                    cardActionList3.Add(aSpawn6);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}