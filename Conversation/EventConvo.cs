using System.Linq;
namespace Sorwest.LenMod.Conversation;

internal class EventConvo
{
    private static ModEntry Instance => ModEntry.Instance;
    internal static void Inject()
    {
        string story;
        string len = Instance.LenDeck.Deck.Key();
        System.Collections.Generic.Dictionary<string, StoryNode> bank = DB.story.all;
        var loc = Instance.StoryLocs.Localize;

        // INSERT TO EXISTING EVENTS
        {
            DB.story.GetNode(story = "GrandmaShop")?.lines.OfType<SaySwitch>().FirstOrDefault()?.lines.Insert(0, new CustomSay()
            {
                who = loc([story, "1", "who"]),
                text = loc([story, "1", "what"]),
                loopTag = loc([story, "1", "tag"])
            });
        }
        // NEW DIALOGUE FOR EXISTING EVENT CONDITIONS
        {
            bank[story = $"ChoiceCardRewardOfYourColorChoice_{len}"] = new()
            {
                type = NodeType.@event,
                oncePerRun = true,
                allPresent = [len],
                bg = "BGBootSequence",
                lines =
                [
                    new SaySwitch()
                    {
                        lines =
                        [
                            new CustomSay()
                            {
                                who = loc([story, "1", "who"]),
                                text = loc([story, "1", "what"]),
                                loopTag = loc([story, "1", "tag"]),
                            },
                            new CustomSay()
                            {
                                who = loc([story, "2", "who"]),
                                text = loc([story, "2", "what"]),
                                loopTag = loc([story, "2", "tag"]),
                            }
                        ]
                    }
                ]
            };
            bank[story = $"LoseCharacterCard_{len}"] = new()
            {
                type = NodeType.@event,
                oncePerRun = true,
                allPresent = [len],
                bg = "BGSupernova",
                lines =
                [
                    new SaySwitch()
                    {
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
                    }
                ]
            };
            bank[story = $"CrystallizedFriendEvent_{len}"] = new()
            {
                type = NodeType.@event,
                oncePerRun = true,
                allPresent = [len],
                bg = "BGCrystalizedFriend",
                lines =
                [
                    new Wait() {secs = 1.5},
                    new SaySwitch()
                    {
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
                    }
                ]
            };
        }
        // START RUN
        {
            bank[story = $"{len}_CharStart_0"] = new()
            {
                type = NodeType.@event,
                priority = true,
                once = true,
                lookup = ["zone_first"],
                allPresent = [len],
                bg = "BGRunStart",
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
                        loopTag = loc([story, "2", "tag"]),
                        flipped = true
                    },
                    new CustomSay()
                    {
                        who = loc([story, "3", "who"]),
                        text = loc([story, "3", "what"]),
                        loopTag = loc([story, "3", "tag"])
                    },
                    new CustomSay()
                    {
                        who = loc([story, "4", "who"]),
                        text = loc([story, "4", "what"]),
                        loopTag = loc([story, "4", "tag"])
                    },
                    new CustomSay()
                    {
                        who = loc([story, "5", "who"]),
                        text = loc([story, "5", "what"]),
                        loopTag = loc([story, "5", "tag"])
                    },
                    new CustomSay()
                    {
                        who = loc([story, "6", "who"]),
                        text = loc([story, "6", "what"]),
                        loopTag = loc([story, "1", "tag"]),
                        flipped = true
                    },
                    new CustomSay()
                    {
                        who = loc([story, "7", "who"]),
                        text = loc([story, "7", "what"]),
                        loopTag = loc([story, "7", "tag"])
                    },
                    new CustomSay()
                    {
                        who = loc([story, "8", "who"]),
                        text = loc([story, "8", "what"]),
                        loopTag = loc([story, "8", "tag"])
                    }
                ]
            };
        }
        // NEW RUN WIN
        {

        }
    }
}
