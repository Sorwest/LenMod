using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;

namespace Sorwest.LenMod.Actions;

public class AGainBanana : CardAction
{
    public int amount;
    public bool loseAll;
    public override void Begin(G g, State state, Combat combat)
    {
        Status status = BananaManager.BananaStatus.Status;
        if (loseAll)
            amount = 0;
        timer = 0;
        combat.QueueImmediate(new AStatus()
        {
            status = status,
            statusAmount = amount,
            mode = loseAll ? AStatusMode.Set : AStatusMode.Add,
            targetPlayer = true
        });
    }
    public override Icon? GetIcon(State state)
    {
        Icon icon = new();
        if (amount < 0)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaLose"].Sprite, amount * -1, Colors.textMain);
        else if (amount > 0)
            icon = new Icon(ModEntry.Instance.Sprites["Banana"].Sprite, amount, Colors.textMain);
        else if (loseAll)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite, null, Colors.textMain);
        return icon;
    }
    public override List<Tooltip> GetTooltips(State state)
    {
        List<Tooltip> tooltips = [];
        if (amount > 0)
            tooltips = [.. StatusMeta.GetTooltips(BananaManager.BananaStatus.Status, amount)];
        else if (loseAll)
            tooltips = [new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::AGainBananaLoseAll")
            {
                Icon = ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "LoseAll", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "LoseAll", "description"]),
            }];
        return tooltips;
    }
}