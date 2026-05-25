using Content.Server._Harmony.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared.Objectives.Components;

namespace Content.Server._Harmony.Objectives.Systems;

public sealed class PhotographConditionSystem : EntitySystem
{
    [Dependency] private readonly NumberObjectiveSystem _number = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PhotographConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnGetProgress(Entity<PhotographConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        if (!TryComp<ObsessedComponent>(args.Mind.OwnedEntity, out var obsessed))
        {
            args.Progress = 0f;
            return;
        }

        args.Progress = (float)obsessed.PhotosTaken / _number.GetTarget(ent);
    }
}
