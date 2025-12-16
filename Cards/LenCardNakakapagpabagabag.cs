using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Linq;
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
            buoyant = upgrade == Upgrade.B ? true : false
        };
    }
    private static int GetBananaDmg(State s)
    {
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return s.route is not Combat ? dmg : s.ship.Get(BananaManager.BananaStatus.Status) > 0 ? dmg : 0;
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        ExternalAPI.IKokoroApi.IV2.IActionCostsApi.IResourceCost spoofCostResource = ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
            ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(
                s.EnumerateAllArtifacts().OfType<LenArtifactGuillotine>().FirstOrDefault() is not null ? BananaManager.ThrowBananaPiercingStatus.Status : BananaManager.ThrowBananaStatus.Status),
                amount: 1
            );
        if (s.ship.Get(BananaManager.BananaStatus.Status) > 0)
            spoofCostResource.CostUnsatisfiedIconOverride = [ModEntry.Instance.Sprites["ThrowBanana"].Sprite];
        CardAction spoofCostAction = ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
            spoofCostResource,
            new AStatus()
            {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true
            }).AsCardAction;
        spoofCostAction.omitFromTooltips = true;
        List<CardAction> result =
        [
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(spoofCostAction,new ASoundDummyAction()
            {
                sound = NakakapagSound
            }).AsCardAction
        ];
        result.Add(ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(
            new ABananaAttack()
            {
                damage = GetDmg(s, GetBananaDmg(s)),
                targetPlayer = false
            }).SetShowTooltips(true).AsCardAction);
        result.Add(ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(
            new AStatus()
            {
                status = Status.overdrive,
                statusAmount = s.ship.Get(BananaManager.BananaStatus.Status) > 0 ? 1 : 0,
                targetPlayer = true
            }).SetShowTooltips(true).AsCardAction);
        return result;
    }
}