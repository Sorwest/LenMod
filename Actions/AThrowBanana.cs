using Sorwest.LenMod.Artifacts;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod.Actions;
public class AThrowBanana : AGainBanana
{
    private int GetBananaDmg(State s)
    {
        var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash != null)
        {
            return artifactBananaStash.enemyDamage;
        }
        return 0;
    }
    private int GetBananaShield(State s)
    {
        var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash != null)
        {
            return artifactBananaStash.shieldNumber;
        }
        return 0;
    }
    public override Icon? GetIcon(State s)
    {
        return new Icon(ModEntry.Instance.Sprites["ThrowBanana"].Sprite, null, Colors.textMain);
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = new List<Tooltip>();
        var shieldAmount = GetBananaShield(s);
        var damageAmount = GetBananaDmg(s);
        var str = "";
        if (shieldAmount > 0)
        {
            str = ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "flavor"], new { Amount = shieldAmount });
        }
        tooltips.Add(new CustomTTGlossary(
            CustomTTGlossary.GlossaryType.action,
            () => ModEntry.Instance.Sprites["ThrowBanana"].Sprite,
            () => ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "name"]),
            () => ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "description"], new { Damage = damageAmount, MaidDress = str }),
            key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::ThrowBanana"
        ));
        return tooltips;
    }
}
