using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Triaged,
    Assigned,
    InProgress,
    Fixed,
    ReadyForTesting,
    Reopened,
    Closed,
    Rejected,
    Deferred
}

public enum BugTrigger
{
    Triage,
    Assign,
    StartProgress,
    Resolve,
    SendToTesting,
    Verify,
    Reopen,
    Reject,
    Defer,
    Reconsider
}

public sealed class Bug
{
    private readonly List<string> _history = [];
    private readonly StateMachine<BugState, BugTrigger> _machine;
    private string? _pendingAssignee;
    private string? _pendingResolution;

    public Bug(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Bug title cannot be empty.", nameof(title));
        }

        Title = title;
        State = BugState.New;

        _machine = new StateMachine<BugState, BugTrigger>(() => State, state => State = state);

        ConfigureStateMachine();

        _machine.OnTransitioned(transition =>
            _history.Add($"{transition.Source} --{transition.Trigger}--> {transition.Destination}"));

        _history.Add($"Bug '{Title}' created in state {State}.");
    }

    public string Title { get; }

    public BugState State { get; private set; }

    public string? Assignee { get; private set; }

    public string? Resolution { get; private set; }

    public int ReopenCount { get; private set; }

    public bool IsClosed => State == BugState.Closed;

    public IReadOnlyList<string> History => _history;

#pragma warning disable CS0618
    public IEnumerable<BugTrigger> PermittedTriggers => _machine.PermittedTriggers;
#pragma warning restore CS0618

    public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);

    public void Triage() => _machine.Fire(BugTrigger.Triage);

    public void Reject() => _machine.Fire(BugTrigger.Reject);

    public void Defer() => _machine.Fire(BugTrigger.Defer);

    public void Reconsider() => _machine.Fire(BugTrigger.Reconsider);

    public void StartProgress() => _machine.Fire(BugTrigger.StartProgress);

    public void SendToTesting() => _machine.Fire(BugTrigger.SendToTesting);

    public void Verify() => _machine.Fire(BugTrigger.Verify);

    public void Reopen() => _machine.Fire(BugTrigger.Reopen);

    public void AssignTo(string assignee)
    {
        if (string.IsNullOrWhiteSpace(assignee))
        {
            throw new ArgumentException("Assignee cannot be empty.", nameof(assignee));
        }

        _pendingAssignee = assignee;

        try
        {
            _machine.Fire(BugTrigger.Assign);
        }
        catch
        {
            _pendingAssignee = null;
            throw;
        }
    }

    public void Resolve(string resolution)
    {
        if (string.IsNullOrWhiteSpace(resolution))
        {
            throw new ArgumentException("Resolution cannot be empty.", nameof(resolution));
        }

        _pendingResolution = resolution;

        try
        {
            _machine.Fire(BugTrigger.Resolve);
        }
        catch
        {
            _pendingResolution = null;
            throw;
        }
    }

    private void ConfigureStateMachine()
    {
        _machine.Configure(BugState.New)
            .Permit(BugTrigger.Triage, BugState.Triaged)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        _machine.Configure(BugState.Triaged)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        _machine.Configure(BugState.Assigned)
            .OnEntry(() =>
            {
                if (_pendingAssignee is null)
                {
                    return;
                }

                Assignee = _pendingAssignee;
                _history.Add($"Assigned to {_pendingAssignee}.");
                _pendingAssignee = null;
            })
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Resolve, BugState.Fixed);

        _machine.Configure(BugState.Fixed)
            .OnEntry(() =>
            {
                if (_pendingResolution is null)
                {
                    return;
                }

                Resolution = _pendingResolution;
                _history.Add($"Resolution saved: {_pendingResolution}");
                _pendingResolution = null;
            })
            .Permit(BugTrigger.SendToTesting, BugState.ReadyForTesting);

        _machine.Configure(BugState.ReadyForTesting)
            .Permit(BugTrigger.Verify, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .OnEntry(() =>
            {
                ReopenCount++;
                _history.Add("Bug was reopened.");
            })
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.StartProgress, BugState.InProgress);

        _machine.Configure(BugState.Deferred)
            .Permit(BugTrigger.Reconsider, BugState.Triaged)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Rejected);
    }
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug("Login page returns 500");

        Console.WriteLine($"Title: {bug.Title}");
        Console.WriteLine($"Initial state: {bug.State}");

        bug.Triage();
        bug.AssignTo("Alice");
        bug.StartProgress();
        bug.Resolve("Null check added for empty credentials.");
        bug.SendToTesting();
        bug.Verify();

        Console.WriteLine($"Final state: {bug.State}");
        Console.WriteLine("History:");

        foreach (var record in bug.History)
        {
            Console.WriteLine($"- {record}");
        }
    }
}
