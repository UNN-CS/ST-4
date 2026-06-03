using Stateless;

namespace BugPro;

public enum BugState
{
    Draft,
    Triaged,
    WaitingForReporter,
    Accepted,
    InProgress,
    Verification,
    Closed,
    Rejected,
    Deferred
}

public enum BugAction
{
    Submit,
    AskDetails,
    AddDetails,
    Accept,
    Reject,
    StartWork,
    SendToCheck,
    ApproveFix,
    DeclineFix,
    Reopen,
    Postpone,
    Restore
}

public class Bug
{
    private readonly StateMachine<BugState, BugAction> workflow;

    public Bug()
    {
        workflow = new StateMachine<BugState, BugAction>(BugState.Draft);

        workflow.Configure(BugState.Draft)
            .Permit(BugAction.Submit, BugState.Triaged)
            .Permit(BugAction.Postpone, BugState.Deferred);

        workflow.Configure(BugState.Triaged)
            .Permit(BugAction.AskDetails, BugState.WaitingForReporter)
            .Permit(BugAction.Accept, BugState.Accepted)
            .Permit(BugAction.Reject, BugState.Rejected)
            .Permit(BugAction.Postpone, BugState.Deferred);

        workflow.Configure(BugState.WaitingForReporter)
            .Permit(BugAction.AddDetails, BugState.Triaged)
            .Permit(BugAction.Reject, BugState.Rejected);

        workflow.Configure(BugState.Accepted)
            .Permit(BugAction.StartWork, BugState.InProgress)
            .Permit(BugAction.Postpone, BugState.Deferred);

        workflow.Configure(BugState.InProgress)
            .Permit(BugAction.SendToCheck, BugState.Verification)
            .Permit(BugAction.AskDetails, BugState.WaitingForReporter)
            .Permit(BugAction.Postpone, BugState.Deferred);

        workflow.Configure(BugState.Verification)
            .Permit(BugAction.ApproveFix, BugState.Closed)
            .Permit(BugAction.DeclineFix, BugState.InProgress);

        workflow.Configure(BugState.Closed)
            .Permit(BugAction.Reopen, BugState.Triaged);

        workflow.Configure(BugState.Deferred)
            .Permit(BugAction.Restore, BugState.Triaged)
            .Permit(BugAction.Reject, BugState.Rejected);
    }

    public BugState CurrentState => workflow.State;

    public bool IsClosed => CurrentState is BugState.Closed or BugState.Rejected;

    public bool Can(BugAction action) => workflow.CanFire(action);

    public void Submit() => workflow.Fire(BugAction.Submit);
    public void AskDetails() => workflow.Fire(BugAction.AskDetails);
    public void AddDetails() => workflow.Fire(BugAction.AddDetails);
    public void Accept() => workflow.Fire(BugAction.Accept);
    public void Reject() => workflow.Fire(BugAction.Reject);
    public void StartWork() => workflow.Fire(BugAction.StartWork);
    public void SendToCheck() => workflow.Fire(BugAction.SendToCheck);
    public void ApproveFix() => workflow.Fire(BugAction.ApproveFix);
    public void DeclineFix() => workflow.Fire(BugAction.DeclineFix);
    public void Reopen() => workflow.Fire(BugAction.Reopen);
    public void Postpone() => workflow.Fire(BugAction.Postpone);
    public void Restore() => workflow.Fire(BugAction.Restore);
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();

        Console.WriteLine("Bug workflow demo");
        Console.WriteLine($"Created: {bug.CurrentState}");

        bug.Submit();
        bug.Accept();
        bug.StartWork();
        bug.SendToCheck();
        bug.ApproveFix();

        Console.WriteLine($"Finished: {bug.CurrentState}");
    }
}
