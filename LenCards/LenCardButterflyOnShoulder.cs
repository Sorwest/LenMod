using LenMod.LenActions;
using LenMod.LenArtifacts;

namespace LenMod.LenCards
{
    [CardMeta(rarity = Rarity.rare, upgradesTo = new Upgrade[] { Upgrade.A, Upgrade.B })]
    public class LenCardButterflyOnShoulder : Card
    {
        public override string Name() => "Butterfly On Shoulder";
        public override CardData GetData(State state)
        {
            CardData result = new CardData();
            result.exhaust = true;
            switch (upgrade)
            {
                case Upgrade.None:
                    result.cost = 4;
                    result.description = Loc.GetLocString(Manifest.LenCardButterflyOnShoulder?.DescLocKey ?? throw new Exception("Missing card"));
                    break;
                case Upgrade.A:
                    result.cost = 2;
                    result.description = Loc.GetLocString(Manifest.LenCardButterflyOnShoulder?.DescLocKey ?? throw new Exception("Missing card"));
                    break;
                case Upgrade.B:
                    result.cost = 4;
                    result.description = Loc.GetLocString(Manifest.LenCardButterflyOnShoulder?.DescBLocKey ?? throw new Exception("Missing card"));
                    break;
            }
            return result;
        }
        public override List<CardAction> GetActions(State s, Combat c)
        {
            var result = new List<CardAction>();
            var flagNoBananas = false;
            var internalCounter = 0;
            var notes_status = (Status)(Manifest.LenStatusNotes?.Id ?? throw new Exception("Missing NotesStatus"));
            var artifactBananaStash = s.EnumerateAllArtifacts().OfType<LenArtifactBananaStash>().FirstOrDefault();
            if (artifactBananaStash != null)
            {
                internalCounter = artifactBananaStash.counter;
                if (artifactBananaStash.counter <= 0)
                    flagNoBananas = true;
            }
            switch (upgrade)
            {
                case Upgrade.None:
                    List<CardAction> cardActionList1 = new List<CardAction>();
                    ASmashBanana aSmashBanana1 = new ASmashBanana();
                    aSmashBanana1.disabled = flagNoBananas;
                    cardActionList1.Add(aSmashBanana1);
                    AGainBanana aGainBanana1 = new AGainBanana();
                    aGainBanana1.loseAll = true;
                    aGainBanana1.disabled = flagNoBananas;
                    cardActionList1.Add(aGainBanana1);
                    AStatus aStatus1 = new AStatus();
                    aStatus1.status = notes_status;
                    aStatus1.statusAmount = internalCounter;
                    aStatus1.targetPlayer = true;
                    aStatus1.disabled = flagNoBananas;
                    cardActionList1.Add(aStatus1);
                    result = cardActionList1;
                    break;
                case Upgrade.A:
                    List<CardAction> cardActionList2 = new List<CardAction>();
                    ASmashBanana aSmashBanana2 = new ASmashBanana();
                    aSmashBanana2.disabled = flagNoBananas;
                    cardActionList2.Add(aSmashBanana2);
                    AGainBanana aGainBanana2 = new AGainBanana();
                    aGainBanana2.loseAll = true;
                    aGainBanana2.disabled = flagNoBananas;
                    cardActionList2.Add(aGainBanana2);
                    AStatus aStatus2 = new AStatus();
                    aStatus2.status = notes_status;
                    aStatus2.statusAmount = internalCounter;
                    aStatus2.targetPlayer = true;
                    aStatus2.disabled = flagNoBananas;
                    cardActionList2.Add(aStatus2);
                    result = cardActionList2;
                    break;
                case Upgrade.B:
                    List<CardAction> cardActionList3 = new List<CardAction>();
                    ASmashBanana aSmashBanana3 = new ASmashBanana();
                    aSmashBanana3.disabled = flagNoBananas;
                    cardActionList3.Add(aSmashBanana3);
                    AGainBanana aGainBanana3 = new AGainBanana();
                    aGainBanana3.loseAll = true;
                    aGainBanana3.disabled = flagNoBananas;
                    cardActionList3.Add(aGainBanana3);
                    AStatus aStatus3 = new AStatus();
                    aStatus3.status = notes_status;
                    aStatus3.statusAmount = internalCounter * 2;
                    aStatus3.targetPlayer = true;
                    aStatus3.disabled = flagNoBananas;
                    cardActionList3.Add(aStatus3);
                    result = cardActionList3;
                    break;
            }
            return result;
        }
    }
}