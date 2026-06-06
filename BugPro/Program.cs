using Stateless;

namespace BugPro;

public class Bug
{
    public enum State
    {
        New,
        Analysis,
        NeedInfo,
        Fixing,
        NotABug,
        Duplicate,
        CannotReproduce,
        Resolved,
        Closed,
        Reopened
    }

    public enum Trigger
    {
        StartAnalysis,
        RequestInfo,
        ReceiveInfo,
        StartFix,
        MarkNotABug,
        MarkDuplicate,
        MarkCannotReproduce,
        Resolve,
        VerifySuccess,
        VerifyFail,
        Reopen
    }

    private readonly StateMachine<State, Trigger> machine;

    public Bug()
    {
        machine = new StateMachine<State, Trigger>(State.New);

        machine.Configure(State.New)
            .Permit(Trigger.StartAnalysis, State.Analysis);

        machine.Configure(State.Analysis)
            .Permit(Trigger.RequestInfo, State.NeedInfo)
            .Permit(Trigger.StartFix, State.Fixing)
            .Permit(Trigger.MarkNotABug, State.NotABug)
            .Permit(Trigger.MarkDuplicate, State.Duplicate)
            .Permit(Trigger.MarkCannotReproduce, State.CannotReproduce);

        machine.Configure(State.NeedInfo)
            .Permit(Trigger.ReceiveInfo, State.Analysis);

        machine.Configure(State.Fixing)
            .Permit(Trigger.Resolve, State.Resolved);

        machine.Configure(State.Resolved)
            .Permit(Trigger.VerifySuccess, State.Closed)
            .Permit(Trigger.VerifyFail, State.Reopened);

        machine.Configure(State.Reopened)
            .Permit(Trigger.StartAnalysis, State.Analysis);

        machine.Configure(State.NotABug)
            .Permit(Trigger.Reopen, State.Reopened);

        machine.Configure(State.Duplicate)
            .Permit(Trigger.Reopen, State.Reopened);

        machine.Configure(State.CannotReproduce)
            .Permit(Trigger.Reopen, State.Reopened);

        machine.Configure(State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);
    }

    public State CurrentState => machine.State;

    public void Fire(Trigger trigger)
    {
        machine.Fire(trigger);
    }
}

internal class Program
{
    static void Main()
    {
        var bug = new Bug();

        Console.WriteLine($"Начальное состояние: {bug.CurrentState}");

        bug.Fire(Bug.Trigger.StartAnalysis);

        Console.WriteLine($"После анализа: {bug.CurrentState}");
    }
}
