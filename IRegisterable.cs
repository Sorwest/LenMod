using Nanoray.PluginManager;
using Nickel;
namespace Sorwest.LenMod;

internal interface IRegisterable
{
    static abstract void Register(IPluginPackage<IModManifest> package, IModHelper helper);
}