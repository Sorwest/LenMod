using Nickel;
using FSPRO;
using System.Collections.Generic;
using System.Linq;
using Sorwest.LenMod.Features;

namespace Sorwest.LenMod.Actions;
public class AButterflyField : CardAction
{
    public override void Begin(G g, State s, Combat c)
    {
        foreach (StuffBase item in c.stuff.Values.ToList())
        {
            c.stuff.Remove(item.x);
            Butterfly newStuff = new()
            {
                x = item.x,
                xLerped = item.xLerped,
                bubbleShield = item.bubbleShield,
                targetPlayer = item.targetPlayer,
                age = item.age
            };
            c.stuff[item.x] = newStuff;
        }

        Audio.Play(Event.Status_PowerUp);
    }

    public override List<Tooltip> GetTooltips(State s)
    {
        if (s.route is Combat combat)
        {
            foreach (StuffBase value in combat.stuff.Values)
            {
                value.hilight = 2;
            }
        }

        return new List<Tooltip>
        {
            new GlossaryTooltip($"midrow.{ModEntry.Instance.Package.Manifest.UniqueName}::Butterfly")
            {
                Icon = ModEntry.Instance.Sprites["ButterflyField"].Sprite,
                TitleColor = Colors.midrow,
                Title = ModEntry.Instance.Localizations.Localize(["midrow", "Butterfly", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["midrow", "Butterfly", "description"]),
            }
        };
    }

    public override Icon? GetIcon(State s)
    {
        return new Icon(ModEntry.Instance.Sprites["ButterflyField"].Sprite, null, Colors.textMain);
    }
}