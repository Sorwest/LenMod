using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using System.Linq;

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
        var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        int shieldNumber;
        int enemyDamage;
        int internalCounter = upgrade == Upgrade.A ? 3 : 2;
        if (artifactBananaStash is null || artifactBananaStash.counter <= 0)
        {
            return new();
        }
        else
        {
            enemyDamage = artifactBananaStash.enemyDamage;
            shieldNumber = artifactBananaStash.shieldNumber;
        }
        List<CardAction> result = new()
        {
            new AThrowBanana()
            {
                amount = -1
            }
        };
        do
        {
            if (internalCounter <= 0)
                break;
            if (shieldNumber > 0)
            {
                result.Add(new AStatus()
                {
                    status = Status.shield,
                    statusAmount = shieldNumber,
                    targetPlayer = true
                });
            }
            result.Add(new AAttack()
            {
                damage = GetDmg(s, enemyDamage),
                piercing = true
            });
            internalCounter -= 1;
        }
        while (internalCounter > 0);
        return result;
    }
}