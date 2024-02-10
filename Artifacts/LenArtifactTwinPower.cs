using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactTwinPower : Artifact, IModdedArtifact
{
    public static void Register(IModHelper helper)
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
    public override string Name() => "BRIOCHE";
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
            AStatus aStatus1 = new AStatus();
            aStatus1.status = Status.overdrive;
            aStatus1.statusAmount = 2;
            aStatus1.targetPlayer = false;
            combat.QueueImmediate(aStatus1);
        }
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        return new()
        {
            new TTGlossary("status.overdrive", 2),
        };
    }
}
