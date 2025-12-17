using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class LenCardMN4 : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("LenCardMN4", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Encore", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 3,
            retain = true,
            singleUse = true,
            temporary = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "Encore", "descriptionLen"])
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            new AAddCard()
            {
                amount = 1,
                card = new LenCardBanana() { discount = -1, temporaryOverride = true },
                destination = CardDestination.Deck,
                omitFromTooltips = true
            },
            new AAddCard()
            {
                amount = 1,
                card = new LenCardBanana() { discount = -1, temporaryOverride = true, upgrade = Upgrade.A },
                destination = CardDestination.Deck,
                omitFromTooltips = true
            },
            new AAddCard()
            {
                amount = 1,
                card = new LenCardBanana() { discount = -1, temporaryOverride = true, upgrade = Upgrade.B },
                destination = CardDestination.Deck,
                omitFromTooltips = true
            },
            new AAddCard()
            {
                amount = 1,
                card = new LenCardMN1() { discount = -1 },
                destination = CardDestination.Deck,
                omitFromTooltips = true
            },
            new AAddCard()
            {
                amount = 2,
                card = new LenCardMN1() { discount = -1, upgrade = Upgrade.A },
                destination = CardDestination.Deck,
                omitFromTooltips = true
            },
            new AAddCard()
            {
                amount = 2,
                card = new LenCardMN1() { discount = -1, upgrade = Upgrade.B },
                destination = CardDestination.Deck
            },
            new AAddCard()
            {
                amount = 1,
                card = new LenCardMN5() { discount = -1 },
                destination = CardDestination.Deck
            },
            new AAddCard()
            {
                amount = 1,
                card = new LenCardMN5() { discount = -1, upgrade = Upgrade.A },
                destination = CardDestination.Deck,
                omitFromTooltips = true
            },
            new AAddCard()
            {
                amount = 2,
                card = new LenCardMN5() { discount = -1, upgrade = Upgrade.B },
                destination = CardDestination.Deck,
                omitFromTooltips = true
            },
        ];
    }
}
