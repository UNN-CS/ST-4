using System.Text;
using Stateless;

namespace BugPro;

/// <summary>Состояния жизненного цикла дефекта (workflow из README).</summary>
public enum BugState
{
    New,
    DefectAnalysis,
    Deferred,
    NeedMoreInfo,
    Fixing,
    PendingClosureNotDefect,
    PendingClosureWontFix,
    PendingClosureDuplicate,
    PendingClosureNotReproducible,
    Blocked,
    Closed,
    Reopened
}

/// <summary>События перехода между состояниями.</summary>
public enum BugTrigger
{
    SubmitForTriage,
    Defer,
    ResumeAnalysis,
    RequestMoreInfo,
    InformationProvided,
    AssignToDeveloper,
    MarkNotDefect,
    MarkWontFix,
    MarkDuplicate,
    MarkNotReproducible,
    ReportNotReproducibleFromFix,
    TesterApprovesClosure,
    TesterRequestsReturn,
    ConfirmProblemSolved,
    ReportProblemUnsolved,
    ReopenClosed,
    CompleteReopen,
    EscalateBlocked,
    Unblock
}

/// <summary>Автомат состояний для работы с багом на базе Stateless.</summary>
public sealed class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;

    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(BugState.New);
        ConfigureMachine();
    }

    public BugState State => _machine.State;

    public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);

    public void Fire(BugTrigger trigger) => _machine.Fire(trigger);

    public IReadOnlyCollection<BugTrigger> PermittedTriggers =>
        _machine.GetPermittedTriggers().Cast<BugTrigger>().ToList();

    /// <summary>Краткое текстовое представление автомата (текущее состояние и допустимые триггеры).</summary>
    public string DescribeWorkflow() => _machine.ToString() ?? string.Empty;

    private void ConfigureMachine()
    {
        _machine.Configure(BugState.New)
            .Permit(BugTrigger.SubmitForTriage, BugState.DefectAnalysis);

        _machine.Configure(BugState.DefectAnalysis)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugTrigger.AssignToDeveloper, BugState.Fixing)
            .Permit(BugTrigger.MarkNotDefect, BugState.PendingClosureNotDefect)
            .Permit(BugTrigger.MarkWontFix, BugState.PendingClosureWontFix)
            .Permit(BugTrigger.MarkDuplicate, BugState.PendingClosureDuplicate)
            .Permit(BugTrigger.MarkNotReproducible, BugState.PendingClosureNotReproducible)
            .Permit(BugTrigger.EscalateBlocked, BugState.Blocked);

        _machine.Configure(BugState.Deferred)
            .Permit(BugTrigger.ResumeAnalysis, BugState.DefectAnalysis);

        _machine.Configure(BugState.NeedMoreInfo)
            .Permit(BugTrigger.InformationProvided, BugState.DefectAnalysis);

        _machine.Configure(BugState.Fixing)
            .Permit(BugTrigger.Defer, BugState.Deferred)
            .Permit(BugTrigger.ReportNotReproducibleFromFix, BugState.PendingClosureNotReproducible)
            .Permit(BugTrigger.ConfirmProblemSolved, BugState.Closed)
            .Permit(BugTrigger.ReportProblemUnsolved, BugState.DefectAnalysis);

        ConfigurePendingClosure(BugState.PendingClosureNotDefect);
        ConfigurePendingClosure(BugState.PendingClosureWontFix);
        ConfigurePendingClosure(BugState.PendingClosureDuplicate);
        ConfigurePendingClosure(BugState.PendingClosureNotReproducible);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.ReopenClosed, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.CompleteReopen, BugState.DefectAnalysis)
            .OnEntry(() => _machine.Fire(BugTrigger.CompleteReopen));

        _machine.Configure(BugState.Blocked)
            .Permit(BugTrigger.Unblock, BugState.DefectAnalysis);
    }

    private void ConfigurePendingClosure(BugState pendingState)
    {
        _machine.Configure(pendingState)
            .Permit(BugTrigger.TesterApprovesClosure, BugState.Closed)
            .Permit(BugTrigger.TesterRequestsReturn, BugState.DefectAnalysis);
    }
}

internal static class Program
{
    private static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        var bug = new Bug();
        Console.WriteLine($"Начальное состояние: {bug.State}");
        Console.WriteLine($"Доступные действия: {string.Join(", ", bug.PermittedTriggers)}");

        bug.Fire(BugTrigger.SubmitForTriage);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.ReportProblemUnsolved);
        bug.Fire(BugTrigger.AssignToDeveloper);
        bug.Fire(BugTrigger.ConfirmProblemSolved);

        Console.WriteLine($"После сценария «исправление → не решено → исправление → закрытие»: {bug.State}");
        Console.WriteLine();
        Console.WriteLine("Описание автомата (Stateless):");
        Console.WriteLine(bug.DescribeWorkflow());
    }
}
