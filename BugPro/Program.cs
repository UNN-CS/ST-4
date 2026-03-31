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
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public string Title { get; }

    public BugState State => _machine.State;

    public Bug(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Bug title cannot be empty.", nameof(title));
        }

        Title = title;
        _machine = new StateMachine<BugState, BugTrigger>(BugState.New);

        ConfigureStateMachine();
    }

    private void ConfigureStateMachine()
    {
        _machine.Configure(BugState.New)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce);

        _machine.Configure(BugState.Assigned)
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Resolve, BugState.Fixed)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Fixed)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.ReturnToWork, BugState.Reopened);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.MarkCannotReproduce, BugState.CannotReproduce);

        _machine.Configure(BugState.Rejected);
        _machine.Configure(BugState.Duplicate);
        _machine.Configure(BugState.CannotReproduce);
    }

    public void Fire(BugTrigger trigger)
    {
        _machine.Fire(trigger);
    }

    public bool CanFire(BugTrigger trigger)
    {
        return _machine.CanFire(trigger);
    }
}   

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug("App crashes on login");
        Console.WriteLine($"Bug: {bug.Title}");
        Console.WriteLine($"Current state: {bug.State}");
    }
}