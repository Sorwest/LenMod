using Nickel;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod.Features;

internal sealed class Butterfly : StuffBase
{
    internal static List<Spr> animationButterfly = [.. Enumerable.Range(1, 6).Select(i => ModEntry.Instance.Sprites[$"Butterfly_{i}"].Sprite)];
    public override Spr? GetIcon()
    {
        return ModEntry.Instance.Sprites["ButterflyField"].Sprite;
    }
    public override double GetWiggleAmount()
    {
        return 2.0;
    }

    public override double GetWiggleRate()
    {
        return 1.0;
    }
    public override void Render(G g, Vec v)
    {
        DrawWithHilight(g, ModEntry.Instance.Sprites["Butterfly"].Sprite, v + GetOffset(g), false, false);
        DrawWithHilight(g, animationButterfly.GetModulo((int)(g.state.time * 3.14 + (double)(x * 10))), v + GetOffset(g), false, false);
    }
    public override List<Tooltip> GetTooltips()
    {
        List<Tooltip> list = new List<Tooltip>
        {
            new GlossaryTooltip($"midrow.{ModEntry.Instance.Package.Manifest.UniqueName}::Butterfly")
            {
                Icon = ModEntry.Instance.Sprites["ButterflyField"].Sprite,
                TitleColor = Colors.midrow,
                Title = ModEntry.Instance.Localizations.Localize(["midrow", "Butterfly", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["midrow", "Butterfly", "description"]),
            }
        };
        if (bubbleShield)
        {
            list.Add(new TTGlossary("midrow.bubbleShield"));
        }

        return list;
    }
    public override List<CardAction>? GetActionsOnDestroyed(State s, Combat c, bool wasPlayer, int worldX)
    {
        foreach (Artifact item in s.EnumerateAllArtifacts())
        {
            item.OnAsteroidIsDestroyed(s, c, wasPlayer, worldX);
        }

        return [
            new AStatus
            {
                targetPlayer = wasPlayer,
                status = MusicNoteManager.MusicNoteStatus.Status,
                statusAmount = 1
            }
        ];
    }
}