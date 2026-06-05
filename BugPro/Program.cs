using Stateless;

namespace BugPro;

public enum TicketPhase
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

public enum WorkflowSignal
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
    private TicketPhase phase;
    private readonly StateMachine<TicketPhase, WorkflowSignal> workflow;

    public Bug()
    {
        phase = TicketPhase.New;
        workflow = new StateMachine<TicketPhase, WorkflowSignal>(
            () => phase,
            value => phase = value);

        RegisterTransitions();
    }

    public TicketPhase State => phase;

    public void StartAnalysis() => Fire(WorkflowSignal.StartAnalysis);

    public void RequestInfo() => Fire(WorkflowSignal.RequestInfo);

    public void MarkDuplicate() => Fire(WorkflowSignal.MarkDuplicate);

    public void MarkNotReproducible() => Fire(WorkflowSignal.MarkNotReproducible);

    public void StartFix() => Fire(WorkflowSignal.StartFix);

    public void Fix() => Fire(WorkflowSignal.Fix);

    public void ConfirmResolved() => Fire(WorkflowSignal.ConfirmResolved);

    public void RejectFix() => Fire(WorkflowSignal.RejectFix);

    public void Reopen() => Fire(WorkflowSignal.Reopen);

    public void Defer() => Fire(WorkflowSignal.Defer);

    public void ReturnToAnalysis() => Fire(WorkflowSignal.ReturnToAnalysis);

    public bool CanStartAnalysis() => workflow.CanFire(WorkflowSignal.StartAnalysis);

    public bool CanFix() => workflow.CanFire(WorkflowSignal.Fix);

    public bool CanClose() => workflow.CanFire(WorkflowSignal.ConfirmResolved);

    public bool CanReopen() => workflow.CanFire(WorkflowSignal.Reopen);

    public bool CanReturnToAnalysis() => workflow.CanFire(WorkflowSignal.ReturnToAnalysis);

    private void Fire(WorkflowSignal signal) => workflow.Fire(signal);

    private void RegisterTransitions()
    {
        ConfigureNewPhase();
        ConfigureAnalysisPhase();
        ConfigureNeedMoreInfoPhase();
        ConfigureInProgressPhase();
        ConfigureFixedPhase();
        ConfigureClosedPhase();
        ConfigureReopenedPhase();
        ConfigureDeferredPhase();
        ConfigureTerminalPhases();
    }

    private void ConfigureNewPhase()
    {
        workflow.Configure(TicketPhase.New)
            .Permit(WorkflowSignal.StartAnalysis, TicketPhase.Analysis)
            .Permit(WorkflowSignal.Defer, TicketPhase.Deferred);
    }

    private void ConfigureAnalysisPhase()
    {
        workflow.Configure(TicketPhase.Analysis)
            .Permit(WorkflowSignal.RequestInfo, TicketPhase.NeedMoreInfo)
            .Permit(WorkflowSignal.MarkDuplicate, TicketPhase.Duplicate)
            .Permit(WorkflowSignal.MarkNotReproducible, TicketPhase.NotReproducible)
            .Permit(WorkflowSignal.StartFix, TicketPhase.InProgress)
            .Permit(WorkflowSignal.Defer, TicketPhase.Deferred);
    }

    private void ConfigureNeedMoreInfoPhase()
    {
        workflow.Configure(TicketPhase.NeedMoreInfo)
            .Permit(WorkflowSignal.ReturnToAnalysis, TicketPhase.Analysis)
            .Permit(WorkflowSignal.Defer, TicketPhase.Deferred);
    }

    private void ConfigureInProgressPhase()
    {
        workflow.Configure(TicketPhase.InProgress)
            .Permit(WorkflowSignal.Fix, TicketPhase.Fixed)
            .Permit(WorkflowSignal.ReturnToAnalysis, TicketPhase.Analysis)
            .Permit(WorkflowSignal.Defer, TicketPhase.Deferred);
    }

    private void ConfigureFixedPhase()
    {
        workflow.Configure(TicketPhase.Fixed)
            .Permit(WorkflowSignal.ConfirmResolved, TicketPhase.Closed)
            .Permit(WorkflowSignal.RejectFix, TicketPhase.Reopened);
    }

    private void ConfigureClosedPhase()
    {
        workflow.Configure(TicketPhase.Closed)
            .Permit(WorkflowSignal.Reopen, TicketPhase.Reopened);
    }

    private void ConfigureReopenedPhase()
    {
        workflow.Configure(TicketPhase.Reopened)
            .Permit(WorkflowSignal.ReturnToAnalysis, TicketPhase.Analysis)
            .Permit(WorkflowSignal.StartFix, TicketPhase.InProgress)
            .Permit(WorkflowSignal.Defer, TicketPhase.Deferred);
    }

    private void ConfigureDeferredPhase()
    {
        workflow.Configure(TicketPhase.Deferred)
            .Permit(WorkflowSignal.ReturnToAnalysis, TicketPhase.Analysis);
    }

    private void ConfigureTerminalPhases()
    {
        workflow.Configure(TicketPhase.Duplicate)
            .Permit(WorkflowSignal.ConfirmResolved, TicketPhase.Closed);

        workflow.Configure(TicketPhase.NotReproducible)
            .Permit(WorkflowSignal.ConfirmResolved, TicketPhase.Closed);
    }
}

internal static class Program
{
    private static void Main()
    {
        var ticket = new Bug();

        PrintState(ticket, "created");
        ticket.StartAnalysis();
        PrintState(ticket, "analysis started");
        ticket.StartFix();
        ticket.Fix();
        ticket.ConfirmResolved();
        PrintState(ticket, "resolved and closed");
    }

    private static void PrintState(Bug ticket, string stage)
    {
        Console.WriteLine($"[{stage}] phase = {ticket.State}");
    }
}
