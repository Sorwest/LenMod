using Nanoray.PluginManager;
using Newtonsoft.Json;
using Nickel;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;
using Sorwest.LenMod.Features;
using System.Linq;
using System.Reflection;

namespace Sorwest.LenMod.Enemies;
internal sealed class MikuAI : AI, IRegisterable
{
    [JsonProperty]
    private int aiCounter;
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        helper.Content.Enemies.RegisterEnemy(new()
        {
            EnemyType = MethodBase.GetCurrentMethod()!.DeclaringType!,
            ShouldAppearOnMap = (_, map)
            => (ModEntry.Instance.Settings.ProfileBased.Current.EnabledMikuAI
                && _.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault() is not null
                && map is MapLawless) ? BattleType.Elite : null,
            Name = ModEntry.Instance.AnyLocalizations.Bind(["enemy", "miku", "title"]).Localize
        });
        helper.Content.Ships.RegisterPart("mikuchassis", new()
        {
            Sprite = ModEntry.Instance.Sprites["mikuchassis"].Sprite
        });
        helper.Content.Ships.RegisterPart("mikuwing", new()
        {
            Sprite = ModEntry.Instance.Sprites["mikuwing"].Sprite
        });
        helper.Content.Ships.RegisterPart("mikucannon", new()
        {
            Sprite = ModEntry.Instance.Sprites["mikucannon"].Sprite
        });
        helper.Content.Ships.RegisterPart("mikumissiles", new()
        {
            Sprite = ModEntry.Instance.Sprites["mikumissiles"].Sprite
        });
        helper.Content.Ships.RegisterPart("mikuempty", new()
        {
            Sprite = ModEntry.Instance.Sprites["mikuempty"].Sprite
        });
        helper.Content.Ships.RegisterPart("mikucannonempty", new()
        {
            Sprite = ModEntry.Instance.Sprites["mikucannonempty"].Sprite
        });
    }
    public override void OnCombatStart(State state, Combat combat)
    {
        combat.bg = new BGCrystalNebula();
    }
    public override Ship BuildShipForSelf(State state)
    {
        character = new()
        {
            type = ModEntry.Instance.MikuNPC.CharacterType
        };
        bool hard = state.GetHarderElites();
        int hp = hard ? 20 : 15;
        int shield = hard ? 7 : 3;
        return new Ship()
        {
            x = 4,
            hull = hp,
            hullMax = hp,
            shieldMaxBase = shield,
            chassisUnder = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikuchassis"].UniqueName,
            ai = this,
            parts =
            [
                new Part()
                {
                    type = PType.wing,
                    skin = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikuwing"].UniqueName,
                    damageModifier = hard ? PDamMod.armor : PDamMod.none
                },
                new Part()
                {
                    type = PType.cannon,
                    skin = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikucannon"].UniqueName,
                },
                new Part()
                {
                    type = PType.empty,
                    skin = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikuempty"].UniqueName,
                },
                new Part()
                {
                    type = PType.empty,
                    skin = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikucannonempty"].UniqueName,
                },
                new Part()
                {
                    type = PType.cannon,
                    skin = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikucannon"].UniqueName,
                },
                new Part()
                {
                    type = PType.missiles,
                    skin = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikumissiles"].UniqueName,
                },
                new Part()
                {
                    type = PType.wing,
                    skin = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikuwing"].UniqueName,
                    flip = true,
                    damageModifier = hard ? PDamMod.armor : PDamMod.none
                }
            ]
        };
    }
    public override Song? GetSong(State state)
    {
        return Song.Elite;
    }
    public override EnemyDecision PickNextIntent(State state, Combat combat, Ship ownShip)
    {
        if (aiCounter >= 39)
            return MoveSet(
                aiCounter++,
                () => new EnemyDecision
                {

                    actions = AIHelpers.MoveToAimAt(state, ownShip, state.ship, 1, 99, movesFast: true, attackWeakPoints: true, avoidAsteroids: true, avoidMines: true),
                    intents =
                [
                    new IntentAttack
                    {
                        damage = aiCounter,
                        fromX = 0
                    },
                    new IntentAttack
                    {
                        damage = aiCounter,
                        fromX = 1
                    },
                    new IntentAttack
                    {
                        damage = aiCounter,
                        fromX = 3
                    },
                    new IntentAttack
                    {
                        damage = aiCounter,
                        fromX = 4
                    },
                    new IntentAttack
                    {
                        damage = aiCounter,
                        fromX = 5
                    },
                    new IntentAttack
                    {
                        damage = aiCounter,
                        fromX = 6
                    }
                ]
            });
        bool hard = state.GetHarderElites();
        MissileType m1 = MissileType.normal;
        int b1 = 1;
        int d1 = 1;
        int p1 = 1;
        if (hard)
        {
            m1 = MissileType.heavy;
            b1 = 2;
            d1 = 2;
        }
        if (aiCounter > 9)
        {
            m1 = hard ? MissileType.seeker : MissileType.heavy;
            p1 = 2;
        }
        return MoveSet(
            aiCounter++,
            () => new EnemyDecision
            {
                actions = AIHelpers.MoveToAimAt(state, ownShip, state.ship, 0, 5, movesFast: false, attackWeakPoints: false, avoidAsteroids: false, avoidMines: false),
                intents =
                [
                    new IntentAttack
                    {
                        damage = aiCounter / 3,
                        status = Status.heat,
                        statusAmount = 1,
                        fromX = 0
                    },
                    new IntentAttack
                    {
                        damage = d1 + (aiCounter / 3),
                        status = Status.boost,
                        statusAmount = 1,
                        fromX = 1
                    },
                    new IntentAttack
                    {
                        damage = d1,
                        fromX = 3
                    },
                    hard ? new IntentAttack
                    {
                        damage = 0,
                        fromX = 4
                    } : new IntentAttack
                    {
                        damage = 0,
                        status = Status.boost,
                        statusAmount = 1,
                        fromX = 4
                    },
                    new IntentStatus
                    {
                        status = MusicNoteManager.MusicNoteStatus.Status,
                        amount = b1,
                        targetSelf = true,
                        fromX = 5
                    },
                    new IntentAttack
                    {
                        damage = aiCounter / (hard ? 3 : 5),
                        status = Status.heat,
                        statusAmount = 1,
                        fromX = 6
                    },
                ]
            },
            () => new EnemyDecision
            {
                actions = AIHelpers.MoveToAimAt(state, ownShip, state.ship, 1, 5, movesFast: false, attackWeakPoints: hard, avoidAsteroids: true, avoidMines: true),
                intents =
                [
                    hard ? new IntentMissile
                    {
                        targetPlayer = true,
                        missileType = m1,
                        fromX = 0
                    } : new IntentSpawn
                    { 
                        thing = new Asteroid(),
                        fromX = 0
                    },
                    new IntentStatus
                    {
                        status = Status.shield,
                        amount = b1,
                        targetSelf = true,
                        fromX = 2
                    },
                    new IntentStatus
                    {
                        status = MusicNoteManager.MusicNoteStatus.Status,
                        amount = b1,
                        targetSelf = true,
                        fromX = 5
                    },
                    new IntentStatus
                    {
                        status = MusicNoteManager.MusicNoteStatus.Status,
                        amount = b1,
                        targetSelf = true,
                        fromX = 6
                    }
                ]
            },
            () => new EnemyDecision
            {
                actions = AIHelpers.MoveToAimAt(state, ownShip, state.ship, 1, 5, movesFast: false, attackWeakPoints: true, avoidAsteroids: false, avoidMines: false),
                intents = 
                [
                    new IntentStatus
                    {
                        status = MusicNoteManager.MusicNoteStatus.Status,
                        amount = b1,
                        targetSelf = true,
                        fromX = 0
                    },
                    new IntentAttack
                    {
                        damage = d1,
                        status = Status.boost,
                        statusAmount = 1,
                        fromX = 1
                    },
                    new IntentSwapAllPartToEmptyType
                    {
                        keyNormal = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikucannon"].UniqueName,
                        keyEmpty = ModEntry.Instance.Helper.Content.Ships.RegisteredParts["mikucannonempty"].UniqueName,
                        type = PType.cannon,
                        fromX = 3
                    },
                    new IntentAttack
                    {
                        damage = 1,
                        status = Status.boost,
                        fromX = 4
                    },
                    new IntentStatus
                    {
                        status = Status.powerdrive,
                        amount = p1,
                        targetSelf = true,
                        fromX = 6
                    }
                ]
            },
            () => new EnemyDecision
            {
                actions = AIHelpers.MoveToAimAt(state, ownShip, state.ship, 0, 5, movesFast: false, attackWeakPoints: false, avoidAsteroids: false, avoidMines: false),
                intents =
                [
                    hard ? new IntentMissile
                    {
                        targetPlayer = true,
                        missileType = m1,
                        fromX = 0
                    } : new IntentSpawn
                    {
                        thing = new Asteroid(),
                        fromX = 0
                    },
                    new IntentAttack
                    {
                        damage = hard ? d1 : 0,
                        status = Status.boost,
                        statusAmount = 1,
                        fromX = 1
                    },
                    new IntentMissile
                    {
                        targetPlayer = true,
                        missileType = m1,
                        fromX = 5
                    },
                    hard ? new IntentMissile
                    {
                        targetPlayer = true,
                        missileType = m1,
                        fromX = 6
                    } : new IntentSpawn
                    {
                        thing = new Asteroid(),
                        fromX = 6
                    }
                ]
            });
    }
}