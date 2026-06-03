using Content.Server._Harmony.Objectives.Components;
using Content.Server.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared._Harmony.Obsessed.EntitySystems;
using Content.Shared.Objectives.Components;
using Content.Shared.Objectives.Systems;

namespace Content.Server._Harmony.Objectives.Systems;

public sealed partial class PickObsessionSystem : EntitySystem
{
    [Dependency] private ObsessedSystem _obsessed = default!;
    [Dependency] private TargetObjectiveSystem _objective = default!;
    [Dependency] private TargetSystem _target = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PickObsessionComponent, ObjectiveAssignedEvent>(OnAssigned);
    }

    private void OnAssigned(Entity<PickObsessionComponent> ent, ref ObjectiveAssignedEvent args)
    {
        // invalid objective prototype
        if (!TryComp<TargetObjectiveComponent>(ent, out var target))
        {
            args.Cancelled = true;
            return;
        }

        // target already assigned
        if (target.Target != null)
            return;

        // owner is not obsessed or the obsession is null
        if (!TryComp<ObsessedComponent>(args.Mind.OwnedEntity, out var obsessed)
            || !_obsessed.TryGetObsession((args.Mind.OwnedEntity.Value, obsessed), out _, out var obsession))
            return;

        _objective.SetTarget(ent, obsession.Value, target);
    }
}