using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Analysis,
    InProgress,
    NeedMoreInfo,
    NotReproducible,
    Duplicate,
    Fixed,
    Closed,
    Reopened,
    Deferred
}

public enum BugTrigger
{
    StartAnalysis,
    RequestInfo,
    MarkDuplicate,
    MarkNotReproducible,
    StartFix,
    Fix,
    ConfirmResolved,
    RejectFix,
    Reopen,
    Defer,
    ReturnToAnalysis
}

public sealed class Bug
{
    private readonly StateMachine<BugState, BugTrigger> stateMachine;
    private BugState state;

    public Bug()
    {
        state = BugState.New;
        stateMachine = new StateMachine<BugState, BugTrigger>(
            () => state,
            newState => state = newState);

        ConfigureWorkflow();
    }

    public BugState State => state;

    public void StartAnalysis() => stateMachine.Fire(BugTrigger.StartAnalysis);

    public void RequestInfo() => stateMachine.Fire(BugTrigger.RequestInfo);

    public void MarkDuplicate() => stateMachine.Fire(BugTrigger.MarkDuplicate);

    public void MarkNotReproducible() =>
        stateMachine.Fire(BugTrigger.MarkNotReproducible);

    public void StartFix() => stateMachine.Fire(BugTrigger.StartFix);

    public void Fix() => stateMachine.Fire(BugTrigger.Fix);

    public void ConfirmResolved() => stateMachine.Fire(BugTrigger.ConfirmResolved);

    public void RejectFix() => stateMachine.Fire(BugTrigger.RejectFix);

    public void Reopen() => stateMachine.Fire(BugTrigger.Reopen);

    public void Defer() => stateMachine.Fire(BugTrigger.Defer);

    public void ReturnToAnalysis() =>
        stateMachine.Fire(BugTrigger.ReturnToAnalysis);

    public bool CanStartAnalysis() =>
        stateMachine.CanFire(BugTrigger.StartAnalysis);

    public bool CanFix() => stateMachine.CanFire(BugTrigger.Fix);

    public bool CanClose() =>
        stateMachine.CanFire(BugTrigger.ConfirmResolved);

    public bool CanReopen() => stateMachine.CanFire(BugTrigger.Reopen);

    public bool CanReturnToAnalysis() =>
        stateMachine.CanFire(BugTrigger.ReturnToAnalysis);

    private void ConfigureWorkflow()
    {
        stateMachine.Configure(BugState.New)
            .Permit(BugTrigger.StartAnalysis, BugState.Analysis)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        stateMachine.Configure(BugState.Analysis)
            .Permit(BugTrigger.RequestInfo, BugState.NeedMoreInfo)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.MarkNotReproducible, BugState.NotReproducible)
            .Permit(BugTrigger.StartFix, BugState.InProgress)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        stateMachine.Configure(BugState.NeedMoreInfo)
            .Permit(BugTrigger.ReturnToAnalysis, BugState.Analysis)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        stateMachine.Configure(BugState.InProgress)
            .Permit(BugTrigger.Fix, BugState.Fixed)
            .Permit(BugTrigger.ReturnToAnalysis, BugState.Analysis)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        stateMachine.Configure(BugState.Fixed)
            .Permit(BugTrigger.ConfirmResolved, BugState.Closed)
            .Permit(BugTrigger.RejectFix, BugState.Reopened);

        stateMachine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        stateMachine.Configure(BugState.Reopened)
            .Permit(BugTrigger.ReturnToAnalysis, BugState.Analysis)
            .Permit(BugTrigger.StartFix, BugState.InProgress)
            .Permit(BugTrigger.Defer, BugState.Deferred);

        stateMachine.Configure(BugState.Deferred)
            .Permit(BugTrigger.ReturnToAnalysis, BugState.Analysis);

        stateMachine.Configure(BugState.Duplicate)
            .Permit(BugTrigger.ConfirmResolved, BugState.Closed);

        stateMachine.Configure(BugState.NotReproducible)
            .Permit(BugTrigger.ConfirmResolved, BugState.Closed);
    }
}

public static class Program
{
    public static void Main()
    {
        var bug = new Bug();

        Console.WriteLine($"Initial bug state: {bug.State}");
        bug.StartAnalysis();
        Console.WriteLine($"After analysis starts: {bug.State}");
        bug.StartFix();
        bug.Fix();
        bug.ConfirmResolved();
        Console.WriteLine($"After fix confirmation: {bug.State}");
    }
}
