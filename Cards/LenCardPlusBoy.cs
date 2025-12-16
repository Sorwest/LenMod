using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardPlusBoy : Card, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("PlusBoy", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "PlusBoy", "name"]).Localize
        });
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            exhaust = upgrade == Upgrade.None,
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            upgrade == Upgrade.B ?
                new AStatus()
                {
                    status = MusicNoteManager.MusicNoteStatus.Status,
                    statusAmount = 1,
                    targetPlayer = true
                } :
                new AGainBanana()
                {
                    amount = 3
                }
        ];
    }
}
