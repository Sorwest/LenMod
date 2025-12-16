using Sorwest.LenMod.Features;

namespace Sorwest.LenMod.Actions;

public class ABananaTreatDamage : CardAction
{
    private static int GetBananaDmg(State state)
    {
        return 1 + ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage");
    }
    public bool targetPlayer;
    public override void Begin(G g, State state, Combat c)
    {
        Ship source = targetPlayer ? c.otherShip : state.ship;
        Status status = BananaTreatManager.BananaTreatStatus.Status;
        int amount = source.Get(status);
        if (amount > 0)
        {
            int internalCounter = amount;
            do
            {
                combat.Queue(new ABananaDamage()
                {
                    damage = GetBananaDmg(state),
                    statusPulse = status,
                    targetPlayer = targetPlayer,
                    keepBanana = true
                });
                internalCounter -= 1;
            }
            while (internalCounter > 0);
        }
    }
}
