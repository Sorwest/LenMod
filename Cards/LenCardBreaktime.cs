using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardBreaktime : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Breaktime", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                dontOffer = true,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Breaktime", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
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
        List<CardAction> result =
        [
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaAttack()
                {
                    damage = GetDmg(state, GetBananaDmg(state))
                },
                new ABananaDamage()
                {
                    damage = GetDmg(state, GetBananaDmg(state)),
                    isThrow = true
                }
                ).AsCardAction,
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaHunger()
                {
                    hurtAmount = GetBananaDmg(state),
                    targetPlayer = true,
                    minimumBanana = 2
                },
                new ABananaDamage()
                {
                    damage = GetBananaDmg(state),
                    targetPlayer = false,
                    minimumBanana = 2
                }
                ).AsCardAction
        ];
        if (upgrade == Upgrade.A)
        {
            result.Add(
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaHunger()
                {
                    hurtAmount = GetBananaDmg(state),
                    targetPlayer = true
                },
                new ABananaDamage()
                {
                    damage = GetBananaDmg(state),
                    keepBanana = true,
                    minimumBanana = 3
                }).AsCardAction
            );
        }
        else if (upgrade == Upgrade.B)
        {
            result.Add(ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                    ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.BananaStatus.Status),
                    amount: 1
                ),
                new AGainBanana()
                {
                    amount = 4
                }
                ).AsCardAction);
        }
        return result;
    }
}