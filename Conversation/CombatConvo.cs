namespace Sorwest.LenMod.Conversation;

internal class CombatConvo
{
    internal static void Inject()
    {
        string story;
        string len = ModEntry.Instance.LenCharacter.CharacterType;
        string miku = ModEntry.Instance.MikuNPC.CharacterType;
        System.Collections.Generic.Dictionary<string, StoryNode> bank = DB.story.all;
        var loc = ModEntry.Instance.StoryLocs.Localize;
        {
            bank[story = $"{len}_MikuFight_0"] = new()
            {
                type = NodeType.combat,
                allPresent = [len, miku],
                turnStart = true,
                oncePerRun = true,
                priority = true,
                lines =
                [
                    new CustomSay()
                    {
                        who = loc([story, "1", "who"]),
                        text = loc([story, "1", "what"]),
                        loopTag = loc([story, "1", "tag"])
                    },
                    new CustomSay()
                    {
                        who = loc([story, "2", "who"]),
                        text = loc([story, "2", "what"]),
                        loopTag = loc([story, "2", "tag"])
                    }
                ]
            };
        }
        // MID COMBAT SHOUTS
        {
            bank[story = $"{len}_WeGotHurtButNotTooBad_0"] = new()
            {
                type = NodeType.combat,
                allPresent = [len],
                enemyShotJustHit = true,
                minDamageDealtToPlayerThisTurn = 1,
                maxDamageDealtToPlayerThisTurn = 1,
                lines =
                [
                    new CustomSay()
                    {
                        who = loc([story, "1", "who"]),
                        text = loc([story, "1", "what"]),
                        loopTag = loc([story, "1", "tag"])
                    }
                ]
            };
            bank[story = $"{len}_OneHitPointThisIsFine_0"] = new()
            {
                type = NodeType.combat,
                allPresent = [len],
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                enemyShotJustHit = true,
                maxHull = 1,
                lines =
                [
                    new CustomSay()
                    {
                        who = loc([story, "1", "who"]),
                        text = loc([story, "1", "what"]),
                        loopTag = loc([story, "1", "tag"])
                    }
                ]
            };
            bank[story = $"{len}_OneHitPointThisIsFine_1"] = new()
            {
                type = NodeType.combat,
                allPresent = [len],
                oncePerCombatTags = ["aboutToDie"],
                oncePerRun = true,
                enemyShotJustHit = true,
                maxHull = 1,
                lines =
                [
                    new CustomSay()
                    {
                        who = loc([story, "1", "who"]),
                        text = loc([story, "1", "what"]),
                        loopTag = loc([story, "1", "tag"])
                    }
                ]
            };
            bank[story = $"{len}_JustPlayedASashaCard_0"] = new()
            {
                type = NodeType.combat,
                allPresent = [len],
                oncePerRunTags = ["usedASashaCard"],
                whoDidThat = Deck.sasha,
                lines =
                [
                    new CustomSay()
                    {
                        who = loc([story, "1", "who"]),
                        text = loc([story, "1", "what"]),
                        loopTag = loc([story, "1", "tag"])
                    }
                ]
            };
        }
        // CARD LOOKUPS
        {
            bank[story = $"{len}_Card_ServantOfEvil_0"] = new()
            {
                type = NodeType.combat,
                allPresent = [len],
                priority = true,
                oncePerCombat = true,
                oncePerRun = true,
                lookup =
                [
                    "card_servantofevil_played"
                ],
                lines =
                [
                    new CustomSay()
                    {
                        who = loc([story, "1", "who"]),
                        text = loc([story, "1", "what"])
                    }
                ]
            };
        }
    }
}