using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardParadichlorobenzene : Card, IRegisterable, IHasCustomCardTraits
{
    public static IModSoundEntry BenzeneSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Paradichlorobenzene", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.rare,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Paradichlorobenzene", "name"]).Localize
        });
        BenzeneSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/benzene.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 3 : 4,
            exhaust = true
        };
    }
    public IReadOnlySet<ICardTraitEntry> GetInnateTraits(State state)
    {
        return new HashSet<ICardTraitEntry> { ModEntry.Instance.KokoroApi.Fleeting.Trait };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        List<CardAction> result = new()
        {
            ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ASoundDummyAction()
            {
                sound = BenzeneSound
            }).AsCardAction,
            ModEntry.Instance.KokoroApi.Conditional.MakeAction(
                ModEntry.Instance.KokoroApi.Conditional.HasStatus(BananaManager.BananaStatus.Status),
                new AStatus()
                {
                    status = Status.shield,
                    statusAmount = 0,
                    mode = AStatusMode.Set,
                    targetPlayer = false
                }).AsCardAction,
            new AGainBanana() { loseAll = true }
        };
        if (upgrade == Upgrade.B)
            result.Add(new AGainBanana() { amount = 1 });
        return result;
    }
}