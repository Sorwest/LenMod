using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardRemoteControl : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("RemoteControl", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RemoteControl", "name"]).Localize
        });
    }
    public override string Name() => "Remote Control";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AAttack()
            {
                damage = GetDmg(s, 0),
                moveEnemy = 1
            },
            new AAttack()
            {
                damage = GetDmg(s, upgrade == Upgrade.None ? 2 : 3),
                moveEnemy = -1
            },
            new AAttack()
            {
                damage = GetDmg(s, upgrade == Upgrade.B ? 1 : 0),
                moveEnemy = 1
            }
        };
    }
}