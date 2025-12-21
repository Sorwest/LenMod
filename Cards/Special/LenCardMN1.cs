using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
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
                dontOffer = true,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FanLetter", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 0,
            retain = upgrade != Upgrade.None,
            temporary = true
        };
    }
    private static int GetBananaDmg(State state)
    {
        bool normalDisplay = state.route is not Combat || state.ship.Get(BananaManager.BananaStatus.Status) >= 0;
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return normalDisplay ? dmg : 0;
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaAttack()
                {
                    damage = GetDmg(state, (upgrade == Upgrade.B ? 1 : 0) + GetBananaDmg(state))
                },
                new ABananaDamage()
                {
                    damage = GetDmg(state, (upgrade == Upgrade.B ? 1 : 0) + GetBananaDmg(state)),
                    isThrow = true
                }
            ).AsCardAction,
            new ADrawCard()
            {
                count = 1
            }
        ];
    }
}
