using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Features;
internal sealed class Butterfly : StuffBase
{
    public override Spr? GetIcon()
    {
        return ModEntry.Instance.Sprites["ButterflyField"].Sprite;
    }
    public override double GetWiggleAmount()
    {
        return 1.0;
    }

    public override double GetWiggleRate()
    {
        return 1.0;
    }
    public override void Render(G g, Vec v)
    {
        DrawWithHilight(g, ModEntry.Instance.Sprites["ButterflyDrone"].Sprite, v + GetOffset(g), Mutil.Rand((double)x + 0.1) > 0.5, Mutil.Rand((double)x + 0.2) > 0.5);
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