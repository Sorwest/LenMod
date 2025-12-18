using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardBanana : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Banana", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Banana", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return new()
        {
            ModEntry.Instance.KokoroApi.Conditional.MakeAction(
                ModEntry.Instance.KokoroApi.Conditional.Equation(
                    ModEntry.Instance.KokoroApi.Conditional.Status(BananaManager.BananaStatus.Status),
                    ExternalAPI.IKokoroApi.IV2.IConditionalApi.EquationOperator.LessThanOrEqual,
                    ModEntry.Instance.KokoroApi.Conditional.Constant(upgrade == Upgrade.A ? 7 : 4),
                    ExternalAPI.IKokoroApi.IV2.IConditionalApi.EquationStyle.Possession),
                new ADrawCard()
            {
                count = upgrade == Upgrade.B ? 3 : 1
            }).AsCardAction,
            new AGainBanana()
            {
                amount = 1
            }
        };
    }
}