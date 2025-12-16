using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class LenCardMN3 : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("LenCardMN3", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TrendSetter", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 2,
            retain = true,
            singleUse = true,
            temporary = true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
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
            new AStatus()
            {
                status = BananaManager.BananaStatus.Status,
                statusAmount = 2,
                targetPlayer = true
            },
            new ASpawn() { thing = new Missile() { missileType = MissileType.seeker, bubbleShield = true } }
        ];
    }
}
