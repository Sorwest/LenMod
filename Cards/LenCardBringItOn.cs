using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardBringItOn : Card, IRegisterable
{
    //public static IModSoundEntry BringitSound { get; set; } = null!;
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
        //BringitSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/bringiton.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            exhaust = true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        List<CardAction> result =
        [
            //ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ASoundDummyAction() { sound = BringitSound } ).AsCardAction,
            new ASpawn()
            {
                thing = new RinMidrow() { upgraded = upgrade == Upgrade.B },
                offset = -5
            },
            new ASpawn()
            {
                thing = new RinMidrow() { upgraded = upgrade == Upgrade.B },
                offset = 5
            }
        ];
        if (upgrade == Upgrade.B)
        {
            result.Add(new ASpawn()
            {
                thing = new RinMidrow() { targetPlayer = true, upgraded = true, bubbleShield = true }
            });
        }
        return result;
    }
}