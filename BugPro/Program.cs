using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Open,
    InProgress,
    CodeReview,
    ReadyForTesting,
    InTesting,
    Verified,
    Resolved,
    Reopened,
    Deferred,
    Cancelled,
    Closed
}

public enum BugTrigger
{
    Open,
    Assign,
    SubmitForReview,
    RequestChanges,
    ApproveReview,
    StartTesting,
    PassTesting,
    FailTesting,
    Resolve,
    Reopen,
    Close,
    Cancel,
    Defer,
    Resume
}

public sealed class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;
    private readonly List<string> _history = new();

    public Bug(string title, string? description = null)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Bug title cannot be empty.", nameof(title));
        }

        Id = Guid.NewGuid();
        Title = title.Trim();
        Description = description?.Trim() ?? string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
        State = BugState.New;

        _machine = new StateMachine<BugState, BugTrigger>(() => State, state => State = state);
        ConfigureStateMachine();

        _history.Add($"Created in {State}");
    }

    public Guid Id { get; }

    public string Title { get; }

    public string Description { get; }

    public BugState State { get; private set; }

    public DateTime CreatedAt { get; }

    public DateTime UpdatedAt { get; private set; }

    public DateTime? ClosedAt { get; private set; }

    public IReadOnlyList<string> History => _history.AsReadOnly();

    public IReadOnlyCollection<BugTrigger> PermittedTriggers => _machine.PermittedTriggers.ToArray();

    public bool IsClosed => State is BugState.Closed or BugState.Cancelled;

    public void Fire(BugTrigger trigger) => _machine.Fire(trigger);

    public bool TryFire(BugTrigger trigger)
    {
        try
        {
            _machine.Fire(trigger);
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);

    private void ConfigureStateMachine()
    {
        _machine.Configure(BugState.New)
            .Permit(BugTrigger.Open, BugState.Open)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.Open)
            .Permit(BugTrigger.Assign, BugState.InProgress)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.SubmitForReview, BugState.CodeReview)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.CodeReview)
            .Permit(BugTrigger.RequestChanges, BugState.InProgress)
            .Permit(BugTrigger.ApproveReview, BugState.ReadyForTesting)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.ReadyForTesting)
            .Permit(BugTrigger.StartTesting, BugState.InTesting)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.InTesting)
            .Permit(BugTrigger.PassTesting, BugState.Verified)
            .Permit(BugTrigger.FailTesting, BugState.Reopened)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.Verified)
            .Permit(BugTrigger.Resolve, BugState.Resolved)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Resolved)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Assign, BugState.InProgress)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.Deferred)
            .Permit(BugTrigger.Resume, BugState.Open)
            .Permit(BugTrigger.Cancel, BugState.Cancelled);

        _machine.Configure(BugState.Closed);
        _machine.Configure(BugState.Cancelled);

        _machine.OnTransitioned(transition =>
        {
            UpdatedAt = DateTime.UtcNow;
            _history.Add($"{transition.Source} --{transition.Trigger}--> {transition.Destination}");

            if (transition.Destination is BugState.Closed or BugState.Cancelled)
            {
                ClosedAt = UpdatedAt;
            }
        });
    }
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug("Wrong total is shown on checkout page", "The order total is rounded incorrectly.");

        foreach (var trigger in new[]
        {
            BugTrigger.Open,
            BugTrigger.Assign,
            BugTrigger.SubmitForReview,
            BugTrigger.ApproveReview,
            BugTrigger.StartTesting,
            BugTrigger.PassTesting,
            BugTrigger.Resolve,
            BugTrigger.Close
        })
        {
            bug.Fire(trigger);
        }

        Console.WriteLine($"Bug: {bug.Title}");
        Console.WriteLine($"Current state: {bug.State}");
        Console.WriteLine("Workflow history:");

        foreach (var item in bug.History)
        {
            Console.WriteLine($" - {item}");
        }
    }
}
