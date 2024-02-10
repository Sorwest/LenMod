using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using System.Linq;

namespace Sorwest.LenMod.Cards;
public class LenCardBananaWall : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("BananaWall", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BananaWall", "name"]).Localize
        });
    }
    public override string Name() => "Banana Wall";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            description = ModEntry.Instance.Localizations.Localize(["card", "BananaWall", "description"], new { Amount = upgrade == Upgrade.B ? 3 : 2 })
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        var flagNoBananas = false;
        var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash is null || artifactBananaStash.counter <= 0)
        {
                flagNoBananas = true;
        }
        return new()
        {
            new ASmashBanana()
            {
                amount = -1,
                disabled = flagNoBananas
            },
            new AStatus()
            {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.B ? 3 : 2,
                targetPlayer = true,
                disabled = flagNoBananas
            }
        };
    }
}