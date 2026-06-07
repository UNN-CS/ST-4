using Stateless;

namespace BugPro;

public enum BugStatus
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
    private BugStatus _status;
    private readonly StateMachine<BugStatus, BugTrigger> _fsm;

    public Bug()
    {
        _status = BugStatus.New;
        _fsm = new StateMachine<BugStatus, BugTrigger>(
            () => _status,
            value => _status = value);

        InitStateMachine();
    }

    public BugStatus State => _status;

    public void StartAnalysis()
    {
        Trigger(BugTrigger.StartAnalysis);
    }

    public void RequestInfo()
    {
        Trigger(BugTrigger.RequestInfo);
    }

    public void MarkDuplicate()
    {
        Trigger(BugTrigger.MarkDuplicate);
    }

    public void MarkNotReproducible()
    {
        Trigger(BugTrigger.MarkNotReproducible);
    }

    public void StartFix()
    {
        Trigger(BugTrigger.StartFix);
    }

    public void Fix()
    {
        Trigger(BugTrigger.Fix);
    }

    public void ConfirmResolved()
    {
        Trigger(BugTrigger.ConfirmResolved);
    }

    public void RejectFix()
    {
        Trigger(BugTrigger.RejectFix);
    }

    public void Reopen()
    {
        Trigger(BugTrigger.Reopen);
    }

    public void Defer()
    {
        Trigger(BugTrigger.Defer);
    }

    public void ReturnToAnalysis()
    {
        Trigger(BugTrigger.ReturnToAnalysis);
    }

    public bool CanStartAnalysis()
    {
        return _fsm.CanFire(BugTrigger.StartAnalysis);
    }

    public bool CanFix()
    {
        return _fsm.CanFire(BugTrigger.Fix);
    }

    public bool CanClose()
    {
        return _fsm.CanFire(BugTrigger.ConfirmResolved);
    }

    public bool CanReopen()
    {
        return _fsm.CanFire(BugTrigger.Reopen);
    }

    public bool CanReturnToAnalysis()
    {
        return _fsm.CanFire(BugTrigger.ReturnToAnalysis);
    }

    private void Trigger(BugTrigger action)
    {
        _fsm.Fire(action);
    }

    private void InitStateMachine()
    {
        BindNewState();
        BindAnalysisState();
        BindNeedMoreInfoState();
        BindInProgressState();
        BindFixedState();
        BindClosedState();
        BindReopenedState();
        BindDeferredState();
        BindTerminalStates();
    }

    private void BindNewState()
    {
        _fsm.Configure(BugStatus.New)
            .Permit(BugTrigger.StartAnalysis, BugStatus.Analysis)
            .Permit(BugTrigger.Defer, BugStatus.Deferred);
    }

    private void BindAnalysisState()
    {
        _fsm.Configure(BugStatus.Analysis)
            .Permit(BugTrigger.RequestInfo, BugStatus.NeedMoreInfo)
            .Permit(BugTrigger.MarkDuplicate, BugStatus.Duplicate)
            .Permit(BugTrigger.MarkNotReproducible, BugStatus.NotReproducible)
            .Permit(BugTrigger.StartFix, BugStatus.InProgress)
            .Permit(BugTrigger.Defer, BugStatus.Deferred);
    }

    private void BindNeedMoreInfoState()
    {
        _fsm.Configure(BugStatus.NeedMoreInfo)
            .Permit(BugTrigger.ReturnToAnalysis, BugStatus.Analysis)
            .Permit(BugTrigger.Defer, BugStatus.Deferred);
    }

    private void BindInProgressState()
    {
        _fsm.Configure(BugStatus.InProgress)
            .Permit(BugTrigger.Fix, BugStatus.Fixed)
            .Permit(BugTrigger.ReturnToAnalysis, BugStatus.Analysis)
            .Permit(BugTrigger.Defer, BugStatus.Deferred);
    }

    private void BindFixedState()
    {
        _fsm.Configure(BugStatus.Fixed)
            .Permit(BugTrigger.ConfirmResolved, BugStatus.Closed)
            .Permit(BugTrigger.RejectFix, BugStatus.Reopened);
    }

    private void BindClosedState()
    {
        _fsm.Configure(BugStatus.Closed)
            .Permit(BugTrigger.Reopen, BugStatus.Reopened);
    }

    private void BindReopenedState()
    {
        _fsm.Configure(BugStatus.Reopened)
            .Permit(BugTrigger.ReturnToAnalysis, BugStatus.Analysis)
            .Permit(BugTrigger.StartFix, BugStatus.InProgress)
            .Permit(BugTrigger.Defer, BugStatus.Deferred);
    }

    private void BindDeferredState()
    {
        _fsm.Configure(BugStatus.Deferred)
            .Permit(BugTrigger.ReturnToAnalysis, BugStatus.Analysis);
    }

    private void BindTerminalStates()
    {
        _fsm.Configure(BugStatus.Duplicate)
            .Permit(BugTrigger.ConfirmResolved, BugStatus.Closed);

        _fsm.Configure(BugStatus.NotReproducible)
            .Permit(BugTrigger.ConfirmResolved, BugStatus.Closed);
    }
}

internal static class Program
{
    private static void Main()
    {
        var instance = new Bug();

        LogStatus(instance, "создан");
        instance.StartAnalysis();
        LogStatus(instance, "анализ");
        instance.StartFix();
        instance.Fix();
        instance.ConfirmResolved();
        LogStatus(instance, "закрыт");
    }

    private static void LogStatus(Bug instance, string caption)
    {
        Console.WriteLine($"== {caption} => {instance.State} ==");
    }
}
