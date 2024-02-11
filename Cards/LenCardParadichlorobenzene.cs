using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardParadichlorobenzene : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Paradichlorobenzene", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Paradichlorobenzene", "name"]).Localize
        });
    }
    public override string Name() => "Paradichlorobenzene";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.None ? 2 : (upgrade == Upgrade.A ? 1 : 3),
            buoyant = upgrade == Upgrade.B ? true : false,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "Paradichlorobenzene", "description"])
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new()
        {
            new AGainBanana()
            {
                amount = 1
            }
        };
        if (s.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0 || s.route is not Combat)
        {
            result.Insert(0, new AStatus()
            {
                status = Status.shield,
                statusAmount = 0,
                mode = AStatusMode.Set,
                targetPlayer = false
            });
        }
        return result;
    }
}