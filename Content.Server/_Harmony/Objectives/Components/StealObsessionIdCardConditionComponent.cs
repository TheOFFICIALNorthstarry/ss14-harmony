using Content.Server._Harmony.Objectives.Systems;

namespace Content.Server._Harmony.Objectives.Components;

/// <summary>
/// Requires the holder to be an Obsessed and requires them to steal their obsession's ID card.
/// </summary>
[RegisterComponent, Access(typeof(StealObsessionIdCardConditionSystem))]
public sealed partial class StealObsessionIdCardConditionComponent : Component;
