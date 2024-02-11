using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardBanana : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Banana", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Banana", "name"]).Localize
        });
    }
    public override string Name() => "Banana";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            exhaust = upgrade == Upgrade.B ? false : true
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AGainBanana()
            {
                amount = upgrade == Upgrade.A ? 3 : 2
            },
            new ADrawCard()
            {
                count = upgrade == Upgrade.B ? 2 : 1
            }
        };
    }
}