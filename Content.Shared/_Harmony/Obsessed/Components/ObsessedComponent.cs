using Content.Shared.Mind.Filters;
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
    /// Stores the name of the obsession entity at the time of assignment as to not cause issues in case the obsession is renamed ("zombified Urist McObsession") or deleted.
    /// </summary>
    [DataField]
    public string? ObsessionName;

    /// <summary>
    /// A list of filters to apply to choosing an obsession.
    /// </summary>
    [DataField]
    public List<MindFilter> Filters = new();

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
    public float Distance = 16f; // same as a pinpointer turning blue

    /// <summary>
    /// The interval between the update loop which checks the distance between the Obsessed and obsession.
    /// </summary>
    [DataField]
    public TimeSpan DistanceCheckInterval = TimeSpan.FromSeconds(1);

    /// <summary>
    /// When the distance between the Obsessed and obsession will next be  checked.
    /// </summary>
    [DataField]
    public TimeSpan NextUpdate = TimeSpan.Zero;

    /// <summary>
    /// The amount of time the Obsessed has currently spent with their obsession.
    /// </summary>
    [DataField]
    public TimeSpan TimeSpent = TimeSpan.Zero;
}
