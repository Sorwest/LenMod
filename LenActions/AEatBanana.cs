using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LenMod.LenArtifacts;

namespace LenMod.LenActions
{
    public class AEatBanana : CardAction
    {
        private int GetBananaDmg(State s)
        {
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                return artifactBananaStash.enemyDamage;
            }
            return 0;
        }
        private int GetBananaShield(State s)
        {
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                return artifactBananaStash.shieldNumber;
            }
            return 0;
        }
        public override Icon? GetIcon(State s)
        {
            return new Icon((Spr)(Manifest.LenActionEatBanana!.Id!), null, Colors.textMain);
        }
        public override List<Tooltip> GetTooltips(State s)
        {
            List<Tooltip> tooltips = new List<Tooltip>();
            var shieldAmount = GetBananaShield(s);
            var damageAmount = GetBananaDmg(s);
            var glossary = Manifest.LenGlossaryEatBanana?.Head ?? throw new Exception("missing LenGlossaryEatBanana glossary");
            var str = "";
            if (shieldAmount > 0)
            {
                str = string.Format("\n<c=text>Before</c> <c=status>eating</c><c=text>, gain</c> {0} <c=status>SHIELD</c><c=text>.</c>", shieldAmount);
            }
            tooltips.Add(new TTGlossary(glossary, damageAmount > 0 ? damageAmount : 1, str));
            return tooltips;
        }
    }
}
