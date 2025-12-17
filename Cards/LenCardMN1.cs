using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class LenCardMN1 : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("LenCardMN1", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
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
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.B ? 2 : upgrade == Upgrade.A ? 1 : 0),
                stunEnemy = true
            },
            new AGainBanana()
            {
                amount = upgrade == Upgrade.A ? 2 : 1
            }
        ];
    }
}
