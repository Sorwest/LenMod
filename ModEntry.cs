using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Artifacts;
using Sorwest.LenMod.Cards;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod;

public class ModEntry : SimpleMod
{
    public string Name => "Sorwest.CobaltLen";
    internal bool LockedChar = false;
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony { get; }
    internal IKokoroApi KokoroApi { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }
    internal IDraculaApi? DraculaApi { get; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal IDeckEntry LenDeck { get; }
    internal ICharacterEntry LenCharacter { get; }
    internal static Color LenColor => new Color("ffe569");
    internal static Color BlackTitle => new("000000");
    internal IStatusEntry MusicNoteStatus { get; }
    internal IStatusEntry BananaStatus { get; }
    internal static IReadOnlyList<Type> LenStarterCardTypes { get; } = [
        typeof(LenCardBanana),
        typeof(LenCardBanana),
        typeof(LenCardBanana),
        typeof(LenCardBananaWall),
        typeof(LenCardBreaktime),
    ];
    internal static IReadOnlyList<Type> LenCommonCardTypes { get; } = [
        typeof(LenCardBanana),
        typeof(LenCardBananaWall),
        typeof(LenCardBreaktime),
        typeof(LenCardNakakapagpabagabag),
        typeof(LenCardPlusBoy),
        typeof(LenCardFunkyNightTown),
        typeof(LenCardTelecasterBBoy),
        typeof(LenCardLawEvadingRock),
        typeof(LenCardHolyLanceExplosion),
        typeof(LenCardFifthPierrot),
    ];
    internal static IReadOnlyList<Type> LenUncommonCardTypes { get; } = [
        typeof(LenCardRemoteControl),
        typeof(LenCardChildishWar),
        typeof(LenCardBringItOn),
        typeof(LenCardLikeDislike),
        typeof(LenCardBarisolChild),
        typeof(LenCardNiccoriTeamSurvey)
    ];
    internal static IReadOnlyList<Type> LenRareCardTypes { get; } = [
        typeof(LenCardButterflyOnShoulder),
        typeof(LenCardVampiresPathos),
        typeof(LenCardServantOfEvil),
        typeof(LenCardParadichlorobenzene),
        typeof(LenCardToluthinAntenna)
    ];
    internal static IReadOnlyList<Type> LenStarterArtifactTypes { get; } = [
        typeof(LenArtifactBananaStash)
    ];
    internal static IReadOnlyList<Type> LenCommonArtifactTypes { get; } = [
        typeof(LenArtifactGlassBottle),
        typeof(LenArtifactMaidDress)
    ];
    internal static IReadOnlyList<Type> LenBossArtifactTypes { get; } = [
        typeof(LenArtifactBrioche),
        typeof(LenArtifactTwinPower)
    ];
    internal static IEnumerable<Type> AllCards
        => LenStarterCardTypes
        .Concat(LenCommonCardTypes)
        .Concat(LenUncommonCardTypes)
        .Concat(LenRareCardTypes);
    internal static IEnumerable<Type> AllArtifacts
        => LenStarterArtifactTypes
        .Concat(LenCommonArtifactTypes)
        .Concat(LenBossArtifactTypes);
    internal IList<string> FaceSprites { get; } = [
        "Gameover",
        "Mini",
        "Neutral",
        "Squint"
    ];
    internal IList<string> CardBGs { get; } = [
        "SimpleBackground"
    ];
    internal IList<string> ArtifactSprites { get; } = [
        "BananaStash",
        "BananaStashOff",
        "Brioche",
        "GlassBottle",
        "MaidDress",
        "TwinPower"
    ];
    internal IList<string> IconSprites { get; } = [
        "Banana",
        "EatBanana",
        "GainBananaGain",
        "GainBananaLose",
        "GainBananaLoseAll",
        "SmashBanana",
        "ThrowBanana",
        "MusicNote"
    ];
    internal Dictionary<string, ISpriteEntry> Sprites { get; } = [];

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new(package.Manifest.UniqueName);
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!;
        MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");
        DraculaApi = helper.ModRegistry.GetApi<IDraculaApi>("Shockah.Dracula");

        _ = new MusicNoteManager();
        _ = new BananaManager();

        CustomTTGlossary.ApplyPatches(Harmony);

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        // SPRITE REGISTRATION BLOCK
        IFileInfo file;
        foreach (string face in FaceSprites)
        {
            for (int frame = 0; frame < 10; frame++)
            {
                file = package.PackageRoot.GetRelativeFile($"assets/character/{face}/Len_{face}_{frame}.png");
                if (file.Exists)
                    Sprites.Add(key: $"len.{face.ToLower()}.{frame}", value: helper.Content.Sprites.RegisterSprite(file));
                else
                    break;
            }
        }
        file = package.PackageRoot.GetRelativeFile($"assets/character/Len_PanelFrame_0.png");
        if (file.Exists && !Sprites.ContainsKey("len.panel"))
            Sprites.Add(key: "len.panel", value: helper.Content.Sprites.RegisterSprite(file));

        file = package.PackageRoot.GetRelativeFile($"assets/cardborder/BorderLen.png");
        if (file.Exists && !Sprites.ContainsKey("len.border"))
            Sprites.Add(key: "len.border", value: helper.Content.Sprites.RegisterSprite(file));

        foreach (string bg in CardBGs)
        {
            file = package.PackageRoot.GetRelativeFile($"assets/cardbg/{bg}.png");
            if (file.Exists)
                Sprites.Add(key: $"{bg}", value: helper.Content.Sprites.RegisterSprite(file));
        }

        foreach (string artifact in ArtifactSprites)
        {
            file = package.PackageRoot.GetRelativeFile($"assets/artifacts/{artifact}.png");
            if (!file.Exists)
                file = package.PackageRoot.GetRelativeFile($"assets/artifacts/Duo/{artifact}.png");
            if (file.Exists)
                Sprites.Add(key: $"{artifact}", value: helper.Content.Sprites.RegisterSprite(file));
        }

        foreach (string icon in IconSprites)
        {
            file = package.PackageRoot.GetRelativeFile($"assets/icons/{icon}.png");
            if (!file.Exists)
                file = package.PackageRoot.GetRelativeFile($"assets/icons/Duo/{icon}.png");
            if (file.Exists)
                Sprites.Add(key: $"{icon}", value: helper.Content.Sprites.RegisterSprite(file));
        }

        // STATUS REGISTRATION BLOCK
        MusicNoteStatus = helper.Content.Statuses.RegisterStatus("MusicNoteStatus", new()
        {
            Definition = new()
            {
                icon = Sprites["MusicNote"].Sprite,
                color = new Color("8cfffb"),
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "MusicNote", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "MusicNote", "description"]).Localize
        });
        BananaStatus = helper.Content.Statuses.RegisterStatus("BananaStatus", new()
        {
            Definition = new()
            {
                icon = Sprites["Banana"].Sprite,
                color = LenColor,
                isGood = true
            },
            Name = AnyLocalizations.Bind(["status", "Banana", "name"]).Localize,
            Description = AnyLocalizations.Bind(["status", "Banana", "description"]).Localize
        });

        // DECK REGISTRATION BLOCK
        LenDeck = helper.Content.Decks.RegisterDeck("Len", new()
        {
            Definition = new() { color = LenColor, titleColor = BlackTitle },
            DefaultCardArt = Sprites["SimpleBackground"].Sprite,
            BorderSprite = Sprites["len.border"].Sprite,
            Name = AnyLocalizations.Bind(["character", "len", "name"]).Localize,
        });
        helper.Content.Characters.RegisterCharacterAnimation(new()
        {
            Deck = LenDeck.Deck,
            LoopTag = "neutral",
            Frames = [
                Sprites["len.neutral.0"].Sprite
            ]
        });
        helper.Content.Characters.RegisterCharacterAnimation(new()
        {
            Deck = LenDeck.Deck,
            LoopTag = "mini",
            Frames = [
                Sprites["len.mini.0"].Sprite
            ]
        });
        helper.Content.Characters.RegisterCharacterAnimation(new()
        {
            Deck = LenDeck.Deck,
            LoopTag = "squint",
            Frames = [
                Sprites["len.squint.0"].Sprite
            ]
        });
        helper.Content.Characters.RegisterCharacterAnimation(new()
        {
            Deck = LenDeck.Deck,
            LoopTag = "gameover",
            Frames = [
                Sprites["len.gameover.0"].Sprite
            ]
        });
        LenCharacter = helper.Content.Characters.RegisterCharacter("Len", new()
        {
            Deck = LenDeck.Deck,
            StartLocked = LockedChar,
            Starters = new()
            {
                cards = [
                    new LenCardBanana(),
                    new LenCardBanana(),
                    new LenCardBreaktime(),
                    new LenCardBananaWall()
                ],
                artifacts = [
                    new LenArtifactBananaStash()
                ]
            },
            BorderSprite = Sprites["len.panel"].Sprite,
            Description = AnyLocalizations.Bind(["character", "len", "description"]).Localize
        });

        // CARD REGISTRATION BLOCK
        foreach (var cardType in AllCards)
            AccessTools.DeclaredMethod(cardType, nameof(IModdedCard.Register))?.Invoke(null, [package, helper]);

        // ARTIFACT REGISTRATION BLOCK
        foreach (var artifactType in AllArtifacts)
            AccessTools.DeclaredMethod(artifactType, nameof(IModdedArtifact.Register))?.Invoke(null, [helper]);

        // MORE DIFFICULTIES OPTIONS BLOCK
        MoreDifficultiesApi?.RegisterAltStarters(
            deck: LenDeck.Deck,
            starterDeck: new StarterDeck
            {
                cards = new()
                {
                    new LenCardTelecasterBBoy()
                    {
                        upgrade = Upgrade.A
                    },
                    new LenCardHolyLanceExplosion()
                    {
                        upgrade = Upgrade.A
                    }
                },
                artifacts = new()
                {
                    new LenArtifactBananaStash()
                    {
                        counter = 3
                    }
                }
            }
        );

        // DRACULA BLOCK
        DraculaApi?.RegisterBloodTapOptionProvider(MusicNoteStatus.Status, (_, _, status) => [
            new AHurt { targetPlayer = true, hurtAmount = 1 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 2 },
        ]);
        DraculaApi?.RegisterBloodTapOptionProvider(BananaStatus.Status, (_, _, status) => [
            new AHurt { targetPlayer = true, hurtAmount = 1 },
            new AStatus { targetPlayer = true, status = status, statusAmount = 2 },
        ]);
    }
}