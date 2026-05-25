namespace Content.Shared._Harmony.Photography;

public sealed partial class PhotographTakenEvent : EntityEventArgs
{
    public EntityUid User;
    public EntityUid Target;
}
