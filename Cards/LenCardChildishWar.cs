using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardChildishWar : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("ChildishWar", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ChildishWar", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.None ? 3 : (upgrade == Upgrade.A ? 2 : 0),
            retain = upgrade == Upgrade.A
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        List<CardAction> result = [
            new AStatus()
            {
                status = Status.payback,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.tempPayback,
                statusAmount = 1,
                targetPlayer = false
            },
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.B ? 2 : 1)
            }
        ];
        if (upgrade != Upgrade.B)
            result.Insert(2, new AStatus()
            {
                status = Status.tempShield,
                statusAmount = 6,
                targetPlayer = true
            });
        return result;
    }
}