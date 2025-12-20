using HarmonyLib;
using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Nanoray.PluginManager;
using Nanoray.Shrike;
using Nanoray.Shrike.Harmony;
using Newtonsoft.Json;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.ExternalAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Sorwest.LenMod.Cards;

//triad copied from: Dracula by Shockah (Dec 2, 2024. Commit 5498214)

public class LenCardWishBottle : Card, IRegisterable
{
    private static List<ISpriteEntry> TriadArt = null!;
    private static List<ISpriteEntry> CleanArt = null!;
    private static List<ISpriteEntry> TriadIcon = null!;

    [JsonProperty]
    public int FlipIndex { get; private set; }

    [JsonProperty]
    private bool LastFlipped { get; set; }

    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Cards.RegisterCard("WishBottle", new()
        {
            CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            Meta = new()
            {
                deck = ModEntry.Instance.LenDeck.Deck,
                rarity = Rarity.common,
                dontOffer = true
            },
            Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "WishBottle", "name"]).Localize
        });
        TriadArt = Enumerable.Range(0, 3)
            .Select(i => helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile($"assets/cardbg/TriadWish{i}.png")))
            .ToList();
        CleanArt = Enumerable.Range(0, 3)
            .Select(i => helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile($"assets/cardbg/TriadClean{i}.png")))
            .ToList();
        TriadIcon = Enumerable.Range(0, 3)
            .Select(i => helper.Content.Sprites.RegisterSprite(ModEntry.Instance.Package.PackageRoot.GetRelativeFile($"assets/icons/Triad{i}.png")))
            .ToList();

        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Card), nameof(Render)),
            transpiler: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_Render_Transpiler))
        );
        ModEntry.Instance.Harmony.Patch(
            original: AccessTools.DeclaredMethod(typeof(Card), nameof(GetAllTooltips)),
            postfix: new HarmonyMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_GetAllTooltips_Postfix))
        );

        ModEntry.Instance.KokoroApi.CardRendering.RegisterHook(new Hook());
    }
    public override CardData GetData(State state)
    {
        return new()
        {
            art = (ModEntry.Instance.Settings.ProfileBased.Current.EnabledBlood ? TriadArt : CleanArt)[FlipIndex % 3].Sprite,
            cost = 0,
            floppable = true,
            exhaust = true,
            artOverlay = ModEntry.Instance.Settings.ProfileBased.Current.EnabledBlood ? ModEntry.Instance.Sprites["BorderWish"].Sprite : null
        };
    }
    public override void ExtraRender(G g, Vec v)
    {
        base.ExtraRender(g, v);
        if (LastFlipped != flipped)
        {
            LastFlipped = flipped;
            FlipIndex = (FlipIndex + 1) % 3;
        }
    }
    public override List<CardAction> GetActions(State s, Combat c)
    {
        return
        [
            new AGainBanana
            {
                amount = 3,
                disabled = FlipIndex % 3 != 0,
            },
            new AStatus
            {
                targetPlayer = true,
                status = Status.evade,
                statusAmount = 1,
                disabled = FlipIndex % 3 != 1,
            },
            new ADrawCard
            {
                count = 1,
                disabled = FlipIndex % 3 != 2,
            }
        ];
    }
    private static IEnumerable<CodeInstruction> Card_Render_Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase originalMethod)
    {
        // ReSharper disable PossibleMultipleEnumeration
        try
        {
            return new SequenceBlockMatcher<CodeInstruction>(instructions)
                .Find(
                    ILMatches.Ldloc<CardData>(originalMethod),
                    ILMatches.Ldfld("floppable")
                )
                .Find(ILMatches.LdcI4((int)StableSpr.icons_floppable))
                .Insert(
                    SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion,
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_Render_Transpiler_ReplaceFloppableIcon)))
                )
                .Find(ILMatches.Ldfld("flipped"))
                .Insert(
                    SequenceMatcherPastBoundsDirection.After, SequenceMatcherInsertionResultingBounds.IncludingInsertion,
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.DeclaredMethod(MethodBase.GetCurrentMethod()!.DeclaringType!, nameof(Card_Render_Transpiler_ReplaceFlipped)))
                )
                .AllElements();
        }
        catch (Exception ex)
        {
            ModEntry.Instance.Logger.LogError("Could not patch method {Method} - {Mod} probably won't work.\nReason: {Exception}", originalMethod, ModEntry.Instance.Package.Manifest.GetDisplayName(@long: false), ex);
            return instructions;
        }
        // ReSharper restore PossibleMultipleEnumeration
    }

    private static Spr Card_Render_Transpiler_ReplaceFloppableIcon(Spr sprite, Card card)
    {
        if (card is not LenCardWishBottle wishbottle)
            return sprite;
        return TriadIcon[wishbottle.FlipIndex % TriadIcon.Count].Sprite;
    }

    private static bool Card_Render_Transpiler_ReplaceFlipped(bool flipped, Card card)
        => card is not LenCardWishBottle && flipped;

    private static void Card_GetAllTooltips_Postfix(Card __instance, bool showCardTraits, ref IEnumerable<Tooltip> __result)
    {
        if (!showCardTraits)
            return;
        if (__instance is not LenCardWishBottle)
            return;

        __result = __result
            .Select(tooltip =>
            {
                if (tooltip is not TTGlossary { key: "cardtrait.floppable" })
                    return tooltip;

                var buttonText = PlatformIcons.GetPlatform() switch
                {
                    Platform.NX => Loc.T("controller.nx.b"),
                    Platform.PS => Loc.T("controller.ps.circle"),
                    _ => Loc.T("controller.xbox.b"),
                };

                return new GlossaryTooltip("cardtrait.triad")
                {
                    Icon = TriadIcon[0].Sprite,
                    TitleColor = Colors.cardtrait,
                    Title = ModEntry.Instance.Localizations.Localize(["cardTrait","triad","name"]),
                    Description = ModEntry.Instance.Localizations.Localize(["cardTrait","triad","description",
                        PlatformIcons.GetPlatform() == Platform.MouseKeyboard ? "m&k" : "controller"],
                    new { Button = buttonText })
                };
            });
    }
    private sealed class Hook : IKokoroApi.IV2.ICardRenderingApi.IHook
    {
        public Matrix ModifyCardActionRenderMatrix(IKokoroApi.IV2.ICardRenderingApi.IHook.IModifyCardActionRenderMatrixArgs args)
        {
            if (args.Card is not LenCardWishBottle)
                return Matrix.Identity;

            var spacing = 12 * args.G.mg.PIX_SCALE;
            var halfYCenterOffset = 16 * args.G.mg.PIX_SCALE;
            var index = args.Actions.ToList().IndexOf(args.Action);
            var recenterY = -(int)((index - args.Actions.Count / 2.0 + 0.5) * spacing);
            return index switch
            {
                0 => Matrix.CreateTranslation(0, recenterY - halfYCenterOffset - 3, 0),
                2 => Matrix.CreateTranslation(0, recenterY + halfYCenterOffset + 1, 0),
                _ => Matrix.Identity
            };
        }
    }
}