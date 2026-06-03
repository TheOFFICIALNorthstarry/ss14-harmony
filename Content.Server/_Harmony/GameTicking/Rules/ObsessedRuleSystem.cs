using Content.Server._Harmony.GameTicking.Rules.Components;
using Content.Server.GameTicking.Rules;
using Content.Server.Roles;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared._Harmony.Obsessed.EntitySystems;
using Content.Shared._Harmony.Roles.Components;
using Content.Shared.Mind;

namespace Content.Server._Harmony.GameTicking.Rules;

public sealed partial class ObsessedRuleSystem : GameRuleSystem<ObsessedRuleComponent>
{
    [Dependency] private readonly ObsessedSystem _obsessed = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ObsessedRoleComponent, GetBriefingEvent>(OnGetBriefing);
    }

    private void OnGetBriefing(Entity<ObsessedRoleComponent> ent, ref GetBriefingEvent args)
    {
        args.Append(Loc.GetString("obsessed-role-greeting"));

        if (args.Mind.Comp.OwnedEntity == null
            || !TryComp<ObsessedComponent>(args.Mind.Comp.OwnedEntity, out var obsessed)
            || !_obsessed.TryGetObsession((args.Mind.Comp.OwnedEntity.Value, obsessed), out var obsession, out var obsessionMind))
            return;

        args.Append(Loc.GetString("obsessed-obsession", ("obsession", Name(obsession.Value))));
    }
}
