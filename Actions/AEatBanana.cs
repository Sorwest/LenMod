﻿using Sorwest.LenMod.Artifacts;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod.Actions;
public class AEatBanana : AGainBanana
{
    private int GetBananaDmg(State s)
    {
        var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        if (artifactBananaStash != null)
        {
            return artifactBananaStash.enemyDamage;
        }
        return 1;
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
        return new Icon(ModEntry.Instance.Sprites["EatBanana"].Sprite, null, Colors.textMain);
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        var shieldAmount = GetBananaShield(s);
        var damageAmount = GetBananaDmg(s);
        var str = "";
        if (shieldAmount > 0)
        {
            str = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "maidDress"], new { Amount = shieldAmount });
        }
        return [
            new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.Sprites["EatBanana"].Sprite,
                () => ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "name"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "description"], new { Damage = damageAmount, MaidDress = str }),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::EatBanana"
            ),
            .. base.GetTooltips(s)
        ];
    }
}