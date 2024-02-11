using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardHolyLanceExplosion : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("HolyLanceExplosion", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "HolyLanceExplosion", "name"]).Localize
        });
    }
    public override string Name() => "Holy Lance Explosion";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = upgrade == Upgrade.B ? false : true,
            description = ModEntry.Instance.Localizations.Localize(["card", "HolyLanceExplosion", "description"])
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new()
        {
            new AStatus()
            {
                status = Status.droneShift,
                statusAmount = 1,
                targetPlayer = true
            }
        };
        if (s.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0 || s.route is not Combat)
        {
            result.Add(new ASmashBanana()
            {
                amount = -1
            });
            result.Add(new ASpawn()
            {
                thing = new Missile()
                {
                    missileType = MissileType.heavy,
                    skin = "sword"
                }
            });
        }
        return result;
    }
}