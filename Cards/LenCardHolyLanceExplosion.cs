using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
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
    private static int GetBananaDmg(State state)
    {
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return state.route is not Combat ? dmg : state.ship.Get(BananaManager.BananaStatus.Status) > 0 ? dmg : 0;
    }
    public override List<CardAction> GetActions(State state, Combat c)
    {
        List<CardAction> result =
        [
            new AAttack()
            {
                damage = GetDmg(state, GetBananaDmg(state) + (upgrade == Upgrade.B ? 1 : 0)),
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
                        missileType = MissileType.heavy,
                        skin = "sword"
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
        return result;
    }
}