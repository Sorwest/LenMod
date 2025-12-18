using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;
namespace Sorwest.LenMod.Cards;

public class LenCardNakakapagpabagabag : Card, IRegisterable
{
    public static IModSoundEntry NakakapagSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("Nakakapagpabagabag", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "Nakakapagpabagabag", "name"]).Localize
        });
        NakakapagSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/nakakapag.mp3"));

    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            buoyant = upgrade == Upgrade.B
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            new AAttack()
            {
                damage = GetDmg(state, 0)
            },
            ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                    ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.BananaStatus.Status),
                    amount: 1),
                new AStatus()
                {
                    status = Status.overdrive,
                    statusAmount = 1,
                    targetPlayer = true
                }
            ).AsCardAction
        ];
    }
}