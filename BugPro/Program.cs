using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Assigned,
    InProgress,
    Fixed,
    Closed,
    Reopened,
    Rejected,
    Duplicate,
    CannotReproduce
}

public enum BugTrigger
{
    Assign,
    StartProgress,
    Resolve,
    Close,
    Reopen,
    Reject,
    MarkDuplicate,
    MarkCannotReproduce,
    ReturnToWork
}

public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _stateMachine;

    public string Title { get; }

    public BugState State => _stateMachine.State;

    public Bug(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be empty or whitespace.", nameof(title));

        Title = title;
        _stateMachine = new StateMachine<BugState, BugTrigger>(BugState.New);

        ConfigureStates();
    }

    private void ConfigureStates()
    {
        _stateMachine.Configure(BugState.New)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce);

        _stateMachine.Configure(BugState.Assigned)
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce);

        _stateMachine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Resolve, BugState.Fixed)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _stateMachine.Configure(BugState.Fixed)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.ReturnToWork, BugState.Reopened);

        _stateMachine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _stateMachine.Configure(BugState.Reopened)
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce);

        _stateMachine.Configure(BugState.Rejected);
        _stateMachine.Configure(BugState.Duplicate);
        _stateMachine.Configure(BugState.CannotReproduce);
    }

    public void Fire(BugTrigger trigger)
    {
        if (!_stateMachine.CanFire(trigger))
            throw new InvalidOperationException($"Cannot fire '{trigger}' in state '{State}'.");

        _stateMachine.Fire(trigger);
    }

    public bool CanFire(BugTrigger trigger) => _stateMachine.CanFire(trigger);
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug("Login button freezes UI");
        Console.WriteLine($"Bug title: {bug.Title}");
        Console.WriteLine($"Initial state: {bug.State}");
    }
}