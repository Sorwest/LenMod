using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardToluthinAntenna : Card, IRegisterable
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
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "ToluthinAntenna", "description"], new { Amount = upgrade == Upgrade.B ? 4 : 2 })
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new()
        {
            new AStatus()
            {
                status = MusicNoteManager.MusicNoteStatus.Status,
                statusAmount = 1,
                targetPlayer = true
            },
            new ACardOffering()
            {
                amount = upgrade == Upgrade.B ? 4 : 2,
                makeAllCardsTemporary = true,
                canSkip = false,
                inCombat = true,
                discount = -1
            }
        };
        return result;
    }
}