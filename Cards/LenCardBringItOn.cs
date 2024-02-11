using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardBringItOn : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("BringItOn", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BringItOn", "name"]).Localize
        });
    }
    public override string Name() => "Bring It On";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 2,
            buoyant = upgrade == Upgrade.A ? true : false
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new();
        switch (upgrade)
        {
            case Upgrade.None:
                result = new()
                {
                    new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.tempPayback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AEndTurn()
                };
                break;
            case Upgrade.A:
                result = new()
                {
                    new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 3,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.tempPayback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AEndTurn()
                };
                break;
            case Upgrade.B:
                result = new()
                {
                    new AStatus()
                    {
                        status = Status.tempShield,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.tempPayback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.payback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AEndTurn()
                };
                break;
        }
        return result;
    }
}