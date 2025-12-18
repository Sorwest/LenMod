using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

internal class MikuCardMN6 : Card, IRegisterable
{
    /*public static IModSoundEntry WorldIsMine1 { get; set; } = null!;
    public static IModSoundEntry WorldIsMine2 { get; set; } = null!;
    public static IModSoundEntry WorldIsMine3 { get; set; } = null!;
    */
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("MikuCardMN6", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.MikuDeck.Deck,
                rarity = Rarity.rare,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "WorldIsMine", "name"]).Localize
        });
        /*WorldIsMine1 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/worldismine1.mp3"));
        WorldIsMine2 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/worldismine2.mp3"));
        WorldIsMine3 = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/worldismine3.mp3"));*/
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            temporary = true,
            retain = true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return [
            ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new AHeal(){ healAmount = 2, targetPlayer = false }).AsCardAction,
            //ModEntry.Instance.KokoroApi.OnTurnEnd.MakeAction(new ARandomSoundDummyAction() { sounds = [WorldIsMine1,WorldIsMine2,WorldIsMine3] }).AsCardAction
        ];
    }
}
