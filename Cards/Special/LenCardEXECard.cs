using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenEXECard : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("LenExe", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = Deck.colorless,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "LenExe", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "LenExe", "description"], new { Amount = upgrade == Upgrade.B ? 3 : 2 })
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            new AStatus()
            {
                status = BananaTreeManager.BananaTreeStatus.Status,
                statusAmount = 1,
                targetPlayer = true
            },
            new ACardOffering()
            {
                amount = upgrade == Upgrade.B ? 3 : 2,
                limitDeck = ModEntry.Instance.LenDeck.Deck,
                makeAllCardsTemporary = true,
                canSkip = false,
                inCombat = true,
                discount = -1
            }
        ];
    }
}