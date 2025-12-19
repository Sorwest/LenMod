using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class MikuCardMN5 : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("MikuCardMN5", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.MikuDeck.Deck,
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
            cost = 39,
            temporary = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "Encore", "descriptionMiku"])
        };
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        return (HashSet<ICardTraitEntry>)ModEntry.Instance.KokoroApi.Fleeting.Trait;
    }

    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new AAddCard()
            {
                amount = 1,
                card = new MikuCardMN1(),
                destination = CardDestination.Deck,
            }).AsCardAction,
            ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new AAddCard()
            {
                amount = 2,
                card = new MikuCardMN3(),
                destination = CardDestination.Deck
            }).AsCardAction,
            ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new AAddCard()
            {
                amount = 3,
                card = new MikuCardMN6(),
                destination = CardDestination.Deck
            }).AsCardAction
        ];
    }
}
