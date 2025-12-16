using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardServantOfEvil : Card, IRegisterable
{
    public static IModSoundEntry ServantSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("ServantOfEvil", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "ServantOfEvil", "name"]).Localize
        });
        ServantSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/servant.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.B ? 1 : 2,
            exhaust = true,
            retain = upgrade == Upgrade.A ? true : false
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return new()
        {
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ASoundDummyAction()
            {
                sound = ServantSound,
                dialogueSelector = ".card_servantofevil_played"
            }).AsCardAction,
            new AStatus()
            {
                status = Status.serenity,
                statusAmount = upgrade == Upgrade.A ? 2 : 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.lockdown,
                statusAmount = upgrade == Upgrade.B ? 1 : 2,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.shield,
                statusAmount = upgrade == Upgrade.A ? 5 : 4,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.strafe,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = ModEntry.Instance.LenCharacter.MissingStatus.Status,
                statusAmount = upgrade == Upgrade.B ? 3 : 1,
                targetPlayer = true
            }
        };
    }
}