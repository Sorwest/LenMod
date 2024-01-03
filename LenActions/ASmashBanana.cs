namespace LenMod.LenActions
{
    public class ASmashBanana : AGainBanana
    {
        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)(Manifest.LenActionSmashBanana!.Id!), null, Colors.textMain);
        }
        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>
            {
                new TTText("<c=405a7f>Len is crying in the corner.</c>")
            };
            tooltips.AddRange(base.GetTooltips(s));
            return tooltips;
        }
    }
}
