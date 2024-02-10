using Sorwest.LenMod.Artifacts;
using System.Linq;

namespace Sorwest.LenMod;
internal sealed class MusicNoteManager : IStatusLogicHook
{
    public static ModEntry Instance => ModEntry.Instance;
    public MusicNoteManager()
    {
        Instance.KokoroApi.RegisterStatusLogicHook(this, 0);
    }
    public void OnStatusTurnTrigger(State state, Combat combat, StatusTurnTriggerTiming timing, Ship ship, Status status, int oldAmount, int newAmount)
    {
        if (status != Instance.MusicNoteStatus.Status)
            return;
        if (timing != StatusTurnTriggerTiming.TurnStart)
            return;
        if (oldAmount <= 0)
            return;
        combat.Queue(new AMusicNoteAction()
        {
            targetPlayer = !ship.isPlayerShip
        });
    }
}
public class AMusicNoteAction : CardAction
{
    public bool targetPlayer;
    public override void Begin(G g, State s, Combat c)
    {
        Ship target = targetPlayer ? s.ship : c.otherShip;
        Ship source = targetPlayer ? c.otherShip : s.ship;
        Status status = ModEntry.Instance.MusicNoteStatus.Status;
        int amount = source.Get(status);
        if (amount > 0)
        {
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                int enemyDamage = artifactBananaStash.enemyDamage;
                int shieldNumber = artifactBananaStash.shieldNumber;
                int internalCounter = amount;
                do
                {
                    if (internalCounter <= 0)
                        break;
                    if (shieldNumber > 0)
                    {
                        c.Queue(new AStatus()
                        {
                            status = Status.shield,
                            statusAmount = shieldNumber,
                            targetPlayer = !targetPlayer
                        });
                    }
                    if (enemyDamage > 0)
                    {
                        c.Queue(new AHurt()
                        {
                            hurtAmount = enemyDamage,
                            targetPlayer = targetPlayer
                        });
                    }
                    internalCounter -= 1;
                    s.ship.PulseStatus(status);
                }
                while (internalCounter > 0);
            }
        }
    }
}