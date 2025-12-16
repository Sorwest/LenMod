using Sorwest.LenMod.Features;

namespace Sorwest.LenMod.Actions;

public class ABananaTreatDamage : CardAction
{
    private static int GetBananaDmg(State s)
    {
        return 1 + ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage");
    }
    public bool targetPlayer;
    public override void Begin(G g, State s, Combat c)
    {
        Ship source = targetPlayer ? c.otherShip : s.ship;
        Status status = BananaTreatManager.BananaTreatStatus.Status;
        int amount = source.Get(status);
        if (amount > 0)
        {
            int internalCounter = amount;
            do
            {
                c.Queue(new ABananaDamage()
                {
                    damage = GetBananaDmg(s),
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
