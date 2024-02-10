using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Sorwest.LenMod.Actions;

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
            cost = upgrade == Upgrade.None ? 1 : 0,
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
                count = 1
            }
        };
    }
}