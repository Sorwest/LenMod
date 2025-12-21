using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;
namespace Sorwest.LenMod.Cards;

public class LenCardNakakapagpabagabag : Card, IRegisterable
{
    //public static IModSoundEntry NakakapagSound { get; set; } = null!;
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
        //NakakapagSound = ModEntry.Instance.Helper.Content.Audio.RegisterSound(ModEntry.Instance.Package.PackageRoot.GetRelativeFile("assets/sound/nakakapag.mp3"));

    }
    public override CardData GetData(State state)
    {
        return new()
        {
            cost = upgrade == Upgrade.A ? 0 : 1,
            buoyant = upgrade == Upgrade.B
        };
    }
    private static int GetBananaDmg(State state)
    {
        bool normalDisplay = state.route is not Combat || state.ship.Get(BananaManager.BananaStatus.Status) >= 0;
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return normalDisplay ? dmg : 0;
    }
    public override List<CardAction> GetActions(State state, Combat combat)
    {
        return
        [
            //ModEntry.Instance.KokoroApi.HiddenActions.MakeAction(new ASoundDummyAction() { sound = NakakapagSound }).AsCardAction,
            ModEntry.Instance.KokoroApi.SpoofedActions.MakeAction(
                    new ABananaAttack()
                    {
                        damage = GetDmg(state, GetBananaDmg(state))
                    },
                    new ABananaDamage()
                    {
                        damage = GetDmg(state, GetBananaDmg(state)),
                        isThrow = true
                    }
                    ).AsCardAction,
            new AStatus()
            {
                status = Status.overdrive,
                statusAmount = 1,
                targetPlayer = true
            }
        ];
    }
}