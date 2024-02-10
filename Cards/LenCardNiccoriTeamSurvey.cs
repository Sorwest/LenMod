using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using System.Linq;

namespace Sorwest.LenMod.Cards;

public class LenCardNiccoriTeamSurvey : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("NiccoriTeamSurvey", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "NiccoriTeamSurvey", "name"]).Localize
        });
    }
    public override string Name() => "Niccori Team Survey";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 2 : 3,
            exhaust = upgrade == Upgrade.B ? false : true,
            description = ModEntry.Instance.Localizations.Localize(["card", "NiccoriTeamSurvey", "description"])
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        int shieldNumber;
        int enemyDamage;
        int internalCounter;
        if (artifactBananaStash is null || artifactBananaStash.counter <= 0)
        {
            return new();
        }
        else
        {
            enemyDamage = artifactBananaStash.enemyDamage;
            shieldNumber = artifactBananaStash.shieldNumber;
            internalCounter = artifactBananaStash.counter;
        }
        List<CardAction> result = new()
        {
            new AThrowBanana()
            {
                loseAll = true
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