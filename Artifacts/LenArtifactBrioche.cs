using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactBrioche : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("Brioche", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.Boss],
                unremovable = true
            },
            Sprite = ModEntry.Instance.Sprites["Brioche"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Brioche", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "Brioche", "description"]).Localize
        });
    }
    /*public override List<Tooltip>? GetExtraTooltips()
    {
        int dmg = 1;
        Spr icon = ModEntry.Instance.Sprites["ThrowBanana"].Sprite;
        if (!(MG.inst.g?.state is not { } state || state.IsOutsideRun()))
        {
            dmg += ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage");
            if (ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaPiercing") > 0)
            {
                icon = ModEntry.Instance.Sprites["ThrowBananaPiercing"].Sprite;
            }
        }
        return [
            new GlossaryTooltip($"action.{GetType().Namespace!}::AThrowBanana")
            {
                Icon = icon,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "description"], new { Damage = dmg }),
            },
            new GlossaryTooltip($"action.{GetType().Namespace!}::AEatBanana")
            {
                Icon = ModEntry.Instance.Sprites["EatBanana"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "description"], new { Damage = dmg }),
            }
            ];
    }*/
    public override void OnReceiveArtifact(State state)
    {
        ModEntry.Instance.Helper.ModData.SetModData(state, "BananaDamage", 1);
    }
    public override void OnRemoveArtifact(State state)
    {
        ModEntry.Instance.Helper.ModData.RemoveModData(state, "BananaDamage");
    }
}
