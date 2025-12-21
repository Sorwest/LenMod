using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.ExternalAPI;

namespace Sorwest.LenMod.Features;

public class BananaTreeManager : IRegisterable
{
    internal static IStatusEntry BananaTreeStatus { get; private set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        BananaTreeStatus = helper.Content.Statuses.RegisterStatus("BananaTreeStatus", new()
        {
            Definition = new()
            {
                icon = ModEntry.Instance.Sprites["BananaTree"].Sprite,
                color = ModEntry.LenColor,
                border = new Color("00c009"),
                isGood = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["status", "BananaTree", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["status", "BananaTree", "description"]).Localize
        });
        ModEntry.Instance.KokoroApi.StatusLogic.RegisterHook(new BananaTreeLogicHook());
    }
    private sealed class BananaTreeLogicHook : IKokoroApi.IV2.IStatusLogicApi.IHook
    {
        public bool HandleStatusTurnAutoStep(IKokoroApi.IV2.IStatusLogicApi.IHook.IHandleStatusTurnAutoStepArgs args)
        {
            if (args.Status != BananaTreeStatus.Status)
                return false;
            int amount = args.Ship.Get(args.Status);
            if (amount <= 0)
                return false;
            if (args.Timing == IKokoroApi.IV2.IStatusLogicApi.StatusTurnTriggerTiming.TurnStart)
            {
                args.Combat.Queue(new AStatus() { status = BananaManager.BananaStatus.Status, statusAmount = amount * 2, targetPlayer = args.Ship.isPlayerShip });
                return false;
            }
            return false;
        }
    }
}
