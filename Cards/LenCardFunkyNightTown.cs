using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardFunkyNightTown : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("FunkyNightTown", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FunkyNightTown", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.B ? 1 : 2
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return new()
        {
            new AStatus()
            {
                status = Status.tempShield,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = upgrade == Upgrade.B ? Status.tempShield : Status.shield,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.evade,
                statusAmount = 1,
                targetPlayer = true
            },
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.A ? 2 : upgrade == Upgrade.B ? 0 : 1)
            }
        };
    }
}