using Nanoray.PluginManager;
using Nickel;
namespace Sorwest.LenMod;

internal interface IModdedCard
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}
internal interface IModdedArtifact
{
    static abstract void Register(IModHelper helper);
}