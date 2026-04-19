using System.Collections.Generic;
using System.Linq;
using Stateless;

namespace BugPro;

public enum BugState
{
    New,
    Triage,
    NeedMoreInfo,
    Fixing,
    ReadyForVerification,
    Returned,
    NotABug,
    WontFix,
    Duplicate,
    NotReproducible,
    Closed,
    Reopened
}

public enum BugTrigger
{
    StartTriage,
    RequestMoreInfo,
    ProvideMoreInfo,
    StartFixing,
    ReturnFromFix,
    MarkAsNotABug,
    MarkAsWontFix,
    MarkAsDuplicate,
    MarkAsNotReproducible,
    MarkFixed,
    AcceptFix,
    RejectFix,
    ReturnToTriage,
    Reopen
}

public enum TriageDecision
{
    NeedMoreInfo,
    FixNow,
    NotABug,
    WontFix,
    Duplicate,
    NotReproducible
}

public enum ReturnReason
{
    NoTimeRightNow,
    SeparateSolutionRequired,
    AnotherProductIssue,
    FailedVerification
}

public sealed class Bug
{
    private readonly StateMachine<BugState, BugTrigger> _machine;
    private readonly List<string> _timeline = new();

    public Bug(string id, string title)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("Bug id must not be empty.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Bug title must not be empty.", nameof(title));
        }

        Id = id;
        Title = title;

        _machine = new StateMachine<BugState, BugTrigger>(BugState.New);
        ConfigureStateMachine();

        _timeline.Add($"{State}: defect created");
    }

    public string Id { get; }

    public string Title { get; }

    public BugState State => _machine.State;

    public string? LastActor { get; private set; }

    public string? LastActionComment { get; private set; }

    public TriageDecision? LastTriageDecision { get; private set; }

    public ReturnReason? LastReturnReason { get; private set; }

    public IReadOnlyList<string> Timeline => _timeline;

    public IReadOnlyCollection<BugTrigger> PermittedTriggers => _machine.PermittedTriggers.ToArray();

    public bool CanFire(BugTrigger trigger) => _machine.CanFire(trigger);

    public void StartTriage(string comment = "Дефект принят в разбор.", string actor = "Тестировщик") =>
        Fire(BugTrigger.StartTriage, actor, comment);

    public void RequestMoreInfo(string comment, string actor = "Продуктовая команда") =>
        Fire(
            BugTrigger.RequestMoreInfo,
            actor,
            comment,
            triageDecision: TriageDecision.NeedMoreInfo);

    public void ProvideMoreInfo(string comment, string actor = "Тестировщик") =>
        Fire(BugTrigger.ProvideMoreInfo, actor, comment);

    public void StartFixing(string comment, string actor = "Продуктовая команда") =>
        Fire(
            BugTrigger.StartFixing,
            actor,
            comment,
            triageDecision: TriageDecision.FixNow);

    public void MarkAsNotABug(string comment, string actor = "Продуктовая команда") =>
        Fire(
            BugTrigger.MarkAsNotABug,
            actor,
            comment,
            triageDecision: TriageDecision.NotABug);

    public void MarkAsWontFix(string comment, string actor = "Продуктовая команда") =>
        Fire(
            BugTrigger.MarkAsWontFix,
            actor,
            comment,
            triageDecision: TriageDecision.WontFix);

    public void MarkAsDuplicate(string comment, string actor = "Продуктовая команда") =>
        Fire(
            BugTrigger.MarkAsDuplicate,
            actor,
            comment,
            triageDecision: TriageDecision.Duplicate);

    public void MarkAsNotReproducible(string comment, string actor = "Продуктовая команда") =>
        Fire(
            BugTrigger.MarkAsNotReproducible,
            actor,
            comment,
            triageDecision: TriageDecision.NotReproducible);

    public void ReturnFromFix(ReturnReason reason, string comment, string actor = "Разработчик") =>
        Fire(BugTrigger.ReturnFromFix, actor, comment, returnReason: reason);

    public void MarkFixed(string comment = "Исправление готово к проверке.", string actor = "Разработчик") =>
        Fire(BugTrigger.MarkFixed, actor, comment);

    public void AcceptFix(string comment = "Исправление подтверждено.", string actor = "Тестировщик") =>
        Fire(BugTrigger.AcceptFix, actor, comment);

    public void RejectFix(string comment, string actor = "Тестировщик") =>
        Fire(
            BugTrigger.RejectFix,
            actor,
            comment,
            returnReason: ReturnReason.FailedVerification);

    public void ReturnToTriage(string comment, string actor = "Продуктовая команда") =>
        Fire(BugTrigger.ReturnToTriage, actor, comment);

    public void Reopen(string comment, string actor = "Тестировщик") =>
        Fire(BugTrigger.Reopen, actor, comment);

    public override string ToString() => $"{Id}: {Title} ({State})";

    private void ConfigureStateMachine()
    {
        _machine.Configure(BugState.New)
            .Permit(BugTrigger.StartTriage, BugState.Triage);

        _machine.Configure(BugState.Triage)
            .Permit(BugTrigger.RequestMoreInfo, BugState.NeedMoreInfo)
            .Permit(BugTrigger.StartFixing, BugState.Fixing)
            .Permit(BugTrigger.MarkAsNotABug, BugState.NotABug)
            .Permit(BugTrigger.MarkAsWontFix, BugState.WontFix)
            .Permit(BugTrigger.MarkAsDuplicate, BugState.Duplicate)
            .Permit(BugTrigger.MarkAsNotReproducible, BugState.NotReproducible);

        _machine.Configure(BugState.NeedMoreInfo)
            .Permit(BugTrigger.ProvideMoreInfo, BugState.Triage);

        _machine.Configure(BugState.Fixing)
            .Permit(BugTrigger.ReturnFromFix, BugState.Returned)
            .Permit(BugTrigger.MarkFixed, BugState.ReadyForVerification);

        _machine.Configure(BugState.ReadyForVerification)
            .Permit(BugTrigger.AcceptFix, BugState.Closed)
            .Permit(BugTrigger.RejectFix, BugState.Returned);

        _machine.Configure(BugState.Returned)
            .Permit(BugTrigger.ReturnToTriage, BugState.Triage);

        _machine.Configure(BugState.NotABug)
            .Permit(BugTrigger.ReturnToTriage, BugState.Triage);

        _machine.Configure(BugState.WontFix)
            .Permit(BugTrigger.ReturnToTriage, BugState.Triage);

        _machine.Configure(BugState.Duplicate)
            .Permit(BugTrigger.ReturnToTriage, BugState.Triage);

        _machine.Configure(BugState.NotReproducible)
            .Permit(BugTrigger.ReturnToTriage, BugState.Triage);

        _machine.Configure(BugState.Closed)
            .Permit(BugTrigger.Reopen, BugState.Reopened);

        _machine.Configure(BugState.Reopened)
            .Permit(BugTrigger.ReturnToTriage, BugState.Triage);
    }

    private void Fire(
        BugTrigger trigger,
        string actor,
        string? comment = null,
        TriageDecision? triageDecision = null,
        ReturnReason? returnReason = null)
    {
        var source = State;
        _machine.Fire(trigger);
        var destination = State;

        LastActor = actor;
        LastActionComment = string.IsNullOrWhiteSpace(comment) ? null : comment;
        LastTriageDecision = triageDecision;
        LastReturnReason = returnReason;

        var logEntry = $"{source} --{trigger}--> {destination} [{actor}]";

        if (triageDecision.HasValue)
        {
            logEntry += $" decision={triageDecision.Value}";
        }

        if (returnReason.HasValue)
        {
            logEntry += $" reason={returnReason.Value}";
        }

        if (!string.IsNullOrWhiteSpace(comment))
        {
            logEntry += $" comment=\"{comment}\"";
        }

        _timeline.Add(logEntry);
    }
}

public static class Program
{
    public static void Main()
    {
        var bug = new Bug("BUG-101", "Приложение падает при пустом пароле");

        PrintStatus("Создание дефекта", bug);

        bug.StartTriage();
        PrintStatus("Разбор", bug);

        bug.RequestMoreInfo("Не хватает шагов воспроизведения и логов.");
        PrintStatus("Запрос дополнительной информации", bug);

        bug.ProvideMoreInfo("Добавлены логи, видео и точные шаги.");
        PrintStatus("Информация получена", bug);

        bug.StartFixing("Дефект подтвержден, отправляем в разработку.");
        PrintStatus("Исправление", bug);

        bug.MarkFixed();
        PrintStatus("Проверка исправления", bug);

        bug.AcceptFix();
        PrintStatus("Закрытие", bug);

        bug.Reopen("После повторного теста проблема воспроизвелась снова.");
        bug.ReturnToTriage("Повторный разбор после переоткрытия.");
        PrintStatus("Переоткрытие", bug);

        Console.WriteLine();
        Console.WriteLine("История переходов:");

        foreach (var item in bug.Timeline)
        {
            Console.WriteLine($"- {item}");
        }
    }

    private static void PrintStatus(string step, Bug bug)
    {
        Console.WriteLine($"[{step}] {bug}");
        Console.WriteLine($"Доступные действия: {string.Join(", ", bug.PermittedTriggers)}");
        Console.WriteLine();
    }
}
