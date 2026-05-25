using Content.Server._Harmony.Objectives.Components;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Robust.Shared.Containers;

namespace Content.Server._Harmony.Objectives.Systems;

public sealed partial class StealObsessionIdCardConditionSystem : EntitySystem
{
    [Dependency] private readonly SharedIdCardSystem _idCard = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;

    private EntityQuery<ContainerManagerComponent> _containerQuery;

    public override void Initialize()
    {
        base.Initialize();

        _containerQuery = GetEntityQuery<ContainerManagerComponent>();

        SubscribeLocalEvent<StealObsessionIdCardConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
        SubscribeLocalEvent<StealObsessionIdCardConditionComponent, ObjectiveAssignedEvent>(OnAssigned);
    }

    private void OnGetProgress(Entity<StealObsessionIdCardConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        if (ent.Comp.Target == null) // ID doesn't exist, give them a free greentext so they can progress
        {
            args.Progress = 1f;
            return;
        }


        if (!_containerQuery.TryGetComponent(args.Mind.OwnedEntity, out var currentManager))
        {
            args.Progress = 0f;
            return;
        }

        var containerStack = new Stack<ContainerManagerComponent>();
        // recursively check each container for the ID
        // checks inventory, bag, implants, etc.
        do
        {
            foreach (var container in currentManager.Containers.Values)
            {
                foreach (var entity in container.ContainedEntities)
                {
                    // check if this is the ID
                    if (entity == ent.Comp.Target)
                    {
                        args.Progress = 1f;
                        return;
                    }

                    // if it is a container check its contents
                    if (_containerQuery.TryGetComponent(entity, out var containerManager))
                        containerStack.Push(containerManager);
                }
            }
        } while (containerStack.TryPop(out currentManager));
    }

    private void OnAssigned(Entity<StealObsessionIdCardConditionComponent> ent, ref ObjectiveAssignedEvent args)
    {
        if (!TryComp<ObsessedComponent>(args.Mind.OwnedEntity, out var obsessed))
            return;

        if (obsessed.Obsession == null
            || !_mind.TryGetMind(obsessed.Obsession.Value, out _, out var mind)
            || mind.OwnedEntity == null
            || !_idCard.TryFindIdCard(mind.OwnedEntity.Value, out var idCard))
            return;

        ent.Comp.Target = idCard;
    }
}