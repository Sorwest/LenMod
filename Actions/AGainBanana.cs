using System.Collections.Generic;

namespace Sorwest.LenMod.Actions;
public class AGainBanana : CardAction
{
    public int amount;
    public bool loseAll;
    public override void Begin(G g, State s, Combat c)
    {
        timer = 0;
        c.QueueImmediate(new AStatus()
        {
            status = ModEntry.Instance.BananaStatus.Status,
            statusAmount = amount,
            targetPlayer = true
        });
    }
    public override Icon? GetIcon(State s)
    {
        Icon icon = new Icon();
        if (amount < 0)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaLose"].Sprite, amount * -1, Colors.textMain);
        else if (amount > 0)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaGain"].Sprite, amount, Colors.textMain);
        else if (loseAll)
            icon = new Icon(ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite, null, Colors.textMain);
        return icon;
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        List<Tooltip> tooltips = new();
        if (amount < 0)
            tooltips.Add(new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.Sprites["GainBananaLose"].Sprite,
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "lose"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "lose"], new { Amount = -1 * amount }),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::LoseBanana"
            ));
        else if (amount > 0)
            tooltips.Add(new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.Sprites["GainBananaGain"].Sprite,
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "gain"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "gain"], new { Amount = amount }),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::GainBanana"
            ));
        else if (loseAll)
            tooltips.Add(new CustomTTGlossary(
                CustomTTGlossary.GlossaryType.action,
                () => ModEntry.Instance.Sprites["GainBananaLoseAll"].Sprite,
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "name", "loseAll"]),
                () => ModEntry.Instance.Localizations.Localize(["action", "GainBanana", "description", "loseAll"]),
                key: $"{ModEntry.Instance.Package.Manifest.UniqueName}::LoseBananaAll"
            ));
        return tooltips;
    }
}