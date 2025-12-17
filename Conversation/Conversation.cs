using Nanoray.PluginManager;
using Nickel;
using System;
using System.Collections.Generic;

namespace Sorwest.LenMod.Conversation;
internal sealed class ConvoManager : IRegisterable
{
    public static void Register(IPluginPackage<IModManifest> package, IModHelper helper)
    {
        EventConvo.Inject();
        CombatConvo.Inject();
    }
}
internal sealed class CustomSay : Say
{
    private static int NextId = 1;

    public string? text { get; set; }
    public string? DynamicLoopTag { get; set; }

    internal static readonly Dictionary<string, Func<G, string>> RegisteredDynamicLoopTags = [];

    public override bool Execute(G g, IScriptTarget target, ScriptCtx ctx)
    {
        if (text is null)
            return base.Execute(g, target, ctx);
        if (!string.IsNullOrEmpty(hash))
            return base.Execute(g, target, ctx);

        if (DynamicLoopTag is not null)
            loopTag = RegisteredDynamicLoopTags.TryGetValue(DynamicLoopTag, out Func<G, string>? dynamicLoopTagFunction)
                ? dynamicLoopTagFunction(g)
                : DynamicLoopTag;

        hash = $"{GetType().FullName}:{NextId++}";
        DB.currentLocale.strings[GetLocKey(ctx.script, hash)] = text;
        return base.Execute(g, target, ctx);
    }
}
internal sealed class CustomTitle : TitleCard
{
    public string? text { get; set; }

    public override bool Execute(G g, IScriptTarget target, ScriptCtx ctx)
    {
        if (target is Dialogue dialogue)
        {
            dialogue.titleCard = empty == true ? null : text;
        }

        return true;
    }
}