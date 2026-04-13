using Stateless;

public enum State
{
    Open,
    Triage,
    InProgress,
    Resolved,
    Closed,
    Reopened
}

public enum Trigger
{
    Assign,
    NeedMoreInfo,
    StartFix,
    NotABug,
    NotReproducible,
    Duplicate,
    WontFix,
    MarkResolved,
    Approve,
    Return,
    Reopen,
    AssignForFix
}

public class Bug
{
    private readonly StateMachine<State, Trigger> _machine;

    public string Title { get; }
    public State CurrentState => _machine.State;

    public Bug(string title)
    {
        Title = title;
        _machine = new StateMachine<State, Trigger>(State.Open);

        _machine.Configure(State.Open)
            .Permit(Trigger.Assign, State.Triage);

        _machine.Configure(State.Triage)
            .PermitReentry(Trigger.NeedMoreInfo)
            .Permit(Trigger.StartFix, State.InProgress);

        _machine.Configure(State.InProgress)
            .Permit(Trigger.NotABug, State.Triage)
            .Permit(Trigger.NotReproducible, State.Triage)
            .Permit(Trigger.Duplicate, State.Triage)
            .Permit(Trigger.WontFix, State.Triage)
            .Permit(Trigger.MarkResolved, State.Resolved);

        _machine.Configure(State.Resolved)
            .Permit(Trigger.Approve, State.Closed)
            .Permit(Trigger.Return, State.Triage);

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Reopened)
            .Permit(Trigger.AssignForFix, State.InProgress);
    }

    public void Assign() => _machine.Fire(Trigger.Assign);
    public void NeedMoreInfo() => _machine.Fire(Trigger.NeedMoreInfo);
    public void StartFix() => _machine.Fire(Trigger.StartFix);
    public void NotABug() => _machine.Fire(Trigger.NotABug);
    public void NotReproducible() => _machine.Fire(Trigger.NotReproducible);
    public void Duplicate() => _machine.Fire(Trigger.Duplicate);
    public void WontFix() => _machine.Fire(Trigger.WontFix);
    public void MarkResolved() => _machine.Fire(Trigger.MarkResolved);
    public void Approve() => _machine.Fire(Trigger.Approve);
    public void Return() => _machine.Fire(Trigger.Return);
    public void Reopen() => _machine.Fire(Trigger.Reopen);
    public void AssignForFix() => _machine.Fire(Trigger.AssignForFix);

    public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

    public override string ToString() => $"Bug '{Title}': {CurrentState}";
}

class Program
{
    static void Main()
    {
        var bug1 = new Bug("Crash on startup");
        Console.WriteLine($"Создан баг: {bug1}");

        bug1.Assign();
        Console.WriteLine($"После назначения: {bug1}");

        bug1.NeedMoreInfo();
        Console.WriteLine($"Нужно больше информации: {bug1}");

        bug1.StartFix();
        Console.WriteLine($"Начато исправление: {bug1}");

        bug1.MarkResolved();
        Console.WriteLine($"Помечен как исправленный: {bug1}");

        bug1.Approve();
        Console.WriteLine($"Одобрен и закрыт: {bug1}");

        Console.WriteLine();

        var bug2 = new Bug("Login button not working");
        Console.WriteLine($"Создан баг: {bug2}");
        bug2.Assign();
        bug2.StartFix();
        bug2.NotReproducible();
        Console.WriteLine($"Не воспроизводится, возврат в триаж: {bug2}");
        bug2.StartFix();
        bug2.MarkResolved();
        bug2.Return();
        Console.WriteLine($"Возврат в триаж после проверки: {bug2}");
        bug2.StartFix();
        bug2.MarkResolved();
        bug2.Approve();
        Console.WriteLine($"Закрыт: {bug2}");
        bug2.Reopen();
        Console.WriteLine($"Переоткрыт: {bug2}");
        bug2.AssignForFix();
        Console.WriteLine($"Назначен на повторное исправление: {bug2}");
    }
}
