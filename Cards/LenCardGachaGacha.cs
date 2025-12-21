using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardGachaGacha : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("LenCardGachaGacha", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "GachaGacha", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        int brioche = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage");

        return (List<CardAction>)(upgrade switch
        {
            Upgrade.B => [
                new AAttack()
                {
                    damage = GetDmg(state, 5)
                }
            ],
            _ => [
                ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                    ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                        ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.BananaStatus.Status),
                        amount: 2
                    ),
                    ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                        new AAttack()
                        {
                            damage = GetDmg(state, 5 + brioche)
                        },
                        new ABananaAttack()
                        {
                            minimumBanana = 2,
                            damage = GetDmg(state, 5 + brioche)
                        }
                    ).AsCardAction
                ).AsCardAction
            ]
        });
    }
}