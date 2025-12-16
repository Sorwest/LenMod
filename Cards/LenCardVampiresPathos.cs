using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardVampiresPathos : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("VampiresPathoS", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "VampiresPathoS", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2
        };
    }
    private static int GetBananaDmg(State state)
    {
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return state.route is not Combat ? dmg : state.ship.Get(BananaManager.BananaStatus.Status) > 0 ? dmg : 0;
    }
    public override List<CardAction> GetActions(State state, Combat c)
    {
        List<CardAction> result =
        [
            ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                    ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.BananaStatus.Status),
                    amount: 1
                    ),
                    new AHeal()
                    {
                        healAmount = 1,
                        targetPlayer = true
                    }
                ).AsCardAction
        ];
        result.Add(ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
            new ABananaHunger()
            {
                hurtAmount = GetBananaDmg(state),
                targetPlayer = true
            },
            new ABananaDamage()
            {
                damage = GetBananaDmg(state)
            }).AsCardAction
        );
        if (upgrade == Upgrade.B)
        {
            result.Add(ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                    ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.BananaStatus.Status),
                    amount: 2
                    ),
                    new AHeal()
                    {
                        healAmount = 1,
                        targetPlayer = true
                    }
                ).AsCardAction);
        }
        return result;
    }
}