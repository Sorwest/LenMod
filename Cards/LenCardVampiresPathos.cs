using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using System.Linq;

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
            description = ModEntry.Instance.Localizations.Localize(["card", "VampiresPathoS", "description", upgrade.ToString()])
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
        var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        int shieldNumber;
        int enemyDamage;
        int internalCounter = upgrade == Upgrade.B ? 2 : 1;
        if (artifactBananaStash is null || artifactBananaStash.counter <= 0)
        {
            return result;
        }
        else
        {
            enemyDamage = artifactBananaStash.enemyDamage;
            shieldNumber = artifactBananaStash.shieldNumber;
            internalCounter = upgrade == Upgrade.B ? 2 : 1;
            if (artifactBananaStash.counter < 2)
                internalCounter = 1;
        }
        result.Insert(0, new AThrowBanana()
        {
            amount = -1
        });
        do
        {
            if (internalCounter <= 0)
                break;
            if (shieldNumber > 0)
            {
                result.Insert(result.Count - 2, new AStatus()
                {
                    status = Status.shield,
                    statusAmount = shieldNumber,
                    targetPlayer = true
                });
            }
            result.Insert(result.Count - 2, new AAttack()
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