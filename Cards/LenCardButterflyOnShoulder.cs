using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardButterflyOnShoulder : Card, IRegisterable
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
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 2 : 4,
            exhaust = upgrade == Upgrade.B ? false : true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            new AStatus()
            {
                status = MusicNoteManager.MusicNoteStatus.Status,
                statusAmount = 12,
                mode = AStatusMode.Set,
                targetPlayer = true
            }
        ];
    }
}