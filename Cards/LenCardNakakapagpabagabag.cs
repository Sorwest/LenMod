using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardNakakapagpabagabag : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Nakakapagpabagabag", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Nakakapagpabagabag", "name"]).Localize
        });
    }
    public override string Name() => "Nakakapagpabagabag";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            buoyant = upgrade == Upgrade.B ? true : false,
            description = ModEntry.Instance.Localizations.Localize(["card", "Nakakapagpabagabag", "description"])
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
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true
                }
            };
        }
        return result;
    }
}