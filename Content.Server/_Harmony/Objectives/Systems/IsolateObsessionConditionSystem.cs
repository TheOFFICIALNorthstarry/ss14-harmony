using Content.Server._Harmony.Objectives.Components;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared.Humanoid;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Content.Shared.Roles;
using Content.Shared.Roles.Jobs;
using Robust.Shared.Player;

namespace Content.Server._Harmony.Objectives.Systems;

public sealed class IsolateObsessionConditionSystem : EntitySystem
{
    [Dependency] private readonly SharedJobSystem _job = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<IsolateObsessionConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnGetProgress(Entity<IsolateObsessionConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        args.Progress = GetProgress(ent, ref args);
    }

    private List<EntityUid> GetTargets(Entity<IsolateObsessionConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        var targets = new List<EntityUid>();
        var players = AllEntityQuery<HumanoidProfileComponent, ActorComponent>();

        if (!TryComp<ObsessedComponent>(args.Mind.OwnedEntity, out var obsessed)
            || obsessed.Obsession == null
            || !_mind.TryGetMind(obsessed.Obsession.Value, out var obsessionMindId, out var obsessionMind))
            return targets;

        if (!_job.MindTryGetJobId(obsessionMindId, out var obsessionJobId))
            return targets;

        if (!_job.TryGetAllDepartments(obsessionJobId!, out var departments))
            return targets;

        var target = false;

        while (players.MoveNext(out var uid, out _, out _))
        {
            if (!_mind.TryGetMind(uid, out var mindId, out var mind))
                continue; // they don't have a mind
            if (!_job.MindTryGetJobId(mindId, out var jobId) || jobId is null)
                continue; // they don't have a job
            if (!_job.TryGetAllDepartments(jobId, out var departmentProtos))
                continue; // their job doesn't have any departments?
            if (uid == args.Mind.OwnedEntity)
                continue; // this IS the obsessed
            if (uid == obsessionMind.OwnedEntity)
                continue; // this IS the obsession

            foreach (DepartmentPrototype obsessionDepartmentProto in departments)
            {
                foreach (DepartmentPrototype departmentProto in departmentProtos)
                {
                    if (obsessionDepartmentProto.ID == departmentProto.ID)
                    {
                        target = true;
                        break;
                    }
                }
                if (target)
                    break;
            }

            if (target)
                targets.Add(uid);
        }

        return targets;
    }

    private float GetProgress(Entity<IsolateObsessionConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        var targets = GetTargets(ent, ref args);
        var killed = 0;

        foreach (var target in targets)
        {
            // no mind, catatonic or something, counts as dead
            if (!_mind.TryGetMind(target, out var mindId, out var mindComp))
            {
                killed++;
                continue;
            }

            if (_mind.IsCharacterDeadIc(mindComp))
                killed++;
        }

        if (targets.Count == 0)
            return 1f; // they're all gibbed or dead or something so you get the greentext

        return (float)killed / targets.Count;
    }
}
