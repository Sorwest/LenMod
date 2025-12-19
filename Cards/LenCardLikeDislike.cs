using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardLikeDislike : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("LikeDislike", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "LikeDislike", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 2 : 3,
            flippable = upgrade == Upgrade.B ? true : false
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
        return new()
        {
            new AAttack()
            {
                damage = GetDmg(state, 0),
                stunEnemy = true
            },
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
            new AMove()
            {
                dir = 2,
                isRandom = upgrade == Upgrade.B ? false : true
            },
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.B ? 3 : 2)
            }
        };
    }
}