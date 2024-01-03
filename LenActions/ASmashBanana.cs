using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LenMod.LenArtifacts;

namespace LenMod.LenActions
{
    public class ASmashBanana : CardAction
    {
        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)(Manifest.LenActionSmashBanana!.Id!), null, Colors.textMain);
        }
        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>
            {
                new TTGlossary(Manifest.LenGlossarySmashBanana?.Head ?? throw new Exception("missing LenGlossarySmashBanana glossary"), null)
            };
            return tooltips;
        }
    }
}
