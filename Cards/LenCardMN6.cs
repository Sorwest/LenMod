using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class LenCardMN6 : Card, IRegisterable
{
    public static IModSoundEntry BoundlessSound1 { get; set; } = null!;
    public static IModSoundEntry BoundlessSound2 { get; set; } = null!;
    public static IModSoundEntry BoundlessSound3 { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("LenCardMN6", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                dontOffer = true,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "CloseToGray", "name"]).Localize
        });
        BoundlessSound1 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/boundless1.mp3"));
        BoundlessSound2 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/boundless2.mp3"));
        BoundlessSound3 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/boundless3.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            temporary = true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ARandomSoundDummyAction()
            {
                sounds = [BoundlessSound1,BoundlessSound2,BoundlessSound3]
            }).AsCardAction,
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.A ? 2 : 1),
                piercing = true
            },
            new ADrawCard()
            {
                count = upgrade == Upgrade.B ? 2 : 1
            }
        ];
    }
}
