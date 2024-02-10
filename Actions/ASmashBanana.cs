using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod.Actions;
public class ASmashBanana : AGainBanana
{
    public override Icon? GetIcon(State s)
    {
        return new Icon(ModEntry.Instance.Sprites["SmashBanana"].Sprite, null, Colors.textMain);
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips =
        [
            new TTText(ModEntry.Instance.Localizations.Localize(["action", "SmashBanana", "flavor"])),
            .. base.GetTooltips(s)
        ];
        return tooltips;
    }
}
