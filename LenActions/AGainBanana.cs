using LenMod.LenArtifacts;

namespace LenMod.LenActions
{
    public class AGainBanana : CardAction
    {
        public int amount;
        public bool loseAll;
        public override void Begin(G g, State s, Combat c)
        {
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                if (loseAll)
                    artifactBananaStash.counter = 0;
                else if (!loseAll)
                    artifactBananaStash.counter += amount;
                artifactBananaStash.Pulse();
            }
            else if (artifactBananaStash == null)
            {
                s.artifacts.Add(new LenArtifactBananaStash());
                artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
                if (!loseAll)
                    artifactBananaStash?.OnReceiveArtifact(s);
                artifactBananaStash?.Pulse();
            }
        }
        public override Icon? GetIcon(State s)
        {
            Icon icon = new Icon();
            if (amount < 0)
                icon = new Icon((Spr)(Manifest.LenActionGainBananaNegative!.Id!), amount * -1, Colors.textMain);
            else if (amount > 0)
                icon = new Icon((Spr)(Manifest.LenActionGainBananaPositive!.Id!), amount, Colors.textMain);
            else if (loseAll)
                icon = new Icon((Spr)(Manifest.LenActionGainBananaNegative!.Id!), null, Colors.textMain);
            return icon;
        }
        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>();
            if (amount < 0)
                tooltips.Add(new TTGlossary(Manifest.LenGlossaryGainBananaNegative?.Head ?? throw new Exception("missing LenGlossaryGainBananaNegative glossary"), amount * -1));
            else if (amount > 0)
                tooltips.Add(new TTGlossary(Manifest.LenGlossaryGainBananaPositive?.Head ?? throw new Exception("missing LenGlossaryGainBananaPositive glossary"), amount));
            else if (loseAll)
                tooltips.Add(new TTGlossary(Manifest.LenGlossaryGainBananaLoseAll?.Head ?? throw new Exception("missing LenGlossaryGainBananaLoseAll glossary"), null));
            return tooltips;
        }
    }
}
