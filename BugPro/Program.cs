using Stateless;

namespace BugPro;

public sealed class Bug
{
    public enum State
    {
        New,
        Triaging,
        WaitingForInfo,
        Postponed,
        InProgress,
        Resolved,
        Reopened,
        Closed,
        Rejected,
        Duplicate,
        CannotReproduce
    }

    public enum Trigger
    {
        OpenForTriaging,
        NeedMoreInfo,
        InfoProvided,
        Postpone,
        ResumeTriaging,
        StartFix,
        RequestMoreInfoFromFix,
        Resolve,
        VerifyPassed,
        VerifyFailed,
        Reevaluate,
        Reopen,
        MarkAsNotABug,
        MarkAsDuplicate,
        MarkAsCannotReproduce
    }

    private State _state = State.New;
    private readonly StateMachine<State, Trigger> _machine;

    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(() => _state, s => _state = s);

        _machine.Configure(State.New)
            .Permit(Trigger.OpenForTriaging, State.Triaging);

        _machine.Configure(State.Triaging)
            .Permit(Trigger.NeedMoreInfo, State.WaitingForInfo)
            .Permit(Trigger.Postpone, State.Postponed)
            .Permit(Trigger.StartFix, State.InProgress)
            .Permit(Trigger.MarkAsNotABug, State.Rejected)
            .Permit(Trigger.MarkAsDuplicate, State.Duplicate)
            .Permit(Trigger.MarkAsCannotReproduce, State.CannotReproduce);

        _machine.Configure(State.WaitingForInfo)
            .Permit(Trigger.InfoProvided, State.Triaging);

        _machine.Configure(State.Postponed)
            .Permit(Trigger.ResumeTriaging, State.Triaging);

        _machine.Configure(State.InProgress)
            .Permit(Trigger.RequestMoreInfoFromFix, State.WaitingForInfo)
            .Permit(Trigger.Resolve, State.Resolved);

        _machine.Configure(State.Resolved)
            .Permit(Trigger.VerifyPassed, State.Closed)
            .Permit(Trigger.VerifyFailed, State.Reopened);

        _machine.Configure(State.Reopened)
            .Permit(Trigger.Reevaluate, State.Triaging);

        _machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Rejected)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.Duplicate)
            .Permit(Trigger.Reopen, State.Reopened);

        _machine.Configure(State.CannotReproduce)
            .Permit(Trigger.Reopen, State.Reopened);
    }

    public State CurrentState => _state;

    public IEnumerable<Trigger> GetPermittedTriggers() => _machine.PermittedTriggers;

    public bool CanFire(Trigger trigger) => _machine.CanFire(trigger);

    public void Fire(Trigger trigger) => _machine.Fire(trigger);
}

internal static class Program
{
    private static void Main()
    {
        var bug = new Bug();
        Console.WriteLine($"Initial state: {bug.CurrentState}");

        bug.Fire(Bug.Trigger.OpenForTriaging);
        bug.Fire(Bug.Trigger.StartFix);
        bug.Fire(Bug.Trigger.Resolve);
        bug.Fire(Bug.Trigger.VerifyPassed);

        Console.WriteLine($"Final state: {bug.CurrentState}");
        Console.WriteLine("Permitted triggers from final state:");
        foreach (var trigger in bug.GetPermittedTriggers())
        {
            Console.WriteLine($" - {trigger}");
        }
    }
}
