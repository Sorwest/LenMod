using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardButterflyOnShoulder : Card, IModdedCard
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("ButterflyOnShoulder", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ButterflyOnShoulder", "name"]).Localize
        });
    }
    public override string Name() => "Butterfly On Shoulder";
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 2 : 4,
            exhaust = true,
            description = ModEntry.Instance.Localizations.Localize(["card", "ButterflyOnShoulder", "description", upgrade.ToString()])
        };
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        List<CardAction> result = new();
        if (s.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0 || s.route is not Combat)
        {
            int internalCounter = s.ship.Get(ModEntry.Instance.BananaStatus.Status);
            result = new()
            {
                new ASmashBanana()
                {
                    loseAll = true
                },
                new AStatus()
                {
                    status = ModEntry.Instance.MusicNoteStatus.Status,
                    statusAmount = internalCounter * (upgrade == Upgrade.B ? 2 : 1),
                    targetPlayer = true
                }
            };
        }
        return result;
    }
}