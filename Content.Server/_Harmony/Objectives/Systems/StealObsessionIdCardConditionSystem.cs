using Content.Server._Harmony.Objectives.Components;
using Content.Shared._Harmony.Obsessed.EntitySystems;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Mind;
using Content.Shared.Objectives.Components;
using Robust.Shared.Containers;

namespace Content.Server._Harmony.Objectives.Systems;

public sealed partial class StealObsessionIdCardConditionSystem : EntitySystem
{
    [Dependency] private readonly ObsessedSystem _obsessed = default!;
    [Dependency] private readonly SharedIdCardSystem _idCard = default!;
    [Dependency] private readonly SharedMindSystem _mind = default!;

    private EntityQuery<ContainerManagerComponent> _containerQuery;

    public override void Initialize()
    {
        base.Initialize();

        _containerQuery = GetEntityQuery<ContainerManagerComponent>();

        SubscribeLocalEvent<StealObsessionIdCardConditionComponent, ObjectiveGetProgressEvent>(OnGetProgress);
    }

    private void OnGetProgress(Entity<StealObsessionIdCardConditionComponent> ent, ref ObjectiveGetProgressEvent args)
    {
        if (!TryComp<ObsessedComponent>(args.Mind.OwnedEntity, out var obsessedComp)
            || !_obsessed.TryGetObsession((args.Mind.OwnedEntity.Value, obsessedComp), out var obsession, out _)
            || !_containerQuery.TryGetComponent(args.Mind.OwnedEntity, out var currentManager))
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
                    // check if this ID matches
                    if (TryComp<IdCardComponent>(entity, out var id) && id.FullName == obsessedComp.ObsessionName) // yes, if somebody breaks into HoP to change an ID's name to be the same as their obsession and holds onto that ID, that counts as a greentext. that is also just as antagonistic and requires you to either steal someone else's ID or rename your OWN ID and make it obvious anyways so I think it's fine.
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

        args.Progress = 0f;
    }
}