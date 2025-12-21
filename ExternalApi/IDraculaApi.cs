using Nickel;
using System;
using System.Collections.Generic;

namespace Sorwest.LenMod.ExternalAPI;

public interface IDraculaApi
{
    IDeckEntry DraculaDeck { get; }

    void RegisterBloodTapOptionProvider(Status status, Func<State, Combat, Status, List<CardAction>> provider);
}