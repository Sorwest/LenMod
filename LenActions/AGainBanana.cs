using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LenMod.LenArtifacts;

namespace LenMod.LenActions
{
    public class AGainBanana : CardAction
    {
        public int amount;
        public override void Begin(G g, State s, Combat c)
        {
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                artifactBananaStash.counter += amount;
                artifactBananaStash.Pulse();
            }
            else if (artifactBananaStash == null)
            {
                s.artifacts.Add(new LenArtifactBananaStash());
                artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
                artifactBananaStash?.OnReceiveArtifact(s);
                artifactBananaStash?.Pulse();
            }
        }
    }
}
