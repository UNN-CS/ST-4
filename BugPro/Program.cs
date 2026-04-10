using Stateless;

namespace BugPro;

public enum BugState
{
    Open,
    Triaged,
    InProgress,
    WaitingForInfo,
    Resolved,
    Closed,
    Reopened,
    Rejected,
    Duplicate,
    CannotReproduce
}

public enum BugTrigger
{
    Triage,
    StartWork,
    Fix,
    RequestInfo,
    ProvideInfo,
    Close,
    Reopen,
    Reject,
    MarkDuplicate,
    MarkCannotReproduce,
    SendBack
}

public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _stateMachine;
    private readonly List<string> _history = new();

    public string Title { get; }
    public BugState State => _stateMachine.State;
    public IReadOnlyList<string> History => _history.AsReadOnly();

    public Bug(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Bug title cannot be null or whitespace.", nameof(title));

        Title = title;
        _stateMachine = new StateMachine<BugState, BugTrigger>(BugState.Open);
        SetupTransitions();
    }

    private void SetupTransitions()
    {
        _stateMachine.Configure(BugState.Open)
            .OnEntry(() => Record("Bug opened"))
            .Permit(BugTrigger.Triage, BugState.Triaged)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate);

        _stateMachine.Configure(BugState.Triaged)
            .OnEntry(() => Record("Bug triaged"))
            .Permit(BugTrigger.StartWork, BugState.InProgress)
            .Permit(BugTrigger.RequestInfo, BugState.WaitingForInfo)
            .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _stateMachine.Configure(BugState.InProgress)
            .OnEntry(() => Record("Work started"))
            .Permit(BugTrigger.Fix, BugState.Resolved)
            .Permit(BugTrigger.RequestInfo, BugState.WaitingForInfo)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _stateMachine.Configure(BugState.WaitingForInfo)
            .OnEntry(() => Record("Waiting for additional information"))
            .Permit(BugTrigger.ProvideInfo, BugState.Triaged)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _stateMachine.Configure(BugState.Resolved)
            .OnEntry(() => Record("Bug resolved"))
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.SendBack, BugState.Reopened);

        _stateMachine.Configure(BugState.Closed)
            .OnEntry(() => Record("Bug closed"))
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Reopened)
            .OnEntry(() => Record("Bug reopened"))
            .Permit(BugTrigger.Triage, BugState.Triaged)
            .Permit(BugTrigger.StartWork, BugState.InProgress);

        _stateMachine.Configure(BugState.Rejected)
            .OnEntry(() => Record("Bug rejected"));

        _stateMachine.Configure(BugState.Duplicate)
            .OnEntry(() => Record("Marked as duplicate"));

        _stateMachine.Configure(BugState.CannotReproduce)
            .OnEntry(() => Record("Cannot reproduce"));
    }

    private void Record(string message)
    {
        _history.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
    }

    public void Fire(BugTrigger trigger) => _stateMachine.Fire(trigger);

    public bool CanFire(BugTrigger trigger) => _stateMachine.CanFire(trigger);
}

internal static class Program
{
    private static void Main()
    {
        Console.WriteLine("=== Bug Tracking Workflow Demo ===\n");

        Console.WriteLine("-- Scenario 1: Full resolution path --");
        var bug1 = new Bug("Memory leak in data processing module");
        PrintState(bug1);
        bug1.Fire(BugTrigger.Triage);      PrintState(bug1);
        bug1.Fire(BugTrigger.StartWork);   PrintState(bug1);
        bug1.Fire(BugTrigger.Fix);         PrintState(bug1);
        bug1.Fire(BugTrigger.Close);       PrintState(bug1);

        Console.WriteLine("\n-- Scenario 2: Bug needs more information --");
        var bug2 = new Bug("Intermittent connection drop");
        bug2.Fire(BugTrigger.Triage);
        bug2.Fire(BugTrigger.RequestInfo);   PrintState(bug2);
        bug2.Fire(BugTrigger.ProvideInfo);   PrintState(bug2);
        bug2.Fire(BugTrigger.StartWork);
        bug2.Fire(BugTrigger.Fix);
        bug2.Fire(BugTrigger.Close);         PrintState(bug2);

        Console.WriteLine("\n-- Scenario 3: Bug reopened after close --");
        var bug3 = new Bug("Login button unresponsive");
        bug3.Fire(BugTrigger.Triage);
        bug3.Fire(BugTrigger.StartWork);
        bug3.Fire(BugTrigger.Fix);
        bug3.Fire(BugTrigger.Close);
        bug3.Fire(BugTrigger.Reopen);        PrintState(bug3);
        bug3.Fire(BugTrigger.Triage);
        bug3.Fire(BugTrigger.StartWork);
        bug3.Fire(BugTrigger.Fix);
        bug3.Fire(BugTrigger.Close);         PrintState(bug3);

        Console.WriteLine($"\n-- Transition history for bug1 ({bug1.History.Count} entries) --");
        foreach (var entry in bug1.History)
            Console.WriteLine($"  {entry}");
    }

    private static void PrintState(Bug bug) =>
        Console.WriteLine($"  [{bug.Title}] State: {bug.State}");
}
