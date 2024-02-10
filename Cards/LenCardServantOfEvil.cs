using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardServantOfEvil : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Banana", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Banana", "name"]).Localize
        });
    }
    public override string Name() => "Servant of Evil";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.B ? 3 : 2,
            exhaust = upgrade == Upgrade.B ? false : true,
            retain = upgrade == Upgrade.A ? true : false
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AStatus()
            {
                status = Status.serenity,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.lockdown,
                statusAmount = 2,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.shield,
                statusAmount = 4,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.strafe,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = ModEntry.Instance.LenCharacter.MissingStatus.Status,
                statusAmount = 1,
                targetPlayer = true
            }
        };
    }
}