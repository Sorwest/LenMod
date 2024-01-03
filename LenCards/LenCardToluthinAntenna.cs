namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardToluthinAntenna : Card
    {
        public override string Name() => "Toluthin Antenna";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.exhaust = true;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 2;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardToluthinAntenna?.DescLocKey ?? throw new Exception("Missing card")), 2);
                    break;
                case Upgrade.A:
                    result.cost = 1;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardToluthinAntenna?.DescLocKey ?? throw new Exception("Missing card")), 2);
                    break;
                case Upgrade.B:
                    result.cost = 2;
                    result.description = string.Format(Loc.GetLocString(Manifest.LenCardToluthinAntenna?.DescLocKey ?? throw new Exception("Missing card")), 3);
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
                    ACardOffering acardOffering1 = new ACardOffering();
                    acardOffering1.amount = 2;
                    acardOffering1.limitDeck = new Deck?(Deck.hacker);
                    acardOffering1.makeAllCardsTemporary = true;
                    acardOffering1.overrideUpgradeChances = new bool?(false);
                    acardOffering1.canSkip = false;
                    acardOffering1.inCombat = true;
                    acardOffering1.discount = -1;
                    cardActionList1.Add(acardOffering1);
                    ACardOffering acardOffering2 = new ACardOffering();
                    acardOffering2.amount = 2;
                    acardOffering2.limitDeck = new Deck?(Deck.peri);
                    acardOffering2.makeAllCardsTemporary = true;
                    acardOffering2.overrideUpgradeChances = new bool?(false);
                    acardOffering2.canSkip = false;
                    acardOffering2.inCombat = true;
                    acardOffering2.discount = -1;
                    cardActionList1.Add(acardOffering2);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    ACardOffering acardOffering3 = new ACardOffering();
                    acardOffering3.amount = 2;
                    acardOffering3.limitDeck = new Deck?(Deck.hacker);
                    acardOffering3.makeAllCardsTemporary = true;
                    acardOffering3.overrideUpgradeChances = new bool?(false);
                    acardOffering3.canSkip = false;
                    acardOffering3.inCombat = true;
                    acardOffering3.discount = -1;
                    cardActionList2.Add(acardOffering3);
                    ACardOffering acardOffering4 = new ACardOffering();
                    acardOffering4.amount = 2;
                    acardOffering4.limitDeck = new Deck?(Deck.peri);
                    acardOffering4.makeAllCardsTemporary = true;
                    acardOffering4.overrideUpgradeChances = new bool?(false);
                    acardOffering4.canSkip = false;
                    acardOffering4.inCombat = true;
                    acardOffering4.discount = -1;
                    cardActionList2.Add(acardOffering4);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    ACardOffering acardOffering5 = new ACardOffering();
                    acardOffering5.amount = 3;
                    acardOffering5.limitDeck = new Deck?(Deck.hacker);
                    acardOffering5.makeAllCardsTemporary = true;
                    acardOffering5.overrideUpgradeChances = new bool?(false);
                    acardOffering5.canSkip = false;
                    acardOffering5.inCombat = true;
                    acardOffering5.discount = -1;
                    cardActionList3.Add(acardOffering5);
                    ACardOffering acardOffering6 = new ACardOffering();
                    acardOffering6.amount = 3;
                    acardOffering6.limitDeck = new Deck?(Deck.peri);
                    acardOffering6.makeAllCardsTemporary = true;
                    acardOffering6.overrideUpgradeChances = new bool?(false);
                    acardOffering6.canSkip = false;
                    acardOffering6.inCombat = true;
                    acardOffering6.discount = -1;
                    cardActionList3.Add(acardOffering6);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}