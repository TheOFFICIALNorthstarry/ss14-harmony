using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Shared._Harmony.Obsessed.Components;

/// <summary>
/// Marks a player as obsessed.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ObsessedComponent : Component
{
    [DataField]
    public EntityUid? Obsession;

    [DataField]
    public int PhotosTaken = 0;

    [DataField]
    public TimeSpan LastPhotoTaken = TimeSpan.Zero;
}
