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
        List<CardAction> result = [];
        if (upgrade == Upgrade.B)
        {
            result.Add(ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaHunger()
                {
                    hurtAmount = GetBananaDmg(state),
                    targetPlayer = true
                },
                new ABananaDamage()
                {
                    damage = GetBananaDmg(state),
                    targetPlayer = false
                }
                ).AsCardAction);
            result.Add(ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaHunger()
                {
                    hurtAmount = GetBananaDmg(state),
                    targetPlayer = true,
                    minimumBanana = 3
                },
                new ABananaDamage()
                {
                    damage = GetBananaDmg(state),
                    targetPlayer = false,
                    minimumBanana = 3
                }
                ).AsCardAction);
        }
        else
        {
            result.Add(ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                    new ABananaAttack()
                    {
                        damage = GetDmg(state, GetBananaDmg(state))
                    },
                    new ABananaDamage()
                    {
                        damage = GetDmg(state, GetBananaDmg(state)),
                        isThrow = true
                    }
                    ).AsCardAction);
        }
        if (upgrade == Upgrade.A)
        {
            result.Add(ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                    new ABananaAttack()
                    {
                        damage = GetDmg(state, GetBananaDmg(state))
                    },
                    new ABananaDamage()
                    {
                        damage = GetDmg(state, GetBananaDmg(state)),
                        isThrow = true
                    }
                    ).AsCardAction);
            result.Add(ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                    new ABananaAttack()
                    {
                        damage = GetDmg(state, GetBananaDmg(state))
                    },
                    new ABananaDamage()
                    {
                        damage = GetDmg(state, GetBananaDmg(state)),
                        isThrow = true
                    }
                    ).AsCardAction);
        }
        else
        {
            result.Insert(1,ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
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
                ).AsCardAction);
        }
        return result;
    }
}