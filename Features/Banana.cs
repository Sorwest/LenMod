using HarmonyLib;
using Nanoray.PluginManager;
using Nickel;
using System.Reflection;

namespace Sorwest.LenMod.Features;
internal sealed class BananaManager : IRegisterable
{
    internal static IStatusEntry BananaStatus { get; private set; } = null!;
    internal static IStatusEntry ThrowBananaStatus { get; private set; } = null!;
    internal static IStatusEntry ThrowBananaPiercingStatus { get; private set; } = null!;
    internal static IStatusEntry EatBananaStatus { get; private set; } = null!;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        BananaStatus = helper.Content.Statuses.RegisterStatus("BananaStatus", new()
        {
            Definition = new()
            {
                icon = ModEntry.Instance.Sprites["Banana"].Sprite,
                color = ModEntry.LenColor,
                border = new Color("bdbd3c"),
                isGood = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "name"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "description"]).Localize
        });

        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(State), nameof(State.PopulateRun)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(State_PopulateRun_Postfix))
        );

        // SPOOF STATUSES
        EatBananaStatus = helper.Content.Statuses.RegisterStatus("EatBananaStatus", new()
        {
            Definition = new()
            {
                icon = ModEntry.Instance.Sprites["EatBanana"].Sprite,
                color = ModEntry.LenColor,
                isGood = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "spoof"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "description"]).Localize
        });
        ThrowBananaStatus = helper.Content.Statuses.RegisterStatus("ThrowBananaStatus", new()
        {
            Definition = new()
            {
                icon = ModEntry.Instance.Sprites["ThrowBanana"].Sprite,
                color = ModEntry.LenColor,
                isGood = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "spoof"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "description"]).Localize
        });
        ThrowBananaPiercingStatus = helper.Content.Statuses.RegisterStatus("ThrowBananaPiercingStatus", new()
        {
            Definition = new()
            {
                icon = ModEntry.Instance.Sprites["ThrowBananaPiercing"].Sprite,
                color = ModEntry.LenColor,
                isGood = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "spoof"]).Localize,
            Description = ModEntry.Instance.AnyLocalizations.Bind(["status", "Banana", "description"]).Localize
        });
    }
    private static void State_PopulateRun_Postfix(State __instance)
    {
        ModEntry.Instance.Helper.ModData.RemoveModData(__instance, "BananaStored");
        ModEntry.Instance.Helper.ModData.RemoveModData(__instance, "BananaDamage");
        ModEntry.Instance.Helper.ModData.RemoveModData(__instance, "BananaShield");
        ModEntry.Instance.Helper.ModData.RemoveModData(__instance, "BananaPiercing");
    }
}