using Content.Server._Harmony.GameTicking.Rules.Components;
using Content.Server.GameTicking.Rules;
using Content.Server.Roles;
using Content.Shared._Harmony.Obsessed.Components;
using Content.Shared._Harmony.Roles.Components;
using Content.Shared.Mind;

namespace Content.Server._Harmony.GameTicking.Rules;

public sealed partial class ObsessedRuleSystem : GameRuleSystem<ObsessedRuleComponent>
{
    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<ObsessedRoleComponent, GetBriefingEvent>(OnGetBriefing);
    }

    private void OnGetBriefing(Entity<ObsessedRoleComponent> ent, ref GetBriefingEvent args)
    {
        args.Append(Loc.GetString("obsessed-role-greeting"));

        if (!TryComp<MindComponent>(ent, out var mind)
            || mind.OwnedEntity == null
            || !TryComp<ObsessedComponent>(mind.OwnedEntity, out var obsessed)
            || obsessed.Obsession == null)
            return;

        args.Append(Loc.GetString("obsessed-obsession", ("obsession", Name(obsessed.Obsession.Value))));
    }
}
