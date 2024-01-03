using System.Reflection;
using CobaltCoreModding.Definitions;
using CobaltCoreModding.Definitions.ExternalItems;
using CobaltCoreModding.Definitions.ModContactPoints;
using CobaltCoreModding.Definitions.ModManifests;
using HarmonyLib;
using LenMod.LenArtifacts;
using LenMod.LenCards;
using Microsoft.Extensions.Logging;

namespace LenMod
{
    public class Manifest :
        ISpriteManifest,
        IManifest,
        ICharacterManifest,
        IAnimationManifest,
        IArtifactManifest,
        ICardManifest,
        IDeckManifest,
        IStatusManifest,
        IGlossaryManifest
    {
        public string Name => "Sorwest.CobaltLen";

        public static System.Drawing.Color CobaltLen_Primary_Color = System.Drawing.Color.FromArgb(255, 255, 0);
        public static string CobaltLen_CharacterColH = string.Format("<c={0:X2}{1:X2}{2:X2}>", (object)CobaltLen_Primary_Color.R, (object)CobaltLen_Primary_Color.G, (object)CobaltLen_Primary_Color.B.ToString("X2"));
        public IEnumerable<DependencyEntry> Dependencies => new DependencyEntry[] { };
        public DirectoryInfo? ModRootFolder { get; set; }
        public ILogger? Logger { get; set; }
        public static ExternalDeck? CobaltLenDeck { get; private set; }
        //character art sprites
        public static ExternalCharacter? CobaltLen_Character { get; private set; }
        public static ExternalSprite? CobaltLen_CharacterMini_Sprite { get; private set; }
        public static ExternalSprite? CobaltLen_CharacterPortrait_Sprite { get; private set; }
        public static ExternalSprite? CobaltLen_CharacterPanelFrame_Sprite { get; private set; }
        public static ExternalSprite? CobaltLen_CharacterGameover_Sprite { get; private set; }
        public static ExternalSprite? CobaltLen_CharacterSquint_Sprite { get; private set; }
        //character animation sprites
        public static ExternalAnimation? CobaltLen_Character_DefaultAnimation { get; private set; }
        public static ExternalAnimation? CobaltLen_Character_MiniAnimation { get; private set; }
        public static ExternalAnimation? CobaltLen_Character_GameoverAnimation { get; private set; }
        public static ExternalAnimation? CobaltLen_Character_SquintAnimation { get; private set; }
        //background art sprites
        public static ExternalSprite? CobaltLen_CardBackgroud { get; private set; }
        // card borders sprites
        public static ExternalSprite? BorderCobaltLenBasic { get; private set; }
        // artifact sprites
        public static ExternalSprite? LenArtifactBananaStashSprite { get; private set; }
        public static ExternalSprite? LenArtifactBananaStashOffSprite { get; private set; }
        public static ExternalSprite? LenArtifactBriocheSprite { get; private set; }
        public static ExternalSprite? LenArtifactMaidDressSprite { get; private set; }
        public static ExternalSprite? LenArtifactGlassBottleSprite { get; private set; }
        public static ExternalSprite? LenArtifactTwinPowerSprite { get; private set; }
        // status sprites
        public static ExternalSprite? LenStatusNotesSprite { get; private set; }
        // icon sprites
        public static ExternalSprite? LenActionGainBananaPositive { get; private set; }
        public static ExternalSprite? LenActionGainBananaNegative { get; private set; }
        public static ExternalSprite? LenActionEatBanana { get; private set; }
        public static ExternalSprite? LenActionThrowBanana { get; private set; }
        public static ExternalSprite? LenActionSmashBanana { get; private set; }
        // artifact
        public static ExternalArtifact? LenArtifactBananaStash { get; private set; }
        public static ExternalArtifact? LenArtifactBrioche { get; private set; }
        public static ExternalArtifact? LenArtifactMaidDress { get; private set; }
        public static ExternalArtifact? LenArtifactGlassBottle { get; private set; }
        public static ExternalArtifact? LenArtifactTwinPower { get; private set; }
        // status
        public static ExternalStatus? LenStatusNotes { get; private set; }
        // glossary
        public static ExternalGlossary? LenGlossaryGainBananaPositive { get; private set; }
        public static ExternalGlossary? LenGlossaryGainBananaNegative { get; private set; }
        public static ExternalGlossary? LenGlossaryGainBananaLoseAll { get; private set; }
        public static ExternalGlossary? LenGlossaryEatBanana { get; private set; }
        public static ExternalGlossary? LenGlossaryThrowBanana { get; private set; }
        public static ExternalGlossary? LenGlossarySmashBanana { get; private set; }
        // card
        public static ExternalCard? LenCardBanana { get; private set; }
        public static ExternalCard? LenCardBreaktime { get; private set; }
        public static ExternalCard? LenCardBananaWall { get; private set; }
        public static ExternalCard? LenCardNakakapagpabagabag { get; private set; }
        public static ExternalCard? LenCardPlusBoy { get; private set; }
        public static ExternalCard? LenCardFunkyNightTown { get; private set; }
        public static ExternalCard? LenCardTelecasterBoy { get; private set; }
        public static ExternalCard? LenCardLawEvadingRock { get; private set; }
        public static ExternalCard? LenCardHolyLanceExplosion { get; private set; }
        public static ExternalCard? LenCardFifthPierrot { get; private set; }
        public static ExternalCard? LenCardRemoteControl { get; private set; }
        public static ExternalCard? LenCardChildishWar { get; private set; }
        public static ExternalCard? LenCardBringItOn { get; private set; }
        public static ExternalCard? LenCardLikeDislike { get; private set; }
        public static ExternalCard? LenCardBarisolChild { get; private set; }
        public static ExternalCard? LenCardNiccoriTeamSurvey { get; private set; }
        public static ExternalCard? LenCardButterflyOnShoulder { get; private set; }
        public static ExternalCard? LenCardVampiresPathos { get; private set; }
        public static ExternalCard? LenCardServantOfEvil { get; private set; }
        public static ExternalCard? LenCardParadichlorobenzene { get; private set; }
        public static ExternalCard? LenCardToluthinAntenna { get; private set; }
        public DirectoryInfo? GameRootFolder { get; set; }
        void ISpriteManifest.LoadManifest(ISpriteRegistry artRegistry)
        {
            if (this.ModRootFolder == null)
                throw new Exception("Root Folder not set");

            //character sprites
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLen.png"));
                    CobaltLen_CharacterPortrait_Sprite = new ExternalSprite("CobaltLen.sprites.CobaltLen_CharacterPortrait_Sprite", new FileInfo(path));
                    artRegistry.RegisterArt(CobaltLen_CharacterPortrait_Sprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenMini.png"));
                    CobaltLen_CharacterMini_Sprite = new ExternalSprite("CobaltLen.sprites.CobaltLen_CharacterMini_Sprite", new FileInfo(path));
                    artRegistry.RegisterArt(CobaltLen_CharacterMini_Sprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenFrame.png"));
                    CobaltLen_CharacterPanelFrame_Sprite = new ExternalSprite("CobaltLen.sprites.CobaltLen_CharacterPanelFrame_Sprite", new FileInfo(path));
                    artRegistry.RegisterArt(CobaltLen_CharacterPanelFrame_Sprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLen.png"));
                    CobaltLen_CharacterGameover_Sprite = new ExternalSprite("CobaltLen.sprites.CobaltLen_CharacterGameover_Sprite", new FileInfo(path));
                    artRegistry.RegisterArt(CobaltLen_CharacterGameover_Sprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLen.png"));
                    CobaltLen_CharacterSquint_Sprite = new ExternalSprite("CobaltLen.sprites.CobaltLen_CharacterSquint_Sprite", new FileInfo(path));
                    artRegistry.RegisterArt(CobaltLen_CharacterSquint_Sprite);
                }
            }
            //card background
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenCardBackground.png"));
                    CobaltLen_CardBackgroud = new ExternalSprite("CobaltLen.sprites.CobaltLen_CardBackground", new FileInfo(path));
                    artRegistry.RegisterArt(CobaltLen_CardBackgroud);
                }
            }
            //card border
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenCardBorder.png"));
                    BorderCobaltLenBasic = new ExternalSprite("CobaltLen.sprites.CobaltLenCardBorder", new FileInfo(path));
                    artRegistry.RegisterArt(BorderCobaltLenBasic);
                }
            }
            //artifact sprite
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenArtifactBananaStash.png"));
                    LenArtifactBananaStashSprite = new ExternalSprite("CobaltLen.sprites.LenArtifactBananaStashSprite", new FileInfo(path));
                    artRegistry.RegisterArt(LenArtifactBananaStashSprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenArtifactBananaStash.png"));
                    LenArtifactBananaStashOffSprite = new ExternalSprite("CobaltLen.sprites.LenArtifactBananaStashOffSprite", new FileInfo(path));
                    artRegistry.RegisterArt(LenArtifactBananaStashOffSprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenArtifactBrioche.png"));
                    LenArtifactBriocheSprite = new ExternalSprite("CobaltLen.sprites.LenArtifactBriocheSprite", new FileInfo(path));
                    artRegistry.RegisterArt(LenArtifactBriocheSprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenArtifactMaidDress.png"));
                    LenArtifactMaidDressSprite = new ExternalSprite("CobaltLen.sprites.LenArtifactMaidDressSprite", new FileInfo(path));
                    artRegistry.RegisterArt(LenArtifactMaidDressSprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenArtifactGlassBottle.png"));
                    LenArtifactGlassBottleSprite = new ExternalSprite("CobaltLen.sprites.LenArtifactGlassBottleSprite", new FileInfo(path));
                    artRegistry.RegisterArt(LenArtifactGlassBottleSprite);
                }
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenArtifactTwinPower.png"));
                    LenArtifactTwinPowerSprite = new ExternalSprite("CobaltLen.sprites.LenArtifactTwinPowerSprite", new FileInfo(path));
                    artRegistry.RegisterArt(LenArtifactTwinPowerSprite);
                }
            }
            //status sprite
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenStatusNotes.png"));
                    LenStatusNotesSprite = new ExternalSprite("CobaltLen.sprites.LenStatusNotesSprite", new FileInfo(path));
                    artRegistry.RegisterArt(LenStatusNotesSprite);
                }
            }
            //icons sprite
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenActionGainBananaPositive.png"));
                    LenActionGainBananaPositive = new ExternalSprite("CobaltLen.sprites.LenActionGainBananaPositive", new FileInfo(path));
                    artRegistry.RegisterArt(LenActionGainBananaPositive);
                }
            }
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenActionGainBananaNegative.png"));
                    LenActionGainBananaNegative = new ExternalSprite("CobaltLen.sprites.LenActionGainBananaNegative", new FileInfo(path));
                    artRegistry.RegisterArt(LenActionGainBananaNegative);
                }
            }
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenActionEatBanana.png"));
                    LenActionEatBanana = new ExternalSprite("CobaltLen.sprites.LenActionEatBanana", new FileInfo(path));
                    artRegistry.RegisterArt(LenActionEatBanana);
                }
            }
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenActionThrowBanana.png"));
                    LenActionThrowBanana = new ExternalSprite("CobaltLen.sprites.LenActionThrowBanana", new FileInfo(path));
                    artRegistry.RegisterArt(LenActionThrowBanana);
                }
            }
            {
                {
                    var path = Path.Combine(ModRootFolder.FullName, "Sprites", Path.GetFileName("CobaltLenActionSmashBanana.png"));
                    LenActionSmashBanana = new ExternalSprite("CobaltLen.sprites.LenActionSmashBanana", new FileInfo(path));
                    artRegistry.RegisterArt(LenActionSmashBanana);
                }
            }
        }
        public void LoadManifest(IDeckRegistry registry)
        {
            var card_DefaultArt = CobaltLen_CardBackgroud ?? throw new Exception();
            var borderCobaltLenDeckSprite = BorderCobaltLenBasic ?? throw new Exception();
            CobaltLenDeck = new ExternalDeck(
                "Sorwest.LenMod.CobaltLenDeck",
                CobaltLen_Primary_Color,
                System.Drawing.Color.Black,
                card_DefaultArt,
                borderCobaltLenDeckSprite,
                null);
            registry.RegisterDeck(Manifest.CobaltLenDeck);
        }
        void ICharacterManifest.LoadManifest(ICharacterRegistry registry)
        {
            {
                CobaltLen_Character = new ExternalCharacter("Sorwest.LenMod.Character.CobaltLen",
                CobaltLenDeck ?? throw new Exception("Missing Deck"),
                CobaltLen_CharacterPanelFrame_Sprite ?? throw new Exception("Missing Portrait"),
                new Type[]
                    {
                        typeof(LenCardBanana),
                        typeof(LenCardBanana),
                        typeof(LenCardBanana),
                        typeof(LenCardBananaWall),
                        typeof(LenCardBreaktime),
                    },
                new Type[]
                    {
                        typeof(LenArtifactBananaStash),
                    },
                CobaltLen_Character_DefaultAnimation ?? throw new Exception("missing default animation"),
                CobaltLen_Character_MiniAnimation ?? throw new Exception("missing mini animation"));
                CobaltLen_Character.AddNameLocalisation(CobaltLen_CharacterColH + "Len</c>", "en");
                CobaltLen_Character.AddDescLocalisation(CobaltLen_CharacterColH + "LEN</c>\nWhat is a VTuber doing in this game?", "en");
                registry.RegisterCharacter(CobaltLen_Character);
            }
        }
        void IAnimationManifest.LoadManifest(IAnimationRegistry registry)
        {
            {
                CobaltLen_Character_DefaultAnimation = new ExternalAnimation("CobaltLen.Animation.CobaltLen_Character_DefaultAnimation",
                    CobaltLenDeck ?? throw new Exception("missing deck"),
                    "neutral",
                    false,
                    new ExternalSprite[] {
                        CobaltLen_CharacterPortrait_Sprite ?? throw new Exception("missing portrait") });

                registry.RegisterAnimation(CobaltLen_Character_DefaultAnimation);
            }
            {
                CobaltLen_Character_MiniAnimation = new ExternalAnimation("CobaltLen.Animation.CobaltLen_Character_MiniAnimation",
                    CobaltLenDeck ?? throw new Exception("missing deck"),
                    "mini",
                    false,
                    new ExternalSprite[] {
                        CobaltLen_CharacterMini_Sprite ?? throw new Exception("missing mini") });

                registry.RegisterAnimation(CobaltLen_Character_MiniAnimation);
            }
            {
                CobaltLen_Character_GameoverAnimation = new ExternalAnimation("CobaltLen.Animation.CobaltLen_Character_GameoverAnimation",
                CobaltLenDeck ?? throw new Exception("missing deck"),
                "gameover",
                false,
                new ExternalSprite[] {
                    CobaltLen_CharacterGameover_Sprite ?? throw new Exception("missing portrait") });

                registry.RegisterAnimation(CobaltLen_Character_GameoverAnimation);
            }
            {
                CobaltLen_Character_SquintAnimation = new ExternalAnimation("CobaltLen.Animation.CobaltLen_Character_SquintAnimation",
                CobaltLenDeck ?? throw new Exception("missing deck"),
                "squint",
                false,
                new ExternalSprite[] {
                    CobaltLen_CharacterSquint_Sprite ?? throw new Exception("missing portrait") });

                registry.RegisterAnimation(CobaltLen_Character_SquintAnimation);
            }
        }
        void ICardManifest.LoadManifest(ICardRegistry registry)
        {
            var card_DefaultArt = CobaltLen_CardBackgroud ?? throw new Exception("missing card_DefaultArt");
            {
                LenCardBanana = new ExternalCard("CobaltLen.LenCardBanana",
                    typeof(LenCardBanana),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardBanana);
                LenCardBanana.AddLocalisation("Banana");
            }
            {
                LenCardBreaktime = new ExternalCard("CobaltLen.LenCardBreaktime",
                    typeof(LenCardBreaktime),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardBreaktime);
                LenCardBreaktime.AddLocalisation("Breaktime", desc: "<c=card>Throw</c> {0} bananas for the price of one.");
            }
            {
                LenCardBananaWall = new ExternalCard("CobaltLen.LenCardBananaWall",
                    typeof(LenCardBananaWall),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardBananaWall);
                LenCardBananaWall.AddLocalisation("Banana Wall", desc: "<c=hurt>Smash</c> a banana to gain {0} <c=status>shield</c>.");
            }
            {
                LenCardNakakapagpabagabag = new ExternalCard("CobaltLen.LenCardNakakapagpabagabag",
                    typeof(LenCardNakakapagpabagabag),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardNakakapagpabagabag);
                LenCardNakakapagpabagabag.AddLocalisation("Nakakapagpabagabag", desc: "<c=hurt>Smash</c> a banana to gain 1 <c=status>overdrive</c>.");
            }
            {
                LenCardPlusBoy = new ExternalCard("CobaltLen.LenCardPlusBoy",
                    typeof(LenCardPlusBoy),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardPlusBoy);
                LenCardPlusBoy.AddLocalisation("Plus Boy", desc: "<c=healing>Gain</c> {0} bananas.");
            }
            {
                LenCardFunkyNightTown = new ExternalCard("CobaltLen.LenCardFunkyNightTown",
                    typeof(LenCardFunkyNightTown),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardFunkyNightTown);
                LenCardFunkyNightTown.AddLocalisation("Funky Night Town");
            }
            {
                LenCardTelecasterBoy = new ExternalCard("CobaltLen.LenCardTelecasterBoy",
                    typeof(LenCardTelecasterBoy),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardTelecasterBoy);
                LenCardTelecasterBoy.AddLocalisation("Telecaster B-Boy");
            }
            {
                LenCardLawEvadingRock = new ExternalCard("CobaltLen.LenCardLawEvadingRock",
                    typeof(LenCardLawEvadingRock),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardLawEvadingRock);
                LenCardLawEvadingRock.AddLocalisation("LawEvading Rock");
            }
            {
                LenCardFifthPierrot = new ExternalCard("CobaltLen.LenCardFifthPierrot",
                    typeof(LenCardFifthPierrot),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardFifthPierrot);
                LenCardFifthPierrot.AddLocalisation("Fifth Pierrot");
            }
            {
                LenCardHolyLanceExplosion = new ExternalCard("CobaltLen.LenCardHolyLanceExplosion",
                    typeof(LenCardHolyLanceExplosion),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardHolyLanceExplosion);
                LenCardHolyLanceExplosion.AddLocalisation("Holy Lance Explosion", desc: "Gain 1 <c=status>droneshift</c>. <c=hurt>Smash</c> a banana to launch a sword.");
            }
            {
                LenCardRemoteControl = new ExternalCard("CobaltLen.LenCardRemoteControl",
                    typeof(LenCardRemoteControl),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardRemoteControl);
                LenCardRemoteControl.AddLocalisation("Remote Control");
            }
            {
                LenCardChildishWar = new ExternalCard("CobaltLen.LenCardChildishWar",
                    typeof(LenCardChildishWar),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardChildishWar);
                LenCardChildishWar.AddLocalisation("Childish War");
            }
            {
                LenCardBringItOn = new ExternalCard("CobaltLen.LenCardBringItOn",
                    typeof(LenCardBringItOn),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardBringItOn);
                LenCardBringItOn.AddLocalisation("Bring It On");
            }
            {
                LenCardLikeDislike = new ExternalCard("CobaltLen.LenCardLikeDislike",
                    typeof(LenCardLikeDislike),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardLikeDislike);
                LenCardLikeDislike.AddLocalisation("Like, Dislike");
            }
            {
                LenCardBarisolChild = new ExternalCard("CobaltLen.LenCardBarisolChild",
                    typeof(LenCardBarisolChild),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardBarisolChild);
                LenCardBarisolChild.AddLocalisation("Barisol's Child");
            }
            {
                LenCardNiccoriTeamSurvey = new ExternalCard("CobaltLen.LenCardNicorriTeamSurvey",
                    typeof(LenCardNiccoriTeamSurvey),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardNiccoriTeamSurvey);
                LenCardNiccoriTeamSurvey.AddLocalisation("Niccori Team Survey", desc: "<c=card>Throw</c> all bananas.");
            }
            {
                LenCardButterflyOnShoulder = new ExternalCard("CobaltLen.LenCardButterflyOnShoulder",
                    typeof(LenCardButterflyOnShoulder),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardButterflyOnShoulder);
                LenCardButterflyOnShoulder.AddLocalisation("Butterfly On Shoulder", desc: "<c=hurt>Smash</c> all bananas. Gain as many <c=status>notes</c>.", descB: "<c=hurt>Smash</c> all bananas. Gain twice many <c=status>notes</c>.");
            }
            {
                LenCardVampiresPathos = new ExternalCard("CobaltLen.LenCardVampiresPathos",
                    typeof(LenCardVampiresPathos),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardVampiresPathos);
                LenCardVampiresPathos.AddLocalisation("Vampire's PathoS", desc: "<c=card>Throw</c> a banana. <c=healing>Heal 1</c>.", descB: "<c=card>Throw</c> 2 bananas. <c=healing>Heal 1</c>.");
            }
            {
                LenCardServantOfEvil = new ExternalCard("CobaltLen.LenCardServantOfEvil",
                    typeof(LenCardServantOfEvil),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardServantOfEvil);
                LenCardServantOfEvil.AddLocalisation("Servant of Evil");
            }
            {
                LenCardParadichlorobenzene = new ExternalCard("CobaltLen.LenCardParadichlorobenzene",
                    typeof(LenCardParadichlorobenzene),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardParadichlorobenzene);
                LenCardParadichlorobenzene.AddLocalisation("Paradichlorobenzene", desc: "If you have bananas, the enemy loses all <c=status>shield</c>. <c=healing>Gain</c> a banana.");
            }
            {
                LenCardToluthinAntenna = new ExternalCard("CobaltLen.LenCardToluthinAntenna",
                    typeof(LenCardToluthinAntenna),
                    card_DefaultArt,
                    CobaltLenDeck);
                registry.RegisterCard(LenCardToluthinAntenna);
                LenCardToluthinAntenna.AddLocalisation("Toluthin Antenna", desc: "Add 1 of {0} <c=cardtrait>discount, temp</c> <c=hacker>Max</c> & <c=peri>Peri</c> cards to your hand.");
            }
        }
        public void LoadManifest(IArtifactRegistry registry)
        {
            {
                LenArtifactBananaStash = new ExternalArtifact("CobaltLen.Artifacts.LenArtifactBananaStash",
                    typeof(LenArtifactBananaStash),
                    LenArtifactBananaStashSprite ?? throw new Exception("missing artifact sprite"),
                    ownerDeck: CobaltLenDeck ?? throw new Exception("missing deck."));

                LenArtifactBananaStash.AddLocalisation("BANANA STASH",
                    "Each turn, Len <c=status>eats</c> a banana.\n<c=hurt>If there are no bananas, there will be no effect</c>.");

                registry.RegisterArtifact(LenArtifactBananaStash);
            }
            {
                LenArtifactBrioche = new ExternalArtifact("CobaltLen.Artifacts.LenArtifactBrioche",
                    typeof(LenArtifactBrioche),
                    LenArtifactBriocheSprite ?? throw new Exception("missing artifact sprite"),
                    ownerDeck: CobaltLenDeck ?? throw new Exception("missing deck."));

                LenArtifactBrioche.AddLocalisation("BRIOCHE",
                    "On pickup, Banana damage is increased by 1.");

                registry.RegisterArtifact(LenArtifactBrioche);
            }
            {
                LenArtifactMaidDress = new ExternalArtifact("CobaltLen.Artifacts.LenArtifactMaidDress",
                    typeof(LenArtifactMaidDress),
                    LenArtifactMaidDressSprite ?? throw new Exception("missing artifact sprite"),
                    ownerDeck: CobaltLenDeck ?? throw new Exception("missing deck."));

                LenArtifactMaidDress.AddLocalisation("MAID DRESS",
                    "<c=healing>#Blessed.</c> Bananas now give you 2 <c=status>SHIELD</c> before <c=status>eating</c> or <c=card>throwing</c> them.");

                registry.RegisterArtifact(LenArtifactMaidDress);
            }
            {
                LenArtifactGlassBottle = new ExternalArtifact("CobaltLen.Artifacts.LenArtifactGlassBottle",
                    typeof(LenArtifactGlassBottle),
                    LenArtifactGlassBottleSprite ?? throw new Exception("missing artifact sprite"),
                    ownerDeck: CobaltLenDeck ?? throw new Exception("missing deck."));

                LenArtifactGlassBottle.AddLocalisation("GLASS BOTTLE",
                    "At start of combat, <c=healing>gain</c> 3 bananas.");

                registry.RegisterArtifact(LenArtifactGlassBottle);
            }
            {
                LenArtifactTwinPower = new ExternalArtifact("CobaltLen.Artifacts.LenArtifactTwinPower",
                    typeof(LenArtifactTwinPower),
                    LenArtifactTwinPowerSprite ?? throw new Exception("missing artifact sprite"),
                    ownerDeck: CobaltLenDeck ?? throw new Exception("missing deck."));

                LenArtifactTwinPower.AddLocalisation("TWIN POWER",
                    "<c=455c92>Rin is hiding in the other ship.</c>\nGain 1 extra <c=energy>ENERGY</c> every turn. <c=hurt>On even turns, enemy ship gains 2</c> <c=status>overdrive</c>.");

                registry.RegisterArtifact(LenArtifactTwinPower);
            }
        }
        public void LoadManifest(IStatusRegistry statusRegistry)
        {
            //patch in logic for our statuses
            var harmony = new Harmony("Sorwest.LenMod.harmonyStatus");
            LenStatusNotesLogic(harmony);
            {
                LenStatusNotes = new ExternalStatus("CobaltLen.Status.NotesStatus",
                    true,
                    CobaltLen_Primary_Color,
                    null,
                    LenStatusNotesSprite ?? throw new Exception("MissingSprite"),
                    true);
                LenStatusNotes.AddLocalisation("Music Note", "At start of turn, <c=status>eat</c> {0} bananas for free. <c=hurt>Banana Stash passive is disabled</c>.");
                statusRegistry.RegisterStatus(LenStatusNotes);
            }
        }
        public void LoadManifest(IGlossaryRegisty glossaryRegistry)
        {
            {
                LenGlossaryGainBananaPositive = new ExternalGlossary("CobaltLen.Glossary.LenGlossaryGainBananaPositive",
                    "LenGlossaryGainBananaPositive",
                    false,
                    ExternalGlossary.GlossayType.action,
                    LenActionGainBananaPositive ?? throw new Exception("Missing LenActionGainBananaPositive Icon"));
                LenGlossaryGainBananaPositive.AddLocalisation("en", "Gain Banana", "Add {0} bananas to your Banana Stash.");
                glossaryRegistry.RegisterGlossary(LenGlossaryGainBananaPositive);
            }
            {
                LenGlossaryGainBananaNegative = new ExternalGlossary("CobaltLen.Glossary.LenGlossaryGainBananaNegative",
                    "LenGlossaryGainBananaNegative",
                    false,
                    ExternalGlossary.GlossayType.action,
                    LenActionGainBananaNegative ?? throw new Exception("Missing LenActionGainBananaNegative Icon"));
                LenGlossaryGainBananaNegative.AddLocalisation("en", "Lose Banana", "Remove {0} bananas from your Banana Stash.");
                glossaryRegistry.RegisterGlossary(LenGlossaryGainBananaNegative);
            }
            {
                LenGlossaryGainBananaLoseAll = new ExternalGlossary("CobaltLen.Glossary.LenGlossaryGainBananaLoseAll",
                    "LenGlossaryGainBananaLoseAll",
                    false,
                    ExternalGlossary.GlossayType.action,
                    LenActionGainBananaNegative ?? throw new Exception("Missing LenActionGainBananaNegative Icon"));
                LenGlossaryGainBananaLoseAll.AddLocalisation("en", "Lose All Bananas", "Remove all bananas from your Banana Stash.");
                glossaryRegistry.RegisterGlossary(LenGlossaryGainBananaLoseAll);
            }
            {
                LenGlossaryEatBanana = new ExternalGlossary("CobaltLen.Glossary.LenGlossaryEatBanana",
                    "LenGlossaryEatBanana",
                    false,
                    ExternalGlossary.GlossayType.action,
                    LenActionEatBanana ?? throw new Exception("Missing LenActionEatBanana Icon"));
                LenGlossaryEatBanana.AddLocalisation("en", "Eat Banana", "Len <c=status>eats</c> a banana, making the enemy so hungry they lose {0} hull.{1}");
                glossaryRegistry.RegisterGlossary(LenGlossaryEatBanana);
            }
            {
                LenGlossaryThrowBanana = new ExternalGlossary("CobaltLen.Glossary.LenGlossaryThrowBanana",
                    "LenGlossaryThrowBanana",
                    false,
                    ExternalGlossary.GlossayType.action,
                    LenActionThrowBanana ?? throw new Exception("Missing LenActionThrowBanana Icon"));
                LenGlossaryThrowBanana.AddLocalisation("en", "Throw Banana", "Len <c=card>throws</c> a banana so fast and strongly it deals {0} piercing damage to the enemy.{1}");
                glossaryRegistry.RegisterGlossary(LenGlossaryThrowBanana);
            }
            {
                LenGlossarySmashBanana = new ExternalGlossary("CobaltLen.Glossary.LenGlossarySmashBanana",
                    "LenGlossarySmashBanana",
                    false,
                    ExternalGlossary.GlossayType.action,
                    LenActionSmashBanana ?? throw new Exception("Missing LenActionSmashBanana Icon"));
                LenGlossarySmashBanana.AddLocalisation("en", "Smash Banana", "<c=455c92>Len is crying in the corner.</c>");
                glossaryRegistry.RegisterGlossary(LenGlossarySmashBanana);
            }
        }
        /*
         * Harmony stuff
         */
        private void LenStatusNotesLogic(Harmony harmony)
        {
            {
                MethodInfo method1 = typeof(Ship).GetMethod("OnBeginTurn") ?? throw new Exception("Couldn't find Combat.OnBeginTurn method");
                MethodInfo method2 = typeof(Manifest).GetMethod("LenStatusTurnBegin", BindingFlags.Public | BindingFlags.Static) ?? throw new Exception("Couldn't find Manifest.LenStatusTurnBegin method");
                harmony.Patch(method1, prefix: new HarmonyMethod(method2));
            }
        }
        public static bool LenStatusTurnBegin(Ship __instance, State s, Combat c)
        {
            if (LenStatusNotes?.Id != null && __instance.isPlayerShip)
            {
                var status = (Status)LenStatusNotes.Id;
                var amount = s.ship.Get(status);
                if (amount != 0)
                {
                    var enemyDamage = 0;
                    var shieldNumber = 0;
                    var internalCounter = 0;
                    var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
                    if (artifactBananaStash != null)
                    {
                        enemyDamage = artifactBananaStash.enemyDamage;
                        shieldNumber = artifactBananaStash.shieldNumber;
                        internalCounter = amount;
                    }
                    do
                    {
                        if (internalCounter <= 0)
                            break;
                        if (shieldNumber > 0)
                        {
                            AStatus aStatus1 = new AStatus();
                            aStatus1.status = Status.shield;
                            aStatus1.statusAmount = shieldNumber;
                            aStatus1.targetPlayer = true;
                            c.QueueImmediate(aStatus1);
                        }
                        if (enemyDamage > 0)
                        {
                            AHurt aHurt1 = new AHurt();
                            aHurt1.hurtAmount = enemyDamage;
                            aHurt1.targetPlayer = false;
                            c.QueueImmediate(aHurt1);
                        }
                        internalCounter -= 1;
                        s.ship.PulseStatus(status);
                    }
                    while (internalCounter > 0);
                }
            }
            return true;
        }
    }
}