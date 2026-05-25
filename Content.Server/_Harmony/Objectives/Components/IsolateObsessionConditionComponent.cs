using Content.Server._Harmony.Objectives.Systems;

namespace Content.Server._Harmony.Objectives.Components;

/// <summary>
/// Requires the holder to be an Obsessed and requires them to kill every other member of their obsession's department.
/// </summary>
[RegisterComponent, Access(typeof(IsolateObsessionConditionSystem))]
public sealed partial class IsolateObsessionConditionComponent : Component;
