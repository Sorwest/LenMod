using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactTwinPower : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("TwinPower", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = ModEntry.Instance.Sprites["TwinPower"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TwinPower", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "TwinPower", "description"]).Localize
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
        => [
            ..StatusMeta.GetTooltips(Status.overdrive, 2)
        ];
    public override void OnReceiveArtifact(State state)
    {
        state.ship.baseEnergy += 1;
    }
    public override void OnRemoveArtifact(State state)
    {
        state.ship.baseEnergy -= 1;
    }
    public override void OnTurnStart(State state, Combat combat)
    {
        if (combat.turn % 2 == 0)
        {
            combat.QueueImmediate(new AStatus()
            {
                status = Status.overdrive,
                statusAmount = 2,
                targetPlayer = false
            });
            Pulse();
        }
    }
}
