using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardFifthPierrot : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("FifthPierrot", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FifthPierrot", "name"]).Localize
        });
    }
    public override string Name() => "Fifth Pierrot";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            exhaust = upgrade == Upgrade.B ? false : true
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new()
        {
            new AStatus()
            {
                status = Status.powerdrive,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.shield,
                statusAmount = 0,
                mode = AStatusMode.Set,
                targetPlayer = true
            },
            new AEndTurn()
        };
        if (upgrade == Upgrade.A)
            result.Insert(0, new AAttack()
            {
                damage = GetDmg(s, 0),
                stunEnemy = true
            });
        return result;
    }
}