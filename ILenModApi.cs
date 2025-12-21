using Nickel;

namespace Sorwest.LenMod;

public interface ILenModApi
{
    IDeckEntry LenDeck { get; }
    IStatusEntry BananaStatus { get; }
    IStatusEntry BananaTreatStatus { get; }
    IStatusEntry BananaTreeStatus { get; }
    IStatusEntry MusicNoteStatus { get; }

}