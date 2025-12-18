using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class MikuCardMN4 : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("MikuCardMN4", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.MikuDeck.Deck,
                rarity = Rarity.uncommon,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Vocavoid", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 2,
            exhaust = true,
            retain = true,
            temporary = true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new AStatus()
            {
                status = ModEntry.Instance.LenCharacter.MissingStatus.Status,
                statusAmount = 1,
                targetPlayer = true
            }).AsCardAction,
            ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new ADrawCard()
            {
                count = 1
            }).AsCardAction
        ];
    }
}
