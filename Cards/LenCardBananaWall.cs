using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardBananaWall : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("BananaWall", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BananaWall", "name"]).Localize
        });
    }
    public override string Name() => "Banana Wall";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            description = ModEntry.Instance.Localizations.Localize(["card", "BananaWall", "description"], new { Amount = upgrade == Upgrade.B ? 3 : 2 })
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new();
        if (s.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0 || s.route is not Combat)
        {
            result = new()
            {
                new ASmashBanana()
                {
                    amount = -1
                },
                new AStatus()
                {
                    status = Status.shield,
                    statusAmount = upgrade == Upgrade.B ? 3 : 2,
                    targetPlayer = true
                }
            };
        }
        return result;
    }
}