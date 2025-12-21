using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sorwest.LenMod.Cards;

public class LenCardHolyLanceExplosion : Card, IRegisterable
{
    public static IModSoundEntry HolyLanceSound { get; set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("HolyLanceExplosion", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                upgradesTo = [Upgrade.A, Upgrade.B]
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "HolyLanceExplosion", "name"]).Localize
        });
        HolyLanceSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(
            ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/holylance.mp3"));
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 1 : 2,
            buoyant = upgrade == Upgrade.B
        };
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        bool brioche = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") > 0;
        List<CardAction> result =
        [
            new AAttack()
            {
                damage = GetDmg(state, upgrade == Upgrade.B ? 2 : 1),
                piercing = true,
                fast = true
            },
            ModEntry.Instance.KokoroApi.ActionCosts.MakeCostAction(
                ModEntry.Instance.KokoroApi.ActionCosts.MakeResourceCost(
                    ModEntry.Instance.KokoroApi.ActionCosts.MakeStatusResource(BananaManager.BananaStatus.Status),
                    amount: 1
                ),
                new ASpawn()
                {
                    thing = new Missile()
                    {
                        missileType = brioche ? MissileType.heavy : MissileType.normal,
                        skin = brioche ? "sword" : null
                    }
                }).AsCardAction
        ];
        if (state.ship.Get(BananaManager.BananaStatus.Status) > 0)
            result.Add(ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(
                new ASoundDummyAction()
                {
                    sound = HolyLanceSound
                }).AsCardAction
            );
        if (brioche)
        {
            state.EnumerateAllArtifacts().OfType<LenArtifactMaidDress>().FirstOrDefault()?.Pulse();
        }
        return result;
    }
}