using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactGuillotine : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Guillotine", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.EventOnly]
            },
            Sprite = ModEntry.Instance.Sprites["Guillotine"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Guillotine", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Guillotine", "description"]).Localize
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        int dmg = 1;
        if (!(MG.inst.g?.state is not { } state || state.IsOutsideRun()))
            dmg += ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage");
        return
        [
            new GlossaryTooltip($"action.{GetType().Namespace!}::AThrowBanana")
            {
                Icon = ModEntry.Instance.Sprites["ThrowBananaPiercing"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "description"], new { Damage = dmg }),
            },
            new TTGlossary("action.attackPiercing")
        ];
    }

    public override void OnReceiveArtifact(State state)
    {
        ModEntry.Instance.Helper.ModData.SetModData(state, "BananaPiercing", 1);
    }
    public override void OnRemoveArtifact(State state)
    {
        ModEntry.Instance.Helper.ModData.RemoveModData(state, "BananaPiercing");
    }
}
