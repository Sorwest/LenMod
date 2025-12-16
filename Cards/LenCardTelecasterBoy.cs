using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardTelecasterBBoy : Card, IRegisterable
{
    public static IModSoundEntry TelecasterSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("TelecasterBBoy", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "TelecasterBBoy", "name"]).Localize
        });
        TelecasterSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/telecaster.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            exhaust = true,
            cost = upgrade == Upgrade.A ? 1 : 2,
            retain = upgrade == Upgrade.B ? true : false
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return new()
        {
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ASoundDummyAction()
            {
                sound = TelecasterSound
            }).AsCardAction,
            new AStatus()
            {
                status = Status.droneShift,
                statusAmount = 1,
                targetPlayer = true
            },
            new AMove()
            {
                dir = 2,
                isRandom = true,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.evade,
                statusAmount = 3,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.loseEvadeNextTurn,
                statusAmount = 1,
                targetPlayer = true
            }
        };
    }
}