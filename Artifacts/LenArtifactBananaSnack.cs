using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Artifacts;

public class LenArtifactBananaSnack : Artifact, IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Artifacts.RegisterArtifact("BananaSnack", new()
        {
            ArtifactType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                owner = ModEntry.Instance.LenDeck.Deck,
                pools = [ArtifactPool.EventOnly],
                unremovable = true
            },
            Sprite = ModEntry.Instance.Sprites["BananaSnack"].Sprite,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BananaSnack", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["artifact", "BananaSnack", "description"]).Localize
        });
    }
    public override List<Tooltip>? GetExtraTooltips()
    {
        int dmg = 1;
        if (!(MG.inst.g?.state is not { } state || state.IsOutsideRun()))
            dmg += ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage");
        return [
            ..StatusMeta.GetTooltips(BananaTreatManager.BananaTreatStatus.Status, 1),
            new GlossaryTooltip($"action.{GetType().Namespace!}::AEatBanana")
            {
                Icon = ModEntry.Instance.Sprites["EatBanana"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "description"], new { Damage = dmg }),
            }
        ];
    }
}
