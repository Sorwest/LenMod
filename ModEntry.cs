using HarmonyLib;
using Microsoft.Extensions.Logging;
using Nanoray.PluginManager;
using Nickel;
using Sorwest.LenMod.Artifacts;
using Sorwest.LenMod.Cards;
using Sorwest.LenMod.Conversation;
using Sorwest.LenMod.Enemies;
using Sorwest.LenMod.ExternalAPI;
using Sorwest.LenMod.Features;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sorwest.LenMod;

public class ModEntry : SimpleMod
{
    public static string Name => "Sorwest.LenMod::Len";
    internal static bool LockedChar => false;
    internal static ModEntry Instance { get; private set; } = null!;
    internal Harmony Harmony { get; }
    internal IKokoroApi.IV2 KokoroApi { get; }
    internal IMoreDifficultiesApi? MoreDifficultiesApi { get; }
    internal IDraculaApi? DraculaApi { get; }
    internal Settings Settings { get; private set; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyLocalizations { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> Localizations { get; }
    internal ILocalizationProvider<IReadOnlyList<string>> AnyStoryLocs { get; }
    internal ILocaleBoundNonNullLocalizationProvider<IReadOnlyList<string>> StoryLocs { get; }
    internal IDeckEntry LenDeck { get; }
    internal IDeckEntry MikuDeck { get; }
    internal IPlayableCharacterEntryV2 LenCharacter { get; }
    internal INonPlayableCharacterEntryV2 MikuNPC { get; }
    internal static Color LenColor => new("ffe569");
    internal static Color BlackTitle => new("000000");
    internal static IReadOnlyList<Type> LenCommonCardTypes { get; } = [
        typeof(LenCardBanana),
        typeof(LenCardBananaDance),
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
    internal static IReadOnlyList<Type> LenEncoreCardTypes { get; } = [
        typeof(LenCardMN1),
        typeof(LenCardMN2),
        typeof(LenCardMN3),
        typeof(LenCardMN4),
        typeof(LenCardMN5)
    ];
    internal static IReadOnlyList<Type> MikuEncoreCardTypes { get; } = [
        typeof(MikuCardMN1),
        typeof(MikuCardMN2),
        typeof(MikuCardMN3),
        typeof(MikuCardMN4),
        typeof(MikuCardMN5)
    ];
    internal static IReadOnlyList<Type> LenStarterArtifactTypes { get; } = [
        typeof(LenArtifactBananaStash),
        typeof(LenArtifactGuillotine)
    ];
    internal static IReadOnlyList<Type> LenCommonArtifactTypes { get; } = [
        typeof(LenArtifactGlassBottle),
        typeof(LenArtifactBananaSnack),
        typeof(LenArtifactMaidDress)
    ];
    internal static IReadOnlyList<Type> LenBossArtifactTypes { get; } = [
        typeof(LenArtifactBrioche),
        typeof(LenArtifactTwinPower)
    ];
    internal static IEnumerable<Type> AllCards
        => [
            .. LenCommonCardTypes,
            .. LenUncommonCardTypes,
            .. LenRareCardTypes,
            .. LenEncoreCardTypes,
            typeof(LenCardEXECard),
            .. MikuEncoreCardTypes
        ];
    internal static IEnumerable<Type> AllArtifacts
        => [
            .. LenStarterArtifactTypes,
            .. LenCommonArtifactTypes,
            .. LenBossArtifactTypes
        ];
    internal static IEnumerable<Type> DuoArtifacts
        => [];
    internal static IEnumerable<Type> EnemiesAI
        => [
            typeof(MikuAI)
        ];
    internal static IEnumerable<Type> Managers
        => [
            typeof(BananaManager),
            typeof(BananaTreatManager),
            typeof(MusicNoteManager),
            typeof(ConvoManager)
        ];
    private static readonly IEnumerable<Type> RegisterableTypes
        = [
            .. AllCards,
            .. AllArtifacts,
            .. DuoArtifacts,
            .. EnemiesAI,
            .. Managers
        ];
    internal IList<string> FaceSprites { get; } = [
        "Gameover",
        "Mini",
        "Neutral",
        "Squint",
        "Happy",
        "Sad"
        ];
    internal IList<string> CardBGs { get; } = [
        "SimpleBackground"
        ];
    internal IList<string> Enemies { get; } = [
        "miku",
        "mikuchassis",
        "mikuwing",
        "mikucannon",
        "mikumissiles",
        "mikuempty",
        "mikucannonempty"
        ];
    internal IList<string> ArtifactSprites { get; } = [
        "BananaSnack",
        "BananaStash",
        "BananaStashOn",
        "BananaStashOff",
        "Brioche",
        "GlassBottle",
        "Guillotine",
        "MaidDress",
        "TwinPower"
        ];
    internal IList<string> IconSprites { get; } = [
        "Banana",
        "BananaCost",
        "BananaCostSatisfied",
        "BananaTreat",
        "EatBanana",
        "EatBananaCost",
        "GainBananaGain",
        "GainBananaLoseAll",
        "ThrowBanana",
        "ThrowBananaCost",
        "ThrowBananaPiercing",
        "ThrowBananaPiercingCost",
        "MusicNote"
        ];
    internal Dictionary<string, ISpriteEntry> Sprites { get; } = [];

    public ModEntry(IPluginPackage<IModManifest> package, IModHelper helper, ILogger logger) : base(package, helper, logger)
    {
        Instance = this;
        Harmony = new(package.Manifest.UniqueName);
        KokoroApi = helper.ModRegistry.GetApi<IKokoroApi>("Shockah.Kokoro")!.V2;
        MoreDifficultiesApi = helper.ModRegistry.GetApi<IMoreDifficultiesApi>("TheJazMaster.MoreDifficulties");
        DraculaApi = helper.ModRegistry.GetApi<IDraculaApi>("Shockah.Dracula");

        AnyLocalizations = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/game/{locale}.json").OpenRead()
        );
        Localizations = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyLocalizations)
        );
        AnyStoryLocs = new JsonLocalizationProvider(
            tokenExtractor: new SimpleLocalizationTokenExtractor(),
            localeStreamFunction: locale => package.PackageRoot.GetRelativeFile($"i18n/story/story_{locale}.json").OpenRead()
        );
        StoryLocs = new MissingPlaceholderLocalizationProvider<IReadOnlyList<string>>(
            new CurrentLocaleOrEnglishLocalizationProvider<IReadOnlyList<string>>(AnyStoryLocs)
        );

        this.Settings = helper.Storage.LoadJson<Settings>(helper.Storage.GetMainStorageFile("json"));
        helper.ModRegistry.AwaitApi<IModSettingsApi>(
            "Nickel.ModSettings",
            api =>
            {
                api.RegisterModSettings(
                    api.MakeList([
                        api.MakeProfileSelector(
                            () => package.Manifest.DisplayName ?? package.Manifest.UniqueName,
                            Settings.ProfileBased
                            ),
                        api.MakeCheckbox(
                            () => Localizations.Localize(["settings", nameof(ProfileSettings.EnabledMikuAI), "title"]),
                            () => Settings.ProfileBased.Current.EnabledMikuAI,
                            (_, _, value) => Settings.ProfileBased.Current.EnabledMikuAI = value
                            ).SetTooltips(() => [
                                new GlossaryTooltip($"settings.{package.Manifest.UniqueName}::{nameof(ProfileSettings.EnabledMikuAI)}")
                                {
                                    TitleColor = Colors.textBold,
                                    Title = Localizations.Localize(["settings", nameof(ProfileSettings.EnabledMikuAI), "tooltipTitle"]),
                                    Description = Localizations.Localize(["settings", nameof(ProfileSettings.EnabledMikuAI), "description"])
                                }]
                            ),
                        api.MakeCheckbox(
                            () => Localizations.Localize(["settings", nameof(ProfileSettings.EnabledSounds), "title"]),
                            () => Settings.ProfileBased.Current.EnabledSounds,
                            (_, _, value) => Settings.ProfileBased.Current.EnabledSounds = value
                            ).SetTooltips(() => [
                                new GlossaryTooltip($"settings.{package.Manifest.UniqueName}::{nameof(ProfileSettings.EnabledSounds)}")
                                {
                                    TitleColor = Colors.textBold,
                                    Title = Localizations.Localize(["settings", nameof(ProfileSettings.EnabledSounds), "tooltipTitle"]),
                                    Description = Localizations.Localize(["settings", nameof(ProfileSettings.EnabledSounds), "description"])
                                }]
                            )
                        ]).SubscribeToOnMenuClose(_ => helper.Storage.SaveJson(helper.Storage.GetMainStorageFile("json"), Settings))
                );
            });

        // SPRITE REGISTRATION BLOCK
        IFileInfo file;
        foreach (string bg in CardBGs)
        {
            file = package.PackageRoot.GetRelativeFile($"assets/cardbg/{bg}.png");
            if (file.Exists)
                Sprites.Add(key: $"{bg}", value: helper.Content.Sprites.RegisterSprite(file));
        }
        foreach (string enemy in Enemies)
        {
            file = package.PackageRoot.GetRelativeFile($"assets/enemies/{enemy}.png");
            if (file.Exists)
                Sprites.Add(key: $"{enemy}", value: helper.Content.Sprites.RegisterSprite(file));
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

        // DECK REGISTRATION BLOCK
        LenDeck = helper.Content.Decks.RegisterDeck("Len", new()
        {
            Definition = new() { color = LenColor, titleColor = BlackTitle },
            DefaultCardArt = Sprites["SimpleBackground"].Sprite,
            BorderSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cardBorder/BorderLen.png")).Sprite,
            Name = AnyLocalizations.Bind(["character", "len", "name"]).Localize,
        });

        // CHARACTER REGISTRATION BLOCK
        LenCharacter = helper.Content.Characters.V2.RegisterPlayableCharacter("Len", new()
        {
            Deck = LenDeck.Deck,
            StartLocked = LockedChar,
            Description = AnyLocalizations.Bind(["character", "len", "description"]).Localize,
            BorderSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/character/Len_PanelFrame_0.png")).Sprite,
            Starters = new()
            {
                cards = [
                    new LenCardBanana(),
                    new LenCardBreaktime()
                ],
                artifacts = [
                    new LenArtifactBananaStash()
                ]
            },
            SoloStarters = new()
            {
                cards = [
                    new LenCardBanana(),
                    new LenCardBananaDance(),
                    new LenCardFunkyNightTown()
                ],
                artifacts = [
                    new LenArtifactBananaStash(),
                    new LenArtifactBananaSnack()
                ]
            },
            ExeCardType = typeof(LenCardToluthinAntenna),
            NeutralAnimation = new()
            {
                CharacterType = LenDeck.UniqueName,
                LoopTag = "neutral",
                Frames = [.. Enumerable.Range(1, 4).Select(i
                    => helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/character/Neutral/Len_Neutral_000{i}.png")).Sprite)]
            },
            MiniAnimation = new()
            {
                CharacterType = LenDeck.UniqueName,
                LoopTag = "mini",
                Frames = [helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/character/Mini/Len_Mini_0.png")).Sprite]
            }
        });
        helper.Content.Characters.V2.RegisterCharacterAnimation(new()
        {
            CharacterType = LenDeck.UniqueName,
            LoopTag = "gameover",
            Frames = [helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/character/Gameover/Len_Gameover_0.png")).Sprite]
        });
        helper.Content.Characters.V2.RegisterCharacterAnimation(new()
        {
            CharacterType = LenDeck.UniqueName,
            LoopTag = "squint",
            Frames = [.. Enumerable.Range(1, 4).Select(i =>
                helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/character/Squint/Len_Squint_000{i}.png")).Sprite)]
        });
        helper.Content.Characters.V2.RegisterCharacterAnimation(new()
        {
            CharacterType = LenDeck.UniqueName,
            LoopTag = "happy",
            Frames = [.. Enumerable.Range(1, 4).Select(i =>
                helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/character/Happy/Len_Happy_000{i}.png")).Sprite)]
        });
        helper.Content.Characters.V2.RegisterCharacterAnimation(new()
        {
            CharacterType = LenDeck.UniqueName,
            LoopTag = "sad",
            Frames = [.. Enumerable.Range(1, 4).Select(i =>
                helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile($"assets/character/Sad/Len_Sad_000{i}.png")).Sprite)]
        });

        // ENEMY BLOCK
        MikuDeck = helper.Content.Decks.RegisterDeck("Miku", new()
        {
            Definition = new() { color = new Color("7ed2d6"), titleColor = BlackTitle },
            DefaultCardArt = Sprites["SimpleBackground"].Sprite,
            BorderSprite = helper.Content.Sprites.RegisterSprite(package.PackageRoot.GetRelativeFile("assets/cardBorder/BorderMiku.png")).Sprite,
            Name = AnyLocalizations.Bind(["enemy", "miku", "name"]).Localize,
        });
        MikuNPC = helper.Content.Characters.V2.RegisterNonPlayableCharacter("Miku", new()
        {
            CharacterType = "miku",
            Name = AnyLocalizations.Bind(["enemy", "miku", "name"]).Localize
        });
        helper.Content.Characters.V2.RegisterCharacterAnimation(new()
        {
            CharacterType = MikuNPC.CharacterType,
            LoopTag = "neutral",
            Frames = [Sprites["miku"].Sprite]
        });

        // IREGISTERABLE BLOCK
        foreach (Type type in RegisterableTypes)
            AccessTools.DeclaredMethod(type, nameof(IRegisterable.Register))?.Invoke(null, [package, helper]);

        // MORE DIFFICULTIES OPTIONS BLOCK
        helper.ModRegistry.AwaitApi<IMoreDifficultiesApi>(
            "TheJazMaster.MoreDifficulties",
            api => api.RegisterAltStarters(
                deck: LenDeck.Deck,
                starterDeck: new StarterDeck
                {
                    cards = [
                        new LenCardPlusBoy() { upgrade = Upgrade.B },
                        new LenCardTelecasterBBoy() { upgrade = Upgrade.A },
                    ],
                    artifacts = [
                        new LenArtifactBananaStash(),
                        new LenArtifactGuillotine()
                    ]
                })
        );

        // DRACULA BLOCK
        helper.ModRegistry.AwaitApi<IDraculaApi>(
            "Shockah.Dracula",
            api =>
            {
                api.RegisterBloodTapOptionProvider(BananaTreatManager.BananaTreatStatus.Status, (_, _, status) => [
                    new AHurt { targetPlayer = true, hurtAmount = 1 },
                    new AStatus { targetPlayer = true, status = status, statusAmount = 1 },
                ]);
                api.RegisterBloodTapOptionProvider(BananaManager.BananaStatus.Status, (_, _, status) => [
                    new AHurt { targetPlayer = true, hurtAmount = 1 },
                    new AStatus { targetPlayer = true, status = status, statusAmount = 3 },
                ]);
                api.RegisterBloodTapOptionProvider(MusicNoteManager.MusicNoteStatus.Status, (_, _, status) => [
                    new AHurt { targetPlayer = true, hurtAmount = 1 },
                    new AStatus { targetPlayer = true, status = status, statusAmount = 2 },
                ]);
            });


        // COST BLOCK
        KokoroApi.ActionCosts.RegisterStatusResourceCostIcon(BananaManager.BananaStatus.Status, Sprites["BananaCostSatisfied"].Sprite, Sprites["BananaCost"].Sprite);
        KokoroApi.ActionCosts.RegisterStatusResourceCostIcon(BananaManager.EatBananaStatus.Status, Sprites["EatBanana"].Sprite, Sprites["EatBananaCost"].Sprite);
        KokoroApi.ActionCosts.RegisterStatusResourceCostIcon(BananaManager.ThrowBananaStatus.Status, Sprites["ThrowBanana"].Sprite, Sprites["ThrowBananaCost"].Sprite);
        KokoroApi.ActionCosts.RegisterStatusResourceCostIcon(BananaManager.ThrowBananaPiercingStatus.Status, Sprites["ThrowBananaPiercing"].Sprite, Sprites["ThrowBananaPiercingCost"].Sprite);
    }
}