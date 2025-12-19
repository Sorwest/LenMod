using Nickel;
using Sorwest.LenMod.Features;

namespace Sorwest.LenMod.Actions;

public class IntentButterfly : Intent
{

    public bool targetPlayer = true;

    public bool bubbleShield;

    public override string GetSingleTooltip(State s, Combat c, Ship fromShip)
    {
        return "intent.spawn";
    }

    public override void Apply(State s, Combat c, Ship fromShip, int actualX)
    {
        c.Queue(new ASpawn
        {
            fromPlayer = false,
            fromX = actualX,
            thing = new Butterfly
            {
                targetPlayer = targetPlayer,
                bubbleShield = bubbleShield
            }
        });
    }
    public override Spr? GetSprite(State s)
    {
        return StableSpr.hints_hint_missile;
    }
}