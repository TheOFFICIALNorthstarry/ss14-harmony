using Robust.Shared.GameStates;
using Robust.Shared.Serialization.TypeSerializers.Implementations;

namespace Content.Shared._Harmony.Obsessed.Components;

/// <summary>
/// Marks a player as obsessed.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ObsessedComponent : Component
{
    /// <summary>
    /// The mind of the Obsessed's obsession.
    /// </summary>
    [DataField]
    public EntityUid? Obsession;

    /// <summary>
    /// The number of photos the Obsessed has taken of their obsession which were valid for their objective.
    /// </summary>
    [DataField]
    public int PhotosTaken = 0;

    /// <summary>
    /// The time at which the Obsessed took their last photo of their obsession.
    /// </summary>
    [DataField]
    public TimeSpan LastPhotoTaken = TimeSpan.Zero;

    /// <summary>
    /// The distance the Obsessed needs to be within of their obsession in order to tick up their objective.
    /// </summary>
    [DataField]
    public float CloseToObsessionDistance = 16f; // same as a pinpointer turning blue
}
