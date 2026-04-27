using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Triage,
    NeedMoreInfo,
    Deferred,
    InProgress,
    Resolved,
    Closed,
    Reopened,
    Rejected
}

public enum BugTrigger
{
    StartTriage,
    RequestMoreInfo,
    ProvideMoreInfo,
    Defer,
    Resume,
    StartFix,
    Resolve,
    Close,
    Reopen,
    ReturnToTriage,
    MarkAsDuplicate,
    MarkAsNotBug,
    MarkAsNotReproducible
}

public sealed class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _stateMachine;

    public Bug()
    {
        _stateMachine = new StateMachine<BugState, BugTrigger>(BugState.New);
        ConfigureStateMachine();
    }

    public BugState State => _stateMachine.State;

    public bool CanApply(BugTrigger trigger)
    {
        return _stateMachine.CanFire(trigger);
    }

    public void Apply(BugTrigger trigger)
    {
        _stateMachine.Fire(trigger);
    }

    private void ConfigureStateMachine()
    {
        _stateMachine.Configure(BugState.New)
            .Permit(BugTrigger.StartTriage, BugState.Triage);

        _stateMachine.Configure(BugState.Triage)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.StartFix, BugState.InProgress)
            .Permit(BugTrigger.MarkAsDuplicate, BugState.Rejected)
            .Permit(BugTrigger.MarkAsNotBug, BugState.Rejected)
            .Permit(BugTrigger.MarkAsNotReproducible, BugState.Rejected);

        _stateMachine.Configure(BugState.NeedMoreInfo)
            .Permit(BugTrigger.ProvideMoreInfo, BugState.Triage);

        _stateMachine.Configure(BugState.Deferred)
            .Permit(BugTrigger.Resume, BugState.Triage);

        _stateMachine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Resolve, BugState.Resolved)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo);

        _stateMachine.Configure(BugState.Resolved)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Reopened)
            .Permit(BugTrigger.ReturnToTriage, BugState.Triage);

        _stateMachine.Configure(BugState.Rejected)
            .Permit(BugTrigger.Reopen, BugState.Reopened);
    }
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Initial state: {bug.State}");

        bug.Apply(BugTrigger.StartTriage);
        bug.Apply(BugTrigger.StartFix);
        bug.Apply(BugTrigger.Resolve);
        bug.Apply(BugTrigger.Close);
        Console.WriteLine($"Final state: {bug.State}");
    }
}
