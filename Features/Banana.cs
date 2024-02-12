using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sorwest.LenMod.Artifacts;
using System.Linq;

namespace Sorwest.LenMod;
internal sealed class BananaManager : IStatusLogicHook
{
    public static ModEntry Instance => ModEntry.Instance;
    public BananaManager()
    {
        Instance.KokoroApi.RegisterStatusLogicHook(this, 0);
    }
    public void OnStatusTurnTrigger(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, int oldAmount, int newAmount)
    {
        if (status != Instance.BananaStatus.Status)
            return;
        if (timing != StatusTurnTriggerTiming.TurnStart)
            return;
        if (oldAmount <= 0)
            return;
        if (ship.Get(Instance.MusicNoteStatus.Status) > 0)
            return;
        combat.Queue(new ABananaDamage()
        {
            type = BananaType.AHurt,
            targetPlayer = !ship.isPlayerShip
        });
    }
    public bool HandleStatusTurnAutoStep(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, ref int amount, ref StatusTurnAutoStepSetStrategy setStrategy)
    {
        if (status != Instance.BananaStatus.Status)
            return false;
        if (timing != StatusTurnTriggerTiming.TurnStart)
            return false;
        if (amount > 0)
            amount--;
        return false;
    }
}
[JsonConverter(typeof(StringEnumConverter))]
public enum BananaType
{
    AHurt,
    AAttack
}
public class ABananaDamage : CardAction
{
    public BananaType type;
    public int? dmg;
    public bool fast;
    public bool targetPlayer;
    public override void Begin(G g, State s, Combat c)
    {
        Ship source = targetPlayer ? c.otherShip : s.ship;
        var artifact = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
        int damage = 1 + (dmg is not null ? (int)dmg : 0);
        int shield = 0;
        if (artifact is not null && !targetPlayer)
        {
            damage = artifact.enemyDamage;
            shield = artifact.shieldNumber;
        }
        if (shield > 0)
        {
            c.Queue(new AStatus()
            {
                status = Status.shield,
                statusAmount = shield,
                targetPlayer = !targetPlayer
            });
        }
        if (type == BananaType.AAttack)
        {
            c.Queue(new AAttack()
            {
                damage = damage,
                piercing = targetPlayer,
                fast = fast
            });
        }
        else
        {
            c.Queue(new AHurt()
            {
                hurtAmount = damage,
                targetPlayer = targetPlayer
            });
        }

    }
}