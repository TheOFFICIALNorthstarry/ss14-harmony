using System.Diagnostics.CodeAnalysis;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared._Harmony.Photography;
using Content.Shared.Humanoid;
using Content.Shared.Mind;
using Content.Shared.Mind.Filters;
using Content.Shared.Objectives.Systems;
using Robust.Shared.Timing;

namespace Content.Shared._Harmony.Obsessed.EntitySystems;

public sealed partial class ObsessedSystem : EntitySystem
{
    [Dependency] private readonly SharedMindSystem _mind = default!;
    [Dependency] private readonly TargetSystem _target = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ObsessedComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<ObsessedComponent, PhotographTakenEvent>(OnPhotoTaken);
    }

    private void OnMapInit(Entity<ObsessedComponent> ent, ref MapInitEvent args)
    {
        // target already assigned
        if (ent.Comp.Obsession != null)
            return;

        var pool = new AliveHumansPool();
        _mind.TryGetMind(ent, out var mindId, out _);

        ent.Comp.Obsession = _target.PickFromPool(pool, new(), mindId);
    }

    private void OnPhotoTaken(Entity<ObsessedComponent> ent, ref PhotographTakenEvent args)
    {
        if (!TryComp<MindComponent>(ent.Comp.Obsession, out var mind))
            return;

        if (args.Target == mind.OwnedEntity && _timing.CurTime > ent.Comp.LastPhotoTaken + TimeSpan.FromMinutes(5))
        {
            ent.Comp.PhotosTaken++;
            ent.Comp.LastPhotoTaken = _timing.CurTime;
        }
    }

    /// <summary>
    /// Gets an Obsessed's obsession. Returns false if either output was null.
    /// </summary>
    /// <param name="ent">The entity with ObsessedComponent</param>
    /// <param name="obsessionEntity">Outputs the entity the obsession is currently controlling.</param>
    /// <param name="obsessionMind">Outputs the obsession's mind.</param>
    /// <returns></returns>

    public bool TryGetObsession(Entity<ObsessedComponent?> ent, [NotNullWhen(true)] out Entity<HumanoidProfileComponent?>? obsessionEntity, [NotNullWhen(true)] out Entity<MindComponent?>? obsessionMind)
    {
        obsessionEntity = null;
        obsessionMind = null;
        if (ent.Comp == null)
            return false;

        if (ent.Comp.Obsession == null)
            return false;

        if (!TryComp<MindComponent>(ent.Comp.Obsession.Value, out var obsMind))
            return false;

        obsessionMind = (ent.Comp.Obsession.Value, obsMind);
        obsessionEntity = obsMind.OwnedEntity;

        return obsessionMind is not null && obsessionEntity is not null;
    }
}
