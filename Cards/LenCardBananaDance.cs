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
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            ModEntry.Instance.KokoroApi.Conditional.MakeAction(
                ModEntry.Instance.KokoroApi.Conditional.Equation(
                    ModEntry.Instance.KokoroApi.Conditional.Status(BananaManager.BananaStatus.Status),
                    ExternalAPI.IKokoroApi.IV2.IConditionalApi.EquationOperator.LessThanOrEqual,
                    ModEntry.Instance.KokoroApi.Conditional.Constant(4),
                    ExternalAPI.IKokoroApi.IV2.IConditionalApi.EquationStyle.Possession),
                new AStatus()
                {
                    status = Status.evade,
                    statusAmount = upgrade == Upgrade.B ? 2 : 1,
                    targetPlayer = true
                }).AsCardAction,
            new AGainBanana()
            {
                amount = 1
            }
        ];
    }
}