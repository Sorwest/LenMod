using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;
public class LenCardFifthPierrot : Card, IRegisterable
{
    public static IModSoundEntry PierrotSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("FifthPierrot", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "FifthPierrot", "name"]).Localize
        });
        PierrotSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/pierrot.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = 1,
            exhaust = upgrade == Upgrade.B ? false : true
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        List<CardAction> result = [
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ASoundDummyAction()
            {
                sound = PierrotSound
            }).AsCardAction,
            new AStatus()
            {
                status = Status.powerdrive,
                statusAmount = 1,
                targetPlayer = true
            },
            new AStatus()
            {
                status = Status.shield,
                statusAmount = 0,
                mode = AStatusMode.Set,
                targetPlayer = true
            },
            new AEndTurn()
        ];
        if (upgrade == Upgrade.A)
            result.Insert(1, new AAttack()
            {
                damage = GetDmg(state, 0),
                stunEnemy = true
            });
        return result;
    }
}