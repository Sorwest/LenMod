using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardLikeDislike : Card, IModdedCard
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
    public override string Name() => "Like Dislike";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 2 : 3,
            flippable = upgrade == Upgrade.B ? true : false
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return new()
        {
            new AAttack()
            {
                damage = GetDmg(s, upgrade == Upgrade.B ? 1 : 0),
                stunEnemy = true
            },
            new AAttack()
            {
                damage = GetDmg(s, 1),
                piercing = true
            },
            new AMove()
            {
                dir = 2,
                isRandom = upgrade == Upgrade.B ? false : true
            },
            new AAttack()
            {
                damage = GetDmg(s, upgrade == Upgrade.B ? 3 : 2)
            }
        };
    }
}