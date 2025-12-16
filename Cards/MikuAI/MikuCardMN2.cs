using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class MikuCardMN2 : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("MikuCardMN2", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.MikuDeck.Deck,
                rarity = Rarity.common,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CrowdWork", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 3,
            retain = true,
            singleUse = true,
            temporary = true
        };
    }
    public override List<CardAction> GetActions(State state, Combat c)
    {
        return
        [
            new AStatus()
            {
                status = MusicNoteManager.MusicNoteStatus.Status,
                statusAmount = 1,
                targetPlayer = true,
                omitFromTooltips = true
            },
            ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new AStatus()
            {
                status = MusicNoteManager.MusicNoteStatus.Status,
                statusAmount = 2,
                targetPlayer = false,
                omitFromTooltips = true
            }).AsCardAction
        ];
    }
}
