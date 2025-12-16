using FSPRO;

namespace Sorwest.LenMod.Actions;

public class IntentSwapAllPartToEmptyType : Intent
{
    public required string keyNormal;
    public required string keyEmpty;
    public required PType type;
    public override void Apply(State s, Combat c, Ship fromShip, int actualX)
    {
        c.Queue(new ASwapPart()
        {
            keyNormal = keyNormal,
            keyEmpty = keyEmpty,
            type = type
        });
    }

    public override string GetSingleTooltip(State s, Combat c, Ship fromShip)
    {
        return "intent.completeMystery";
    }
}
public class ASwapPart : CardAction
{
    public required string keyNormal;
    public required string keyEmpty;
    public required PType type;
    public override void Begin(G g, State s, Combat c)
    {
        Ship ship = c.otherShip;
        Audio.Play(Event.TogglePart);
        foreach (Part part in ship.parts)
        {
            if (part.skin == keyNormal)
            {
                part.type = PType.empty;
                part.skin = keyEmpty;
            }
            else if (part.skin == keyEmpty)
            {
                part.type = type;
                part.skin = keyNormal;
            }
        }
    }
}