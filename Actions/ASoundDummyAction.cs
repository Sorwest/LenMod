using Nickel;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod.Actions;

public class ASoundDummyAction : CardAction
{
    public IModSoundEntry? sound;
    public override void Begin(G g, State state, Combat combat)
    {
        if (!ModEntry.Instance.Settings.ProfileBased.Current.EnabledSounds)
            return;
        timer = 0;
        sound?.CreateInstance();
    }
}
public class ARandomSoundDummyAction : CardAction
{
    public required Dictionary<IModSoundEntry, double> sounds;
    public override void Begin(G g, State state, Combat combat)
    {
        if (!ModEntry.Instance.Settings.ProfileBased.Current.EnabledSounds)
            return;
        double choice = state.rngActions.Next();
        double num = 0.0;
        foreach ((IModSoundEntry sound, double weight) in sounds.OrderBy(x => x.Value))
        {
            if (choice <= (num + weight))
            {
                sound?.CreateInstance();
                return;
            }
            else
            {
                num += weight;
            }
        }
        
    }
}