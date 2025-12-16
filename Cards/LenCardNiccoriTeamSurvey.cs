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
            cost = upgrade == Upgrade.A ? 2 : 3,
            exhaust = upgrade == Upgrade.B ? false : true
        };
    }
    private static int GetBananaDmg(State s)
    {
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return s.route is not Combat ? dmg : s.ship.Get(BananaManager.BananaStatus.Status) > 0 ? dmg : 0;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        int amount = s.ship.Get(BananaManager.BananaStatus.Status);
        List<CardAction> result =
        [
            new AVariableHint()
            {
                status = BananaManager.BananaStatus.Status,
            },
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                new ABananaAttack()
                {
                    xHint = 1,
                    damage = GetDmg(s, amount * GetBananaDmg(s)),
                    targetPlayer = false
                },
                new ABananaDamage()
                {
                    isThrow = true,
                    damage = GetDmg(s, amount * GetBananaDmg(s)),
                    targetPlayer = false
                }
            ).AsCardAction,
            new AGainBanana() { loseAll = true }
        ];
        if (upgrade != Upgrade.None)
        {
            result.Add(new AGainBanana()
            {
                amount = upgrade == Upgrade.A ? 1 : 2
            });
        }
        return result;
    }
}