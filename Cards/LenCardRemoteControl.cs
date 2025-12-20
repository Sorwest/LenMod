using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardRemoteControl : Card, IRegisterable
{
    public static IModSoundEntry RemoteControlSound { get; set; } = null!;
    public static IModSoundEntry TetorisSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("RemoteControl", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.uncommon,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "RemoteControl", "name"]).Localize
        });
        RemoteControlSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/remotecontrol.mp3"));
        TetorisSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/tetoris.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return new()
        {
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ARandomSoundDummyAction()
            {
                sounds = new Dictionary<IModSoundEntry, double>
                {
                    [RemoteControlSound] = 0.9,
                    [TetorisSound] = 0.1
                }
            }).AsCardAction,
            new AAttack()
            {
                damage = GetDmg(state, 0),
                moveEnemy = 1
            },
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.None ? 2 : 3),
                moveEnemy = -1
            },
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.B ? 1 : 0),
                moveEnemy = 1
            }
        };
    }
}