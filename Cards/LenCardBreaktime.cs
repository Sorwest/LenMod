using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardBreaktime : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Breaktime", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Breaktime", "name"]).Localize
        });
    }
    public override string Name() => "Breaktime";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            description = ModEntry.Instance.Localizations.Localize(["card", "Breaktime", "description"], new { Amount = upgrade == Upgrade.A ? 3 : 2 })
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new();
        if (s.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0 || s.route is not Combat)
        {
            int internalCounter = upgrade == Upgrade.A ? 3 : 2;
            result.Add(new AThrowBanana()
            {
                amount = -1
            });
            do
            {
                if (internalCounter <= 0)
                    break;
                result.Add(new ABananaDamage()
                {
                    type = BananaType.AAttack,
                    dmg = GetDmg(s, 0),
                    targetPlayer = false
                });
                internalCounter--;
            }
            while (internalCounter > 0);
        }
        return result;
    }
}