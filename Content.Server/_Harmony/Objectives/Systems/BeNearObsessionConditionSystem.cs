using Content.Server._Harmony.Objectives.Components;
using Content.Server.Objectives.Systems;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared._Harmony.Obsessed.EntitySystems;
using Content.Shared.Objectives.Components;

namespace Content.Server._Harmony.Objectives.Systems;

public sealed partial class BeNearObsessionConditionSystem : EntitySystem
{
    [Dependency] private NumberObjectiveSystem _number = default!;
    [Dependency] private ObsessedSystem _obsessed = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<BeNearObsessionConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnGetProgress(Entity<BeNearObsessionConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        if (!TryComp<ObsessedComponent>(args.Mind.OwnedEntity, out var obsessed))
        {
            args.Progress = 0f;
            return;
        }

        args.Progress = (float)obsessed.TimeSpent.TotalSeconds / _number.GetTarget(ent);
    }
}