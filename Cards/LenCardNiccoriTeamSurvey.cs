using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

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
        List<CardAction> result = new();
        if (s.ship.Get(ModEntry.Instance.BananaStatus.Status) > 0 || s.route is not Combat)
        {
            int internalCounter = s.ship.Get(ModEntry.Instance.BananaStatus.Status);
            result.Add(new AThrowBanana()
            {
                loseAll = true
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