using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
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
                rarity = Rarity.uncommon,
                dontOffer = true,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TrendSetter", "name"]).Localize
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
                damage = GetDmg(state, (int)upgrade + 2),
                piercing = true
            },
            new AStatus()
            {
                status = ModEntry.Instance.LenCharacter.MissingStatus.Status,
                statusAmount = 1,
                targetPlayer = true
            }
        ];
    }
}
