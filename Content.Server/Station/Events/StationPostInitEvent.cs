using Content.Server.Station.Components;

namespace Content.Server.Station.Events;

/// <summary>
/// Raised directed on a station after it has been initialized, as well as broadcast.
<<<<<<< HEAD
/// This gets raised after the entity has been map-initialized, and the station's centcomm map/entity (if any) has been
/// set up.
=======
>>>>>>> a2133335fb6e574d2811a08800da08f11adab31f
/// </summary>
[ByRefEvent]
public readonly record struct StationPostInitEvent(Entity<StationDataComponent> Station);
