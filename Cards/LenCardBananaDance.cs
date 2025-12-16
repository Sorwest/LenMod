using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardBananaDance : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("BananaDance", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BananaDance", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1
        };
    }
    private static int GetBananaDmg(State s)
    {
        bool normalDisplay = s.route is not Combat || s.ship.Get(BananaManager.BananaStatus.Status) >= 0;
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return normalDisplay ? dmg : 0;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        bool normalDisplay = s.route is not Combat || s.ship.Get(BananaManager.BananaStatus.Status) > 0;
        List<CardAction> result =
        [
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(
                new ABananaDamage()
                {
                    damage = GetBananaDmg(s),
                    targetPlayer = false
                }).SetShowTooltips(true).AsCardAction
        ];
        ExternalAPI.IKokoroApi.IV2.IActionCostsApi.IResourceCost spoofCostResource = ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.EatBananaStatus.Status), amount: 1);
        if (normalDisplay)
            spoofCostResource.CostUnsatisfiedIconOverride = [ModEntry.Instance.Sprites["EatBanana"].Sprite];
        CardAction spoofAction = ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
            ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                spoofCostResource,
                new AStatus()
                {
                    status = Status.evade,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    targetPlayer = true
                }).AsCardAction,
            ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                    ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.BananaStatus.Status),
                    amount: 1),
                new AStatus()
                {
                    status = Status.evade,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    targetPlayer = true
                }).AsCardAction
            ).AsCardAction;
        spoofAction.omitFromTooltips = true;
        result.Add(spoofAction);
        result.Add(ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new AStatus()
                {
                    status = Status.evade,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    targetPlayer = true
                },
                new ADummyAction()
                ).AsCardAction).SetShowTooltips(true).AsCardAction);
        return result;
    }
}