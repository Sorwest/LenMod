using Nickel;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod.Features;

internal sealed class RinMidrow : StuffBase
{
    public bool upgraded;
    public static List<Spr> exhaustSprites = DB.GetSpriteListOrdered("effects_missileExhaust_missileExhaust_");
    public override Spr? GetIcon()
    {
        return ModEntry.Instance.Sprites[(upgraded ? "RinMidrowMK2" : "RinMidrow")].Sprite;
    }
    public override double GetWiggleAmount()
    {
        return 1.2;
    }

    public override double GetWiggleRate()
    {
        return 0.8;
    }
    public int AttackDamage()
    {
        return upgraded ? 6 : 3;
    }
    public override void Render(G g, Vec v)
    {
        Vec vec1 = v + GetOffset(g, doRound: true);
        Vec vec2 = vec1 + (!targetPlayer ? new Vec(0.0, 20.0) : default(Vec)) + new Vec(8.0, 14.0);
        double num = vec2.x - 5.0;
        double y = vec2.y + (double)((!targetPlayer) ? 14 : 0);
        Vec? originRel = new Vec(0.0, 1.0);
        Draw.Sprite(exhaustSprites.GetModulo((int)(g.state.time * 36.0 + (double)(x * 10))), num, y, false, !targetPlayer, 0.0, null, originRel, null, null, new Color("fff387"));
        string str = (targetPlayer ? (upgraded ? "RinFrontMK2" : "RinFront") : (upgraded ? "RinBackMK2" : "RinBack"));
        DrawWithHilight(g, ModEntry.Instance.Sprites[str].Sprite, vec1, false, false);
    }
    public override List<CardAction>? GetActions(State s, Combat c)
    {
        return
        [
            new AAttack
            {
                fromDroneX = x,
                targetPlayer = targetPlayer,
                damage = AttackDamage()
            }
        ];
    }
    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> list =
        [
            new GlossaryTooltip($"midrow.{ModEntry.Instance.Package.Manifest.UniqueName}::Rin")
            {
                Icon = ModEntry.Instance.Sprites[(upgraded ? "RinMidrowMK2" : "RinMidrow")].Sprite,
                TitleColor = Colors.midrow,
                Title = ModEntry.Instance.Localizations.Localize(["midrow", "Rin", "name"], new { upgrade = upgraded ? " MK2" : ""}),
                Description = ModEntry.Instance.Localizations.Localize(["midrow", "Rin", "description"], new { Damage = AttackDamage() }),
            }
        ];
        if (bubbleShield)
        {
            list.Add(new TTGlossary("midrow.bubbleShield"));
        }
        return list;
    }
}