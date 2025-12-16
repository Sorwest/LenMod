using Nanoray.PluginManager;
using Nickel;
using System.Collections.Generic;
using System.Reflection;

namespace Sorwest.LenMod.Cards
{
    [CardMeta(rarity = Rarity.common, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardLawEvadingRock : Card, IRegisterable
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
        public override CardData GetData(State state)
        {
            return new()
            {
                cost = upgrade == Upgrade.B ? 1 : 0
            };
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            List<CardAction> result =
            [
                new ASpawn()
                {
                    thing = new Asteroid(),
                    offset = -1
                }
            ];
            if (upgrade == Upgrade.A)
            {
                result.Add(new AMove()
                {
                    dir = -2,
                    targetPlayer = true
                });
            }
            else if (upgrade == Upgrade.B)
            {
                result.Insert(0, new ASpawn()
                {
                    thing = new Asteroid(),
                    offset = -2
                });
                result.Insert(0, new ASpawn()
                {
                    thing = new Asteroid(),
                    offset = -3
                });
            }
            return result;
        }
    }
}