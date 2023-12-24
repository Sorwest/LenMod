namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardLawEvadingRock : Card
    {
        public override string Name() => "LawEvading Rock";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.cost = 0;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.recycle = false;
                    break;
                case Upgrade.A:
                    result.recycle = false;
                    break;
                case Upgrade.B:
                    result.recycle = true;
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
                    Asteroid asteroid1 = new Asteroid();
                    aSpawn1.thing = asteroid1;
                    cardActionList1.Add(aSpawn1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    ASpawn aSpawn2 = new ASpawn();
                    Asteroid asteroid2 = new Asteroid();
                    aSpawn2.thing = asteroid2;
                    aSpawn2.offset = -1;
                    cardActionList2.Add(aSpawn2);
                    ASpawn aSpawn3 = new ASpawn();
                    Asteroid asteroid3 = new Asteroid();
                    aSpawn3.thing = asteroid3;
                    aSpawn3.offset = 0;
                    cardActionList2.Add(aSpawn3);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    ASpawn aSpawn4 = new ASpawn();
                    Asteroid asteroid4 = new Asteroid();
                    aSpawn4.thing = asteroid4;
                    cardActionList3.Add(aSpawn4);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}