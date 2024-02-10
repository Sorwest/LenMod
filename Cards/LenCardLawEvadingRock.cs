using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;
using Sorwest.LenMod.Actions;
using Sorwest.LenMod.Artifacts;

namespace Sorwest.LenMod.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardLawEvadingRock : Card, IModdedCard
    {
        public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
        {
            helper.Content.Cards.RegisterCard("LawEvadingRock", new()
            {
                CardType = MethodBase.GetCurrentMethod()!.DeclaringType!,
                Meta = new()
                {
                    deck = ModEntry.Instance.LenDeck.Deck,
                    rarity = Rarity.common,
                    upgradesTo = [Upgrade.A, Upgrade.B]
                },
                Name = ModEntry.Instance.AnyLocalizations.Bind(["card", "LawEvadingRock", "name"]).Localize
            });
        }
        public override string Name() => "LawEvading Rock";
        public override CardData GetData(State state)
        {
            return new()
            {
                cost = 0,
                recycle = upgrade == Upgrade.B ? true : false
            };
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> result = new()
            {
                new ASpawn()
                {
                    thing = new Asteroid()
                }
            };
            if (upgrade == Upgrade.A)
                result.Insert(0, new ASpawn()
                {
                    thing = new Asteroid(),
                    offset = -1
                });
            return result;
        }
    }
}