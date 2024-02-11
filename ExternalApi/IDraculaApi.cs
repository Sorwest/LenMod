using Nickel;
using System;
using System.Collections.Generic;

namespace Sorwest.LenMod;
public interface IDraculaApi
{
    IDeckEntry DraculaDeck { get; }

    void RegisterBloodTapOptionProvider(Status status, Func<State, Combat, Status, List<CardAction>> provider);
}