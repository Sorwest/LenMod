using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardNiccoriTeamSurvey : Card, IRegisterable
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
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.B ? 4 : (upgrade == Upgrade.None ? 3 : 2),
            exhaust = true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        int amount = state.ship.Get(BananaManager.BananaStatus.Status) + ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage"); ;
        List<CardAction> result =
        [
            new AVariableHint()
            {
                status = BananaManager.BananaStatus.Status,
            },
            new AStatus()
            {
                xHint = upgrade == Upgrade.B ? 2 : 1,
                status = MusicNoteManager.MusicNoteStatus.Status,
                statusAmount = (upgrade == Upgrade.B ? 2 : 1) * state.ship.Get(BananaManager.BananaStatus.Status),
                targetPlayer = true
            },
            new AGainBanana() { loseAll = true }
        ];
        if (upgrade == Upgrade.B)
        {
            result.Add(new AGainBanana()
            {
                amount = upgrade == Upgrade.A ? 1 : 2
            });
        }
        return result;
    }
}