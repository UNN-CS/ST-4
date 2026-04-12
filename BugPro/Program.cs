using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Triage,
    WaitingForInfo,
    Postponed,
    InProgress,
    Resolved,
    Closed,
    Reopened,
    Rejected
}

public enum BugTrigger
{
    StartTriage,
    RequestInfo,
    ProvideInfo,
    Postpone,
    Resume,
    StartFix,
    Resolve,
    RejectAsNotABug,
    RejectAsDuplicate,
    RejectAsCannotReproduce,
    RejectAsWontFix,
    Close,
    Reopen,
    SendToTriage
}

public sealed class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _stateMachine;

    public Bug(string title)
    {
        Title = string.IsNullOrWhiteSpace(title)
            ? throw new ArgumentException("Bug title cannot be empty.", nameof(title))
            : title;

        _stateMachine = new StateMachine<BugState, BugTrigger>(BugState.New);
        ConfigureStateMachine();
    }

    public string Title { get; }

    public BugState State => _stateMachine.State;

    public IReadOnlyCollection<BugTrigger> PermittedTriggers => _stateMachine.PermittedTriggers.ToArray();

    public bool CanFire(BugTrigger trigger) => _stateMachine.CanFire(trigger);

    public void StartTriage() => _stateMachine.Fire(BugTrigger.StartTriage);

    public void RequestInfo() => _stateMachine.Fire(BugTrigger.RequestInfo);

    public void ProvideInfo() => _stateMachine.Fire(BugTrigger.ProvideInfo);

    public void Postpone() => _stateMachine.Fire(BugTrigger.Postpone);

    public void Resume() => _stateMachine.Fire(BugTrigger.Resume);

    public void StartFix() => _stateMachine.Fire(BugTrigger.StartFix);

    public void Resolve() => _stateMachine.Fire(BugTrigger.Resolve);

    public void RejectAsNotABug() => _stateMachine.Fire(BugTrigger.RejectAsNotABug);

    public void RejectAsDuplicate() => _stateMachine.Fire(BugTrigger.RejectAsDuplicate);

    public void RejectAsCannotReproduce() => _stateMachine.Fire(BugTrigger.RejectAsCannotReproduce);

    public void RejectAsWontFix() => _stateMachine.Fire(BugTrigger.RejectAsWontFix);

    public void Close() => _stateMachine.Fire(BugTrigger.Close);

    public void Reopen() => _stateMachine.Fire(BugTrigger.Reopen);

    public void SendToTriage() => _stateMachine.Fire(BugTrigger.SendToTriage);

    private void ConfigureStateMachine()
    {
        _stateMachine.Configure(BugState.New)
            .Permit(BugTrigger.StartTriage, BugState.Triage);

        _stateMachine.Configure(BugState.Triage)
            .Permit(BugTrigger.RequestInfo, BugState.WaitingForInfo)
            .Permit(BugTrigger.Postpone, BugState.Postponed)
            .Permit(BugTrigger.StartFix, BugState.InProgress)
            .Permit(BugTrigger.RejectAsNotABug, BugState.Rejected)
            .Permit(BugTrigger.RejectAsDuplicate, BugState.Rejected)
            .Permit(BugTrigger.RejectAsCannotReproduce, BugState.Rejected)
            .Permit(BugTrigger.RejectAsWontFix, BugState.Rejected);

        _stateMachine.Configure(BugState.WaitingForInfo)
            .Permit(BugTrigger.ProvideInfo, BugState.Triage);

        _stateMachine.Configure(BugState.Postponed)
            .Permit(BugTrigger.Resume, BugState.Triage);

        _stateMachine.Configure(BugState.InProgress)
            .Permit(BugTrigger.RequestInfo, BugState.WaitingForInfo)
            .Permit(BugTrigger.Resolve, BugState.Resolved);

        _stateMachine.Configure(BugState.Resolved)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Reopened)
            .Permit(BugTrigger.SendToTriage, BugState.Triage)
            .Permit(BugTrigger.StartFix, BugState.InProgress);

        _stateMachine.Configure(BugState.Rejected)
            .Permit(BugTrigger.Reopen, BugState.Reopened);
    }
}

public static class Program
{
    public static void Main()
    {
        var bug = new Bug("Login page returns 500");

        Console.WriteLine($"Bug: {bug.Title}");
        Console.WriteLine($"Initial state: {bug.State}");

        bug.StartTriage();
        Console.WriteLine($"After triage started: {bug.State}");

        bug.StartFix();
        Console.WriteLine($"After fix started: {bug.State}");

        bug.Resolve();
        Console.WriteLine($"After resolve: {bug.State}");

        bug.Close();
        Console.WriteLine($"Final state: {bug.State}");
    }
}
