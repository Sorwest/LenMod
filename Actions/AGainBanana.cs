using Sorwest.LenMod.Artifacts;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod.Actions;
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
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaLose"].Sprite, amount * -1, Colors.textMain);
        else if (amount > 0)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaGain"].Sprite, amount, Colors.textMain);
        else if (loseAll)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite, null, Colors.textMain);
        return icon;
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = new();
        if (amount < 0)
            tooltips.Add(new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.Sprites["GainBananaLose"].Sprite,
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "lose"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "lose"]),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::LoseBanana"
            ));
        else if (amount > 0)
            tooltips.Add(new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.Sprites["GainBananaGain"].Sprite,
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "gain"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "gain"]),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::GainBanana"
            ));
        else if (loseAll)
            tooltips.Add(new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite,
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "lose"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "loseAll"]),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::LoseBananaAll"
            ));
        return tooltips;
    }
}