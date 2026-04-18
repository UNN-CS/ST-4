using Stateless;

namespace BugPro;

public enum BugState
{
    Open,
    Assigned,
    InProgress,
    Fixed,
    Verified,
    Closed,
    Reopened,
    Rejected
}

public enum BugTrigger
{
    Assign,
    StartProgress,
    Fix,
    Verify,
    Close,
    Reopen,
    Reject
}

public class Bug
{
    private StateMachine<BugState, BugTrigger> _machine;
    private BugState _currentState;

    public BugState CurrentState => _currentState;

    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(() => _currentState, state => _currentState = state);

        // Configure state machine
        _machine.Configure(BugState.Open)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Assigned)
            .Permit(BugTrigger.StartProgress, BugState.InProgress)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Fix, BugState.Fixed)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Fixed)
            .Permit(BugTrigger.Verify, BugState.Verified)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Verified)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        _machine.Configure(BugState.Rejected)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _currentState = BugState.Open;
    }

    public void Assign() => _machine.Fire(BugTrigger.Assign);
    public void StartProgress() => _machine.Fire(BugTrigger.StartProgress);
    public void Fix() => _machine.Fire(BugTrigger.Fix);
    public void Verify() => _machine.Fire(BugTrigger.Verify);
    public void Close() => _machine.Fire(BugTrigger.Close);
    public void Reopen() => _machine.Fire(BugTrigger.Reopen);
    public void Reject() => _machine.Fire(BugTrigger.Reject);

    public bool CanAssign() => _machine.CanFire(BugTrigger.Assign);
    public bool CanStartProgress() => _machine.CanFire(BugTrigger.StartProgress);
    public bool CanFix() => _machine.CanFire(BugTrigger.Fix);
    public bool CanVerify() => _machine.CanFire(BugTrigger.Verify);
    public bool CanClose() => _machine.CanFire(BugTrigger.Close);
    public bool CanReopen() => _machine.CanFire(BugTrigger.Reopen);
    public bool CanReject() => _machine.CanFire(BugTrigger.Reject);
}

public class Program
{
    public static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Initial state: {bug.CurrentState}");

        bug.Assign();
        Console.WriteLine($"After Assign: {bug.CurrentState}");

        bug.StartProgress();
        Console.WriteLine($"After StartProgress: {bug.CurrentState}");

        bug.Fix();
        Console.WriteLine($"After Fix: {bug.CurrentState}");

        bug.Verify();
        Console.WriteLine($"After Verify: {bug.CurrentState}");

        bug.Close();
        Console.WriteLine($"After Close: {bug.CurrentState}");

        bug.Reopen();
        Console.WriteLine($"After Reopen: {bug.CurrentState}");
    }
}