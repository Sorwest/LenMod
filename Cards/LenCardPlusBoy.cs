using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardPlusBoy : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("PlusBoy", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PlusBoy", "name"]).Localize
        });
    }
    public override string Name() => "Plus Boy";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 2,
            singleUse = upgrade == Upgrade.B ? false : true
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AGainBanana()
            {
                amount = upgrade == Upgrade.None ? 6 : (upgrade == Upgrade.A ? 9 : 4)
            }
        };
    }
}