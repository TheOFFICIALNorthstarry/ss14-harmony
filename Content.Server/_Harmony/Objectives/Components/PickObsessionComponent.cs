using Content.Server._Harmony.Objectives.Systems;

namespace Content.Server._Harmony.Objectives.Components;

/// <summary>
/// Sets the target for <see cref="TargetObjectiveComponent"/> to the owner's obsession. Will fail if they are not obsessed.
/// </summary>
[RegisterComponent, Access(typeof(PickObsessionSystem))]
public sealed partial class PickObsessionComponent : Component;
