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
    private static int GetBananaDmg(State state)
    {
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return state.route is not Combat ? dmg : (state.ship.Get(BananaManager.BananaStatus.Status) > 0 ? dmg : 0);
    }
    private static int GetBananaShield(State state)
    {
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaShield");
    }
    private static int GetBananaPiercing(State state)
    {
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaPiercing");
    }
    public string GetSpriteStr(State state)
    {
        string spriteName;
        if (isThrow)
        {
            if (GetBananaPiercing(state) > 0)
            {
                spriteName = "ThrowBananaPiercing";
            }
            else
            {
                spriteName = "ThrowBanana";
            }
        }
        else
        {
            spriteName = "EatBanana";
        }
        return spriteName;
    }
    private static bool DoWeHaveCannonsThough(State state)
    {
        foreach (Part part in state.ship.parts)
        {
            if (part.type == PType.cannon)
            {
                return true;
            }
        }

        if (state.route is Combat combat)
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
    public override Icon? GetIcon(State state)
    {
        return new Icon(ModEntry.Instance.Sprites[GetSpriteStr(state)].Sprite, damage, Colors.textMain);
    }
    public override List<Tooltip> GetTooltips(State state)
    {
        int dmg = GetBananaDmg(state);
        string bananaType = isThrow ? "ThrowBanana" : "EatBanana";
        return [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::A{bananaType}")
            {
                Icon = ModEntry.Instance.Sprites[GetSpriteStr(state)].Sprite,
                TitleColor = Colors.action,
                Title = ModEntry.Instance.Localizations.Localize(["action", bananaType, "name"]),
                Description = ModEntry.Instance.Localizations.Localize(["action", bananaType, "description"], new { Damage = dmg }),
            }
        ];
    }
    public override void Begin(G g, State state, Combat combat)
    {
        if (state.ship.Get(BananaManager.BananaStatus.Status) < minimumBanana && !keepBanana)
            return;
        int shield = GetBananaShield(state);
        int piercing = GetBananaPiercing(state);
        if (shield > 0)
        {
            combat.Queue(new AStatus()
            {
                status = Status.shield,
                statusAmount = shield,
                targetPlayer = !targetPlayer,
                timer = 0
            });
        }
        if (!keepBanana && (!isThrow || DoWeHaveCannonsThough(state)))
        {
            combat.Queue(new AStatus()
            {
                status = BananaManager.BananaStatus.Status,
                statusAmount = -1,
                targetPlayer = !targetPlayer,
                timer = 0
            });
        }
        if (isThrow && DoWeHaveCannonsThough(state))
        {
            combat.Queue(new ABananaAttack()
            {
                damage = damage,
                piercing = piercing > 0,
                fast = true
            });
        }
        else if (!isThrow)
        {
            combat.Queue(new ABananaHunger()
            {
                hurtAmount = damage,
                targetPlayer = targetPlayer
            });
        }
    }
}
public class ABananaAttack : AAttack
{
    public int minimumBanana = 1;
    private int GetBananaDmg(State state)
    {
        bool normalDisplay = state.route is not Combat || state.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return normalDisplay ? dmg : 0;
    }
    private Spr GetIconSprite(State state)
    {
        bool normalDisplay = state.route is not Combat || state.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        return ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaPiercing") > 0 ?
            (normalDisplay ? ModEntry.Instance.Sprites["ThrowBananaPiercing"].Sprite : ModEntry.Instance.Sprites["ThrowBananaPiercingCost"].Sprite)
          : (normalDisplay ? ModEntry.Instance.Sprites["ThrowBanana"].Sprite : ModEntry.Instance.Sprites["ThrowBananaCost"].Sprite);
    }
    public override Icon? GetIcon(State state)
    {
        Color color;
        if (state.route is not Combat || DoWeHaveCannonsThough(state))
            color = Colors.redd;
        else
            color = Colors.attackFail;
        return new Icon(GetIconSprite(state), damage, color, false);
    }
    public override List<Tooltip> GetTooltips(State state)
    {
        //attack highlight happens in tooltip call
        Combat? combat = (Combat)state.route;
        int n = state.ship.x;
        foreach (Part part in state.ship.parts)
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
        int dmg = GetBananaDmg(state);
        dmg = dmg > 0 ? dmg : ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return
        [
            new GlossaryTooltip($"action.{ModEntry.Instance.Package.Manifest.UniqueName}::AThrowBanana")
            {
                Icon = GetIconSprite(state),
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
    private int GetBananaDmg(State state)
    {
        bool normalDisplay = state.route is not Combat || state.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        int dmg = ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
        return normalDisplay ? dmg : 0;
    }
    private Spr GetIconSprite(State state)
    {
        bool normalDisplay = state.route is not Combat || state.ship.Get(BananaManager.BananaStatus.Status) >= minimumBanana;
        return normalDisplay ? ModEntry.Instance.Sprites["EatBanana"].Sprite : ModEntry.Instance.Sprites["EatBananaCost"].Sprite;
    }
    public override Icon? GetIcon(State state)
    {
        return new Icon(GetIconSprite(state), hurtAmount, Colors.redd, false);
    }
    public override List<Tooltip> GetTooltips(State state)
    {
        int dmg = GetBananaDmg(state);
        dmg = dmg > 0 ? dmg : ModEntry.Instance.Helper.ModData.GetModDataOrDefault<int>(state, "BananaDamage") + 1;
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