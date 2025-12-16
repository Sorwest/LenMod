using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class MikuCardMN1 : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("MikuCardMN1", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.MikuDeck.Deck,
                rarity = Rarity.common,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FanLetter", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            retain = true,
            singleUse = true,
            temporary = true
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return
        [
            new ASpawn()
            {
                thing = new Asteroid()
            }
        ];
    }
}
