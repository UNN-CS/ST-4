using Stateless;

namespace BugPro;

public enum BugState
{
    Draft,
    Triage,
    Assigned,
    InProgress,
    CodeReview,
    ReadyForTest,
    Testing,
    WaitingForInfo,
    Postponed,
    Duplicate,
    Rejected,
    Resolved,
    Closed,
    Reopened
}

public enum BugTrigger
{
    Submit,
    Assign,
    StartWork,
    SendToReview,
    ApproveReview,
    RequestInfo,
    ProvideInfo,
    Defer,
    Resume,
    MarkDuplicate,
    Reject,
    SendToTesting,
    PassTesting,
    FailTesting,
    Close,
    Reopen
}

public sealed class Bug
{
    private readonly StateMachine<BugState, BugTrigger> machine;
    private BugState state;

    public Bug(BugState initialState = BugState.Draft)
    {
        state = initialState;
        machine = new StateMachine<BugState, BugTrigger>(() => state, next => state = next);
        ConfigureWorkflow();
    }

    public BugState State => machine.State;

    public bool CanFire(BugTrigger trigger) => machine.CanFire(trigger);

    public void Fire(BugTrigger trigger) => machine.Fire(trigger);

    public string StateDescription => Describe(State);

    public static string Describe(BugState state) => state switch
    {
        BugState.Draft => "Draft bug report",
        BugState.Triage => "Triage",
        BugState.Assigned => "Assigned to developer",
        BugState.InProgress => "Fix in progress",
        BugState.CodeReview => "Code review",
        BugState.ReadyForTest => "Ready for test",
        BugState.Testing => "Testing",
        BugState.WaitingForInfo => "Waiting for information",
        BugState.Postponed => "Postponed",
        BugState.Duplicate => "Duplicate",
        BugState.Rejected => "Rejected",
        BugState.Resolved => "Resolved",
        BugState.Closed => "Closed",
        BugState.Reopened => "Reopened",
        _ => state.ToString()
    };

    private void ConfigureWorkflow()
    {
        machine.Configure(BugState.Draft)
            .Permit(BugTrigger.Submit, BugState.Triage);

        machine.Configure(BugState.Triage)
            .Permit(BugTrigger.Assign, BugState.Assigned)
            .Permit(BugTrigger.RequestInfo, BugState.WaitingForInfo)
            .Permit(BugTrigger.Defer, BugState.Postponed)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.Reject, BugState.Rejected);

        machine.Configure(BugState.Assigned)
            .Permit(BugTrigger.StartWork, BugState.InProgress)
            .Permit(BugTrigger.RequestInfo, BugState.WaitingForInfo)
            .Permit(BugTrigger.Defer, BugState.Postponed);

        machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.SendToReview, BugState.CodeReview)
            .Permit(BugTrigger.RequestInfo, BugState.WaitingForInfo);

        machine.Configure(BugState.CodeReview)
            .Permit(BugTrigger.ApproveReview, BugState.ReadyForTest)
            .Permit(BugTrigger.FailTesting, BugState.InProgress);

        machine.Configure(BugState.ReadyForTest)
            .Permit(BugTrigger.SendToTesting, BugState.Testing);

        machine.Configure(BugState.Testing)
            .Permit(BugTrigger.PassTesting, BugState.Resolved)
            .Permit(BugTrigger.FailTesting, BugState.InProgress);

        machine.Configure(BugState.WaitingForInfo)
            .Permit(BugTrigger.ProvideInfo, BugState.Triage)
            .Permit(BugTrigger.Defer, BugState.Postponed);

        machine.Configure(BugState.Postponed)
            .Permit(BugTrigger.Resume, BugState.Triage);

        machine.Configure(BugState.Duplicate)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        machine.Configure(BugState.Rejected)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        machine.Configure(BugState.Resolved)
            .Permit(BugTrigger.Close, BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.Submit, BugState.Triage);
    }
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();

        Console.WriteLine("Bug workflow demo");
        Print(bug);

        foreach (var trigger in new[]
                 {
                     BugTrigger.Submit,
                     BugTrigger.Assign,
                     BugTrigger.StartWork,
                     BugTrigger.SendToReview,
                     BugTrigger.ApproveReview,
                     BugTrigger.SendToTesting,
                     BugTrigger.PassTesting,
                     BugTrigger.Close
                 })
        {
            bug.Fire(trigger);
            Print(bug);
        }
    }

    private static void Print(Bug bug)
    {
        Console.WriteLine($"{bug.State}: {bug.StateDescription}");
    }
}
