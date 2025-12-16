using Nickel;
using Sorwest.LenMod.Features;
using System.Collections.Generic;

namespace Sorwest.LenMod.Actions;
public class ABananaDamage : CardAction
{
    public bool isThrow = false;
    public int damage;
    public bool targetPlayer;
    public bool keepBanana = false;
    public int minimumBanana = 1;
    private static int GetBananaDmg(State s)
    {
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return s.route is not Combat ? dmg : (s.ship.Get(BananaManager.BananaStatus.Status) > 0 ? dmg : 0);
    }
    private static int GetBananaShield(State s)
    {
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaShield");
    }
    private static int GetBananaPiercing(State s)
    {
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaPiercing");
    }
    private static bool DoWeHaveCannonsThough(State s)
    {
        foreach (Part part in s.ship.parts)
        {
            if (part.type == PType.cannon)
            {
                return true;
            }
        }

        if (s.route is Combat combat)
        {
            foreach (StuffBase value in combat.stuff.Values)
            {
                if (value is JupiterDrone)
                {
                    return true;
                }
            }
        }

        return false;
    }
    public override Icon? GetIcon(State s)
    {
        string spriteName = isThrow ? (ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaPiercing") > 0 ? "ThrowBananaPiercing" : "ThrowBanana") : "EatBanana";
        return new Icon(ModEntry.Instance.Sprites[spriteName].Sprite, damage, Colors.textMain);
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        int dmg = GetBananaDmg(s);
        string bananaType = isThrow ? "ThrowBanana" : "EatBanana";
        string spriteName = isThrow ? (ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaPiercing") > 0 ? "ThrowBananaPiercing" : "ThrowBanana") : "EatBanana";
        return
        [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::A{bananaType}")
            {
                Icon = ModEntry.Instance.Sprites[spriteName].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", bananaType, "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", bananaType, "description"], new { Damage = dmg }),
            }
        ];
    }
    public override void Begin(G g, State s, Combat c)
    {
        if (s.ship.Get(BananaManager.BananaStatus.Status) < minimumBanana && !keepBanana)
            return;
        int shield = GetBananaShield(s);
        int piercing = GetBananaPiercing(s);
        if (shield > 0)
        {
            c.Queue(new AStatus()
            {
                status = Status.shield,
                statusAmount = shield,
                targetPlayer = !targetPlayer,
                timer = 0
            });
        }
        if (isThrow && DoWeHaveCannonsThough(s))
        {
            c.Queue(new ABananaAttack()
            {
                damage = damage,
                piercing = piercing > 0,
                fast = true
            });
        }
        else if (!isThrow)
        {
            c.Queue(new ABananaHunger()
            {
                hurtAmount = damage,
                targetPlayer = targetPlayer
            });
        }
        if (!keepBanana && (!isThrow || DoWeHaveCannonsThough(s)))
        {
            c.Queue(new AStatus()
            {
                status = BananaManager.BananaStatus.Status,
                statusAmount = -1,
                targetPlayer = !targetPlayer,
                timer = 0
            });
        }
    }
}
public class ABananaAttack : AAttack
{
    public int minimumBanana = 1;
    private int GetBananaDmg(State s)
    {
        bool normalDisplay = s.route is not Combat || s.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return normalDisplay ? dmg : 0;
    }
    private Spr GetIconSprite(State s)
    {
        bool normalDisplay = s.route is not Combat || s.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaPiercing") > 0 ?
            (normalDisplay ? ModEntry.Instance.Sprites["ThrowBananaPiercing"].Sprite : ModEntry.Instance.Sprites["ThrowBananaPiercingCost"].Sprite)
          : (normalDisplay ? ModEntry.Instance.Sprites["ThrowBanana"].Sprite : ModEntry.Instance.Sprites["ThrowBananaCost"].Sprite);
    }
    public override Icon? GetIcon(State s)
    {
        Color color;
        if (s.route is not Combat || DoWeHaveCannonsThough(s))
            color = Colors.redd;
        else
            color = Colors.attackFail;
        return new Icon(GetIconSprite(s), damage, color, false);
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        //attack highlight happens in tooltip call
        Combat? combat = (Combat)s.route;
        int n = s.ship.x;
        foreach (Part part in s.ship.parts)
        {
            if (part.type == PType.cannon && part.active)
            {
                if (combat is not null && combat.stuff.TryGetValue(n, out StuffBase? value))
                {
                    value.hilight = 2;
                }
                part.hilight = true;
            }
            n++;
        }
        int dmg = GetBananaDmg(s);
        dmg = dmg > 0 ? dmg : ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return
        [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::AThrowBanana")
            {
                Icon = GetIconSprite(s),
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "ThrowBanana", "description"], new { Damage = dmg }),
            }
        ];
    }
}
public class ABananaHunger : AHurt
{
    public int minimumBanana = 1;
    private int GetBananaDmg(State s)
    {
        bool normalDisplay = s.route is not Combat || s.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return normalDisplay ? dmg : 0;
    }
    private Spr GetIconSprite(State s)
    {
        bool normalDisplay = s.route is not Combat || s.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        return normalDisplay ? ModEntry.Instance.Sprites["EatBanana"].Sprite : ModEntry.Instance.Sprites["EatBananaCost"].Sprite;
    }
    public override Icon? GetIcon(State s)
    {
        return new Icon(GetIconSprite(s), hurtAmount, Colors.redd, false);
    }
    public override List<Tooltip> GetTooltips(State s)
    {
        int dmg = GetBananaDmg(s);
        dmg = dmg > 0 ? dmg : ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(s, "BananaDamage") + 1;
        return
        [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::AEatBanana")
            {
                Icon = ModEntry.Instance.Sprites["EatBanana"].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", "EatBanana", "description"], new { Damage = dmg }),
            }
        ];
    }
}