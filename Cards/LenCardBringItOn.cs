using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardBringItOn : Card, IRegisterable
{
    public static IModSoundEntry BringitSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("BringItOn", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "BringItOn", "name"]).Localize
        });
        BringitSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/bringiton.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 2,
            buoyant = upgrade == Upgrade.A ? true : false,
            exhaust = upgrade == Upgrade.B ? true : false
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        List<CardAction> result = new();
        switch (upgrade)
        {
            case Upgrade.None:
                result = [
                    new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 2,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.tempPayback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AEndTurn()
                ];
                break;
            case Upgrade.A:
                result =
                [
                    new AStatus()
                    {
                        status = Status.shield,
                        statusAmount = 4,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.tempPayback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AEndTurn()
                ];
                break;
            case Upgrade.B:
                result =
                [
                    new AStatus()
                    {
                        status = Status.tempPayback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AStatus()
                    {
                        status = Status.payback,
                        statusAmount = 1,
                        targetPlayer = true
                    },
                    new AEndTurn()
                ];
                break;
        }
        result.Insert(0, ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ASoundDummyAction()
        {
            sound = BringitSound
        }).AsCardAction);
        return result;
    }
}