using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardBarisolChild : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("BarisolsChild", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BarisolsChild", "name"]).Localize
        });
    }
    public override string Name() => "Barisol's Child";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = upgrade == Upgrade.B ? false : true,
            buoyant = upgrade == Upgrade.B ? true : false
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new ASpawn()
            {
                thing = new ShieldDrone()
                {
                    targetPlayer = true
                },
                offset = -1
            },
            new ASpawn()
            {
                thing = new AttackDrone()
                {
                    targetPlayer = false,
                    upgraded = upgrade == Upgrade.None ? false : true
                },
                offset = 1
            }
        };
    }
}