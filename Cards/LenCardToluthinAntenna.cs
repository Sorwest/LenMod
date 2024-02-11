using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardToluthinAntenna : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("ToluthinAntenna", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ToluthinAntenna", "name"]).Localize
        });
    }
    public override string Name() => "Toluthin Antenna";
    public override CardData GetData(State state)
    {
        return new()
        {
            exhaust = true,
            cost = upgrade == Upgrade.A ? 1 : 2,
            description = ModEntry.Instance.Localizations.Localize(["card", "ToluthinAntenna", "description"], new { Amount = upgrade == Upgrade.B ? 4 : 2 })
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new ACardOffering()
            {
                amount = upgrade == Upgrade.B ? 4 : 2,
                limitDeck = Deck.hacker,
                makeAllCardsTemporary = true,
                canSkip = false,
                inCombat = true,
                discount = -1
            },
            new ACardOffering()
            {
                amount = upgrade == Upgrade.B ? 4 : 2,
                limitDeck = Deck.peri,
                makeAllCardsTemporary = true,
                canSkip = false,
                inCombat = true,
                discount = -1
            }
        };
    }
}