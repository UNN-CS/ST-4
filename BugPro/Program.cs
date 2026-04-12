using Stateless;

namespace BugPro;

/// <summary>
/// Автомат состояний жизненного цикла бага (Stateless).
/// </summary>
public class Bug
{
    public enum State
    {
        Open,
        Assigned,
        InProgress,
        InReview,
        Deferred,
        Duplicate,
        Fixed,
        Verified,
        Closed
    }

    public enum Trigger
    {
        Assign,
        Start,
        Defer,
        Resume,
        MarkDuplicate,
        SubmitForReview,
        ApproveReview,
        RequestChanges,
        MarkFixed,
        Verify,
        Close,
        Reopen
    }

    private readonly StateMachine<State, Trigger> _machine;

    public Bug(State initial = State.Open)
    {
        _machine = new StateMachine<State, Trigger>(initial);
        ConfigureMachine();
    }

    private void ConfigureMachine()
    {
        _machine.Configure(State.Open)
            .Permit(Trigger.Assign, State.Assigned);

        _machine.Configure(State.Assigned)
            .Permit(Trigger.Start, State.InProgress)
            .Permit(Trigger.MarkDuplicate, State.Duplicate);

        _machine.Configure(State.InProgress)
            .Permit(Trigger.Defer, State.Deferred)
            .Permit(Trigger.SubmitForReview, State.InReview)
            .Permit(Trigger.MarkFixed, State.Fixed);

        _machine.Configure(State.InReview)
            .Permit(Trigger.ApproveReview, State.Fixed)
            .Permit(Trigger.RequestChanges, State.InProgress);

        _machine.Configure(State.Deferred)
            .Permit(Trigger.Resume, State.InProgress);

        _machine.Configure(State.Duplicate)
            .Permit(Trigger.Reopen, State.Open);

        _machine.Configure(State.Fixed)
            .Permit(Trigger.Verify, State.Verified)
            .Permit(Trigger.RequestChanges, State.InProgress);

        _machine.Configure(State.Verified)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.InProgress);

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Open);
    }

    public State CurrentState => _machine.State;

    public void Fire(Trigger trigger) => _machine.Fire(trigger);

    public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

    public IEnumerable<Trigger> PermittedTriggers => _machine.GetPermittedTriggers();
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Начальное состояние: {bug.CurrentState}");
        Console.WriteLine($"Допустимые триггеры: {string.Join(", ", bug.PermittedTriggers)}");

        bug.Fire(Bug.Trigger.Assign);
        Console.WriteLine($"После Assign: {bug.CurrentState}");

        bug.Fire(Bug.Trigger.Start);
        bug.Fire(Bug.Trigger.MarkFixed);
        bug.Fire(Bug.Trigger.Verify);
        bug.Fire(Bug.Trigger.Close);
        Console.WriteLine($"После типового пути (без ревью): {bug.CurrentState}");
    }
}
