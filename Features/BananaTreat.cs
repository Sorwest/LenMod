using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using Sorwest.LenMod.ExternalAPI;
using System.Linq;

namespace Sorwest.LenMod.Features;

internal sealed class BananaTreatManager : IRegisterable, IKokoroApi.IV2.IStatusLogicApi.IHook
{
    internal static IStatusEntry BananaTreatStatus { get; private set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        BananaTreatStatus = helper.Content.Statuses.RegisterStatus("BananaTreatStatus", new()
        {
            Definition = new()
            {
                icon = ModEntry.Instance.Sprites["BananaTreat"].Sprite,
                color = new Color("8cfffb"),
                isGood = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["status", "BananaTreat", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["status", "BananaTreat", "description"]).Localize
        });

        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(new BananaTreatLogicHook());
    }
    private sealed class BananaTreatLogicHook : IKokoroApi.IV2.IStatusLogicApi.IHook
    {
        public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args)
        {
            if (args.Timing != IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnStart)
                return false;
            if (args.Status == BananaManager.BananaStatus.Status
                && args.Ship.Get(args.Status) > 0
                && args.Combat.turn == 1
                && args.State.EnumerateAllArtifacts().OfType<LenArtifactBananaSnack>().FirstOrDefault() is not null)
            {
                args.Combat.Queue(new AStatus()
                {
                    status = BananaManager.BananaStatus.Status,
                    statusAmount = -1,
                    targetPlayer = args.Ship.isPlayerShip,
                    timer = 0
                });
                args.Combat.Queue(new AStatus()
                {
                    status = BananaTreatStatus.Status,
                    statusAmount = 1,
                    targetPlayer = args.Ship.isPlayerShip,
                    timer = 0

                });
                args.Combat.Queue(new ABananaTreatDamage()
                {
                    targetPlayer = !args.Ship.isPlayerShip
                });
                return false;
            }
            if (args.Status != BananaTreatStatus.Status)
                return false;
            if (args.Amount == 0)
                return false;
            args.Combat.Queue(new ABananaTreatDamage()
            {
                targetPlayer = !args.Ship.isPlayerShip
            });
            return false;
        }
    }
}