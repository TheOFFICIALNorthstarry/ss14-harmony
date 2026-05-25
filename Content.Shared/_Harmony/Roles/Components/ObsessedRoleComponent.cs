using Content.Shared.Roles.Components;
using Robust.Shared.GameStates;

namespace Content.Shared._Harmony.Roles.Components;

/// <summary>
/// Added to mind role entities to tag that they are obsessed.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ObsessedRoleComponent : BaseMindRoleComponent;
