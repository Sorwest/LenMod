using Nickel;
using Sorwest.LenMod.Features;

namespace Sorwest.LenMod;

public sealed class ApiImplementation : ILenModApi
{
    public IDeckEntry LenDeck
        => ModEntry.Instance.LenDeck;

    public IStatusEntry BananaStatus
        => BananaManager.BananaStatus;

    public IStatusEntry BananaTreatStatus
        => BananaTreatManager.BananaTreatStatus;

    public IStatusEntry BananaTreeStatus
        => BananaTreeManager.BananaTreeStatus;

    public IStatusEntry MusicNoteStatus
        => MusicNoteManager.MusicNoteStatus;

}