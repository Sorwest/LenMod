using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardNiccoriTeamSurvey : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("NiccoriTeamSurvey", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "NiccoriTeamSurvey", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.B ? 4 : 2,
            exhaust = true,
        };
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        return (HashSet<ICardTraitEntry>)(upgrade switch
        {
            Upgrade.A => [],
            _ => [ModEntry.Instance.KokoroApi.Fleeting.Trait]
        });
    }
    private static int GetBananaDmg(State state)
    {
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage");
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        int amount = state.ship.Get(BananaManager.BananaStatus.Status);
        int num = upgrade == Upgrade.B ? 2 : 1;
        List<CardAction> result =
        [
            new AVariableHint()
            {
                status = BananaManager.BananaStatus.Status,
            },
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaAttack()
                {
                    xHint = num,
                    damage = GetDmg(state, num * ( amount + GetBananaDmg(state))),
                    targetPlayer = false
                },
                new ABananaDamage()
                {
                    isThrow = true,
                    damage = GetDmg(state, num * ( amount + GetBananaDmg(state))),
                    targetPlayer = false
                }
            ).AsCardAction,
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new AGainBanana() { loseAll = true }).AsCardAction
        ];
        return result;
    }
}