using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;

namespace Sorwest.LenMod.Cards;
public class LenCardChildishWar : Card, IModdedCard
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
    public override string Name() => "Childish War";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.None ? 3 : (upgrade == Upgrade.A ? 2 : 0)
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
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
            new AStatus()
            {
                status = Status.tempShield,
                statusAmount = upgrade == Upgrade.B ? 4 : 10,
                targetPlayer = true
            },
            new AAttack()
            {
                damage = GetDmg(s, upgrade == Upgrade.B ? 3 : 2)
            }
        };
    }
}