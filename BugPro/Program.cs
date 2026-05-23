using Stateless;

public class Bug
{
    public enum State
    {
        New,
        InReview,
        InProgress,
        Fixed,
        Closed,
        Reopened,
        Rejected,

        NeedMoreInfo,
        Duplicate,
        NotABug,
        CannotReproduce
    }

    public enum Trigger
    {
        StartReview,
        StartWork,
        Fix,
        Close,
        Reopen,
        Reject,

        RequestMoreInfo,
        ProvideInfo,

        MarkDuplicate,
        MarkNotABug,
        MarkCannotReproduce
    }

    private readonly StateMachine<State, Trigger> _machine;

    public State Current => _machine.State;

    public Bug()
    {
        _machine = new StateMachine<State, Trigger>(State.New);

        // -------------------------
        // NEW
        // -------------------------

        _machine.Configure(State.New)
            .Permit(Trigger.StartReview, State.InReview)
            .Permit(Trigger.Reject, State.Rejected);

        // -------------------------
        // REVIEW
        // -------------------------

        _machine.Configure(State.InReview)
            .Permit(Trigger.StartWork, State.InProgress)
            .Permit(Trigger.Reject, State.Rejected)
            .Permit(Trigger.RequestMoreInfo, State.NeedMoreInfo)
            .Permit(Trigger.MarkDuplicate, State.Duplicate)
            .Permit(Trigger.MarkNotABug, State.NotABug)
            .Permit(Trigger.MarkCannotReproduce, State.CannotReproduce);

        // -------------------------
        // NEED MORE INFO
        // -------------------------

        _machine.Configure(State.NeedMoreInfo)
            .Permit(Trigger.ProvideInfo, State.InReview);

        // -------------------------
        // IN PROGRESS
        // -------------------------

        _machine.Configure(State.InProgress)
            .Permit(Trigger.Fix, State.Fixed);

        // -------------------------
        // FIXED
        // -------------------------

        _machine.Configure(State.Fixed)
            .Permit(Trigger.Close, State.Closed)
            .Permit(Trigger.Reopen, State.Reopened);

        // -------------------------
        // REOPENED
        // -------------------------

        _machine.Configure(State.Reopened)
            .Permit(Trigger.StartWork, State.InProgress);

        // -------------------------
        // FINAL STATES
        // -------------------------

        _machine.Configure(State.Rejected);
        _machine.Configure(State.Closed);
        _machine.Configure(State.Duplicate);
        _machine.Configure(State.NotABug);
        _machine.Configure(State.CannotReproduce);
    }

    public void Fire(Trigger trigger)
    {
        _machine.Fire(trigger);
    }
}
public class Program
{
    public static void Main()
    {
        var bug = new Bug();

        Console.WriteLine(bug.Current);

        bug.Fire(Bug.Trigger.StartReview);
        Console.WriteLine(bug.Current);
    }
}
