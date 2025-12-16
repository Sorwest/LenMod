using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;

namespace Sorwest.LenMod.Actions;
public class AGainBanana : CardAction
{
    public int amount;
    public bool loseAll;
    public bool smashBool;
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
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaGain"].Sprite, amount, Colors.textMain);
        else if (loseAll)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite, null, Colors.textMain);
        return icon;
    }
    public override List<Tooltip> GetTooltips(State state)
    {
        List<Tooltip> tooltips = new();
        if (smashBool && !loseAll)
        {
            tooltips.Add(new TTText(
                ModEntry.Instance.Localizations.Localize(["action", "SmashBanana", "flavor"])
            ));
        }
        if (amount < 0)
            tooltips.Add(new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::AGainBananaLose")
            {
                Icon = ModEntry.Instance.Sprites["GainBananaLose"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "lose"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "lose"], new { Amount = -1 * amount }),
            });
        else if (amount > 0)
            tooltips.Add(new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::AGainBananaGain")
            {
                Icon = ModEntry.Instance.Sprites["GainBananaGain"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "gain"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "gain"], new { Amount = amount }),
            });
        if (loseAll)
            tooltips.Add(new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::AGainBananaLoseAll")
            {
                Icon = ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "loseAll"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "loseAll"]),
            });
        return tooltips;
    }
}