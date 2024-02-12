using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardVampiresPathos : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("VampiresPathoS", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "VampiresPathoS", "name"]).Localize
        });
    }
    public override string Name() => "Vampire's PathoS";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            description = ModEntry.Instance.Localizations.Localize(["card", "VampiresPathoS", "description"], new { Amount = upgrade == Upgrade.B ? 2 : 1 })
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new()
        {
            new AHeal()
            {
                healAmount = 1,
                targetPlayer = true
            }
        };
        if (s.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0 || s.route is not Combat)
        {
            int internalCounter = s.ship.Get(ModEntry.Instance.BananaStatus.Status) == 1 ? 1 : 2;
            do
            {
                if (internalCounter <= 0)
                    break;
                result.Add(new AThrowBanana()
                {
                    amount = -1
                });
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