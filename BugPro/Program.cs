using Stateless;

/// <summary>
/// Состояния жизненного цикла дефекта по workflow (тестировщик / продукт / разработка).
/// </summary>
public enum BugState
{
    NewDefect,
    Triage,
    Fixing,
    NotReproducible,
    /// <summary> Промежуточная проверка после фикса (расширение к диаграмме). </summary>
    InReview,
    Closing,
    Closed,
    NotABug,
    WontFix,
    Duplicate,
    Reopening
}

/// <summary>
/// События перехода автомата Stateless.
/// </summary>
public enum BugTrigger
{
    Report,
    AssignToDeveloper,
    MarkNotABug,
    MarkWontFix,
    MarkDuplicate,
    DeferNoTime,
    DeferNeedInfo,
    DeferSeparateSolution,
    DeferOtherProduct,
    CannotReproduce,
    SubmitFixResolved,
    SubmitFixNotResolved,
    ConfirmNotReproOk,
    ConfirmNotReproReject,
    ApproveReview,
    RejectReview,
    FinalClose,
    Reopen,
    CompleteReopen
}

/// <summary>
/// Автомат работы с багом.
/// </summary>
public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _stateEngine;

    public BugState State => _stateEngine.State;

    public Bug()
    {
        _stateEngine = new StateMachine<BugState, BugTrigger>(BugState.NewDefect);
        BuildStateGraph();
    }

    private void BuildStateGraph()
    {
        _stateEngine.Configure(BugState.NewDefect)
            .Permit(BugTrigger.Report, BugState.Triage);

        _stateEngine.Configure(BugState.Triage)
            .Permit(BugTrigger.AssignToDeveloper, BugState.Fixing)
            .Permit(BugTrigger.MarkNotABug, BugState.NotABug)
            .Permit(BugTrigger.MarkWontFix, BugState.WontFix)
            .Permit(BugTrigger.MarkDuplicate, BugState.Duplicate);

        _stateEngine.Configure(BugState.Fixing)
            .Permit(BugTrigger.CannotReproduce, BugState.NotReproducible)
            .Permit(BugTrigger.SubmitFixResolved, BugState.InReview)
            .Permit(BugTrigger.SubmitFixNotResolved, BugState.Triage)
            .Permit(BugTrigger.DeferNoTime, BugState.Triage)
            .Permit(BugTrigger.DeferNeedInfo, BugState.Triage)
            .Permit(BugTrigger.DeferSeparateSolution, BugState.Triage)
            .Permit(BugTrigger.DeferOtherProduct, BugState.Triage);

        _stateEngine.Configure(BugState.InReview)
            .Permit(BugTrigger.ApproveReview, BugState.Closing)
            .Permit(BugTrigger.RejectReview, BugState.Triage);

        _stateEngine.Configure(BugState.NotReproducible)
            .Permit(BugTrigger.ConfirmNotReproOk, BugState.Closing)
            .Permit(BugTrigger.ConfirmNotReproReject, BugState.Triage);

        _stateEngine.Configure(BugState.Closing)
            .Permit(BugTrigger.FinalClose, BugState.Closed);

        _stateEngine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopening);

        _stateEngine.Configure(BugState.Reopening)
            .Permit(BugTrigger.CompleteReopen, BugState.Triage);

        _stateEngine.Configure(BugState.NotABug);
        _stateEngine.Configure(BugState.WontFix);
        _stateEngine.Configure(BugState.Duplicate);
    }

    public void Fire(BugTrigger action) => _stateEngine.Fire(action);

    public bool CanFire(BugTrigger action) => _stateEngine.CanFire(action);
}

static class Program
{
    static void Main()
    {
        var ticket = new Bug();
        Console.WriteLine($"Начальное состояние: {ticket.State}");

        ticket.Fire(BugTrigger.Report);
        Console.WriteLine($"После регистрации: {ticket.State}");

        ticket.Fire(BugTrigger.AssignToDeveloper);
        Console.WriteLine($"Назначен разработчику: {ticket.State}");

        ticket.Fire(BugTrigger.SubmitFixResolved);
        Console.WriteLine($"Фикс на ревью: {ticket.State}");

        ticket.Fire(BugTrigger.ApproveReview);
        Console.WriteLine($"Одобрено, на закрытие: {ticket.State}");

        ticket.Fire(BugTrigger.FinalClose);
        Console.WriteLine($"Закрыт: {ticket.State}");
    }
}
