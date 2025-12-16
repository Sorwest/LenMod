using Nickel;
using System.Collections.Generic;

namespace Sorwest.LenMod.Actions;
public class ASoundDummyAction : CardAction
{
    public IModSoundEntry? sound;
    public override void Begin(G g, State s, Combat c)
    {
        if (!ModEntry.Instance.Settings.ProfileBased.Current.EnabledSounds)
            return;
        timer = 0;
        if (sound is not ISoundEntry)
            sound?.CreateInstance();
    }
}
public class ARandomSoundDummyAction : CardAction
{
    public required List<IModSoundEntry> sounds;
    public int? weight;
    public override void Begin(G g, State s, Combat c)
    {
        if (!ModEntry.Instance.Settings.ProfileBased.Current.EnabledSounds)
            return;
        if (weight is not null)
        {
            int counter = (int)weight;
            do
            {
                sounds.Insert(0, sounds[0]);
                counter--;
            }
            while (counter > 0);
            IModSoundEntry sound = sounds[s.rngActions.NextInt() % sounds.Count];
            if (sound is not ISoundEntry)
                sound?.CreateInstance();
        }
        else
        {
            IModSoundEntry sound = sounds[s.rngActions.NextInt() % sounds.Count];
            if (sound is not ISoundEntry)
                sound?.CreateInstance();
        }
    }
}