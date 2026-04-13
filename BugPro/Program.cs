using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    UnderReview,
    InProgress,
    CannotReproduce,
    Testing,
    Closed
}

public enum BugTrigger
{
    SubmitForTriage,
    AcceptForFix,
    RejectNotBug,
    RejectWontFix,
    RejectDuplicate,
    StartFix,
    DeveloperNeeds,
    CannotReproduce,
    TesterConfirmsNoRepro,
    TesterDisagrees,
    FixCompleted,
    VerifyFixed,
    VerifyNotFixed,
    Reopen
}

public enum DeveloperReturnReason
{
    NoTimeNow,
    NeedSeparateSolution,
    ExternalProductIssue,
    NeedMoreInformation
}

public enum CloseReason
{
    NotBug,
    WontFix,
    Duplicate,
    NoRepro,
    Fixed,
    Reopened
}

public class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;
    private readonly StateMachine<BugState, BugTrigger>.TriggerWithParameters<DeveloperReturnReason> _developerNeedsTrigger;

    public Bug()
    {
        _machine = new StateMachine<BugState, BugTrigger>(BugState.New);
        _developerNeedsTrigger = _machine.SetTriggerParameters<DeveloperReturnReason>(BugTrigger.DeveloperNeeds);

        _machine.Configure(BugState.New)
            .Permit(BugTrigger.SubmitForTriage, BugState.UnderReview);

        _machine.Configure(BugState.UnderReview)
            .Permit(BugTrigger.AcceptForFix, BugState.InProgress)
            .Permit(BugTrigger.RejectNotBug, BugState.Closed)
            .Permit(BugTrigger.RejectWontFix, BugState.Closed)
            .Permit(BugTrigger.RejectDuplicate, BugState.Closed);

        _machine.Configure(BugState.InProgress)
            .Permit(BugTrigger.StartFix, BugState.Testing)
            .Permit(BugTrigger.FixCompleted, BugState.Testing)
            .Permit(BugTrigger.DeveloperNeeds, BugState.UnderReview)
            .Permit(BugTrigger.CannotReproduce, BugState.CannotReproduce);

        _machine.Configure(BugState.CannotReproduce)
            .Permit(BugTrigger.TesterConfirmsNoRepro, BugState.Closed)
            .Permit(BugTrigger.TesterDisagrees, BugState.UnderReview);

        _machine.Configure(BugState.Testing)
            .Permit(BugTrigger.VerifyFixed, BugState.Closed)
            .Permit(BugTrigger.VerifyNotFixed, BugState.UnderReview);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.UnderReview);

        _machine.OnTransitioned(t =>
        {
            CurrentStateDescription = t.Destination.ToString();
            LastTrigger = t.Trigger.ToString();
        });
    }

    public BugState State => _machine.State;
    public string CurrentStateDescription { get; private set; } = BugState.New.ToString();
    public string? LastTrigger { get; private set; }
    public string? LastReason { get; private set; }
    public CloseReason? FinalCloseReason { get; private set; }

    public bool IsClosed => State == BugState.Closed;

    public void SubmitForTriage()
    {
        _machine.Fire(BugTrigger.SubmitForTriage);
        LastReason = null;
    }

    public void AcceptForFix()
    {
        _machine.Fire(BugTrigger.AcceptForFix);
        LastReason = null;
    }

    public void RejectAsNotBug()
    {
        _machine.Fire(BugTrigger.RejectNotBug);
        FinalCloseReason = CloseReason.NotBug;
        LastReason = "Не дефект";
    }

    public void RejectAsWontFix()
    {
        _machine.Fire(BugTrigger.RejectWontFix);
        FinalCloseReason = CloseReason.WontFix;
        LastReason = "Не исправлять";
    }

    public void RejectAsDuplicate()
    {
        _machine.Fire(BugTrigger.RejectDuplicate);
        FinalCloseReason = CloseReason.Duplicate;
        LastReason = "Дублирование";
    }

    public void StartFix()
    {
        _machine.Fire(BugTrigger.StartFix);
        LastReason = null;
    }

    public void DeveloperNeeds(DeveloperReturnReason reason)
    {
        _machine.Fire(_developerNeedsTrigger, reason);
        LastReason = reason switch
        {
            DeveloperReturnReason.NoTimeNow => "Нет времени сейчас",
            DeveloperReturnReason.NeedSeparateSolution => "Нужно отдельное решение",
            DeveloperReturnReason.ExternalProductIssue => "Проблема другого продукта",
            DeveloperReturnReason.NeedMoreInformation => "Нужно больше информации",
            _ => null
        };
    }

    public void CannotReproduce()
    {
        _machine.Fire(BugTrigger.CannotReproduce);
        LastReason = "Не воспроизводится";
    }

    public void TesterConfirmsNoRepro()
    {
        _machine.Fire(BugTrigger.TesterConfirmsNoRepro);
        FinalCloseReason = CloseReason.NoRepro;
        LastReason = "Тестировщик согласен: не воспроизводится";
    }

    public void TesterDisagreesAndReturns()
    {
        _machine.Fire(BugTrigger.TesterDisagrees);
        LastReason = "Тестировщик возвращает на разбор";
    }

    public void FixCompleted()
    {
        _machine.Fire(BugTrigger.FixCompleted);
        LastReason = "Исправление готово";
    }

    public void VerifyFixed()
    {
        _machine.Fire(BugTrigger.VerifyFixed);
        FinalCloseReason = CloseReason.Fixed;
        LastReason = "Баг исправлен";
    }

    public void VerifyNotFixed()
    {
        _machine.Fire(BugTrigger.VerifyNotFixed);
        LastReason = "Баг не исправлен";
    }

    public void Reopen()
    {
        _machine.Fire(BugTrigger.Reopen);
        FinalCloseReason = CloseReason.Reopened;
        LastReason = "Переоткрытие дефекта";
    }
}

public static class Program
{
    public static void Main()
    {
        var bug = new Bug();
        Console.WriteLine("=== BugPro workflow demo ===");
        Console.WriteLine($"Начальное состояние: {bug.State}");

        bug.SubmitForTriage();
        Console.WriteLine($"После разбора: {bug.State}");

        bug.AcceptForFix();
        Console.WriteLine($"Разработчик взял в работу: {bug.State}");

        bug.StartFix();
        Console.WriteLine($"После выполнения правок: {bug.State}");

        bug.VerifyFixed();
        Console.WriteLine($"После проверки: {bug.State}, результат: {bug.LastReason}");

        Console.WriteLine($"Окончательный статус: {(bug.IsClosed ? "закрыт" : "в работе")}");
    }
}
