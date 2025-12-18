using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Sorwest.LenMod.ExternalAPI.IKokoroApi;

namespace Sorwest.LenMod.Features;
internal sealed class Butterfly : StuffBase
{
    public static List<Spr> flappingSprites = [.. Enumerable.Range(1, 8).Select(i => ModEntry.Instance.Sprites[$"Butterfly_{i}"].Sprite)];
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
        Spr spr = flappingSprites.GetModulo((int)(g.state.time * 24.0 + (double)(x * 10)));
        DrawWithHilight(g, ModEntry.Instance.Sprites["ButterflyTorso"].Sprite, v + GetOffset(g), false, false);
        DrawWithHilight(g, spr, v + GetOffset(g), false, false);
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